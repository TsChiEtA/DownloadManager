using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LinkGrabber
{
    public class LinkGrabber
    {
        public event EventHandler<GrabberResult> GrabbingCompleted;
        public event EventHandler<string> GrabbingStarted;

        private Dictionary<string,IGrabber> supportedHosts;
        private List<IGrabber> grabber;
        private IGrabber defaultGrabber;

        public string[] SupportedHosts
        {
            get { return supportedHosts.Keys.ToArray(); }
        }

        public LinkGrabber()
        {
            supportedHosts = new Dictionary<string, IGrabber>();
            grabber = new List<IGrabber>();

            defaultGrabber = new DefaultGrabber();
            defaultGrabber.GrabbingStarted += HandleGrabbingStarted;
            defaultGrabber.GrabbingCompleted += HandleGrabbingCompleted;
        }

        public void Resolve(Uri uri)
        {
            IGrabber g;
            if (supportedHosts.TryGetValue(uri.Host.Replace("www.","").ToLower(), out g))
            {
                new Thread(() => g.Grab(uri)).Start();
            }
            else
            {
                new Thread(() => defaultGrabber.Grab(uri)).Start();
            }
        }

        public void AddGrabberModule(IGrabber g)
        {
            grabber.Add(g);
            g.GrabbingCompleted += HandleGrabbingCompleted;
            g.GrabbingStarted += HandleGrabbingStarted;

            foreach (string s in g.SupportedHosts)
            {
                supportedHosts.Add(s.ToLower(), g);
            }
        }

        private void HandleGrabbingStarted(object sender, string r)
        {
            if (GrabbingStarted != null)
                GrabbingStarted.Invoke(this, r);
        }

        private void HandleGrabbingCompleted(object sender, GrabberResult r)
        {
            if (GrabbingCompleted != null)
                GrabbingCompleted.Invoke(this, r);
        }
    }
}
