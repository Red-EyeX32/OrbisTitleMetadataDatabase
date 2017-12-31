/* Copyright (c) 2017 Red-EyeX32
*
* This software is provided 'as-is', without any express or implied
* warranty. In no event will the authors be held liable for any damages arising from the use of this software.
*
* Permission is granted to anyone to use this software for any purpose,
* including commercial applications*, and to alter it and redistribute it
* freely, subject to the following restrictions:
*
* 1. The origin of this software must not be misrepresented; you must not
*    claim that you wrote the original software. If you use this software
*    in a product, an acknowledge in the product documentation is required.
*
* 2. Altered source versions must be plainly marked as such, and must not
*    be misrepresented as being the original software.
*
* 3. This notice may not be removed or altered from any source distribution.
*
* *Contact must be made to discuss permission and terms.
*/

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OrbisTitleMetadataDatabase
{
    public partial class Main : Form
    {
        private BackgroundWorker bw;
        private TitleMetadataDatabase tmdb;

        private CultureInfo culture;

        [DllImport("libSceTitleMetadataDatabase.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr tmdb_gen_link(string np_title_id);

        public Main()
        {
            InitializeComponent();

            bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.WorkerReportsProgress = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!bw.IsBusy) {
                bw.RunWorkerAsync();

                btnSearch.Enabled = false;
                lblMore.Enabled = false;
                prgStatus.Value = 0;
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(HttpCompleted);
            string json_link = Marshal.PtrToStringAnsi(tmdb_gen_link(txtNpTitleId.Text));

            wc.DownloadStringAsync(new Uri(json_link));

            bw.ReportProgress(100);
        }

        private void HttpCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null) {
                try {
                    tmdb = JsonConvert.DeserializeObject<TitleMetadataDatabase>(e.Result);

                    if (this.InvokeRequired) {
                        this.Invoke((MethodInvoker)delegate {
                            ChangeLanguage(this, "en");
                            tsbName.DropDownItems.Clear();

                            for (int i = 0; i < tmdb.names.Count(); i++) {
                                if (string.IsNullOrEmpty(tmdb.names[i].lang))
                                    tmdb.names[i].lang = "(Default)";

                                byte[] nameBytes = Encoding.Default.GetBytes(tmdb.names[i].name);
                                tmdb.names[i].name = Encoding.UTF8.GetString(nameBytes);

                                var tsi = new ToolStripMenuItem(tmdb.names[i].name);
                                tsi.DropDownItems.Add(tmdb.names[i].lang, null, tsbName_DropDownItemClicked);
                                tsbName.DropDownItems.Add(tsi);
                            }

                            foreach (var icon in tmdb.icons)
                                picIcon.ImageLocation = icon.icon;

                            tslConsoleType.Text = tmdb.console.ToString();

                            txtRevision.Text = tmdb.revision.ToString();
                            txtFormatVersion.Text = tmdb.formatVersion.ToString();
                            txtParentalLevel.Text = tmdb.parentalLevel.ToString();

                            txtContentId.Text = tmdb.contentId.ToString();

                            txtCategory.Text = tmdb.category.ToString();

                            chkPsVr.Checked = Convert.ToBoolean(tmdb.psVr);
                            chkNeoEnabled.Checked = Convert.ToBoolean(tmdb.neoEnable);
                        });
                    }
                } catch {
                    MessageBox.Show("Error parsing json file for game with np_title_id: " + txtNpTitleId.Text, "Error parsing",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("Error finding game with np_title_id: " + txtNpTitleId.Text + "\n\n" + e.Error, "Not found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbName_DropDownItemClicked(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem) {
                if ((sender as ToolStripItem).Text == "(Default)")
                    ChangeLanguage(this, "en");
                else {
                    ChangeLanguage(this, (sender as ToolStripMenuItem).Text.ToString());
                }
            }
        }

        #region Change Language

        private void ChangeLanguage(Form form, string lang)
        {
            culture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            Form main = form;
            while (main.Owner != null) {
                main = main.Owner;
            }

            ApplyLanguage(form, null);
        }

        private void ApplyLanguage(object value, ComponentResourceManager resources)
        {
            if (value is Form) {
                resources = new ComponentResourceManager(value.GetType());
                resources.ApplyResources(value, "$this");
            }

            Type type = value.GetType();
            foreach (PropertyInfo info in type.GetProperties()) {
                switch (info.Name) {
                    case "Items":
                    case "DropDownItems":
                    case "Controls":
                    case "OwnedForms":
                        if (info.PropertyType.GetInterface("IEnumerable") != null) {
                            IEnumerable collection = (IEnumerable)info.GetValue(value, null);
                            if (collection != null) {
                                foreach (object o in collection){
                                    PropertyInfo objNameProp = o.GetType().GetProperty("Name");
                                    ApplyResourceOnType(resources, o, objNameProp);
                                }
                            }
                        }
                        break;
                    case "ContextMenuStrip":
                        object obj = (object)info.GetValue(value, null);
                        if (obj != null) {
                            ApplyLanguage(obj, resources);
                            resources.ApplyResources(obj, info.Name, culture);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void ApplyResourceOnType(ComponentResourceManager resources, object o, PropertyInfo objNameProp)
        {
            switch (o.GetType().Name) {
                case "ComboBox":
                    for (int i = 0; i < ((ComboBox)o).Items.Count; i++) {
                        ((ComboBox)o).Items[i] = resources.GetString(GetItemName(o, objNameProp, i), culture);
                    }
                    break;
                case "ListBox":
                    for (int i = 0; i < ((ListBox)o).Items.Count; i++) {
                        ((ListBox)o).Items[i] = resources.GetString(GetItemName(o, objNameProp, i), culture);
                    }
                    break;
                default:
                    if (objNameProp != null) {
                        string name = objNameProp.GetValue(o, null).ToString();
                        ApplyLanguage(o, resources);
                        resources.ApplyResources(o, name, culture);
                    }
                    break;
            }
        }

        private string GetItemName(object o, PropertyInfo objNameProp, int i)
        {
            string name = string.Format("{0}.{1}",
                objNameProp.GetValue(o, null).ToString(),
                "Items");

            if (i != 0)
                name = string.Format("{0}{1}", name, i);

            return name;
        }

        #endregion

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Invoke(new Action(() => prgStatus.Value = e.ProgressPercentage));
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnSearch.Enabled = true;
            lblMore.Enabled = true;
        }

        private void picIcon_MouseHover(object sender, EventArgs e)
        {
            if (tmdb != null) {
                picIcon.Cursor = Cursors.Hand;

                var tip = new ToolTip();
                foreach (var icon in tmdb.icons) {
                    if (tmdb.icons.Count == 1)
                        tip.SetToolTip(picIcon, ("Size: " + icon.type.ToString() + 
                            "\n" + "Link: " + icon.icon.ToString()));
                }
            }
        }

        private void picIcon_DoubleClick(object sender, EventArgs e)
        {
            if (tmdb != null) {
                var fbd = new FolderBrowserDialog();
                fbd.Description = "Select folder to save the icon(s).";

                if (fbd.ShowDialog() == DialogResult.OK) {
                    foreach (var icon in tmdb.icons) {
                        var uri = new Uri(icon.icon.ToString());
                        using (var wc = new WebClient()) {
                            wc.DownloadFileAsync(uri, fbd.SelectedPath + "\\" + Path.GetFileName(uri.LocalPath));
                            wc.Dispose();
                        }
                    }

                    MessageBox.Show("All icon(s) has been saved successfully!", "Saved sucessfully",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        public frmExtra dlg;
        private void lblMore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (tmdb != null) {
                var main = Main.ActiveForm;
                using (dlg = new frmExtra(tmdb, txtNpTitleId.Text, tmdb.names[0].name)) {
                    dlg.FormClosing += delegate { main.Show(); };

                    main.Hide();
                    dlg.ShowDialog();
                }
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (File.Exists(dlg.at9Path))
                File.Delete(dlg.at9Path);

            if (File.Exists(dlg.at9toolPath))
                File.Delete(dlg.at9toolPath);

            if (File.Exists(dlg.wavPath))
                File.Delete(dlg.wavPath);
        }
    }


    #region TitleMetadataDatabase Structure

    public class Name
    {
        public string name { get; set; }
        public string lang { get; set; }
    }

    public class Icon
    {
        public string icon { get; set; }
        public string type { get; set; }
    }

    public class TitleMetadataDatabase
    {
        public int revision { get; set; }
        public int patchRevision { get; set; }
        public int formatVersion { get; set; }
        public string npTitleId { get; set; }
        public string console { get; set; }
        public List<Name> names { get; set; }
        public List<Icon> icons { get; set; }
        public int parentalLevel { get; set; }
        public string pronunciation { get; set; }
        public string contentId { get; set; }
        public string backgroundImage { get; set; }
        public string bgm { get; set; }
        public string category { get; set; }
        public int psVr { get; set; }
        public int neoEnable { get; set; }
    }

    #endregion
}
