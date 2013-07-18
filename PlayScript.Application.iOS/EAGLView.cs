using System;

using OpenTK;
using OpenTK.Graphics.ES20;
using GL1 = OpenTK.Graphics.ES11.GL;
using All1 = OpenTK.Graphics.ES11.All;
using OpenTK.Platform.iPhoneOS;

using MonoTouch.Foundation;
using MonoTouch.CoreAnimation;
using MonoTouch.ObjCRuntime;
using MonoTouch.OpenGLES;
using MonoTouch.UIKit;

namespace PlayScript.Application.iOS
{
	[Register ("EAGLView")]
	public class EAGLView : iPhoneOSGameView
	{
		public static System.Type LoadClass;

		// this is our playscript player
		PlayScript.Player      mPlayer;

		[Export("initWithCoder:")]
		public EAGLView (NSCoder coder) : base (coder)
		{
			AutoResize = true;
			LayerRetainsBacking = true;
			LayerColorFormat = EAGLColorFormat.RGBA8;
		}
		
		[Export ("layerClass")]
		public static new Class GetLayerClass ()
		{
			return iPhoneOSGameView.GetLayerClass ();
		}
		
		protected override void ConfigureLayer (CAEAGLLayer eaglLayer)
		{
			eaglLayer.Opaque = true;

			if (this.Frame.Width <= 480.0 || this.Frame.Height <= 480.0) {
				eaglLayer.ContentsScale = 2.0f; // UIScreen.MainScreen.Scale;
			} else {
				eaglLayer.ContentsScale = 1.0f; // UIScreen.MainScreen.Scale;
			}	

			Console.WriteLine("context scale for frame {0} set to {1}", this.Frame, eaglLayer.ContentsScale);
		}
		
		protected override void CreateFrameBuffer ()
		{
			try {
				ContextRenderingApi = EAGLRenderingAPI.OpenGLES2;
				base.CreateFrameBuffer ();
			} catch (Exception) {
				ContextRenderingApi = EAGLRenderingAPI.OpenGLES1;
				base.CreateFrameBuffer ();
			}
		}
		
		protected override void DestroyFrameBuffer ()
		{
			base.DestroyFrameBuffer ();
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			mPlayer.OnTouchesBegan(touches, evt);
		}

		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);
		
			mPlayer.OnTouchesMoved(touches, evt);
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);

			mPlayer.OnTouchesEnded(touches, evt);
		}

		public void OnPinchRecognized(UIPinchGestureRecognizer pinchRecognizer)
		{
			mPlayer.OnPinchRecognized(pinchRecognizer);
		}

		
		#region DisplayLink support
		
		int frameInterval;
		CADisplayLink displayLink;
		
		public bool IsAnimating { get; private set; }
		
		// How many display frames must pass between each time the display link fires.
		public int FrameInterval {
			get {
				return frameInterval;
			}
			set {
				if (value <= 0)
					throw new ArgumentException ();
				frameInterval = value;
				if (IsAnimating) {
					StopAnimating ();
					StartAnimating ();
				}
			}
		}
		
		public void StartAnimating ()
		{
			if (IsAnimating)
				return;
			
			CreateFrameBuffer ();
			CADisplayLink displayLink = UIScreen.MainScreen.CreateDisplayLink (this, new Selector ("drawFrame"));
			displayLink.FrameInterval = frameInterval;
			displayLink.AddToRunLoop (NSRunLoop.Current, NSRunLoop.NSDefaultRunLoopMode);
			this.displayLink = displayLink;

			if (mPlayer == null) {
				InitPlayer();
			}

			IsAnimating = true;
		}
		
		public void StopAnimating ()
		{
			if (!IsAnimating)
				return;
			displayLink.Invalidate ();
			displayLink = null;
			DestroyFrameBuffer ();
			IsAnimating = false;
		}
		
		[Export ("drawFrame")]
		void DrawFrame ()
		{
			OnRenderFrame (new FrameEventArgs ());
		}
		
		#endregion

		private System.Drawing.RectangleF GetScaledFrame()
		{
			var scale = this.Layer.ContentsScale;
			var rect = this.Bounds;
			rect.X      *= scale;
			rect.Y      *= scale;
			rect.Width  *= scale;
			rect.Height *= scale;

			int width, height;
			GL.GetRenderbufferParameter(RenderbufferTarget.Renderbuffer, RenderbufferParameterName.RenderbufferWidth, out width);
			GL.GetRenderbufferParameter(RenderbufferTarget.Renderbuffer, RenderbufferParameterName.RenderbufferHeight, out height);
			return rect;
		}

		protected void InitPlayer()
		{
			flash.display3D.Context3D.OnPresent = this.OnPresent;

			// create player
			var rect = GetScaledFrame();
			mPlayer = new PlayScript.Player(rect);

			// load swf application
			if (LoadClass != null) {
				mPlayer.LoadClass(LoadClass);
			}
		}

		private bool mPresent = false;
		protected void OnPresent(flash.display3D.Context3D context)
		{
			mPresent = true;
//			SwapBuffers ();			
		}

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);
			
			MakeCurrent ();

			mPresent = false;

			if (mPlayer != null) {
				// resize player every frame
				var rect = GetScaledFrame();
				mPlayer.OnResize(rect);

				mPlayer.OnFrame();
			}

			if (mPresent == true) {
				PlayScript.Profiler.Begin("swap");
				SwapBuffers ();
				PlayScript.Profiler.End("swap");
			}
		}
	}
}
