# 실행방법 

###  virtualenv (가상환경) 실행

    source myenv/bin/activate  

![enter image description here](https://raw.githubusercontent.com/daeng325/KMU/master/%EC%8A%A4%ED%81%AC%EB%A6%B0%EC%83%B7,%202019-04-03%2014-32-17.png)

### Install Django and Django REST framework into the virtualenv

    pip install django
    pip install djangorestframework

 - 가상환경에 들어가면 다 이미 다운받아져 있는데 다시 다운받아야 하는 이유를 모르겠지만 안되니까 다운받는다.
 - 본인은 python3를 기본으로 세팅해뒀지만, 리눅스의 파이썬 기본 버전은 2.7이므로 미리 python 기본 버전을 3.x 으로 세팅해두길 바람. 아니면 **pip3** 명령어 사용
 - 그 외에 module import error 뜨면 그 module install 하면 될 것이다. 다른 에러 있으면 문의 바람.



    
### Now sync your database for the first time:

```
python manage.py makemigrations
python manage.py migrate
```

에러문 안 뜨고 잘 넘어가면 된다. 
![enter image description here](https://raw.githubusercontent.com/daeng325/KMU/master/%EC%8A%A4%ED%81%AC%EB%A6%B0%EC%83%B7,%202019-04-03%2014-39-48.png)

###  MySQL 연동

 - MySQLdb : 가장 오래돼서 안정되어 있지만 python 3 지원하지 않음.
 - Mysqlclient : 위에 것을 개선한 패키지. python 3.3 이상 버전 지원. 이거 다운로드!

이 짓을 하긴 했지만 다른 사람들도 똑같이 해야 하는 건지...왜인지...

    sudo apt-get install python-dev default-libmysqlclient-dev 
    pip install mysqlclient 
    sudo apt-get install mysql-server 
    systemctl status mysql.service


    
# 

mysite/setting.py 파일을 보면 

    DATABASES = {
      'default': {
        'ENGINE': 'django.db.backends.mysql',
        'OPTIONS': {
            'read_default_file': '/etc/mysql/my.cnf',
	      },
	   }
	}

이런 코드가 있을 것이다.
#

그리고 명령창에 아래와 같이 쳐준 후 my.cnf파일에 추가한다.

    sudo nano /etc/mysql/my.cnf

​

    ... 
    [client] 
    database = My_Mood_Music
    user = Fortune_Teller
    password = qwer1234 
    default-character-set = utf8



이제 마지막으로, 아래 두 개의 명령어를 쳐준다.

    systemctl daemon-reload 
    systemctl restart mysql

그럼 끝. mysql 프롬프트에서 확인해보자.
![enter image description here](https://raw.githubusercontent.com/daeng325/KMU/master/%EC%8A%A4%ED%81%AC%EB%A6%B0%EC%83%B7,%202019-04-04%2002-03-50.png)mysql -u root -p 에서 grant 를 fortune_teller에게 준 후의 모습이다.
show databases;

![enter image description here](https://raw.githubusercontent.com/daeng325/KMU/master/%EC%8A%A4%ED%81%AC%EB%A6%B0%EC%83%B7,%202019-04-04%2002-04-37.png)
아래 있는 my_mood_music_emotion & music 테이블이 본인이 작성한 테이블이다.
나중에 DB 설계자인 수빈 언니가 다시 수정할 예정이다.



### 서버에서 작업 확인

이제 mmm_project 폴더로 돌아간다. (manage.py가 있는 폴더)

    python manage.py runserver 

python3를 기본으로 세팅하지 않았다면, python3 를 사용한다. 

    python3 manage.py runserver 

![enter image description here](https://raw.githubusercontent.com/daeng325/KMU/master/%EC%8A%A4%ED%81%AC%EB%A6%B0%EC%83%B7,%202019-04-03%2014-40-39.png)

그 결과 오류가 안 뜬다면, url 하나가 나올 것이다. 그 곳으로 들어간다. 
127.0.0.1:8000/
![enter image description here](https://raw.githubusercontent.com/daeng325/KMU/master/%EC%8A%A4%ED%81%AC%EB%A6%B0%EC%83%B7,%202019-04-03%2014-40-56.png)

127.0.0.1:8000/admin/
- 테이블 생성된 것을 확인할 수 있다.
- 로그인 페이지에서 Username: fortune_teller / Password : mmm_project 로 로그인이 되지 않으면 따로 얘기해주길 바람.
![enter image description here](https://raw.githubusercontent.com/daeng325/KMU/master/%EC%8A%A4%ED%81%AC%EB%A6%B0%EC%83%B7,%202019-04-03%2014-41-06.png)


