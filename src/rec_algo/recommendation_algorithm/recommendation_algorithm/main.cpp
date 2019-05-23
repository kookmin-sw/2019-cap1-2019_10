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
	int table = 0; 
	/*
	0 : 거짓말 테이블
	1 : 감정 테이블중 HAPPINESS
	2 : 감정 테이블중 SAD
	3 : 감정 테이블중 CONTEMPT_DISGUST
	4 : 감정 테이블중 FEAR
	5 : 감정 테이블중 ANGER
	6 : 감정 테이블중 SURPRISE
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

		//tmp_res 에는 비율-감정 pair로 담겨있다. ( 0.6, 'FEAR' 등) 

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
				table = 0;
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

		//디버깅
		cout << endl;
		for (auto &iter : emotion_res) {
			cout << iter.first << " " << iter.second << endl;
		}
	}
	else cout << "parse failed" << endl; 
	return 0;
}