using System;
using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Drawing;

namespace PlayScript.Application.OSX
{
        public partial class MainWindow : MonoMac.AppKit.NSWindow
        {
                public MainWindow (IntPtr handle) : base(handle)
                {
                }

                public MainWindow (NSCoder coder) : base(coder)
                {
                }

				public override void SetContentSize(SizeF aSize)
				{
					base.SetContentSize(aSize);
				}

        }
}

