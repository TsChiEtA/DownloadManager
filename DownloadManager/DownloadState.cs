using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager
{ 
    public enum DownloadState
    {
        Queued,
        Downloading,
        Completed,
        Canceled,
        Error
    }
}
