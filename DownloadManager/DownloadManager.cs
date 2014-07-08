using ContinuousLinq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DownloadManager
{ 
    public class DownloadManager : INotifyPropertyChanged
    {

        private string downloadPath;
        private int downloadSlots;
        private ContinuousCollection<Download> downloads;
        private Queue<Download> queue;
        private Dictionary<WebClient, Download> active;
        private bool autoDeleteCompletedDownloads;

        public bool AutoDeleteCompletedDownloads
        {
            get { return autoDeleteCompletedDownloads; }
            set { autoDeleteCompletedDownloads = value; }
        }

        public ObservableCollection<Download> Downloads
        {
            get { return downloads; }
        }

        public int DownloadSlots
        {
            get { return downloadSlots; }
            set
            {
                downloadSlots = value; OnPropertyChanged("DownloadSlots");
                DownloadQueueFile();
            }
        }

        public string DownloadPath
        {
            get { return downloadPath; }
            set { downloadPath = value; OnPropertyChanged("DownloadPath"); }
        }

        public DownloadManager(string path, int slots = 10)
        {
            downloadPath = path;
            downloadSlots = slots;

            downloads = new ContinuousCollection<Download>();
            active = new Dictionary<WebClient, Download>();
            queue = new Queue<Download>();
        }

        public void DownloadFile(Uri url)
        {
            DownloadFile(url, null);
        }

        public void DownloadFile(Uri url, string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                filename = url.Segments.Last();

            if (active.Count < downloadSlots) // Sofort runterladen
            {
                Download d = new Download
                {
                    Url = url,
                    File = filename,
                    State = DownloadState.Downloading
                };

                WebClient c = new WebClient();
                c.DownloadFileCompleted += c_DownloadFileCompleted;
                c.DownloadProgressChanged += c_DownloadProgressChanged;
                c.DownloadFileAsync(url, Path.Combine(downloadPath, filename));

                active.Add(c, d);
                downloads.Add(d);
            }
            else // Queue
            {
                Download d = new Download
                {
                    Url = url,
                    File = filename,
                    State = DownloadState.Queued
                };

                queue.Enqueue(d);
                downloads.Add(d);
            }
        }

        public void RemoveCompletedDownloads()
        {
            var complete = downloads.Where(d => d.Percentage >= 100);
            foreach (Download d in complete)
            {
                downloads.Remove(d);
            }
        }

        void c_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            WebClient c = sender as WebClient;
            if (c != null)
            {
                Download d;
                if (active.TryGetValue(c, out d))
                {
                    d.Percentage = e.ProgressPercentage;
                    d.TotalBytes = e.TotalBytesToReceive;
                    d.DownloadedBytes = e.BytesReceived;
                }
            }
        }

        void c_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            WebClient c = sender as WebClient;
            if (c != null)
            {
                Download d;
                if (active.TryGetValue(c, out d))
                {
                    if (e.Cancelled)
                        d.State = DownloadState.Canceled;
                    else if (e.Error != null)
                        d.State = DownloadState.Error;
                    else
                        d.State = DownloadState.Completed;

                    active.Remove(c);
                    c.Dispose();

                    if (autoDeleteCompletedDownloads)
                    {
                        downloads.Remove(d);
                    }

                    DownloadQueueFile();
                }
            }
        }

        private void DownloadQueueFile()
        {
            while (active.Count < downloadSlots && queue.Count > 0)
            {
                Download d = queue.Dequeue();
                d.State = DownloadState.Downloading;

                WebClient c = new WebClient();
                c.DownloadFileCompleted += c_DownloadFileCompleted;
                c.DownloadProgressChanged += c_DownloadProgressChanged;
                c.DownloadFileAsync(d.Url, Path.Combine(downloadPath, d.File));

                active.Add(c, d);
            }
        }


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
