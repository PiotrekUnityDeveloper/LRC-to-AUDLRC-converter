using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AUDLRCConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "(*.lrc) | *.lrc";
            ofd.Multiselect = false;
            ofd.Title = "select your .lrc";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                string filePath = files[0];

                textBox1.Text = filePath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text;
            string output = "";

            if(textBox2.Text == "<source>")
            {
                output = Path.GetDirectoryName(textBox1.Text) + "\\" + Path.GetFileNameWithoutExtension(textBox1.Text) + ".txt";
            }
            else
            {
                output = textBox2.Text;
            }

            List<string> srcLyrics = new List<string>();
            srcLyrics = File.ReadAllLines(input).ToList();

            MessageBox.Show(string.Join("\n", srcLyrics));

            for (int i = srcLyrics.Count - 1; i >= 0; i--)
            {
                if (srcLyrics[i].Contains("[ti") ||
                    srcLyrics[i].Contains("[la") ||
                    srcLyrics[i].Contains("[re") ||
                    srcLyrics[i].Contains("[ve") ||
                    string.IsNullOrWhiteSpace(srcLyrics[i]) || srcLyrics[i].Trim() == "" ||
                    srcLyrics[i].Contains(" --- "))
                {
                    srcLyrics.RemoveAt(i);
                }
            }

            for (int i = 0; i < srcLyrics.Count; i++)
            {
                string[] splitLine = srcLyrics[i].Split(']');

                //MessageBox.Show(splitLine[0].Replace("[", string.Empty));
                string newLineSeconds = ConvertTimeToSeconds(splitLine[0].Replace("[", string.Empty)).ToString();
                string completeLine = newLineSeconds + " " + splitLine[1];
                srcLyrics[i] = completeLine;
            }

            //export finished lyrics

            File.WriteAllLines(output, srcLyrics);
            MessageBox.Show("extracted: \n" + String.Join("\n", srcLyrics) + "\n\n to:\n " + output);
        }

        public int ConvertTimeToSeconds(string time)
        {
            // Split the time string by ':' or '.'
            string[] parts = time.Split(':', '.');

            // Parse minutes, seconds, and milliseconds
            int minutes = int.Parse(parts[0]);
            int seconds = int.Parse(parts[1]);
            int milliseconds = int.Parse(parts[2]);

            // Convert minutes and seconds into seconds and sum them up
            int totalSeconds = minutes * 60 + seconds;

            return totalSeconds;
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                string filePath = files[0];

                textBox1.Text = filePath;
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
