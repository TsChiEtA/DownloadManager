using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LinkGrabber.Modules
{
    public class StreamCloudGrabber : IGrabber
    {
        private Regex regex;
        public event EventHandler<GrabberResult> GrabbingCompleted;

        public event EventHandler<string> GrabbingStarted;

        public string Identifier
        {
            get
            {
                return "StreamCloudGrabber";
            }
        }

        public StreamCloudGrabber()
        {
            regex = new Regex("file:\\s*\"(?<link>.*?)\"");
        }

        public void Grab(Uri uri)
        {
            if (GrabbingStarted != null)
                GrabbingStarted.Invoke(this, uri.AbsoluteUri);

            WebClient wc = new WebClient();
            string page = wc.DownloadString(uri.AbsoluteUri);
            HtmlDocument doc = new HtmlDocument();

            doc.LoadHtml(page);
            var formInputNodes = doc.DocumentNode.SelectNodes("//input[@type='hidden']");
            string param = "";
            foreach (var inp in formInputNodes)
            {
                if (param != "")
                    param += "&";
                param += inp.GetAttributeValue("name", "x") + "=" + inp.GetAttributeValue("value", "");
            }

            System.Threading.Thread.Sleep(10500);

            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            page = wc.UploadString(uri.AbsoluteUri, param);
            
            doc.LoadHtml(page);

            var title = doc.DocumentNode.SelectSingleNode("//div[@id='page']//h1");

            var match = regex.Match(page);
            Uri link = null;
            if(match.Success)
                link = new Uri(match.Groups["link"].Value);

            GrabberResult gr = new GrabberResult(Identifier, link != null, link, uri, title == null ? null : title.InnerText + ".mp4");

            if (GrabbingCompleted != null)
                GrabbingCompleted.Invoke(this, gr);
        }


        public string[] SupportedHosts
        {
            get { return new string[] { "streamcloud.eu" }; }
        }
    }
}
