using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        Form3 form3 = new Form3();
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            //MessageBox.Show(pic.Name);

            Form2 frm2 = new Form2(pic.Name, this);
            frm2.ShowDialog();
        }
        private void label8_MouseEnter(object sender, EventArgs e)
        {
            button2.BackColor = SystemColors.MenuHighlight;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = SystemColors.ControlDark;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 48 && e.KeyChar <= 57)
            {
                MessageBox.Show("Form.KeyPress : " + e.KeyChar.ToString() + "pressed.");

            }
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackColor=SystemColors.MenuHighlight;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = SystemColors.ControlDark;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            form3.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void button11_Click(object sender, EventArgs e)
        {
            Form5 frm5 = new Form5();
            frm5.ShowDialog();
        }
    }
}
