using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Print_Screen_Utility
{

    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Height = GetHeight(80);
            this.Width = GetWidth(61) + 24;
            this.MaximizeBox = false;

            save.Location = new System.Drawing.Point((GetWidth(60) / 2) + 12 - 57, (GetHeight(63)));
            notifyIcon.Visible = true;
            previewScreenshot.Height = GetHeight(60);
            previewScreenshot.Width = GetWidth(60);
            save.Enabled = img != null;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            gHook.unhook();
            Environment.Exit(0);
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void Form1_FormClosing(object s, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        GlobalKeyboardHook gHook;

        private void Form1_Load(object sender, EventArgs e)
        {
            gHook = new GlobalKeyboardHook();
            gHook.hook();
            gHook.KeyDown += new KeyEventHandler(gHook_KeyDown);
            gHook.HookedKeys.Add(Keys.PrintScreen);
        }

        private void gHook_KeyDown(object sender, KeyEventArgs e)
        {
            notifyIcon.ShowBalloonTip(20);
        }

        Image img;
        private void ballon_Click(object sender, EventArgs e)
        {
            this.Show();
            if (Clipboard.ContainsImage())
            {
                img = Clipboard.GetImage();
                previewScreenshot.SizeMode = PictureBoxSizeMode.StretchImage;
                previewScreenshot.Image = img;
            }
            else
            {
                MessageBox.Show("Cannot get image", "Cannot get image", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            save.Enabled = img != null;
        }

        private void saveButton_click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Screenshot";
            dialog.AddExtension = true;
            dialog.OverwritePrompt = true;
            dialog.Filter = "PNG|*.png|GIF|*.gif|BMP|*.bmp|JPEG|*.jpg;*.jpeg";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                img.Save(dialog.FileName);
            }
        }

        public Rectangle GetScreen()
        {
            return Screen.FromControl(this).Bounds;
        }

        public int GetWidth(int p)
        {
            return (GetScreen().Width * p) / 100;
        }

        public int GetHeight(int p)
        {
            return (GetScreen().Height * p) / 100;
        }
    }
}
