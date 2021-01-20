using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using Utilities;
using System;

namespace AutoClicker
{
    public partial class Form1 : Form
    {
        private globalKeyboardHook gkh = new globalKeyboardHook();
        private System.Windows.Forms.Keys[] keysToAdd = { Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12, Control.ModifierKeys };
        private string[] keyCodes = { "", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12" };

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private Thread thread;
        private int cps = 10;
        private int mspc = 100;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            foreach (System.Windows.Forms.Keys addKey in keysToAdd)
            {
                gkh.HookedKeys.Add(addKey);
            }
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
        }

        private void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (e.KeyCode.ToString() == keyCodes[comboBox2.SelectedIndex])
                {
                    pictureBox1.Visible = true;
                    button1.Enabled = true;
                    button2.Enabled = false;
                    thread.Abort();
                }
                else if (e.KeyCode.ToString() == keyCodes[comboBox1.SelectedIndex])
                {
                    pictureBox1.Visible = false;
                    button2.Enabled = true;
                    button1.Enabled = false;
                    thread = new Thread(this.Click);
                    thread.Start();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            button2.Enabled = true;
            button1.Enabled = false;
            thread = new Thread(this.Click);
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            button1.Enabled = true;
            button2.Enabled = false;
            thread.Abort();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            cps = trackBar1.Value;
            label2.Text = "CPS: " + trackBar1.Value.ToString();
            mspc = 1000 / cps;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cps = getValue(cps + 1, 1, 100);
            label2.Text = "CPS: " + cps.ToString();
            trackBar1.Value = cps;
            mspc = 1000 / cps;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cps = getValue(cps - 1, 1, 100);
            label2.Text = "CPS: " + cps.ToString();
            trackBar1.Value = cps;
            mspc = 1000 / cps;
        }

        private void maxSpeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label2.Text = "CPS:MAX";
            trackBar1.Value = 100;
            mspc = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            contextMenuStrip1.Show(ptLowerLeft);
        }

        private void onTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (onTopToolStripMenuItem.Checked)
            {
                onTopToolStripMenuItem.Checked = false;
                this.TopMost = false;
            }
            else
            {
                onTopToolStripMenuItem.Checked = true;
                this.TopMost = true;
            }
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Credits credits = new Credits();
            credits.Show();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void titleBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (titleBarToolStripMenuItem.Checked)
            {
                titleBarToolStripMenuItem.Checked = false;
                Form1.ActiveForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            }
            else
            {
                titleBarToolStripMenuItem.Checked = true;
                Form1.ActiveForm.FormBorderStyle = FormBorderStyle.None;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine(comboBox1.SelectedIndex);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private int getValue(int value, int min, int max)
        {
            if (value > max)
            {
                return max;
            }
            else if (value < min)
            {
                return min;
            }
            else
            {
                return value;
            }
        }

        private new void Click()
        {
            while (true)
            {
                if (radioButton1.Checked)
                {//Left Click
                    mouse_event(0x0002, 0, 0, 0, 0);
                    mouse_event(0x0004, 0, 0, 0, 0);
                }
                else
                {//Right Click
                    mouse_event(0x0008, 0, 0, 0, 0);
                    mouse_event(0x0010, 0, 0, 0, 0);
                }
                Thread.Sleep(mspc);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}