어플만 실행해보고 싶다! 
>  .apk 다운로드 후 핸드폰에서 실행
<br/>

유니티로 실행 : 
```
1. 유니티에서 New 파일을 하나 생성
2. clone한 unity폴더의 Asset, Packages, ProjectSettings 폴더들을 덮어쓰기
3. Scene에서 Start Scene 더블클릭
4. 재생버튼을 눌러보면 해당 노트북에서 실행해볼 수 있음
```
<br/>

유니티에서 안드로이드로 빌드하려면
1. Edit > Perferences > External Tools 에서 SDK 세팅
2. File > Build Settings 에서 Android Build로
<br/>

안되면 Build Settings에서 Player Settings에
Other Settings에서 Package Name을 다른 거로 바꿔준다



+
서버 주소 바꾸는 방법
음성 : Canvas > Button의 inspector에 url에 주소 넣어주면 알아서 그 주소로 변경됨
사진 : Manager에 phonecamera script에 url 주소 변경

public으로 선언해 놓은 변수값은 inspector창에서 실시간으로 볼 수 있음

음성은 버튼을 누르고 있으면 녹음이 되고 떼면 녹음이 멈춤
녹음이 멈추면 .wav파일로 저장을 하고 파일 형태로 서버로 보내고 파일을 삭제함

사진은 저장하지 않고 byte[]로 서버로 보냄
사진은 자동으로 10초마다 보내게 설정을 해놨고 사진이 찍히고 보내질때마다 화면에 text가 뜨게 해놓음
manager > phonecamera script inspector에서 10으로 되어있는 time을 수정하면 보내는 시간 변경가능
