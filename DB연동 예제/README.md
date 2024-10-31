# DB관련 설치 파일
서버구성 파일
[Xampp](https://www.apachefriends.org/download.html)(Apache, Mysql, phpmyadmin 통합환경 제공)
ㅤ
  

[ngrok 터널링 도구](https://dashboard.ngrok.com/)  

[Windows 11 ngrok설치 및 인증방법](https://newstroyblog.tistory.com/578)     

ㅤ
  
ngrok 실행후 Forwarding에 나오는 주소를 php나 C#에서 로컬호스트(https://127.0.0.1:80) 주소대신 서버 url로 사용하여 외부에서도 DB연동 가능   

실행 방법 `ngrok http 80`

ㅤ
  

# 예제파일 사전안내
- 아래 설명에 나오는 php 코드들은 htdocs폴더 참고
- 아래 설명에 나오는 유니티 .cs파일 및 프로젝트는 game폴더를 유니티 - Add  - Add Project from disk로 불러와서 사용가능 (Unity2D 프로젝트로 만들어져있음)
- Scenes & c# cs파일 위치 game\Assets\Scenes
  
ㅤ
  

  
# Mysql 서버 세팅  
#### Apache, MySQL 실행
![2024-10-31 22 11 18](https://github.com/user-attachments/assets/76c9fcc8-accb-4eff-9268-4bbf590f1b2e)  

Xampp 실행후 Apache, MySQL 우측 Start버튼을 클릭하여 실행  
ㅤ
#### MySQL 비밀번호 설정
![2024-10-31 22 15 45](https://github.com/user-attachments/assets/29477430-543b-4cd0-b402-6d97758005d2)  

  
Apache Config - phpMyAdmin (config.in.php)클릭   
ㅤ

![2024-10-31 22 18 27](https://github.com/user-attachments/assets/bd5a0431-5655-4fcb-8341-6d8e0146915e)  

  
password 부분을 수정하여 비밀번호 설정 가능 (본 프로젝트에서는 '' 작은따옴표 로 설정되어있음)
ㅤ

  ㅤ

#### phpmyadmin 데이터베이스 생성
![2024-10-31 22 21 42](https://github.com/user-attachments/assets/a3fb7d56-6e41-488b-b0a2-b0f6d55ece1f)
-1. Xampp Control Panel에서 MySQL의 Admin을 클릭하여 phpMyAdmin패널 접속  

-2. 상단 데이터베이스 클릭 - 데이터베이스 명 입력 - 만들기 버튼 클릭   

ㅤ
  
#### 테이블 생성   
![2024-10-31 22 24 22](https://github.com/user-attachments/assets/d52ebbc2-6c85-4368-b2b0-8141686ecbd5)
두개의 테이블 생성 방법이 존재함
1. SQL탭에서 SQL질의로 테이블 생성가능  
2. 테이블 이름을 입력하여 자동으로로 테이블 생성 가능

ㅤ
  
#### 테이블 생성후 관리
![2024-10-31 22 28 36](https://github.com/user-attachments/assets/7e68941f-5d09-4abb-9523-792d2bbac668)
1.좌측에 데이터베이스를 클릭하여 2.테이블의 구조를 클릭

   
![2024-10-31 22 33 45](https://github.com/user-attachments/assets/b1c27608-debf-457f-a033-1077b31403d0)
이곳에서 GUI로 테이블 항목을 추가하거나 SQL탭에서 SQL질의로 관리할 수 있음 

ㅤ
  
# php작성하고 업로드하기 (아래 php파일들은 htdocs폴더 참고)
![2024-10-31 22 38 34](https://github.com/user-attachments/assets/459f4a52-bf6e-4ed2-a31e-bca0d668bb17)  

사진속 Explorer 클릭한뒤 htdocs 폴더에 들어가거나 `C:\xampp\htdocs` 경로에 직접 들어간뒤,   
html이나 php등의 파일을 올려두면 Apache로 웹서버처럼 동작함 

유니티에서 사용할때는 https://127.0.0.1/파일이름.php 형태로 연결가능 

ㅤ
  
## login.php (주석포함)
로그인 페이지에서 로그인버튼을 통하여 사용자 계정 검증시 사용   

유니티에서 이메일필드와 비밀번호 필드에서 입력받은 값을 POST로 넘겨받으면 이를 바탕으로 데티어베이스 테이블의 userid와 userpassword를 확인하여 로그인성공시 success반환 실패시 오류메세지 반환

ㅤ
  
## savedata.php ()
회원가입시 사용자의 정보를 데이터베이스에 저장할때 사용   

유니티에서 념겨받은 userid, userpassword, name, birth_date, phone_number를 users 테이블에 추가하여 데이터베이스에 저장한다

이때 prepared statement와 Parameter Binding으로 SQL인젝션 방지
사용자로부터 입력된 값을 SQL구문에 직접 포함하지않고 별도로 바인딩함  

중복된 userid가 삽입될경우 phpmyadmin에서 테이블내의 컬럼에서 제약조건을 걸어줌
아래처럼 SQL구문을 추가해도되고 Phpmyadmin에서 원하는 해당 컬럼을 선택하고 고유값을 통해 추가할 수 있음
```
ALTER TABLE users
ADD CONSTRAINT unique_userid UNIQUE (userid);
```

ㅤ
  
## getUserinfo.php
마이페이지에서 회원정보를 불러올때 사용   
userid로 사용자이름, 생년월일, 전화번호를 검색해서 결과가있으면 JSON으로 데이터를 반환함

ㅤ
  
# 유니티 코딩
모든 C#파일에서 유니티의 UnityWebRequest를 사용하여 서버에 Post 요청을 보낼수 있음 

ㅤ
  
## 'sing in' scenes   
![2024-10-31 22 59 58](https://github.com/user-attachments/assets/02025118-5e8d-4cd1-a9ba-f9bc72405674)
login_button 클릭시 loing.cs 코드 실행    
이때, 로그인 성공시 유니티의 PlayerPrefs함수를 사용해서 UserID는 임시로 사용자기기에 저장함 (마이페이지에서 정보를 불러올때 사용 )

register_button 클릭시 Change_ Scenes.cs 이벤트로 register Scenes로 전환됨 

ㅤ
  
## 'register' scenes
![2024-10-31 23 08 35](https://github.com/user-attachments/assets/31f31f66-1517-40bd-b38f-e8c7b4e0c813) register_button 클릭시 userDB.cs 코드 실행

ㅤ
  
## 'mypage' scenes
![2024-10-31 23 15 05](https://github.com/user-attachments/assets/0bf4e0ab-d8bf-4dcd-8069-f8776d5781be)
scenes 로딩시에 get_userinfo.cs에서 로그인시 저장해되었던 userID로 사용자 정보를 가져옴

ㅤ
  
# 참고자료
씬 전환방법  
https://www.youtube.com/watch?v=6RPNL5E3cnQ


#
유니티 한글폰트추가  
https://blockdmask.tistory.com/590   (font asset creator설정만 이거따라하기  

https://blaupowder.tistory.com/107
#
유니티 스크립트 인자값에 textmeshpro는 안들어갈때  
https://dani2344.tistory.com/99  

https://soopeach.tistory.com/13
#
  
DB요청 참고자료   
https://ggjjdiary.tistory.com/285

