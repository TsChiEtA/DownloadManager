using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkGrabber
{
    public class GrabberResult : EventArgs
    {
        public string ResolvingModule { get; private set; }
        public bool LinkFound { get; private set; }
        public Uri ResultUri { get; private set; }
        public Uri SourceUri { get; private set; }

        public string ResultTitle { get; private set; }

        public GrabberResult(string resolvingModule, bool linkFound, Uri resultUri, Uri sourceUri, string title)
        {
            this.ResolvingModule = resolvingModule;
            this.LinkFound = linkFound;
            this.ResultUri = resultUri;
            this.SourceUri = sourceUri;
            this.ResultTitle = title;
        }
        
    }
}
