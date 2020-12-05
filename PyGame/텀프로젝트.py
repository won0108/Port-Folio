import pygame
import random

puple=(163,73,164)
black = [0,0,0]
white = [255,255,255]
back_color = [255,255,153]
red=(255,0,0)
brown=(128,64,0)
green=(0,255,0)
bright_green=(0,255,128)
bright_red=(255,128,128)

# 장애물을 정의하는 클래스
# Sprite 부모 클래스를 상속
class GraphicWall(pygame.sprite.Sprite):
    # 이미지 추출 여러 개 복사 함수, 타일 위치/크기
    # 스크린 위치/크기 인수로 전달
    def setGraphic(self,tilex,tiley,tilewidth,tileheight,x,y,width,height):
        # 이미지 적재
        myimage = pygame.image.load("terrain_atlas.png").convert()
        # 스크린에 표시될 이미지 Surface 생성
        self.image = pygame.Surface([width, height])

        # 타일 크기 배수로 복사
        for row in range(height//tileheight):
            for column in range(width//tilewidth):
                self.image.blit(myimage,[column*tilewidth,row*tileheight],
                                [tilex,tiley,tilewidth,tileheight])
        # 이미지 크기의 rect 객체 지정
        self.rect = self.image.get_rect()
        # 장애물의 스크린 위치 지정
        self.rect.y = y
        self.rect.x = x
        # 투명키 설정
        self.image.set_colorkey(black)

    # 중간 부분이 확장된 타일 이미지 추출, 타일 위치 스크린 위  치 크기 인수로 전달
    def setGraphic2(self,tilex,tiley,x,y,width,height):
        # 이미지 적재
        myimage = pygame.image.load("terrain_atlas.png").convert()
        # 스크린에 표시될 이미지 Surface 생성
        self.image = pygame.Surface([width, height])

        ## 1행 타일
        # 1행 1열 타일
        self.image.blit(myimage,[0,0],[tilex,tiley,32,32])
        # 1행 중간열 타일
        for column in range(width//32-2):
            self.image.blit(myimage,[(column+1)*32,0],
                            [tilex+32,tiley,32,32])
        # 1행 마지막열 타일
        self.image.blit(myimage,[(width//32-1)*32,0],
                        [tilex+64,tiley,32,32])
        ## 중간행 타일
        for row in range(height//32-2):
            self.image.blit(myimage,[0,(row+1)*32],
                            [tilex,tiley+32,32,32])
            # row 열의 중간행 타일
            for column in range(width//32-2):
                self.image.blit(myimage, [(column+1)*32,(row+1)*32],
                                [tilex+32,tiley+32,32,32])
            # row 열의 마지막 행 타일
            self.image.blit(myimage,[(width//32-1)*32,(row+1)*32],
                            [tilex+64,tiley+32,32,32])
        ## 마지막행 타일
        # 마지막행 1열 타일
        self.image.blit(myimage,[0,(height//32-1)*32],
                        [tilex,tiley+64,32,32])
        # 마지막행 중간열 타일
        for column in range(width//32-2):
            self.image.blit(myimage,[(column+1)*32,(height//32-1)*32],
                            [tilex+32,tiley+64,32,32])
        # 마지막행 마지막열 타일
        self.image.blit(myimage,[(width//32-1)*32,
                                 (height//32-1)*32],[tilex+64,tiley+64,32,32])
        # 이미지 크기의 rect 객체 지정
        self.rect = self.image.get_rect()

        # 장애물의 스크린 위치 지정
        self.rect.y = y
        self.rect.x = x
        # 투명키 설정
        self.image.set_colorkey(black)
def setGraphic4(tilex, tiley, tilewidth, tileheight,x,y):
	atlas_image=pygame.image.load("terrain_atlas.png").convert()
	atlas_image.set_colorkey(black)
	screen.blit(atlas_image, [x,y], [tilex,tiley, tilewidth, tileheight])

#적 파리를 정의하는 클래스
#Sprie 부모 클래스를 상속
class Plat(pygame.sprite.Sprite):
    def __init__(self, filename):
        #부모 클래스 생성자 호출
        pygame.sprite.Sprite.__init__(self)
        #파리의 이미지를 업로드
        self.image=pygame.image.load(filename).convert()
        self.image.set_colorkey(white)
        #이미지 크기의 rect 객체 지정
        self.rect=self.image.get_rect()
    
#적 여드름을 정의하는 클래스
#Sprie 부모 클래스를 상속
class Monster(pygame.sprite.Sprite):
    def __init__(self,filename):
        #부모 클래스 생성자 호출
        pygame.sprite.Sprite.__init__(self)
        #여드름의 이미지를 업로드
        self.image=pygame.image.load(filename).convert()
        self.image.set_colorkey(white)
        #이미지 크기의 rect 객체 지정
        self.rect=self.image.get_rect()
    #y좌표 값 리셋
    def reset_pos(self):
        self.rect.y=random.randrange(-100,-10)
        self.rect.x=random.randrange(0,screen_width)
    def update(self,change_y):
        #블록을 아래로 이동
        self.rect.y+=change_y
        if self.rect.y>screen_height:
            self.reset_pos()

#총알을 정의하는 클래스
#Sprie 부모 클래스를 상속
class Gun(pygame.sprite.Sprite):
    def __init__(self,filename):
        #부모 클래스 생성자 호출
        pygame.sprite.Sprite.__init__(self)
        #총알의 이미지를 업로드
        self.image=pygame.image.load(filename).convert()
        self.image.set_colorkey(white)
        #이미지 크기의 rect 객체 지정
        self.rect=self.image.get_rect()
    def update(self):
        #총알을 x축으로 10만큼 이동
        self.rect.x+=15


# 중간 부분이 확장된 타일 이미지 추출, 타일 위치 스크린 위  치 크기 인수로 전달
def setGraphic3(tilex,tiley,x,y,width,height):
    # 이미지 적재
    myimage = pygame.image.load("terrain_atlas.png").convert()
    # 스크린에 표시될 이미지 Surface 생성
    image = pygame.Surface([width, height])
    image.set_colorkey(black)
    ## 1행 타일
    # 1행 1열 타일
    image.blit(myimage,[0,0],[tilex,tiley,32,32])
    # 1행 중간열 타일
    for column in range(width//32-2):
         image.blit(myimage,[(column+1)*32,0],
                        [tilex+32,tiley,32,32])
    # 1행 마지막열 타일
    image.blit(myimage,[(width//32-1)*32,0],
                     [tilex+64,tiley,32,32])
    ## 중간행 타일
    for row in range(height//32-2):
        image.blit(myimage,[0,(row+1)*32],
                        [tilex,tiley+32,32,32])
        # row 열의 중간행 타일
        for column in range(width//32-2):
            image.blit(myimage, [(column+1)*32,(row+1)*32],
                            [tilex+32,tiley+32,32,32])
        # row 열의 마지막 행 타일
        image.blit(myimage,[(width//32-1)*32,(row+1)*32],
                        [tilex+64,tiley+32,32,32])
    ## 마지막행 타일
    # 마지막행 1열 타일
    image.blit(myimage,[0,(height//32-1)*32],
                    [tilex,tiley+64,32,32])
    # 마지막행 중간열 타일
    for column in range(width//32-2):
        image.blit(myimage,[(column+1)*32,(height//32-1)*32],
                        [tilex+32,tiley+64,32,32])
    # 마지막행 마지막열 타일
    image.blit(myimage,[(width//32-1)*32,
                                (height//32-1)*32],[tilex+64,tiley+64,32,32])
    screen.blit(image,[x,y])

#잔디 클래스
# GraphicWall 부모 클래스를 상속
class Grass1(GraphicWall):
    def __init__(self,x,y):
        #부모 클래스 생성자 호출
        pygame.sprite.Sprite.__init__(self)
        width=32
        height=32*10
        tilewidth=32
        tileheight=32
        #타일 이미지 위치
        tilex=32*5
        tiley=32*25
        #중간 확장 이미지 추출
        self.setGraphic(tilex,tiley,tilewidth,tileheight,x,y,width,height)

#잔디 클래스2
# GraphicWall 부모 클래스를 상속
class Grass2(GraphicWall):
    def __init__(self,x,y):
        #부모 클래스 생성자 호출
        pygame.sprite.Sprite.__init__(self)
        width=32*10
        height=32
        tilewidth=32
        tileheight=32
        #타일 이미지 위치
        tilex=32*5
        tiley=32*25
        #중간 확장 이미지 추출
        self.setGraphic(tilex,tiley,tilewidth,tileheight,x,y,width,height)

#불 구덩이 클래스
# GraphicWall 부모 클래스를 상속
class FireWall(GraphicWall):
    #생성자, 스크린 위치/크기 인수로 전달
    def __init__(self,x,y,width,height):
        #부모 클래스 생성자 호출
        pygame.sprite.Sprite.__init__(self)
        #타일 이미지 위치
        tilex=32*15
        tiley=32*8
        #중간 확장 이미지 추출
        self.setGraphic2(tilex,tiley,x,y,width,height)

#구덩이 클래스
# GraphicWall 부모 클래스를 상속
class PlotWall(GraphicWall):
    #생성자, 스크린 위치/크기 인수로 전달
    def __init__(self,x,y,width,height):
        #부모 클래스 생성자 호출
        pygame.sprite.Sprite.__init__(self)
        #타일 이미지 위치
        tilex=32*12
        tiley=32*8
        #중간 확장 이미지 추출
        self.setGraphic2(tilex,tiley,x,y,width,height)


# 플레이어 정의하는 클래스
# Sprite 부모 클래스를 상속
class Player(pygame.sprite.Sprite):
    # 클래스 속성
    # 플레이어 이동속도 벡터 값 초기화, 왼쪽(음수) 오른쪽(양수) 방향
    change_x=0
    change_y=0
    # 플레이어 이미지 선택을 위한 프레임 카운터
    # 4개 프레임 마다 이미지 선택
    frame = 0
    frame1 = 0
    # 생성자
    def __init__(self):
        # 부모 클래스 생성자 호출
        pygame.sprite.Sprite.__init__(self)
        # 플레이어 이미지 리스트 초기화
        self.images=[]
        self.images1=[]
        # cat1.png ~ cat8.png 플레이어 이미지 적재
        for i in range(1,9):
            img = pygame.image.load("main"+str(i)+".png").convert()
            img.set_colorkey(white)
            self.images.append(img)
        # 디폴트로 플레이어 images[0] 사용
        self.image = self.images[0]
        # 이미지 크기의 rect 객체 지정
        self.rect = self.image.get_rect()

        for i in range(1,9):
            img1 = pygame.image.load("updown"+str(i)+".png").convert()
            img1.set_colorkey(white)
            self.images1.append(img1)
        # 디폴트로 플레이어 images[0] 사용
        self.image = self.images1[0]
        # 이미지 크기의 rect 객체 지정
        self.rect = self.image.get_rect()

    # 이동속도 벡터 값 설정
    def changespeed(self,x,y):
        self.change_x+=x
        self.change_y+=y

    # 플레이어 위치와 이미지 갱신
    def update(self, walls):
        # x 축 새 위치 설정, 충돌 시 복구를 위해 이전 위치 저장
        old_x=self.rect.x
        new_x=old_x+self.change_x
        self.rect.x = new_x
        # x 방향 이동 시 충돌 여부 감지
        collide = pygame.sprite.spritecollide(self, walls, False)
        if collide:
            # 충돌인 경우이 전 위치 복구
            self.rect.x=old_x
        #y 축 새 위치 설정, 충돌 시 복구를 위해 이전 위치 저장
        old_y=self.rect.y
        new_y=old_y+self.change_y
        self.rect.y = new_y
        # y 방향 이동 시 충돌 여부 감지
        collide = pygame.sprite.spritecollide(self, walls, False)
        if collide:
            # 충돌인 경우 이전 위치 복구
            self.rect.y=old_y
                
        # 왼쪽 방향 이동 중인 경우
        if self.change_x < 0:
            # 프레임 카운터 증가
            self.frame += 1
            # 4 프레임 간격으로 플레이어 이미지
            #(images[0]~images[3]) 변경
            # image[3] 이 후의 경우(프레임이 12(3*4)보다 크면)
            # 프레임 리셋
            if self.frame > 3*4:
                self.frame = 0
            # 매 4 프레임 간격으로 플레이어 이미지 추출
            # 프레임 0...3 -> image[0]#
            #프레임 4...7 -> image[1]
            # 프레임 8...11 -> image[2]
            # 프레임 12 -> image[3]
            self.image= self.images[self.frame//4]

        # 오른쪽 방향 이동 중인 경우
        if self.change_x > 0:
            # 프레임 카운터 증가
            self.frame += 1
            # 4 프레임 간격으로 플레이어 이미지
            # (images[4]~images[7]) 변경
            # image[7] 이 후의 경우(프레임이 12(3*4)보다 크면)
            # 프레임 리셋
            if self.frame > 3*4:
                self.frame = 0
            # 매 4 프레임 간격으로 플레이어 이미지 추출
            # 프레임 0...3 -> image[4]
            # 프레임 4...7 -> image[5]
            # 프레임 8...11 -> image[6]
            # 프레임 12 -> image[7]
            self.image = self.images[self.frame//4+4]

        # 왼쪽 방향 이동 중인 경우
        if self.change_y < 0:
            # 프레임 카운터 증가
            self.frame1 += 1
            # 4 프레임 간격으로 플레이어 이미지
            #(images[0]~images[3]) 변경
            # image[3] 이 후의 경우(프레임이 12(3*4)보다 크면)
            # 프레임 리셋
            if self.frame1 > 3*4:
                self.frame1 = 0
            # 매 4 프레임 간격으로 플레이어 이미지 추출
            # 프레임 0...3 -> image[0]#
            #프레임 4...7 -> image[1]
            # 프레임 8...11 -> image[2]
            # 프레임 12 -> image[3]
            self.image= self.images1[self.frame1//4]

        # 오른쪽 방향 이동 중인 경우
        if self.change_y > 0:
            # 프레임 카운터 증가
            self.frame1 += 1
            # 4 프레임 간격으로 플레이어 이미지
            # (images[4]~images[7]) 변경
            # image[7] 이 후의 경우(프레임이 12(3*4)보다 크면)
            # 프레임 리셋
            if self.frame1 > 3*4:
                self.frame1 = 0
            # 매 4 프레임 간격으로 플레이어 이미지 추출
            # 프레임 0...3 -> image[4]
            # 프레임 4...7 -> image[5]
            # 프레임 8...11 -> image[6]
            # 프레임 12 -> image[7]
            self.image = self.images1[self.frame1//4+4]


# 배경 장면1
def setupRoomOne():
    # 스프라이트 그룹 생성
    wall_list=pygame.sprite.Group()
    wall=Grass1(0,0)
    wall_list.add(wall)
    wall=Grass1(0,288)
    wall_list.add(wall)
    wall=Grass1(768,32)
    wall_list.add(wall)
    wall=Grass1(768,320)
    wall_list.add(wall)
    wall=Grass2(0,0)
    wall_list.add(wall)
    wall=Grass2(320,0)
    wall_list.add(wall)
    wall=Grass2(480,0)
    wall_list.add(wall)
    wall=Grass2(0,576)
    wall_list.add(wall)
    wall=Grass2(320,576)
    wall_list.add(wall)
    wall=Grass2(480,576)
    wall_list.add(wall)
    wall=FireWall(565,30,224,160)
    wall_list.add(wall)
    wall=PlotWall(100,400,192,160)
    wall_list.add(wall)
    
    # 스프라이트 그룹 리턴
    return wall_list

# 파이게임 시작 (초기화)
pygame.init()

# 800x600 스크린 윈도우 설정
screen_width=800
screen_height=600
screen = pygame.display.set_mode([screen_width,screen_height])
pygame.display.set_caption("몬스터 죽이기")

gun_sound=pygame.mixer.Sound("gun.wav")
plat_sound=pygame.mixer.Sound("plat.wav")
monster_sound=pygame.mixer.Sound("monster.wav")

# 플레이어 플레이어 생성
player = Player( )
# 플레이어 초기 위치 설정
player.rect.x=70
player.rect.y=210
# 스프라이트 그룹 생성 후 플레이어 추가
movingsprites = pygame.sprite.Group()
movingsprites.add(player)

#스프라이트 그룹 생성
monster_list=pygame.sprite.Group()
all_sprites_list=pygame.sprite.Group()
plat_list=pygame.sprite.Group()
bullet_list=pygame.sprite.Group()


for count in range(3):
    #파리 블록 생성
    plat=Plat("plat.png")
    #파리들의 위치를 랜덤 생성
    plat.rect.x=random.randrange(700)
    plat.rect.y=random.randrange(500)
    #스프라이트 그룹에 파리 객체 추가
    plat_list.add(plat)
    all_sprites_list.add(plat)
for count in range(8):
    #여드름 블록 생성
    monster=Monster("mon.png")
    #여드름의 위치를 랜덤 생성
    monster.rect.x=random.randrange(screen_width)
    monster.rect.y=random.randrange(screen_height)
    #스프라이트 그룹에 여드름 객체 추가
    monster_list.add(monster)
    

def text_objects(text, font):
    textSurface = font.render(text, True, white)
    return textSurface, textSurface.get_rect()

#나가기
def quitgame():
    pygame.quit()
    quit()
#버튼
def button(msg,x,y,w,h,ic,ac,action=None):
    
        mouse = pygame.mouse.get_pos()
        click = pygame.mouse.get_pressed()
        #print(click)
        if x+w > mouse[0] > x and y+h > mouse[1] > y:
            pygame.draw.rect(screen, ac,(x,y,w,h))
            if click[0]==1 and action != None:
                action()
                
        else:
            pygame.draw.rect(screen, ic,(x,y,w,h))
        smallText = pygame.font.Font('nanumgothic.ttf',20)
        TextSurf, TextRect = text_objects(msg, smallText)
        TextRect.center = ( (x+(w/2)), (y+(h/2)) )
        screen.blit(TextSurf, TextRect)
#중지
def unpause():
    global pause
    pause = False
    
def paused():
        pygame.mixer.Sound.play(sound1)
        pygame.mixer.music.stop()
        pygame.mouse.set_visible(1)
        largeText = pygame.font.Font('nanumgothic.ttf',115)
        TextSurf, TextRect = text_objects("정지", largeText)
        TextRect.center = ((screen_width/2),(screen_height/2))
        screen.blit(TextSurf, TextRect)

        while pause:
            for event in pygame.event.get():

                if event.type == pygame.QUIT:
                    pygame.quit()   
                    quit()
                
            button("계속",110,300,100,50,green,bright_green,unpause)
            button("나가기",430,300,100,50,red,bright_red,quitgame)


            pygame.display.update()
            clock.tick(15)


#게임 시작화면
def game_intro():

    pygame.mixer.music.load('tetris-gameboy-03.wav')
    pygame.mixer.music.play(-1)
    
    intro = True

    pygame.mouse.set_visible(1)

    while intro:
        for event in pygame.event.get():
            #print(event)
            if event.type == pygame.QUIT:
                pygame.quit()
                quit()
                
        screen.fill(black)


        largeText = pygame.font.Font('nanumgothic.ttf',50)
        TextSurf, TextRect = text_objects("몬스터 죽이기", largeText)
        TextRect.center = ((screen_width/2),(screen_height/2))#글자를 가운데에
        screen.blit(TextSurf, TextRect)
        button("시작",190,400,100,50,green,bright_green,game_start)#시작화면 메세지
        button("나가기",500,400,100,50,red,bright_red,quitgame)#시작화면 메세지


        pygame.display.update()
        clock.tick(15)


# 초기 배경 장면1 설정
current_room = 1
wall_list = setupRoomOne()

font=pygame.font.Font("nanumgothic.ttf",25)
score=0
level=1
monsterspeed=3

clock = pygame.time.Clock()
def game_start():
    global score
    global level
    global monster

    pygame.mixer.music.load('tetris-gameboy-05.wav')
    pygame.mixer.music.play(-1)

    you_win=False
    done = False

    while  done == False:
        for  event in pygame.event.get():
            if  event.type == pygame.QUIT:
                done=True
            # 키보드 누르면 이동속도 벡터 값 설정
            if event.type == pygame.KEYDOWN:
                # 왼쪽 화살표 키보드이면 이동속도 벡터change_x = -5
                if event.key == pygame.K_LEFT:
                    player.changespeed(-10,0)
                # 오른쪽 화살표 키보드이면 이동속도 벡터 change_x = 5
                if event.key == pygame.K_RIGHT:
                    player.changespeed(10,0)
                # 위쪽 화살표 키보드이면 이동속도 벡터 change_y = -5
                if event.key == pygame.K_UP:
                    player.changespeed(0,-10)
                # 아래쪽 화살표 키보드이면 이동속도 벡터 change_y = 5
                if event.key == pygame.K_DOWN:
                    player.changespeed(0,10)
                if event.key == pygame.K_SPACE:
                    bullet=Gun("mi.png")
                    gun_sound.play()
                    bullet.rect.x=player.rect.x
                    bullet.rect.y=player.rect.y
                    all_sprites_list.add(bullet)
                    bullet_list.add(bullet)
            # 키보드 떼면 이동속도 벡터 값 리셋
            #이전 설정 값과 더해서 change_x = change_y = 0
            if event.type == pygame.KEYUP:
                if event.key == pygame.K_LEFT:
                    player.changespeed(10,0)
                if event.key == pygame.K_RIGHT:
                    player.changespeed(-10,0)
                if event.key == pygame.K_UP:
                    player.changespeed(0,10)
                if event.key == pygame.K_DOWN:
                    player.changespeed(0,-10)

        #게임에서 이기지 않았다면
        if not you_win:
            #monster의 y축 움직임의 스피드를 3으로한다.
            monster_list.update(3)
            #모든 스프라이트의 update메소드를 호출한다.
            all_sprites_list.update()

            #각각의 총알을 추산함
            for bullet in bullet_list:
                #plat과 부딪히면
                plat_hit_list=pygame.sprite.spritecollide(bullet, plat_list, True)

                #각각의 plat의 충돌에 대해서, 총알과 plat이 사라짐
                for plat in plat_hit_list:
                    #plat과 총알이 충돌하였을 때 소리 재생
                    plat_sound.play()
                    bullet_list.remove(bullet)
                    all_sprites_list.remove(bullet)
                #총알이 x축의 810을 넘어갔다면 사라짐
                if bullet.rect.x>810:
                    bullet_list.remove(bullet)
                    all_sprites_list.remove(bullet)

                #충돌된 plat수 조사하여 점수를 조정함
                if len(plat_hit_list)>0:
                    score+=len(plat_hit_list)
                #모든 plat이 수집됬으면 레벨이 증가됨
                if len(plat_list)==0:
                    level+=1
                    #레벨에 따라 새로운 plat을 생성함
                    for count in range(level*3):
                        plat=Plat("plat.png")
                        plat.rect.x=random.randrange(screen_width)
                        plat.rect.y=random.randrange(screen_height)
                        plat_list.add(plat)
                        all_sprites_list.add(plat)
                
            #각각의 총알을 추산함
            for bullet in bullet_list:
                #monster과 부딪히면
                monster_hit_list=pygame.sprite.spritecollide(bullet,monster_list,True)

                #각각의 moster의 충돌에 대해서, 총알과 monster이 사라짐
                for monster in monster_hit_list:
                    #monster과 총알이 충돌하였을 때 소리 재생
                    monster_sound.play()
                    bullet_list.remove(bullet)
                    all_sprites_list.remove(bullet)
                #충돌된 monster수 조사하여 점수를 조정함
                if len(monster_hit_list)>0:
                    score-=len(monster_hit_list)

        #이기면 이긴 소리를 재생하고 문구를 적재함    
        if you_win:
            pygame.mixer.music.load('win.wav')
            pygame.mixer.music.play(-1)
            text=font.render("You Win!",10,white)
            screen.blit(text,[400,200])

        #점수가 30을 넘을 시에 게임에서 이김
        if score>30:
            you_win=True

        # 플레이어 위치와 이미지 갱신
        player.update(wall_list)
        # 스크린 클리어
        screen.fill(back_color)
        setGraphic3(32*18, 32*2, 0,0, 800, 610)
        #잔디 이미지 그리기
        setGraphic3(32*0,32*22, 130,50,96,96)
        setGraphic3(32*0,32*22, 150,0,96,96)
        setGraphic3(32*0,32*22, 0,0,192,192)
        #나무1 이미지 그리기
        setGraphic4(32*29,32*28, 96,128, 30, 30)
        #버섯 그리기
        setGraphic4(32*26,32*31,32,32, 130,50)
        setGraphic4(32*26,32*31,32,32, 140,130)
        setGraphic4(32*26,32*31,32,32, 145,90)
        #큰 잔디
        setGraphic4(32*27,32*12,96,96, 650,480)
        setGraphic4(32*27,32*12,96,96, 550,480)
        #캐릭터 시작위치 잔디 이미지 그리기
        setGraphic4(32*21,32*2, 96,96, 50,250)
        #돌 이미지 그리기
        setGraphic4(32*27,32*26, 64,64,580,300)
        # 배경 장면 그리기
        wall_list.draw(screen)
        all_sprites_list.draw(screen)
        monster_list.draw(screen)
        plat_list.update(level)
        # 플레이어 플레이어 스프라이트 그리기
        movingsprites.draw(screen)
        text=font.render("점수: "+str(score), True, white)
        screen.blit(text,[710,10])
        text=font.render("레벨: "+str(level), True, white)
        screen.blit(text,[710,40])

        # 스크린 갱신하여 디스플레이
        pygame.display.flip()
        # 40 프레임/초
        clock.tick(20)

game_intro()
game_start() 
pygame.quit()
quit()
