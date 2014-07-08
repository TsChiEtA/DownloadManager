using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkGrabber
{
    internal class DefaultGrabber : IGrabber
    {
        public event EventHandler<GrabberResult> GrabbingCompleted;

        public event EventHandler<string> GrabbingStarted;

        public string Identifier
        {
            get { return "Default"; }
        }

        public string[] SupportedHosts
        {
            get { return new string[] { }; }
        }

        public void Grab(Uri uri)
        {
            Console.WriteLine("-- Default unsupported!");
        }
    }
}
