using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LinkGrabber.Modules
{
    public class YoutubeGrabber : IGrabber
    {
        public event EventHandler<GrabberResult> GrabbingCompleted;

        public event EventHandler<string> GrabbingStarted;

        public string Identifier
        {
            get
            {
                return "YoutubeGrabber";
            }
        }

        public void Grab(Uri uri)
        {
            WebClient wc = new WebClient();
            string page = wc.DownloadString(uri.AbsoluteUri + "&html5=1");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);

            var title = doc.DocumentNode.Descendants("title").SingleOrDefault();

            var videoNode = doc.DocumentNode.SelectSingleNode("//video[@src]");
            var link = videoNode.GetAttributeValue("src", null);

            GrabberResult gr = new GrabberResult(Identifier, link != null, new Uri(link), uri, title == null ? null : title.InnerText);

            if (GrabbingCompleted != null)
                GrabbingCompleted.Invoke(this, gr);
        }


        public string[] SupportedHosts
        {
            get { return new string[] { "youtube.com" }; }
        }
    }
}
