# Port-Folio
## 안녕하세요. 개발 포트폴리오 소스를 편리하게 보기 위해 안내 드립니다. 그리고 소스파일 별 약간의 설명을 나타내었습니다. 감사합니다.
### 1. Creating_Homepage 소스 위치
  + Shopping > WebContent
    + js file : 메인 홈 다이나믹 배너 소스
    + Shoping.html : 메인 홈 및 다이나믹 배너 노출
    + 태연.html : 메인 홈 > 태연 이미지 클릭 시, 상세 페이지 노출
    + Login/회원가입 창/마이페이지.html : 로그인/회원가입
    + Pants/skirt/top.html: 각 카테고리 별 페이지
    
### 2. Dungeon_Version2 소스 위치
  + Assets > Script
    + ArrowAct.cs: 필드 위치 기본설정
    + DataManager.cs: Dungeon_Data.xml 파일을 불러오기 위한 기본 과정
    + Field.cs: 필드에 사용한 카드 노출
    + GameManager.cs: 게임진행에 필요한 기능
    + Inven.cs: 각 플레이어마다 갖는 아이템/파워 카드 저장소 및 노출
    + Item.cs: 아이템 카드 불러오기 및 노출
    + LoadMap.cs: 지도 보여주기
    + LoadRoom.cs: 현재 무슨 방인지 노출
    + MouseEvent.cs: 카드와 마우스의 상호작용 이벤트 노출
    + Player.cs: 플레이어 기본설정 및 카드 사용 시, 효과 노출
    + Power.cs: 파워카드 기본설정
    + Select.cs: 특정카드 사용 시, 플레이어 선택관련 소스
    + Stage.cs: 각 방마다 xml 파일 내용 불러오기/노출 및 플레이어와 상호작용 소스
    + StageManager.cs: 방이 일정 수준의 확률로 노출되기 위한 소스
    + StarUI.cs: 게임시작 시, 인원수 선택
    + UIManager.cs: UI 관련 기본 설정
    + Assets > Resources
    + Dungeon_Data.xml: 게임에 필요한 몬스터, 아이템 카드, 현재 방 등 기본 사항 작성
    
### 3. Graduation_work 소스 위치
  + App> src> main> res>layout: UI 설정
  + App> src> main> java/com/example/user/useopencvwithcmake
    + MainActivity.java: 카메라 노출 기본 설정
    + CameraView.java: 카메라 노출 및 TeseractOCR 수행 소스
    
### 4. Movie_Reservation 소스 위치
  + Form1.cs: 메인 화면
  + Form2.cs: 영화 이미지 클릭 시, 상세화면
  + Form3.cs: 영화 예매 페이지
  + Form4.cs: 인원수 및 좌석 선택
  + Form5.cs: 로그인 화면
  + Form6.cs: 회원가입
  + Form7.cs: 영화예매 결제화면
  + Form8.cs: 간식구매

### 5. Niddangneddang 소스 위치
  + Assets > Script
    + Clicktile.cs: 타일 클릭 확인 및 색 변경
    + GameStart.cs: 게임 시작화면 불러오기
    + PaintAnim.cs: 적용하지 않은 스크립트
    + Quit.cs: 창 닫기
    + Result.cs: 게임결과 노출
    + Score.cs: 각 플레이어 점수 노출
    + Tileprefabs.cs: 타일 생성
    + TimeManager.cs: 남은 시간 노출
    + Timecolor.cs: 컴퓨터가 타일 선택 시, 색 변경

### 6. PyGame 소스 위치
  + 텀프로젝트.py: 화면/캐릭터/몬스터 기본 설정 및 게임엔진 포함
