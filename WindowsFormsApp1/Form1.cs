using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        private int _ticks;
        private int deleteValue;
        private int press2key;
        private int keyValue;

        private List<int> termsList = new List<int>();
        public Form1()
        {
        InitializeComponent();
        timer2.Start();



        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            label3.Text =
                "Alt:" + (e.Alt ? "Yes" : "No") + '\n' +
                "Shift:" + (e.Shift ? "Yes" : "No") + '\n' +
                "Ctrl:" + (e.Control ? "Yes" : "No") + '\n' +
                "Delete:" + (e.KeyCode == Keys.Back ? "Yes" : "No") + '\n' +
                "KeyCode:" + e.KeyCode + '\n' +
                "KeyData:" + e.KeyData + '\n' +
                "KeyValue:" + e.KeyValue;

            if (e.Alt == true || e.Control == true || e.KeyCode == Keys.Back || e.KeyValue == 37 || e.KeyValue == 38 || e.KeyValue == 39 || e.KeyValue == 40)
            {
                keyValue = e.KeyValue;
                termsList.Add(keyValue);
            }
            else if (e.Shift == true && e.KeyValue != 0)
                press2key = 2;

            else
                keyValue = 0;

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            _ticks++;
            this.Text = _ticks.ToString();

        }
    

        private void button1_Click_1(object sender, EventArgs e)
        {
            int[] terms = termsList.ToArray();
            for (int run = 0; run < terms.Length; run++)
            {
                if (terms[run] > 0 && terms[run] != 8)
                    keyValue = terms[run];

                else
                  deleteValue = terms[run];
               //   MessageBox.Show("Value in index {"+run+"}:\t"+terms[run]);
            }

            textBox2.Text = "human"; //login time >= 8 seconds, keyin using Backspace, Alt,Option and press 2 key at same time
            if (_ticks < 8 && keyValue == 0 && deleteValue == 0)
             textBox2.Text = "spam";

            string str = string.Join(",", textBox2.Text, keyValue, deleteValue, press2key, _ticks);
            File.AppendAllText(@"C:\Users\ESCA\source\repos\WindowsFormsApp1\WindowsFormsApp1\test.txt", str + Environment.NewLine);

            MessageBox.Show("Successfully past the text in training file");

            termsList.Clear();
            press2key = 0;
            keyValue = 0;
            deleteValue = 0;
            timer2.Stop();
            _ticks = 0;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer2.Stop();
            timer2.Start();
            textBox2.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();

           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
