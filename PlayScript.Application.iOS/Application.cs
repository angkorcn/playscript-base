using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace playscript
{
	public static class Application
	{
		/// <summary>
		/// Does platform specific setup, creates a window, and runs the PlayScript main loop
		/// </summary>
		/// <param name="args">Command line arguments</param>
		/// <param name="loadClass">Type of class to load and run (should be derived from flash.display.Sprite)</param>
		public static void run(string[] args = null, System.Type loadClass = null)
		{
			// set class to be loaded by application
			PlayScript.Player.ApplicationClass = loadClass;
			PlayScript.Player.ApplicationArgs  = args;
			PlayScript.Player.ApplicationLoadDelay  = 0;

			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}
