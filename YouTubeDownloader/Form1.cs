using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace YouTubeDownloader
{
    public partial class Form1 : Form
    {
        string Path = "";
        string fileType = "";
        string URL = "";
        BackgroundWorker Processer = new BackgroundWorker();
        public Form1()
        {
            InitializeComponent();
            Processer.WorkerReportsProgress = true;
            Processer.WorkerSupportsCancellation = true;
            Processer.ProgressChanged += Processer_ProgressChanged;
            Processer.DoWork += Processer_DoWork;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Path = folderBrowserDialog1.SelectedPath;
                label1.Text = "Location: " + folderBrowserDialog1.SelectedPath;
            }
        }
        private void Processer_DoWork(object sender, DoWorkEventArgs e)
        {
            StreamReader StandardOutput = e.Argument as StreamReader;
            string data = StandardOutput.ReadLine();
            while (data != null)
            {
                Processer.ReportProgress(0, data);
                data = StandardOutput.ReadLine();
            }
        }
        private void Processer_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string data = e.UserState as string;
            if (data != null)
                richTextBox1.Text += data + "\r\n";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            fileType = " -x --audio-format mp3 ";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            fileType = " -f mp4 ";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            URL = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Path == "")
            {
                MessageBox.Show("Path cannot be empty");
            }
            else if (fileType == "")
            {
                MessageBox.Show("Select a file type");
            }
            else if (URL == "")
            {
                MessageBox.Show("URL cannot be empty");
            }
            else
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "yt-dlp.exe";
                    if (fileType == "")
                    {
                        process.StartInfo.Arguments = "-P " + Path + URL;
                        MessageBox.Show(process.StartInfo.Arguments);
                    }
                    else
                    {
                        process.StartInfo.Arguments = "-P " + Path + fileType + URL;
                    }
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    Processer.RunWorkerAsync(process.StandardOutput);
                }

            }
        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            fileType = " ";
        }
    }
}