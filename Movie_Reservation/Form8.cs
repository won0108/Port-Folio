using System;
using System.Collections;
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
    public partial class Form8 : Form
    {
        static int margin_x = 110; //체크박스 및 초기 x 위치
        static int margin_y = 130; //체크박스 및 초기 y 위치
        static int checkBoxes_w = 60;   // 체크박스 폭
        static int checkBoxes_h = 20;   // 체크박스 높이
        static int checkBoxes_s = 10; //체크박스 간격
        Checkb checkb = new Checkb();
        
        public class Checkb  //체크박스가 가져야할 값
        {
            public int[] price = new int[7] { 5000, 2500, 2500, 2500, 2500, 10000, 11000 };
            public string[] snack_name = new string[7] { "팝콘", "콜라", "사이다", "환타", "에이드", "팝콘+콜라", "달팝콘+사이다" };
            public CheckBox[] checkBoxes = new CheckBox[7];
            public Label[] checklb = new Label[7];
            
        }

        public Form8()
        {
            InitializeComponent();
            Checkmulti();
            check_box_init();
            
        }

        public void check_box_init()  //체크박스 언체크로 초기화
        {
            for(int i = 0; i < 7; i++){
                
                checkb.checkBoxes[i].Checked = false;
            }
        }
        public void Checkmulti() //체크박스 및 라벨 생성 및 위치 초기화
        {
            for (int i = 0; i < 7; i++)
            {
                checkb.checkBoxes[i] = new CheckBox();
                checkb.checklb[i] = new Label();
                //checkb.checkBoxes[i].Text = i.ToString();
                //checkb.checkBoxes[i].Name = "Button" + i.ToString();
                checkb.checkBoxes[i].Size = new Size(checkBoxes_w, checkBoxes_h);
                //checkb.checkBoxes[6].Size = new Size(checkBoxes_w + 50, checkBoxes_h);
                checkb.checklb[i].Size = new Size(checkBoxes_w, checkBoxes_h);
                if (i == 6)
                {
                    checkb.checkBoxes[i].Size = new Size(checkBoxes_w + 50, checkBoxes_h);
                    checkb.checkBoxes[i-1].Size = new Size(checkBoxes_w + 20, checkBoxes_h);

                    checkb.checklb[i-1].Size = new Size(checkBoxes_w + 10, checkBoxes_h);
                    checkb.checklb[i].Size = new Size(checkBoxes_w + 10, checkBoxes_h);
                }
                if (i >= 5)
                {
                    checkb.checkBoxes[i].Location = new Point(/*x좌표*/(margin_x - 400) + (i % 7) * (checkBoxes_w + (checkBoxes_s + 10)), /*y좌표*/(margin_y + 80) + (i / 7) * (checkBoxes_h + checkBoxes_s));
                    checkb.checklb[i].Location = new Point((margin_x - 335) + (i % 7) * (checkBoxes_w + checkBoxes_s), /*y좌표*/(margin_y + 100) + (i / 7) * (checkBoxes_h + checkBoxes_s));
                }
                else
                {
                    checkb.checkBoxes[i].Location = new Point(/*x좌표*/margin_x + (i % 7) * (checkBoxes_w + (checkBoxes_s + 10)), /*y좌표*/margin_y + (i / 7) * (checkBoxes_h + checkBoxes_s));
                    checkb.checklb[i].Location = new Point(/*x좌표*/(margin_x +15) + (i % 7) * (checkBoxes_w + (checkBoxes_s + 10)), /*y좌표*/(margin_y + 30) + (i / 7) * (checkBoxes_h + checkBoxes_s));

                }
                checkb.checkBoxes[i].Text = checkb.snack_name[i];
                checkb.checklb[i].Text = Convert.ToString(checkb.price[i]);
                checkb.checkBoxes[i].CheckedChanged += checking_box;
                // checkBoxes.Click += 이라고 쓴 후 Tab을 누르면 이벤트함수가 자동 생성됨
                // checkBoxes을 클릭했을 때, checkBoxes_Click을 같이 실행하라는 의미
                Controls.Add(checkb.checkBoxes[i]);   // checkBoxes[i]를 화면에 추가함
                Controls.Add(checkb.checklb[i]);
            }

        }
        //스낵 결제금액 노출
        private void checking_box(object sender, EventArgs e)
        {
            int r_num=0;
            for (int i=0; i<7; i++)
            {
                textBox1.Clear();
                if (checkb.checkBoxes[i].Checked==true)
                {
                    r_num += checkb.price[i];
                }
            }
            textBox1.Text = Convert.ToString(r_num);
        }
        private void button1_Click(object sender, EventArgs e) //결제완료 버튼
        {
            MessageBox.Show("결제 완료!!");
            this.Close();
        }
        
    }
}
