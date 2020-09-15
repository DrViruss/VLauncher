using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace VLauncher_Own_
{
    /*
    * 1. Alternately download: JAVA, ASSETS, MP
    * 2. If any of this foulder not exist or hash != hash from file - 1
    */
    internal class Downloader
    {
        public PC pc = new PC();
        public string gamepath { get; set; }
        public string mpname { get; set; }
        

        public void Download(DownloadType type, Label label, ProgressBar progressBar)
        {
            
            switch (type)
            {
                case DownloadType.mpname:
                    MCDownload(GetMPLink("mpname", mpname), label, progressBar,type);
                    break;
                case DownloadType.java:
                    MCDownload(GetMPLink("java", pc.getOSType().ToString()), label, progressBar, type);
                    break;
                case DownloadType.assets:
                    if (mpname.Contains("v1.12.2"))
                        MCDownload(GetMPLink("assets", "1.12.2"), label, progressBar, type);
                    break;
            }
        }

        private void MCDownload(string link, Label label, ProgressBar progressBar, DownloadType type)
        {
                progressBar.Invoke(new MethodInvoker(delegate{progressBar.Visible = true;}));
                label.Invoke(new MethodInvoker(delegate{label.Visible = true; }));

            using (var client = new System.Net.WebClient())
            {
                Thread thread = new Thread(() =>
                {
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    client.DownloadFileAsync(new Uri(link), gamepath + "\\Archive.rar");

                });
                thread.Start();
            }
            void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
            {
                double MbytesIn = double.Parse(e.BytesReceived.ToString()) * 1024;
                double totalMBytes = double.Parse(e.TotalBytesToReceive.ToString()) * 1024;
                double percentage = MbytesIn / totalMBytes * 100;
                if (label.InvokeRequired && progressBar.InvokeRequired)
                {
                    label.Invoke(new MethodInvoker(delegate
                    {
                    label.Text = "Type: " + char.ToUpper(type.ToString()[0])+type.ToString().Substring(1) +"     " +e.BytesReceived / (1024*1024) +"mb"+ " / " +  e.TotalBytesToReceive / (1024*1024)+"mb";
                    }));

                    progressBar.Invoke(new MethodInvoker(delegate
                    {
                        progressBar.Value = int.Parse(Math.Truncate(percentage).ToString());
                    }));
                }
            }
            void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
            {
                if (label.InvokeRequired)
                {
                    label.Invoke(new MethodInvoker(delegate
                    {
                        label.Text = "Unraring...";
                    }));
                }
                UnArch(label,progressBar);
            }
        }



        private void UnArch(Label label,ProgressBar progressBar)
        {
            using (var archive = RarArchive.Open(gamepath + "\\Archive.rar"))
            {
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(gamepath, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }
            File.Delete(gamepath + "\\Archive.rar");
            MessageBox.Show("Dowload and installing complited.\nNow you have a game!");
            if (label.InvokeRequired && progressBar.InvokeRequired)
            {
                label.Invoke(new MethodInvoker(delegate
                {
                    label.Visible = false;
                }));
                progressBar.Invoke(new MethodInvoker(delegate
                    {
                        progressBar.Visible = false;
                        progressBar.Value = 0;
                    }));
            }


        }


        private string GetMPLink(string type, string var)
        {
            string getlink = "http://v-packs.c1.biz/TEST.php?"+type+"=" + var;
            HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(getlink);
            rq.Method = "POST";
            rq.KeepAlive = false;
            HttpWebResponse response = (HttpWebResponse)rq.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }
        public enum DownloadType
        {
            mpname,
            java,
            assets
        }
    }
    
}
