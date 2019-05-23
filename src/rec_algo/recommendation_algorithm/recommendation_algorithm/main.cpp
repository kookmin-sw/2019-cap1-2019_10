#pragma comment(lib, "lib_json.lib")
#pragma warning(disable:4996)

#include <iostream>
#include <fstream>
#include <cstring>
#include <map>
#include <functional>
#include <memory>
#include "json/json.h"

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
	7 : 동요 테이블 (감정이 필요없음)
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

		//전처리와 예외처리를 하면서 6개의 감정으로 바꾸고, 이를 emotion_res에 저장한다.
		map<double, string, greater<double> > emotion_res;

		//전처리
		for (auto & it : tmp_res) {
			cout << it.first << " " << it.second << endl; //디버깅
			if ( (it.second == "HAPPINESS" && it.first >= 0.2 && tone == 0 ) /*행복얼굴로 슬픈목소리*/
				|| (it.second == "SAD" && it.first >= 0.2 && tone == 1) /*슬픈얼굴로 행복목소리*/ ) { 
				//Q. 행복이 얼마나 나와야 모순적인 결과로 처리할까
				table = 0;
				cout << "거짓말 테이블로 정보를 전달합니다" << endl;
			}
		}
		return 0; //지금은 디버깅 때문에 밖으로 뺌 -> 83번 라인 밑으로 추가

	}
	else cout << "parse failed" << endl; 
	return 0;
}