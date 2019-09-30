import json
from .httpError import serverError

# 이미지, 음성 감정분석 결과를 파일에 저장하거나 읽어온다.


def read_file(request, filename):
    try:
        f = open(filename, 'r')
        result = f.read()
        f.close()
        return result
    except OSError as err:
        serverError(request, "OS error: {0}".format(err))


def write_file(request, filename, data):
    try:
        f = open(filename, 'w')
        json_val = json.dumps(data)
        f.write(json_val)
        f.close()
    except OSError as err:
        serverError(request, "OS error: {0}".format(err))
