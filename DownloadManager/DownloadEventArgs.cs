using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager
{ 
    public class DownloadEventArgs : EventArgs
    {
        private Download download;

        public Download Download
        {
            get { return download; }
        }

        public DownloadEventArgs(Download d)
        {
            this.download = d;
        }
    }
}
