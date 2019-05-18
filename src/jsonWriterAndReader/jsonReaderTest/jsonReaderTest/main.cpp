#pragma comment(lib, "lib_json.lib")
#pragma warning(disable:4996)

#include<iostream>
#include<fstream>
#include<memory>
#include"json/json.h"

using namespace std;
using namespace Json;


int main() {
	ifstream json_dir("JSON_DATA.json");
	CharReaderBuilder builder;
	builder["collectComments"] = false;
	Value value;

	JSONCPP_STRING errs;
	bool ok = parseFromStream(builder, json_dir, &value, &errs);

	if (ok == true) {
		cout << "CPU : " << value["CPU"] << endl;
	}
	else cout << "parse failed" << endl;
	
	return 0;
}