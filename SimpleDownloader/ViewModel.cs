using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DownloadManager;
using System.Windows;
using LinkGrabber;
using LinkGrabber.Modules;

namespace SimpleDownloader
{
    public class ViewModel
    {
        private DownloadManager.DownloadManager dlm;
        private LinkGrabber.LinkGrabber lg;

        public DownloadManager.DownloadManager Dlm
        {
            get { return dlm; }
            set { dlm = value; }
        }

        public ViewModel()
        {
            dlm = new DownloadManager.DownloadManager(@"C:\Users\Pascal\Downloads\_DLM", 1);
            lg = new LinkGrabber.LinkGrabber();
            lg.InitPackageModules();
            foreach (var s in lg.SupportedHosts)
                Console.WriteLine(s);

            lg.GrabbingStarted += (o, s) =>
            {
                Console.WriteLine("-- Looking for media in: " + s);
            };

            lg.GrabbingCompleted += (o, gr) =>
            {
                if (gr.LinkFound)
                {
                    Console.WriteLine("-- Link found via " + gr.ResolvingModule);
                    Console.WriteLine("-- " + gr.ResultUri);
                    dlm.DownloadFile(gr.ResultUri, gr.ResultTitle);
                }
                else
                {
                    Console.WriteLine("-- Grabbing completed without results!");
                }
            };
        }

        public void ClipboardChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Clipboard changed");
            string cb = Clipboard.GetText();
            Uri uri;
            if (Uri.TryCreate(cb, UriKind.Absolute, out uri) && uri.Scheme == Uri.UriSchemeHttp)
            {
                lg.Resolve(uri);
                Console.WriteLine("-- Resolve started!");
            }
        }
    }
}
