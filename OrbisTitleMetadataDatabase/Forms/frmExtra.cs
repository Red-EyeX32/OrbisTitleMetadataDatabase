using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace OrbisTitleMetadataDatabase
{
    public partial class frmExtra : Form
    {
        private BackgroundWorker bw;
        private TitleMetadataDatabase tmdb;
        
        public string at9Path = string.Empty;
        public string wavPath = string.Empty;
        public string at9toolPath = string.Empty;

        public frmExtra(TitleMetadataDatabase _tmdb, string np_title_id, string name)
        {
            InitializeComponent();

            tmdb = _tmdb;
            this.Text = np_title_id + " - " + name;

            axWMP.settings.autoStart = false;

            bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            if (!bw.IsBusy) {
                bw.RunWorkerAsync();
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (tmdb != null) {
                picBackgroundImage.ImageLocation = tmdb.backgroundImage;

                // Use at9tool to convert ATRAC9 to WAV format.
                if (!string.IsNullOrEmpty(tmdb.bgm)) {
                    var uri = new Uri(tmdb.bgm.ToString());
                    string at9 = Path.GetFileName(uri.LocalPath);
                    string wav = Path.GetFileNameWithoutExtension(uri.LocalPath) + ".wav";
                    
                    at9Path = Application.StartupPath + "\\" + at9;
                    wavPath = Application.StartupPath + "\\" + wav;

                    using (var wc = new WebClient()) {
                        wc.DownloadFileTaskAsync(uri, at9Path);
                        wc.Dispose();
                    }
                }

                if (!string.IsNullOrEmpty(tmdb.pronunciation)) {
                    try {
                        cboLanguageId.Enabled = true;
                        cboPronunciation.Enabled = true;

                        var xml = new XmlDocument();
                        xml.Load(tmdb.pronunciation);

                        XmlNodeList nodes = xml.DocumentElement.SelectNodes("/gamePackage/language");
                        foreach (XmlNode node in nodes) {
                            if (node.NodeType == XmlNodeType.Element) {
                                XmlElement elem = (XmlElement)node;
                                cboLanguageId.Items.Add(elem.GetAttribute("id"));
                                cboLanguageId.SelectedIndex = 0;
                            }
                        }
                    } catch {
                        cboLanguageId.Enabled = false;
                        cboPronunciation.Enabled = false;

                        MessageBox.Show("Error parsing pronunciation xml file!", "Error parsing",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                txtAdvanced.Text = JsonConvert.SerializeObject(tmdb, Newtonsoft.Json.Formatting.Indented);
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tmdb.bgm)) {
                at9toolPath = Application.StartupPath + @"\at9tool.exe";
                File.WriteAllBytes(at9toolPath, Properties.Resources.at9tool);

                Thread.Sleep(5000);

                Process proc = new Process {
                    StartInfo = new ProcessStartInfo {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        WorkingDirectory = Path.GetDirectoryName(at9toolPath),
                        FileName = Path.GetFileName(at9toolPath),
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = "-d " + at9Path + " " + wavPath
                    }
                };
                proc.Start();
                proc.WaitForExit();
                
                axWMP.URL = wavPath;
            }
        }

        private void tcExtra_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tmdb != null) {
                if (!string.IsNullOrEmpty(tmdb.bgm)) {
                    if (tcExtra.SelectedIndex != 1) {
                        axWMP.Ctlcontrols.pause();
                    }
                }
            }
        }

        private void cboLanguageId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tmdb.pronunciation)) {
                cboPronunciation.Items.Clear();

                var xml = new XmlDocument();
                xml.Load(tmdb.pronunciation);

                XmlNodeList nodes = xml.DocumentElement.SelectNodes("/gamePackage/language");
                foreach (XmlNode node in nodes) {
                    if (node.NodeType == XmlNodeType.Element) {
                        XmlElement elem = (XmlElement)node;
                        if (elem.GetAttribute("id") == cboLanguageId.SelectedItem.ToString()) {
                            foreach (XmlNode child in elem.ChildNodes) {
                                if (child.NodeType == XmlNodeType.Element) {
                                    foreach (XmlNode childNode in child.ChildNodes) {
                                        cboPronunciation.Items.Add(childNode.Name.ToString());
                                        cboPronunciation.SelectedIndex = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void cboPronunciation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tmdb.pronunciation)) {
                var xml = new XmlDocument();
                xml.Load(tmdb.pronunciation);

                XmlNodeList nodes = xml.DocumentElement.SelectNodes("/gamePackage/language");
                foreach (XmlNode node in nodes) {
                    if (node.NodeType == XmlNodeType.Element) {
                        XmlElement elem = (XmlElement)node;
                        if (elem.GetAttribute("id") == cboLanguageId.SelectedItem.ToString()) {
                            foreach (XmlNode child in elem.ChildNodes) {
                                if (child.NodeType == XmlNodeType.Element) {
                                    foreach (XmlNode childNode in child.ChildNodes) {
                                        if (childNode.Name.ToString() == cboPronunciation.SelectedItem.ToString()) {
                                            txtLanguage.Text = childNode.InnerText.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void picBackgroundImage_MouseHover(object sender, EventArgs e)
        {
            if (tmdb != null) {
                if (!string.IsNullOrEmpty(tmdb.backgroundImage)) {
                    picBackgroundImage.Cursor = Cursors.Hand;

                    var tip = new ToolTip();
                    tip.SetToolTip(picBackgroundImage, "Link: " + tmdb.backgroundImage.ToString());
                }
            }
        }

        private void picBackgroundImage_DoubleClick(object sender, EventArgs e)
        {
            if (tmdb != null) {
                if (!string.IsNullOrEmpty(tmdb.backgroundImage)) {
                    var sfd = new SaveFileDialog();

                    var uri = new Uri(tmdb.backgroundImage.ToString());
                    sfd.FileName = Path.GetFileName(uri.LocalPath);

                    if (sfd.ShowDialog() == DialogResult.OK) {
                        using (var wc = new WebClient()) {
                            wc.DownloadFileAsync(uri, sfd.FileName);
                            wc.Dispose();
                        }

                        MessageBox.Show("Background image has been saved successfully!", "Saved sucessfully",
                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }

        private void frmExtra_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists(at9Path))
                File.Delete(at9Path);

            if (File.Exists(at9toolPath))
                File.Delete(at9toolPath);

            if (File.Exists(wavPath))
                File.Delete(wavPath);
        }
    }
}