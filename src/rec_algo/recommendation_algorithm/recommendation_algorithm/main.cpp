#pragma comment(lib, "lib_json.lib")
#pragma warning(disable:4996)

#include <iostream>
#include <fstream>
#include <cstring>
#include <map>
#include <functional> 
#include <typeinfo>
#include <memory>
#include "json/json.h"

using namespace std;
using namespace Json;

//enum Emotion {
//	ANGER,
//	FEAR,
//	HAPPINESS,
//	CONTEMP_dISGUST,
//	SAD,
//	SURPRISE
//};


int main() {
	ifstream json_dir("JSON_DATA.json");
	CharReaderBuilder builder;
	builder["collectComments"] = false;
	
	Value root; //Json::Value root;
	Reader reader; //Json::Reader reader;

	JSONCPP_STRING errs;
	bool ok = parseFromStream(builder, json_dir, &root, &errs);

	//multimap : 중복키 허용 / 정렬됨 
	multimap<double, string, greater<double>> tmp_res; //greater<int>면 key가 내림차순으로 정렬된다. 
	
	if (ok == true) {
		//json파일에서 각 감정에 해당하는 비율을 가져와 double형의 변수에 넣는다. (face API 결과임)
		double anger = root.get("ANGER", 0.0).asDouble();
		double fear = root.get("FEAR", 0.0).asDouble();
		double fear = root.get("DISGUST", 0.0).asDouble();
		double contempt_disgust = root.get("CONTEMPT", 0.0).asDouble();
		double contempt_disgust = root.get("NEUTRAL", 0.0).asDouble();
		double happiness = root.get("HAPPINESS", 0.0).asDouble();
		double sad = root.get("SAD", 0.0).asDouble();
		double surprise = root.get("SURPRISE", 0.0).asDouble();

		//multimap인 tmp_res에 key-value 형태로 저장한다. key : 비율, value : 감정
		tmp_res.insert(make_pair(anger, "ANGER"));
		tmp_res.insert(make_pair(fear, "FEAR"));
		tmp_res.insert(make_pair(contempt_disgust,"CONTEMPT_DISGUST"));
		tmp_res.insert(make_pair(happiness, "HAPPINESS"));
		tmp_res.insert(make_pair(sad, "SAD"));
		tmp_res.insert(make_pair(surprise,"SURPRISE"));

		//tmp_res 에는 비율-감정 pair로 담겨있다. ( 0.6, 'FEAR' 등) 

		//예외처리와 전처리를 하면서 6개의 감정으로 바꾸고, 이를 emotion_res에 저장한다.
		map<double, string, greater<double> > emotion_res;

	/*	map<double, string>::iterator iter;
		for (iter = tmp_res.begin(); iter != tmp_res.end(); ++iter) {
			cout << "(" << iter->first << "," << iter->second << ")" << " " << endl;
			if (iter->first < 0.05) {

			}
		}
	*/ 
	}
	else cout << "parse failed" << endl;

	return 0;
}