using System;

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
			throw new NotImplementedException("TODO: Android");
		}
	}
}

