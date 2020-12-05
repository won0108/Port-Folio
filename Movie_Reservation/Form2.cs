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
    public partial class Form2 : Form //포스터 클릭 시, 상세설명 창 오픈
    {
        string poster;
        Form1 form1;
        public Form2()
        {
            InitializeComponent();
        }
        public Form2(string in_name, Form1 in_form)
        {
            InitializeComponent();
            poster = in_name;
            form1 = in_form;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            if (poster.Equals("pictureBox1"))
            {
                pictureBox1.Image = Properties.Resources.미이라;
                label1.Text = "미이라";
                label2.Text= "The Mummy";
                label3.Text = "예매율 "+form1.label3.Text;
                label6.Text = "개봉일 "+form1.label4.Text;
                label4.Text= "감독 :  알렉스 커츠먼  / 배우 : 러셀 크로우, 톰 크루즈 ,  소피아 부텔라, 애나벨 월리스";
                label5.Text= "장르 :  액션, 환타지, 어드벤처 / 기본 : 15세 이상, 110분, 미국";
                label7.Text= "신들과 괴물들의 세상, 절대적 존재가 깨어난다!\n\n";
                label8.Text= "사막 한 가운데, 고대 이집트 미이라의 무덤을 발견한 닉(톰 크루즈)은\n\n" +
"미이라의 관을 수송하던 중 의문의 비행기 추락사고로 사망한다.\n\n" +
"그러나 죽음에서 다시 깨어난 닉!\n\n" +
"그는 자신이 발견한 미이라 무덤이 강력한 힘을 갈구한 잘못된 욕망으로 인해\n\n"+ "산 채로 봉인 당해야 했던 아마네트 공주의 것이며,\n\n"+ "자신이 부활하게 된 비밀이 이로부터 시작됨을 감지한다.\n\n"
+ "한편, 수천 년 만에 잠에서 깨어난 아마네트는\n\n"+ "분노와 파괴의 강력한 힘으로 전 세상을 자신의 것으로 만들려 하고,\n\n"+ "지킬 박사(러셀 크로우)는 닉에게 의미심장한 이야기를 전하게 되는데...\n\n";
                label10.Text = "공식사이트 : "+"mummy2017.kr";
            }
            else if (poster.Equals("pictureBox2"))
            {
                pictureBox1.Image = Properties.Resources.악녀;
                label1.Text = form1.label5.Text;
                label2.Text = "The Villainess";
                label3.Text= "예매율 " + form1.label7.Text;
                label6.Text = "개봉일 " + form1.label8.Text;
                label4.Text = "감독 : 정병길  / 배우 : 김옥빈 ,  신하균 ,  성준 ,  김서형 ,  조은지";
                label5.Text = "장르 : 액션 / 기본 : 청소년 관람불가, 123분, 한국";
                label7.Text = "2017년, 액션의 신세계가 펼쳐진다!\n\n";
                label8.Text = "어린 시절부터 킬러로 길러진 숙희.\n\n" +
"그녀는 국가 비밀조직에 스카우트되어 새로운 삶을 살 기회를 얻는다.\n\n" +
"10년만 일해주면 넌 자유야.하지만 가짜처럼 보이는 순간, 그땐 우리가 널 제거한다\n\n" +
"살기 위해 죽여야만 하는 킬러 숙희 앞에\n\n" +
"진실을 숨긴 의문의 두 남자가 등장하고,\n\n" +
"자신을 둘러싼 엄청난 비밀에 마주하게 되면서 운명에 맞서기 시작하는데...\n\n" +
"보여줄게, 내가 어떻게 만들어졌는지";
            }
            else if (poster.Equals("pictureBox3"))
            {
                pictureBox1.Image = Properties.Resources.원더우먼1;
                label1.Text = form1.label9.Text;
                label2.Text = "Wonder Woman";
                label3.Text = "예매율 " + form1.label11.Text;
                label6.Text = "개봉일 " + form1.label12.Text;
                label4.Text= "감독 : 패티 젠킨스  / 배우 : 갤 가돗 ,  로빈 라이트 ,  크리스 파인";
                label5.Text= "장르 : 액션, SF, 어드벤처, 환타지 / 기본 : 12세 이상, 141분, 미국";
                label7.Text= "히어로의 새로운 기준 \"내가 원더 우먼이다!\"\n";
                label8.Text= "아마존 데미스키라 왕국의 공주 ‘다이애나 프린스’(갤 가돗)는 전사로서 훈련을 받던 중 최강 전사로서의 운명을 직감한다.\n\n" +
"때마침 섬에 불시착한 조종사 ‘트레버 대위’(크리스 파인)를 통해 인간 세상의 존재와 그 곳에서 전쟁이 일어나고 있음을 알게 된다.\n\n" +
"신들이 주신 능력으로 세상을 구하는 것이 자신의 사명임을 깨달은 다이애나는 \n\n" +
"낙원과 같은 섬을 뛰쳐나와 1차 세계 대전의 지옥 같은 전장 한가운데로 뛰어드는데…\n\n";

            }
            else if (poster.Equals("pictureBox4"))
            {
                pictureBox1.Image = Properties.Resources.노무현;
                label1.Text = form1.label13.Text;
                label2.Text = "Our President";
                label3.Text = "예매율 " + form1.label15.Text;
                label6.Text = "개봉일 " + form1.label16.Text;
                label4.Text = "감독 : 이창재  / 배우 : 노무현 ,  이화춘 ,  유시민";
                label5.Text = "장르 : 다큐멘터리 / 기본 : 12세 이상, 109분, 한국";
                label7.Text = "지지율 2%의 꼴찌 후보에서 대선후보 1위, 국민의 대통령이 되기까지\n\n"+"2002년 전국을 뒤흔들었던 노무현, 그 기적의 역전 드라마\n\n";
                label8.Text = "국회의원, 시장 선거 등 출마하는 선거마다 번번이 낙선했던 만년 꼴찌 후보 노무현이 2002년 대선 당시 대한민국 정당 최초로 도입된 새천년민주당 국민참여경선에 당당히 출사표를 던진다.\n\n" +
" 제주를 시작으로 전국 16개 도시에서 치러진 대국민 이벤트. 쟁쟁한 후보들과 엎치락뒤치락하며 제주 경선 3위, 울산 1위, 그리고 광주까지 석권한 지지율 2%의 꼴찌 후보 노무현이 전국을 뒤흔들기 시작한다.\n\n";
            }
            else if (poster.Equals("pictureBox5"))
            {
                pictureBox1.Image = Properties.Resources.캐리비안;
                label1.Text = form1.label17.Text;
                label2.Text = "Pirates of the Caribbean: Dead Men Tell No Tales";
                label3.Text = "예매율 " + form1.label19.Text;
                label6.Text = "개봉일 " + form1.label18.Text;
                label4.Text = "감독 : 요아킴 뢰닝 ,  에스펜 잔드베르크  / 프로듀서 :  제리 브룩하이머  / 배우 : 조니 뎁, 하비에르 바르뎀 ,  브렌튼 스웨이츠, 카야 스코델라리오....";
                label5.Text = "장르 : 액션, 환타지, 어드벤처, 코미디 / 기본 : 12세 이상, 129분, 미국.오스트레일리아";
                label7.Text = "죽음마저 집어삼킨 복수가 시작된다!\n\n";
                label10.Text = "공식사이트 : " + "  www.disney.co.kr/movies/poc5/";
                label8.Text = "전설적인 해적 캡틴 ‘잭 스패로우’(조니 뎁)의 눈 앞에 죽음마저 집어삼킨 바다의 학살자 ‘살라자르’(하비에르 바르뎀)가 복수를 위해 찾아온다.\n\n" +
"둘 사이에 숨겨진 엄청난 비밀··· 잭은 자신과 동료들의 죽음에 맞서 살아남기 위해 사투를 시작하는데···\n\n";
            }
        }
    }
}
