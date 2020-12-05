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
    public partial class Form4 : Form
    {
        static int margin_x = 240; //버튼 및 초기 x 위치
        static int margin_y = 100; //버튼 및 초기 y 위치
        static int btn_w = 30;   // 버튼 폭
        static int btn_h = 20;   // 버튼 높이
        static int btn_s = 13;   // 버튼 사이 간격
        Button[] btn = new Button[50];
        Button btn_c;
        int count = 0;
        Form7 frm7 = new Form7();

        public Form4()
        {
            
            InitializeComponent();
            //인원수 초기화
            textBox1.Text = "0"; 
            textBox2.Text = "0";
            textBox3.Text = "0";
            BtnC();
        }
        //코드로 Button을 생성해서 Text와 Size, Location를 설정하고 이벤트 함수를 생성해 Form에 띄운다.
        public void BtnC()
        {
            for (int i = 0; i < 50; i++)
            {
                btn[i] = new Button();
                btn[i].Text = i.ToString();
                btn[i].Name = "Button" + i.ToString();
                btn[i].Size = new Size(btn_w, btn_h);
                btn[i].Location = new Point(/*x좌표*/margin_x + (i % 10) * (btn_w + btn_s), /*y좌표*/margin_y + (i / 10) * (btn_h + btn_s));
                btn[i].Click += btn_Click;
                // btn.Click += 이라고 쓴 후 Tab을 누르면 이벤트함수가 자동 생성됨
                // btn을 클릭했을 때, Btn_Click을 같이 실행하라는 의미
                Controls.Add(btn[i]);   // btn[i]를 화면에 추가함
            }
        }
        //좌석 선택
        private void btn_Click(object sender, EventArgs e)
        {
            //textbox 인원수 합산
            int num = Int32.Parse(textBox1.Text) + Int32.Parse(textBox2.Text) + Int32.Parse(textBox3.Text);
            btn_c = sender as Button;
            if (Int32.Parse(textBox1.Text) == 0 && Int32.Parse(textBox2.Text) == 0 && Int32.Parse(textBox3.Text) == 0)
            {
                MessageBox.Show("인원수를 선택해주세요.");
            }
            else
            {
                //인원수만큼 좌석 선택
                if (count < num)
                {
                    for (int i = 0; i < 19; i++)
                    {
                        if (btn_c.Name == btn[i].Name)
                        {
                            btn_c.BackColor = System.Drawing.Color.Green;
                        }
                    }
                    for (int i = 19; i < 50; i++)
                    {
                        if (btn_c.Name == btn[i].Name)
                        {
                            btn_c.BackColor = System.Drawing.Color.Red;
                        }
                    }
                    count++;
                }
                else
                {
                    MessageBox.Show("인원수 초과입니다. 결제 해주세요.");
                }
            }
        }
        //좌석 결제 버튼 클릭 이벤트
        private void button1_Click_2(object sender, EventArgs e)
        {
            for (int i = 0; i < 19; i++) //스크린 앞 좌석
            {
                if (btn_c.Name == btn[i].Name)
                {
                    if (Int32.Parse(textBox1.Text) != 0 || Int32.Parse(textBox2.Text) != 0 || Int32.Parse(textBox3.Text) != 0)
                    {
                        //결제 금액 추산
                        int price = (Int32.Parse(textBox1.Text) * 7000) + (Int32.Parse(textBox2.Text) * 5000) + (Int32.Parse(textBox3.Text) * 3000);
                        frm7.textBox1.Text = price.ToString(); //결제 금액 전달
                        frm7.ShowDialog(); //결제 창 오픈
                    }
                }
            }
            for (int i = 19; i < 50; i++) //스크린 뒷 좌석
            {
                if (btn_c.Name == btn[i].Name)
                { 
                    if (Int32.Parse(textBox1.Text) != 0 || Int32.Parse(textBox2.Text) != 0 || Int32.Parse(textBox3.Text) != 0)
                    {
                        //결제 금액 추산
                        int price = (Int32.Parse(textBox1.Text) * 10000) + (Int32.Parse(textBox2.Text) * 8000) + (Int32.Parse(textBox3.Text) * 5000);
                        frm7.textBox1.Text = price.ToString(); //결제 금액 전달
                        frm7.ShowDialog();//결제 창 오픈
                    }
                }
            }
        }
        private void Button_MouseClick(Object sender, MouseEventArgs e)
        {
            //button19.BackColor = System.Drawing.Color.Green;
        }
        //좌석 선택
        private void button19_Click(object sender, EventArgs e)
        {
            MessageBox.Show("7000원 입니다."); //선택 시, 메세지 박스 노출
            int a = 7000;
            Form7 frm7 = new Form7(); //결제창 오픈
            frm7.textBox1.Text = Convert.ToString(a); //
            //button19.BackColor = System.Drawing.Color.Green;
            frm7.ShowDialog();
        }
        

        private void button52_Click(object sender, EventArgs e)
        {
            int adult = int.Parse(textBox1.Text);
            adult -= 1;
            textBox1.Text = Convert.ToString(adult);
        }
       
        private void button51_Click(object sender, EventArgs e)
        {
            int adult = int.Parse(textBox1.Text);
            adult += 1;
            textBox1.Text = Convert.ToString(adult);
        }

        private void button53_Click(object sender, EventArgs e)
        {
            int adult = int.Parse(textBox2.Text);
            adult -= 1;
            textBox2.Text = Convert.ToString(adult);
        }

        private void button54_Click(object sender, EventArgs e)
        {
            int adult = int.Parse(textBox2.Text);
            adult += 1;
            textBox2.Text = Convert.ToString(adult);
        }

        private void button55_Click(object sender, EventArgs e)
        {
            int adult = int.Parse(textBox3.Text);
            adult -= 1;
            textBox3.Text = Convert.ToString(adult);
        }

        private void button56_Click(object sender, EventArgs e)
        {
            int adult = int.Parse(textBox3.Text);
            adult += 1;
            textBox3.Text = Convert.ToString(adult);
        }

        private void button57_Click(object sender, EventArgs e)
        {
            Form8 frm8 = new Form8();
            frm8.ShowDialog();
        }
        }
    }

