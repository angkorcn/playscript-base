
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using MonoMac.Foundation;
using MonoMac.AppKit;

namespace PlayScript.Application.OSX
{
        public partial class MainWindowController : MonoMac.AppKit.NSWindowController
        {
			public static System.Type LoadClass;

			PlayScript.Player player;
			bool isAnimating;

                #region Constructors

                // Call to load from the XIB/NIB file
                public MainWindowController () : base("MainWindow")
                {
 
                }

                #endregion

                //strongly typed window accessor
                public new MainWindow Window {
                        get { return (MainWindow)base.Window; }
                }

		public override void AwakeFromNib ()
		{
			Size? desiredSize = PlayScript.Player.GetSWFDesiredSize(LoadClass);
			if (desiredSize.HasValue) {
				// TODO: resize window appropriately here
				Console.WriteLine("Viewport size: {0}", openGLView.Bounds);
				Console.WriteLine("[SWF] desired size: {0}", desiredSize.Value);
			}

			// Allocate the Player object
			player = new PlayScript.Player (openGLView.Bounds);

			if (LoadClass != null) {
				// load class that was assigned statically
				player.LoadClass(LoadClass);
			}

			// set window title
			Window.Title = player.Title;

			// Assign the player to the view
			openGLView.Player = player;

			// reset the viewport and update OpenGL Context
			openGLView.UpdateView ();
			
			// Activate the display link now
			openGLView.StartAnimation ();
			
			isAnimating = true;
		}


		public void startAnimation ()
		{
			if (isAnimating)
				return;
			
			openGLView.StartAnimation ();

			isAnimating = true;
		}

		public void stopAnimation ()
		{
			if (!isAnimating)
				return;
			
			openGLView.StopAnimation ();

			isAnimating = false;
		}

        
        }
}

