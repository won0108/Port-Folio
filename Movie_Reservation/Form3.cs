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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            basic_setting();
        }
        public void basic_setting() //listview 언체크로 디폴트
        {
            for(int i=0; i<5; i++)
            {
                listView1.Items[i].Checked = false;

            }
        }
        //영화 & 세부 항목 선택 후 완료 버튼 클릭 시, 좌석 선택 화면에 영화 & 세부 항목 전달
        private void button1_Click(object sender, EventArgs e)
        {
            Form4 frm4 = new Form4();
            //제목 text 전달
            for (int i = 0; i < listView1.Columns.Count; i++){
                if(listView1.Items[i].Checked)
                {
                    frm4.label10.Text = "영화 : " + this.listView1.Items[i].SubItems[0].Text;
                }
            }
            //극장 text 전달
            for(int j =0; j<listView2.Columns.Count; j++)
            {
                if (listView2.Items[j].Checked)
                {
                    frm4.label11.Text = "극장 : " + this.listView2.Items[j].SubItems[0].Text;
                }
            }
            //날짜 전달
            for (int p = 0; p < listView4.Columns.Count; p++){
                if (listView4.Items[p].Checked)
                {
                    frm4.label12.Text = "날짜:" + this.listView4.Items[p].SubItems[0].Text;
                }
            }
            //시간 전달
            for (int q = 0; q < listView3.Columns.Count; q++)
            {
                if (listView3.Items[q].Checked)
                {
                    frm4.label12.Text = "시간:" + this.listView3.Items[q].SubItems[0].Text;
                }
            }
            frm4.ShowDialog();
        }
        

        
        
    }
}
