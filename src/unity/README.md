 
### 유니티로 실행하는 법
```
1. 유니티 실행 후 New 파일을 하나 생성
2. clone한 unity폴더의 Asset, Packages, ProjectSettings 폴더들을 덮어쓰기
3. Scene에서 Start Scene 더블클릭
4. 재생버튼을 눌러보면 해당 노트북에서 실행해볼 수 있음
```
<br/>

### 유니티에서 안드로이드로 빌드하려면
```
1. Edit > Perferences > External Tools 에서 SDK, NDK 세팅
2. File > Build Settings 에서 Android Build를 하면 .apk 파일이 생성됨
```

빌드가 안되면 Build Settings에서 Player Settings에
Other Settings에서 Package Name을 다른 거로 바꿔준다
<br/>
<br/>
<br/>

### 추가 설명들

- public으로 선언해 놓은 변수값은 inspector창에서 실시간으로 볼 수 있음    
<br/>
    
- 음성은 버튼을 누르면 녹음이 되고 한 번 더 누르면 녹음이 멈춤.    
녹음이 멈추면 .wav파일로 저장을 하고 byte[] 형식으로 서버로 보내고 파일을 삭제함
<br/>

- 사진은 back에서 카메라를 켜서 사진을 찍고 저장하지 않고 byte[]로 서버로 보냄
<br/>

- 추천 결과값들은 서버에서 json 형태로 받아와서 UI 상에 뿌려줌
<br/>
