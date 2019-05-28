#pragma once
#ifdef CREATE_EXPORTS
#define MYDLL __declspec(dllexport)
#else
#define MYDLL __declspec(dllimport)
#endif

MYDLL int add(int x, int y);