using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager
{
    public class Download : INotifyPropertyChanged
    {
        private DownloadState state;
        private long totalBytes, downloadedBytes;
        private int percentage;
        private string file;
        private Uri url;

        #region Properties

        public DownloadState State
        {
            get { return state; }
            set { state = value; OnPropertyChanged("State"); }
        }

        public long DownloadedBytes
        {
            get { return downloadedBytes; }
            set { downloadedBytes = value; OnPropertyChanged("DownloadedBytes"); }
        }

        public long TotalBytes
        {
            get { return totalBytes; }
            set { totalBytes = value; OnPropertyChanged("TotalBytes"); }
        }

        public int Percentage
        {
            get { return percentage; }
            set { percentage = value; OnPropertyChanged("Percentage"); }
        }

        public Uri Url
        {
            get { return url; }
            set { url = value; OnPropertyChanged("Url"); }
        }

        public string File
        {
            get { return file; }
            set { file = value; OnPropertyChanged("File"); }
        }

        #endregion

        internal Download() { }

         
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
