#pragma comment(lib, "lib_json.lib")
#pragma warning(disable:4996)

#include "json/json.h"

#include <iostream>
#include <fstream>
#include <cstdlib>
#include <cstring>
#include <map>
#include <ctime>
#include <functional>
#include <memory>

using namespace std;
using namespace Json;

enum Emotion { 
	ANGER,
	CONTEMPT_DISGUST,
	FEAR,
	HAPPINESS,
	SAD,
	SURPRISE
};

int main() {

	ifstream json_dir("JSON_DATA.json");
	CharReaderBuilder builder;
	builder["collectComments"] = false;
	
	Value root; //Json::Value root;
	Reader reader; //Json::Reader reader;

	JSONCPP_STRING errs;
	bool ok = parseFromStream(builder, json_dir, &root, &errs);

	//multimap : 중복키 허용 & 정렬됨 
	multimap<double, string, greater<double> > tmp_res; //greater<int>면 key가 내림차순으로 정렬된다. 

	// 음성 결과 - 일단 int형으로
	int tone = 0; 
	/* 0 : 모순적인 결과 중 슬픈 목소리(얼굴은 행복), 
	   1 : 모순적인 결과 중 행복 목소리(얼굴은 슬픔)
	   2 : 중립이 0.8이상
	   3 : 나머지
	*/

	//접근할 테이블
	int table1, table2, table3;

	/*
	0 : 감정 테이블 중 ANGER
	1 : 감정 테이블 중 CONTEMPT_DISGUST
	2 : 감정 테이블 중 FEAR
	3 : 감정 테이블 중 HAPPINESS
	4 : 감정 테이블 중 SAD
	5 : 감정 테이블 중 SURPRISE
	6 : 거짓말 테이블
	* 동요테이블은 연령만 필요하므로, 파이썬 서버에서 맨 먼저 연령을 검사하고 영유아 연령이면 추천알고리즘이 필요없다. 
	*/

	if (ok == true) {
		//json파일에서 각 감정에 해당하는 비율을 가져와 double형의 변수에 넣는다. (face API 결과임)
		double anger = root.get("ANGER", 0.0).asDouble();
		double fear = root.get("FEAR", 0.0).asDouble();
		double disgust = root.get("DISGUST", 0.0).asDouble();
		double contempt = root.get("CONTEMPT", 0.0).asDouble();
		double neutral = root.get("NEUTRAL", 0.0).asDouble();
		double happiness = root.get("HAPPINESS", 0.0).asDouble();
		double sad = root.get("SAD", 0.0).asDouble();
		double surprise = root.get("SURPRISE", 0.0).asDouble();

		//multimap인 tmp_res에 key-value 형태로 저장한다. key : 비율, value : 감정
		tmp_res.insert(make_pair(anger, "ANGER"));
		tmp_res.insert(make_pair(fear, "FEAR"));
		tmp_res.insert(make_pair(contempt, "CONTEMPT"));
		tmp_res.insert(make_pair(disgust, "DISGUST"));
		tmp_res.insert(make_pair(happiness, "HAPPINESS"));
		tmp_res.insert(make_pair(neutral, "NEUTRAL"));
		tmp_res.insert(make_pair(sad, "SAD"));
		tmp_res.insert(make_pair(surprise,"SURPRISE"));

		//tmp_res 에는 비율-감정 pair로 담겨있다. ( 0.6, 'FEAR' 등) : raw emotion data

		//전처리(예외처리, 스무딩)을 하면서 6개의 감정으로 바꾸고, 이를 emotion_res에 저장한다.
		multimap<double, string, greater<double> > emotion_res;

		//전처리
		double tmp_contempt_disgust = 0.0; //emotion_res에 넣을 새로운 감정 contempt_disgust 의 감정 비율의 합
		for (auto & it : tmp_res) {
			//cout << it.first << " " << it.second << endl; //디버깅

			//전처리 - 0.05미만은 0.0으로 스무딩 + 6개의 감정으로 만들어 emotion_res에 저장한다. 
			if (it.second != "NEUTRAL") { //중립은 감정테이블에 없으므로 emotion_res에 넣으면 안된다. 
				if (it.second == "CONTEMPT" || it.second == "DISGUST") {
					tmp_contempt_disgust += it.first; //for문 끝나고 emotion_res에 넣는다.  
				}
				else {
					if (it.first < 0.05)
						emotion_res.insert(make_pair(0.0, it.second));
					else
						emotion_res.insert(make_pair(it.first, it.second));
				}
			}

			//예외처리 (1) 거짓말 판별 : 슬픔과 기쁨이 같이 나옴 - face와 음성 API 결과가 모순적이면 예외처리
			if ( (it.second == "HAPPINESS" && it.first >= 0.2 && tone == 0 ) /*행복얼굴로 슬픈목소리*/
				|| (it.second == "SAD" && it.first >= 0.2 && tone == 1) /*슬픈얼굴로 행복목소리*/ ) { 
				//Q. 행복이 얼마나 나와야 모순적인 결과로 처리할까
				table1 = 6;
				table2 = 6;
				table3 = 6;
				cout << "거짓말 테이블로 정보를 전달합니다" << endl;
				//return 0;
			}
			
			//에외처리 (2) neutral 중립이 크게 나왔을때(기준 0.8)는 랜덤 노래 3개 추출 
			else if (it.second == "NEUTRAL" && it.first >= 0.8) {
				srand((size_t)time(NULL)); 
				for (int num = 0; num < 3; num++) {
					cout << "랜덤 테이블은 : " << (rand() % 6) + 1 << endl; //뽑은 3개를 서버로 전달
				}
				//return 0;
			}
		}
		//for문 나와서야 tmp_contempt_disgust도 emotion_res에 넣음(역시 전처리[스무딩] 적용)
		if (tmp_contempt_disgust< 0.05) 
			emotion_res.insert(make_pair(0.0, "CONTEMPT_DISGUST"));
		else 
			emotion_res.insert(make_pair(tmp_contempt_disgust, "CONTEMPT_DISGUST"));
		 
		int cnt = 0;
		int happiness_rate, surprise_rate; //알고리즘 판별에서 비율 사용위해
		vector<string> rank_emotion;

		/* vector<string> 생성 : '테이블 인덱스-감정테이블' 순서로 넣음
		   -> 알고리즘 처리할 때, 인덱스를 이용할 수 있음 (인덱스-감정테이블) */
		vector<string> emotion_tables; 

		//인덱스= DB의 테이블 인덱스 & value = DB의 감정테이블
		emotion_tables.push_back("ANGER");
		emotion_tables.push_back("CONTEMPT_DISGUST");
		emotion_tables.push_back("FEAR");
		emotion_tables.push_back("HAPPINESS");
		emotion_tables.push_back("SAD");
		emotion_tables.push_back("SURPRISE");

		//1~3순위 감정 지정하기위해 개수 세기 & rank_emotion에 저장
		for (auto &iter : emotion_res) {
			cout << iter.first << " " << iter.second << endl; //디버깅
			if (iter.first != 0) {
				rank_emotion.push_back(iter.second);
				cnt++;
			}
			if (iter.second == "HAPPINESS") {
				happiness_rate = iter.first;
			}
			if (iter.second == "SURPRISE") {
				surprise_rate = iter.first;
			}
		}

		//3순위 감정까지 모자란부분 채우기
		if (cnt == 1) { //1순위 감정까지만 있다면
			//2순위, 3순위 감정을 1순위 감정으로 채움
			rank_emotion.push_back(rank_emotion[0]);
			rank_emotion.push_back(rank_emotion[0]);
		}
		else if (cnt == 2) { //2순위 감정까지만 있다면
			//3순위 감정을 1순위 감정으로 채움
			rank_emotion.push_back(rank_emotion[0]);
		}
		else if (cnt >= 3) { //3순위 감정도 있다면
			cout << "3순위까지 있다" << endl;
		}

		cout << endl << endl;
		cout << "디버깅 ㅡㅡㅡㅡ" << endl;
		cout << "cnt : " << cnt << endl;
		cout << rank_emotion[0] << " " << rank_emotion[1] << " " << rank_emotion[2] << endl;
		cout << endl << endl;

		/*
		인덱스 - 테이블
		0 : 감정 테이블중 ANGER (화)
		1 : 감정 테이블중 CONTEMPT_DISGUST (경멸_역겨움)

		2 : 감정 테이블중 FEAR (공포)
		3 : 감정 테이블중 HAPPINESS (행복)
		
		4 : 감정 테이블중 SAD (슬픔)
		5 : 감정 테이블중 SURPRISE (놀람)
		
		6 : 거짓말 테이블
		*/
		
		if (rank_emotion[0] == "ANGER") { //화
			table1 = 41; // 4:슬픔테이블 1:슬픔(1)음악(잔잔)
			table2 = 0;  // 0:화남테이블
			
			//table3 : '2순위 감정'에 해당하는 감정 테이블
			for (int i = 0; i < emotion_tables.size(); i++) {
				if (emotion_tables[i] == rank_emotion[1]) {
					table3 = i;
					if (table3 == 4) table3 = 42;
				}
			} 
		}
		else if (rank_emotion[0] == "FEAR") { //공포
			table1 = 2; //2:공포테이블
			table2 = 41;

			//table3 : '2순위 감정'에 해당하는 감정 테이블
			for (int i = 0; i < emotion_tables.size(); i++) {
				if (emotion_tables[i] == rank_emotion[1]) {
					table3 = i;
					if (table3 == 4) table3 = 42;
				}
			}
		}
		else if (rank_emotion[0] == "HAPPINESS") { //행복
			if (happiness_rate >= 0.8) {
				table1 = 3; //3:행복테이블
				table2 = 3;
				table3 = 3;
			}
			else {
				table1 = 3;
				
				//table2 : '2순위 감정'에 해당하는 감정 테이블
				for (int i = 0; i < emotion_tables.size(); i++) {
					if (emotion_tables[i] == rank_emotion[1]) {
						table2 = i;
						if (table2 == 4) table2 = 42;
					}
				}

				//table3 : '3순위 감정'에 해당하는 감정 테이블
				for (int i = 0; i < emotion_tables.size(); i++) {
					if (emotion_tables[i] == rank_emotion[2]) {
						table3 = i;
						if (table3 == 4) table3 = 42;
					}
				}
			}
		}
		else if (rank_emotion[0] == "CONTEMPT_DISGUST") { //경멸_역겨움
			table1 = 41;
			table2 = 1;
			
			//table3 : '3순위 감정'에 해당하는 감정 테이블
			for (int i = 0; i < emotion_tables.size(); i++) {
				if (emotion_tables[i] == rank_emotion[2]) {
					table3 = i;
					if (table3 == 4) table3 = 42;
				}
			}
		}
		else if (rank_emotion[0] == "SAD") { //슬픔
			table1 = 42; // 4:슬픔테이블, 2:슬픔(2)음악(너무슬퍼서 울고싶은)
			table2 = 41; // 4:슬픔테이블, 1:슬픔(1)음악(잔잔)
			table3 = 43; // 4:슬픔테이블, 3:슬픔(3)음악(기분전환)
		}
		else { //놀람
			if (surprise_rate >= 0.8) {
				table1 = 5; //5:놀람테이블
				table2 = 5;
				table3 = 5;
			}
			else {
				table1 = 5;

				//table2 : '2순위 감정'에 해당하는 감정 테이블
				for (int i = 0; i < emotion_tables.size(); i++) {
					if (emotion_tables[i] == rank_emotion[1]) {
						table2 = i;
						if (table2 == 4) table2 = 42;
					}
				}

				//table3 : '3순위 감정'에 해당하는 감정 테이블
				for (int i = 0; i < emotion_tables.size(); i++) {
					if (emotion_tables[i] == rank_emotion[2]) {
						table3 = i;
						if (table3 == 4) table3 = 42;
					}
				}
			}

		}

		cout << "디버깅 ---------------------------------" << endl;
		cout << rank_emotion[0] << " " << rank_emotion[1] << " " << rank_emotion[2] << endl;
		cout << "선택된 table : "<<table1 << " " << table2 << " " << table3 << endl;
		//table1,2,3에서 나올수있는 형태 : 1/2/3/ 41/42/43 / 5 / 6(거짓말)
	}
	else cout << "parse failed" << endl; 
	return 0;
}