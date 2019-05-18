#pragma comment(lib, "lib_json.lib")
#pragma warning(disable:4996)

#include<iostream>
#include<fstream>
#include<memory>
#include"json/json.h"

using namespace std;
using namespace Json;

//test123

int main() {
	ofstream json_file;
	json_file.open("JSON_DATA.json");

	Value Comp;
	Comp["CPU"] = "I7";
	
	StreamWriterBuilder builder;
	builder["commentStyle"] = "None"; builder["indentation"] = "	";
	unique_ptr<Json::StreamWriter> writer(builder.newStreamWriter());

	writer->write(Comp, &cout);
	writer->write(Comp, &json_file);
	cout << endl;
	json_file.close();

	return 0;
}