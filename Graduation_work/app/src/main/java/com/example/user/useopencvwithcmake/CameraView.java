package com.example.user.useopencvwithcmake;

import android.app.Activity;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.Color;
import android.graphics.Matrix;
import android.hardware.SensorManager;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.support.annotation.RequiresApi;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.OrientationEventListener;
import android.view.SurfaceView;
import android.view.View;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import org.opencv.android.BaseLoaderCallback;
import org.opencv.android.CameraBridgeViewBase;
import org.opencv.android.LoaderCallbackInterface;
import org.opencv.android.OpenCVLoader;
import org.opencv.android.Utils;
import org.opencv.core.Mat;
import org.opencv.core.Rect;
import org.opencv.imgproc.Imgproc;

import java.util.LinkedList;
import java.util.Stack;
import java.util.StringTokenizer;

import static com.example.user.useopencvwithcmake.MainActivity.sTess;

/**
 * Created by USER on 2018-10-01.
 */

public class CameraView extends Activity implements CameraBridgeViewBase.CvCameraViewListener2{
    private Mat img_input; //전체 프레임 사이즈
    private static final String TAG = "opencv";
    private CameraBridgeViewBase mOpenCvCameraView;  //카메라 받아오는 변수
    private String m_strOcrResult ="";

    private Button mBtnOcrStart;  //계산식 이미지 촬영하는 버튼
    private Button mBtnFinish;  //뒤로가기 버튼
    private TextView mTextOcrResult; //화면에 계산식 및 결과 값 노출

    private Bitmap bmp_result; //캡쳐한 블록 계산식 이미지

    private OrientationEventListener mOrientEventListener;

    private Rect mRectRoi; //생성된 roi 영역

    private SurfaceView mSurfaceRoi;
    private SurfaceView mSurfaceRoiBorder;

    private int mRoiWidth; //전체 사이즈의 중심 x 좌표 값
    private int mRoiHeight; //전체 사이즈의 중심 y 좌표 값
    private int mRoiX; //roi 영역을 위한 시작 x 좌표 값
    private int mRoiY; //roi 영역을 위한 시작 y 좌표 값
    private double m_dWscale; //전체 사이즈의 중심 값 구하기 위한 변수
    private double m_dHscale; //전체 사이즈의 중심 값 구하기 위한 변수

    private View m_viewDeco;  //풀스크린 만들기 위한 변수
    private int m_nUIOption;
    private android.widget.RelativeLayout.LayoutParams mRelativeParams;
    private ImageView mImageCapture; //캡쳐한 블록 계산식 roi 영역에 노출
    private Mat m_matRoi; //mRectRoi의 값을 포함한 변수
    private boolean mStartFlag = false;

    // 현재 회전 상태 (하단 Home 버튼의 위치)
    private enum mOrientHomeButton {Right, Bottom, Left, Top}

    private mOrientHomeButton mCurrOrientHomeButton = mOrientHomeButton.Right;

    static final int PERMISSION_REQUEST_CODE = 1;
    String[] PERMISSIONS = {"android.permission.CAMERA"}; //카메라 접근 권한 확인을 위한 변수


    private boolean hasPermissions(String[] permissions) {  //카메라 접근 권한 확인
        // 퍼미션 확인
        int result = -1;
        for (int i = 0; i < permissions.length; i++) {
            result = ContextCompat.checkSelfPermission(getApplicationContext(), permissions[i]);
        }
        if (result == PackageManager.PERMISSION_GRANTED) { //카메라 접근 권한 허용
            return true;

        } else { //카메라 접근 권한 비허용 (onRequestPermissionsResult 실행)
            return false;
        }
    }

    private void requestNecessaryPermissions(String[] permissions) {  //카메라 접근 권한 한번도 없을 경우 실행
        ActivityCompat.requestPermissions(this, permissions, PERMISSION_REQUEST_CODE);
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, String permissions[], int[] grantResults) {
        switch (requestCode) {
            case PERMISSION_REQUEST_CODE: {
                //퍼미션을 거절했을 때 메시지 출력 후 종료
                if (!hasPermissions(PERMISSIONS)) {
                    Toast.makeText(getApplicationContext(), "CAMERA PERMISSION FAIL", Toast.LENGTH_LONG).show();
                    finish();
                }
                return;
            }
        }
    }

    private BaseLoaderCallback mLoaderCallback = new BaseLoaderCallback(this) {
        @Override
        public void onManagerConnected(int status) {
            switch (status) {
                case LoaderCallbackInterface.SUCCESS: {
                    // 퍼미션 확인 후 카메라 활성화
                    if (hasPermissions(PERMISSIONS))
                        mOpenCvCameraView.enableView();
                }
                break;
                default: {
                    super.onManagerConnected(status);
                }
                break;
            }
        }
    };

    @RequiresApi(api = Build.VERSION_CODES.CUPCAKE)
    @Override
    protected void onCreate(Bundle savedInstanceState) { //안드로이드 생명주기 onCreate > onStart > onResume > onPause > onStop > onDestroy
        super.onCreate(savedInstanceState);
        setContentView(R.layout.camera_view);

        getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);

        if (!hasPermissions(PERMISSIONS)) { //퍼미션 허가를 했었는지 여부를 확인
            requestNecessaryPermissions(PERMISSIONS);//퍼미션 허가안되어 있다면 사용자에게 요청
        } else {
            //이미 사용자에게 퍼미션 허가를 받음.
        }

        // 카메라 설정(0번 카메라를 사용하기 위한 준비)
        mOpenCvCameraView = (CameraBridgeViewBase) findViewById(R.id.activity_surface_view);
        mOpenCvCameraView.setVisibility(SurfaceView.VISIBLE);
        mOpenCvCameraView.setCvCameraViewListener(this);
        mOpenCvCameraView.setCameraIndex(0); // front-camera(1),  back-camera(0)
        mLoaderCallback.onManagerConnected(LoaderCallbackInterface.SUCCESS);


        //뷰 선언
        mBtnOcrStart = (Button) findViewById(R.id.btn_ocrstart);
        mBtnFinish = (Button) findViewById(R.id.btn_finish);

        mTextOcrResult = (TextView) findViewById(R.id.text_ocrresult);

        mSurfaceRoi = (SurfaceView) findViewById(R.id.surface_roi);
        mSurfaceRoiBorder = (SurfaceView) findViewById(R.id.surface_roi_border);

        mImageCapture = (ImageView) findViewById(R.id.image_capture);

        //풀스크린 상태 만들기 (상태바, 네비게이션바 없애고 고정시키기)
        m_viewDeco = getWindow().getDecorView(); //메인 액티비티 뷰 정보 구함
        m_nUIOption = getWindow().getDecorView().getSystemUiVisibility();
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.ICE_CREAM_SANDWICH) //아이스크림 샌드위치 버전
            m_nUIOption |= View.SYSTEM_UI_FLAG_HIDE_NAVIGATION;
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN) //젤리빈 버전
            m_nUIOption |= View.SYSTEM_UI_FLAG_FULLSCREEN;
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.KITKAT)  //킷캣 버전
            m_nUIOption |= View.SYSTEM_UI_FLAG_IMMERSIVE_STICKY;

        m_viewDeco.setSystemUiVisibility(m_nUIOption);

		//방향 센서 정상 인식 확인
        mOrientEventListener = new OrientationEventListener(this,
                SensorManager.SENSOR_DELAY_NORMAL) {

            @Override
            public void onOrientationChanged(int arg0) {

                //방향센서값에 따라 화면 요소들 회전

                // 0˚ (portrait)
                if (arg0 >= 315 || arg0 < 45) {
                    rotateViews(270);
                    mCurrOrientHomeButton = mOrientHomeButton.Bottom;
                    // 90˚
                } else if (arg0 >= 45 && arg0 < 135) {
                    rotateViews(180);
                    mCurrOrientHomeButton = mOrientHomeButton.Left;
                    // 180˚
                } else if (arg0 >= 135 && arg0 < 225) {
                    rotateViews(90);
                    mCurrOrientHomeButton = mOrientHomeButton.Top;
                    // 270˚ (landscape)
                } else {
                    rotateViews(0);
                    mCurrOrientHomeButton = mOrientHomeButton.Right;
                }


                //ROI 선 조정
                mRelativeParams = new android.widget.RelativeLayout.LayoutParams(mRoiWidth + 5, mRoiHeight + 5);
                mRelativeParams.setMargins(mRoiX, mRoiY, 0, 0);
                mSurfaceRoiBorder.setLayoutParams(mRelativeParams);

                //ROI 영역 조정
                mRelativeParams = new android.widget.RelativeLayout.LayoutParams(mRoiWidth - 5, mRoiHeight - 5);
                mRelativeParams.setMargins(mRoiX + 5, mRoiY + 5, 0, 0);
                mSurfaceRoi.setLayoutParams(mRelativeParams);

            }
        };

        //방향센서 핸들러 활성화
        mOrientEventListener.enable();

        //방향센서 인식 오류 시, Toast 메시지 출력 후 종료
        if (!mOrientEventListener.canDetectOrientation()) {
            Toast.makeText(this, "Can't Detect Orientation",
                    Toast.LENGTH_LONG).show();
            finish();
        }
    }

    public void onClickButton(View v) {

        switch (v.getId()) {

            //Start 버튼 클릭 시
            case R.id.btn_ocrstart:
                if (!mStartFlag) {
                    // 인식을 새로 시작하는 경우

                    // 버튼 속성 변경
                    mBtnOcrStart.setEnabled(false);
                    mBtnOcrStart.setText("Working...");
                    mBtnOcrStart.setTextColor(Color.LTGRAY);

					//roi 영역에 캡쳐된 이미지 비트맵에 저장
                    bmp_result = Bitmap.createBitmap(m_matRoi.cols(), m_matRoi.rows(), Bitmap.Config.ARGB_8888);

                    Utils.matToBitmap(m_matRoi, bmp_result);

                    // 캡쳐한 이미지를 ROI 영역 안에 표시
                    mImageCapture.setVisibility(View.VISIBLE);
                    mImageCapture.setImageBitmap(bmp_result);


                    //Orientation에 따라 Bitmap 회전 (landscape일 때는 미수행)
                    if (mCurrOrientHomeButton != mOrientHomeButton.Right) {
                        switch (mCurrOrientHomeButton) {
                            case Bottom:
                                bmp_result = GetRotatedBitmap(bmp_result, 90);
                                break;
                            case Left:
                                bmp_result = GetRotatedBitmap(bmp_result, 180);
                                break;
                            case Top:
                                bmp_result = GetRotatedBitmap(bmp_result, 270);
                                break;
                        }
                    }

                    new AsyncTess().execute(bmp_result);
                } else {
                    //Retry를 눌렀을 경우

                    // ImageView에서 사용한 캡쳐이미지 제거
                    mImageCapture.setImageBitmap(null);
                    mTextOcrResult.setText(R.string.ocr_result_tip); //계산식과 결과값 리셋

                    mBtnOcrStart.setEnabled(true);
                    mBtnOcrStart.setText("Start");
                    mBtnOcrStart.setTextColor(Color.WHITE);

                    mStartFlag = false;
                }

                break;


            // 뒤로 버튼 클릭 시
            case R.id.btn_finish:
                //인식 결과물을 MainActivity에 전달하고 종료
                Intent intent = getIntent();
                intent.putExtra("STRING_OCR_RESULT", m_strOcrResult);
                setResult(RESULT_OK, intent);
                mOpenCvCameraView.disableView();
                finish();
                break;
        }
    }

    public void rotateViews(int degree) {
        mBtnOcrStart.setRotation(degree);
        mBtnFinish.setRotation(degree);
        mTextOcrResult.setRotation(degree);

        switch (degree) {
            // 가로
            case 0:
            case 180:

                //ROI 크기 조정 비율 변경
                m_dWscale = (double) 1 / 2;
                m_dHscale = (double) 1 / 2;


                //결과 TextView 위치 조정
                mRelativeParams = new android.widget.RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WRAP_CONTENT, ViewGroup.LayoutParams.WRAP_CONTENT);
                mRelativeParams.setMargins(0, convertDpToPixel(20), 0, 0);
                mRelativeParams.addRule(RelativeLayout.CENTER_HORIZONTAL);
                mTextOcrResult.setLayoutParams(mRelativeParams);

                break;

            // 세로
            case 90:
            case 270:

                m_dWscale = (double) 1 / 4;    //h (반대)
                m_dHscale = (double) 3 / 4;    //w

                mRelativeParams = new android.widget.RelativeLayout.LayoutParams(convertDpToPixel(300), ViewGroup.LayoutParams.WRAP_CONTENT);
                mRelativeParams.setMargins(convertDpToPixel(15), 0, 0, 0);
                mRelativeParams.addRule(RelativeLayout.CENTER_VERTICAL);
                mTextOcrResult.setLayoutParams(mRelativeParams);


                break;
        }
    }

    //dp 단위로 입력하기 위한 변환 함수 (px 그대로 사용 시 기기마다 화면 크기가 다르기 때문에 다른 위치에 가버림)
    public int convertDpToPixel(float dp) {

        Resources resources = getApplicationContext().getResources();

        DisplayMetrics metrics = resources.getDisplayMetrics();

        float px = dp * (metrics.densityDpi / 160f);

        return (int) px;

    }

    public synchronized static Bitmap GetRotatedBitmap(Bitmap bitmap, int degrees) { //비트맵 이미지 방향센서에 따라 변동
        if (degrees != 0 && bitmap != null) {
            Matrix m = new Matrix();
            m.setRotate(degrees, (float) bitmap.getWidth() / 2, (float) bitmap.getHeight() / 2);  //roi 영역 중심 값 추출
            try {
                Bitmap b2 = Bitmap.createBitmap(bitmap, 0, 0, bitmap.getWidth(), bitmap.getHeight(), m, true);
                if (bitmap != b2) {
                    //bitmap.recycle(); (일반적으로는 하는게 옳으나, ImageView에 쓰이는 Bitmap은 recycle을 하면 오류가 발생함.)
                    bitmap = b2;
                }
            } catch (OutOfMemoryError ex) {
                // We have no memory to rotate. Return the original bitmap.
            }
        }

        return bitmap;
    }

    @Override
    public void onPause() {
        super.onPause();
        if (mOpenCvCameraView != null)
            mOpenCvCameraView.disableView();
    }

    @Override
    public void onResume() { //opencv 라이브러리 연결 여부관련 log 노출
        super.onResume();

        if (!OpenCVLoader.initDebug()) {
            Log.d(TAG, "onResume :: Internal OpenCV library not found.");
            OpenCVLoader.initAsync(OpenCVLoader.OPENCV_VERSION_3_2_0, this, mLoaderCallback);
        } else {
            Log.d(TAG, "onResume :: OpenCV library found inside package. Using it!");
            mLoaderCallback.onManagerConnected(LoaderCallbackInterface.SUCCESS);
        }
    }

    public void onDestroy() {
        super.onDestroy();

        if (mOpenCvCameraView != null)
            mOpenCvCameraView.disableView();
    }

    @Override
    public void onCameraViewStarted(int width, int height) {

    }

    @Override
    public void onCameraViewStopped() {

    }

    @Override
    public Mat onCameraFrame(CameraBridgeViewBase.CvCameraViewFrame inputFrame) { //CAMERA 화면 프레임

        // 카메라 화면 전체 가로, 세로 사이즈
        img_input = inputFrame.rgba();


        // 전체 사이즈에서 중심  x,y 좌표값 계산
        mRoiWidth = (int) (img_input.size().width * m_dWscale);
        mRoiHeight = (int) (img_input.size().height * m_dHscale);


        // 중심 x,y 좌표값에서 1/2로 해서 roi 영역 그리기 위한 좌표값 획득
        mRoiX = (int) (img_input.size().width - mRoiWidth) / 2;
        mRoiY = (int) (img_input.size().height - mRoiHeight) / 2;

        // ROI 영역 생성
        mRectRoi = new Rect(mRoiX, mRoiY, mRoiWidth, mRoiHeight);


        // ROI 영역 흑백으로 전환
        m_matRoi = img_input.submat(mRectRoi);
        Imgproc.cvtColor(m_matRoi, m_matRoi, Imgproc.COLOR_RGBA2GRAY);
        Imgproc.cvtColor(m_matRoi, m_matRoi, Imgproc.COLOR_GRAY2RGBA);
        m_matRoi.copyTo(img_input.submat(mRectRoi));
        return img_input;
    }


    private class AsyncTess extends AsyncTask<Bitmap, Integer, String> {

        @Override
        protected String doInBackground(Bitmap... mRelativeParams) {
            //Tesseract OCR 수행
            sTess.setImage(bmp_result);

            return sTess.getUTF8Text();
        }

        protected void onPostExecute(String result) {
            //완료 후 버튼 속성 변경 및 결과 출력

            mBtnOcrStart.setEnabled(true);
            mBtnOcrStart.setText("Retry");
            mBtnOcrStart.setTextColor(Color.WHITE);

            mStartFlag = true;

            m_strOcrResult = result;
            mTextOcrResult.setText(m_strOcrResult+"="+Calc(m_strOcrResult));


        }
    }

    private String Calc(String str){
		//계산식에 괄호'(' 가 존재할 경우
        if(str.indexOf('(') != -1){
            int fs = str.indexOf('(');  //'('의 인덱스를 fs에 저장
            int ls = str.lastIndexOf(')');  //')' 의 인덱스를 ls 에 저장
			//fs 와 ls 를 이용하여, 괄호 안의 substring을 재귀 호출
            String s = Calc(str.substring(fs + 1, ls));
			//str은 s(괄호 안의 계산결과)로 대체하여, 갱신
            str = str.substring(0, fs) + s + str.substring(ls+1, str.length());
        }

        int cnt = 0;  //계산 결과를 저장할 변수
        Stack<Integer> Stk_Num = new Stack <Integer>();  //연산자를 제외한 정수를  저장할 스택 객체 생성
        StringTokenizer ST_Num = new StringTokenizer(str,"+-/* ");  //계산식을 연산자 기준으로 분리
        StringTokenizer ST_Oper = new StringTokenizer(str,"1234567890 ");  //계산식을 정수 기준으로 분리, 연산자가 저장

        Stk_Num.push(Integer.parseInt(ST_Num.nextToken()));  //첫번째 정수 값을 스택에 저장
        while(ST_Num.hasMoreTokens()){  //정수값이 계속 존재할 경우
            char oper = ST_Oper.nextToken().charAt(0); //연산자를 oper 변수에 저장
            String num = ST_Num.nextToken();  //다음 정수값을 스택에 저장
            int a;

            if(oper == '*'){  //연산자가 '*'일 경우
                a = Stk_Num.pop();  //스택에 저장된 값을 꺼내
                a *= Integer.parseInt(num);  //다음 정수값을 곱한 뒤로
                Stk_Num.push(a);  //곱셈 결과를 다시 스택에 저장
            }
            else if(oper == '/'){  //연산자가 '/'일 경우
                a = Stk_Num.pop();  //스택에서 저장된 값을 꺼내
                a /= Integer.parseInt(num);  //다음 정수값으로 나눈 뒤
                Stk_Num.push(a); //나눗셈 결과를 다시 스택에 저장
            }
            else if(oper == '+'){  //연산자가 '+'일 경우
                Stk_Num.push(Integer.parseInt(num));  //스택에 다음 정수값을 저장
            }
            else if(oper == '-'){  //연산자가 '-'일 경우
                Stk_Num.push(-1 * (Integer.parseInt(num)));  //스택에 다음 정수값 *(-1) 결과를 저장
            }
        }

        while(!Stk_Num.isEmpty()){  //스택에 값이 존재하지 않을 때까지
            cnt += Stk_Num.pop();  //정수값을 역순으로 꺼내, cnt 변수에 저장
        }
		 
        return Integer.toString(cnt);  //계산 결과를 String 값으로 변환하여 반환
    }


}
