#include <iostream>
#include <string>

using namespace std;

*최종 결과 형태
	화 / 공포 / 행복 / 경멸+역겨움 / 슬픔 / 놀람
	anger / fear / happiness / contemp+disgust / sad / surprise

* 설계 ver4. 순위없앰(내부정보를 숨기기, 사용자가 순전히 취향대로 선택할 수 있게) + 태그를 보여줌 (+추가사항 : 태그 누르면 비슷한 곡)

* 연령은 함수 만들어야할듯? 추정나이 / 연령범위->취급연령
  : 추정나이가 연령범위에 들어가면 그 취급연령으로 취급하고 테이블에서 해당 연령범위의 노래를 선택
  : 함수 input=추정나이 , output=취급연령

* switch(정수형변수) 이고, 여기에 1순위 감정 중 하나가 들어가야하므로 : 각 감정을 정수형 변수로 선언하고 정수를 대입함 = enum
  -> 쓴 순서대로 1,2,,정수가 부여됨

enum Emotion { 
	ANGER,
	FEAR,
	HAPPINESS,
	CONTEMP_dISGUST,
	SAD,
	SURPRISE 
};

??switch에서 각 테이블에 접근하기 쉽게 이것도 enum같이 틀을 만드는게 좋을지? 코드 복잡할땐?

int main(){

	// 1. 전처리
	if (최종감정결과의 6개의 감정 중 0.01미만인 게 있으면) 그 감정 = 0.0 //for문 안쓰는 방법 없나

	// 2. 모순적인 최종결과 or not
	if (모순적인 경우들 중 한 가지에 속하면){// if( () || () || () )
		'모순테이블'에 접근
		취급연령에 맞는 랜덤 3개 선택
		return 0;
	}

	//감정 순위 가져오기 
	1순위 감정 가져와서 enum에서 찾아서 정수형 변수로 사용할 수 있게 
	2순위 감정도 가져오기
	3순위 감정도 가져오기 
	-> 2,3순위 감정 변수도 int가 최선인가 !?

	int first_rank_emotion = 1; //실제로 가져온거 
	int second_rank_emotion, third_rank_emotion;

	if(second_rank_emotion && third_rank_emotion 둘 다 없는 경우=0.0인 경우){ //1순위 감정만 있는 경우
		2위 음악, 3위 음악은 1순위 감정의 경우에서 다시 랜덤 선택 && 취급연령
		second_rank_emotion = first_rank_emotion;
		third_rank_emotion = first_rank_emotion;
	}
	else if(third_rank_emotion == 0.0 ){ //1,2순위 감정만 있는 경우
		3위 음악은 1순위 감정의 경우에서 다시 랜덤 선택 && 취급연령 
		second_rank_emotion = 4; //실제로 가져온거 
		third_rank_emotion = first_rank_emotion;
	} 
	else { //1,2,3순위 감정이 모두 있는 경우 
		second_rank_emotion = 4; //실제로 가져온거
		third_rank_emotion = 2; //실제로 가져온거 
	}

	// 3. 위에서 return 이 안되면 나머지 경우는 그 외의 경우임. 다중조건 분기문 switch로 처리
	switch(first_rank_emotion){ //0번째 인덱스가 1순위 감정임
		case 1: //화
			(1) 슬픔테이블에 접근 -> 슬픔(1)음악(잔잔) && 취급연령에 맞는 음악 랜덤 
			(2)	화남테이블에 접근 -> 화남음악 && 취급연령에 맞는 음악 랜덤
			(3) second_rank_emotion 판단 : 그 테이블에 해당하는 음악 && 취급연령에 맞는 음악 랜덤 
			break;

		case 2: //공포
			(1) 공포테이블에 접근 -> 공포해소노래 && 취급연령에 맞는 음악 랜덤 
			(2) 슬픔테이블에 접근 -> 슬픔(1)음악(잔잔) && 취급연령에 맞는 음악 랜덤 
			(3) second_rank_emotion 판단 : 그 테이블에 해당하는 음악 && 취급연령에 맞는 음악 랜덤 
			break;

		case 3: //행복
			if(행복감정이 0.8이상){
				(1),(2),(3) : 행복테이블에 접근 -> 행복음악 && 취급연령에 맞는 음악 '3개' 랜덤
			}
			else{
				(1) 행복테이블에 접근 -> 행복음악 && 취급연령에 맞는 음악 랜덤
				(2) second_rank_emotion 판단 : 그 테이블에 해당하는 음악 && 취급연령에 맞는 음악 랜덤
				(3) third_rank_emotion 판단 : 그 테이블에 해당하는 음악 && 취급연령에 맞는 음악 랜덤
			}
			break;

		case 4: //경멸+역겨움
			(1) 슬픔테이블에 접근 -> 슬픔(1)음악(잔잔) && 취급연령에 맞는 음악 랜덤 
			(2) 경멸_역겨움테이블에 접근 -> 경멸+역겨움음악 && 취급연령에 맞는 음악 랜덤 
			(3) second_rank_emotion 판단 : 그 테이블에 해당하는 음악 && 취급연령에 맞는 음악 랜덤
			break;

		case 5: //슬픔
			(1) 슬픔테이블에 접근 -> 슬픔(2)음악(너무 슬퍼서 울고싶은) && 취급연령에 맞는 음악 랜덤 
			(2) 슬픔테이블에 접근 -> 슬픔(1)음악(잔잔) && 취급연령에 맞는 음악 랜덤 
			(3) 슬픔테이블에 접근 -> 슬픔(3)음악(기분전환이 될 밝은) && 취급연령에 맞는 음악 랜덤 
			break;

		case 6:
			if(놀람감정이 0.8이상){
				(1),(2),(3) : 놀람테이블에 접근 -> 놀람음악 && 취급연령에 맞는 음악 '3개' 랜덤
			}
			else{
				(1) 놀람테이블에 접근 -> 놀람음악 && 취급연령에 맞는 음악 랜덤
				(2) second_rank_emotion 판단 : 그 테이블에 해당하는 음악 && 취급연령에 맞는 음악 랜덤
				(3) third_rank_emotion 판단 : 그 테이블에 해당하는 음악 && 취급연령에 맞는 음악 랜덤
			}
			break;

		default : //case상수가 switch블록에 존재하지 않는 경우 
			printf("error, Not any of emotion ranks !! ");
			break;
	}

	return 0; 
}




