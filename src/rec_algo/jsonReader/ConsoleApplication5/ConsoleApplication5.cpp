// ConsoleApplication5.cpp : 이 파일에는 'main' 함수가 포함됩니다. 거기서 프로그램 실행이 시작되고 종료됩니다.
//
#pragma comment (lib, "lib_json.lib")

#include "pch.h"
#include <iostream>
#include <fstream>
#include "json/json.h"

using namespace std;
using namespace Json;

int main()
{
	ifstream json_dir("JSON_DATA.json");
	CharReaderBuilder builder;
	builder["collectcomments"] = false;
	Value value;

	JSONCPP_STRING errs;
	bool ok = parseFromStream(builder, json_dir, &value, &errs);

	if (ok == true) {
		cout << "CPU : " << value["CPU"] << endl;
	}
	return 0;
}
 