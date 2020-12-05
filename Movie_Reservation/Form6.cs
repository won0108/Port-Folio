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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("아이디 : "+textBox1.Text+"비번 : "+textBox2.Text+"이름 : "+textBox3.Text+"전화번호 : "+textBox4.Text+"\n"+"가입 완료!");
            this.Close();
        }
    }
}
