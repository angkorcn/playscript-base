
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
			// set window title
			Window.Title = "PlayScript";

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

