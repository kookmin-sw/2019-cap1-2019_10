# Recommendation Algorithm (추천 알고리즘) - 설계와 구현 
 
#### - What & Why?
  추천 알고리즘은 얼굴 인식을 통한 감정분석 결과와 음성(억양) 인식을 통한 감정분석 결과를 통해 종합해 사용자의 감정에 맞는 노래를 추천하는 것을 목표로 한다. 사용자의 다양한 취향과 상황을 고려하여 3가지 노래를 추천하고 각각 노래 분위기를 설명하는 태그와 함께 보여진다. 
  
#### - How?
앞서 기술한 두 가지 감정분석 결과를 추천 알고리즘의 입력으로 넣으면, 우리 시스템의 정책에 맞게 알고리즘이 동작해 Server에서 접근할 Database의 노래 테이블 인덱스를 반환한다. 

#### - 개발 과정
개발과정에 따른 'Recommendation Algorithm' Branch에 존재하는 결과물 파일들에 대한 설명이다. 개발 과정 중 수정사항이 있으므로 그에 대한 설명이 필요하다고 판단되어 이 부분에 기술한다.

우선, 우리 정책에 따른 추천 알고리즘을 설계하고 이를 바탕으로 구현을 진행하려 한다. 

처리 속도를 위해 C++로 구현하기를 목표로 하고, dll 파일로 만들어 python Server에서 import하여 모듈로 사용하려고 하였다. 하지만 라이브러리와 리눅스 서버로 옮기는 중에 문제가 많이 발생해, 추천 알고리즘을  Python함수로 작성하는 것으로 방향을 바꾸었다.  

이에 따라 추천 알고리즘의 return도 바뀌었다. 원래의 목표는 c++로 작성한 추천 알고리즘에서 Database에 접근해서 사용자의 연령을 고려한, 감정에 따른 노래 3가지를 return하는 것이었다. Python으로 작성하게 되면서,  Server에서 Database에 접근할 감정 테이블 3개를 추천 알고리즘에서 return 하면, Server에서 지정된 테이블로 접근해 연령을 고려하여 노래 3개를 가져오도록 한다. 다시 말해, 추천 알고리즘에서는 Database 테이블 인덱스를 뽑아오고 Server에서는 연령을 고려하여 노래를 가져오는 두 가지로 분리된 셈이다. 
 
 
 
##### (1) 추천 알고리즘 설계 방향 
결과물은 **'추천알고리즘_설계방향 v1~v3 '**이다. 추천 알고리즘 설계와 구성도를 위해 팀원들과 논의한 내용이다. 

##### (2) 추천 알고리즘 설계와 구성도
결과물은 **'추천알고리즘_설계 ver1~4 '**이다. 전반의 설계방향에 맞게 설계한 추천 알고리즘과 그때의 구성도에 대한 내용이다. 우리 시스템만의 정책으로 알고리즘이 처리되기 때문에 논의를 통해 정책이 수정이 되면 버전을 달리한다. 

##### (3) pseudo code 작성 (c++)
결과물은 **'pseudo rec algo.cpp'**이다. 추천알고리즘_설계 ver4에서 설계한 내용에 따라 pseudo code를 작성했다. 

##### (4) c++로 추천 알고리즘 구현
작성한 pseudo code에 따라서 c++로 추천 알고리즘을 작성한 후 dll파일로 만들기로 한다. 입력으로 들어오는 .json 파일을 parsing하기 위해 jsoncpp parser를 이용한다. jsoncpp 라이브러리 테스트 결과물은 **jsonWriterAndReader**와 **ConsoleApplication5Zip**이다. 


-> reference1 : https://github.com/open-source-parsers/jsoncpp/releases/tag/1.8.2


-> reference2 : https://s-engineer.tistory.com/21



jsoncpp 라이브러리 테스트를 완료한 후 c++프로젝트에 include하여 추천 알고리즘을 작성한다. 빌드 완료 후 생성되는 dll 파일을 Server에서 import하여 사용하기로 한다. 결과물은 **'rec_algo'** 폴더이다. 

라이브러리 양이 많아지고 리눅스 서버에 올릴 때 생긴 문제때문에 python으로 다시 작성한다. 그러면 python Server에서 함수를 호출하면 되고, 추천 알고리즘에서 Database테이블에 접근할 필요가 없어진다. 추천 알고리즘 함수에서는 Database 테이블 인덱스를 뽑아오고 Server에서는 연령을 고려하여 노래를 가져오는 두 가지로 분리된 셈이다. 

python버전으로 바꾼 결과물은 **'rec_algo_pythonVersion.py'**이고, Server에서 연령을 고려해 Database의 테이블로 접근하여 노래 3개를 뽑아 music_list로 만드는 함수에 대한 pseudo code는 **'pseudo_music_selection_py**이다.  
 
