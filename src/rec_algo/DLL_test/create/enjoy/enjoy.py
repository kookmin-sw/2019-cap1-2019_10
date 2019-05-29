
from ctypes import *

Load_DLL=WinDLL('dll_test.dll')
plus_func=Load_DLL['plus']
plus_func.argtypes=(c_int,c_int)
plus_func.restype=c_int
#print('Arg PLUS')

#intA=int(input("arg1 : "))
#intB=int(input("arg2 : "))
#plus_res=plus_func(intA,intB)
#print("Arg1 + Arg2 :",intA+intB,'\n')

plus_res = plus_func(10,1)
print("hello")



CDLL('dll_test.dll')

