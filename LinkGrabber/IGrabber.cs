using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkGrabber
{
    public interface IGrabber
    {
        event EventHandler<GrabberResult> GrabbingCompleted;
        event EventHandler<string> GrabbingStarted;

        string Identifier { get; }
        string[] SupportedHosts { get; }

        void Grab(Uri uri);
    }
}
