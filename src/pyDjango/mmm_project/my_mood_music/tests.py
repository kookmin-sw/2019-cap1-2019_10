# from django.test import TestCase
# import operator
#
#
# # Create your tests here.
# import json
#
# jsonString ='{"faceId": "c8a2f7ff-316b-4440-a051-f1bdebe7bebf", "faceRectangle": {"top": 0, "left": 35, "width": 245, "height": 199}, "faceAttributes": {"age": 25.0, "emotion": {"anger": 0.0, "contempt": 0.099, "disgust": 0.073, "fear": 0.0, "happiness": 0.0, "neutral": 0.012, "sadness": 0.816, "surprise": 0.0}}}'
#
# dict = json.loads(jsonString)
#
# print(dict['faceAttributes']['emotion'])
# print(type(dict['faceAttributes']['age'])) # float
#
# emotions = dict['faceAttributes']['emotion']
# sorted_x = sorted(emotions.items(), key=operator.itemgetter(1)) # dictionary를 value값으로 sorting
#
# print(sorted_x[-1]) #tuple
#
# print(sorted_x[-3:])
#
#
# # anger = emotions['anger']
# # contempt = emotions['contempt']
# # disgust = emotions['disgust']
# # fear = emotions['fear']
# # happiness = emotions['happiness']
# # neutral = emotions['neutral']
# # sadness = emotions['sadness']
# # surprise = emotions['surprise']
# #
# # print(contempt)

import cognitive_face as CF

KEY = '86ad6a50a2af46189c45fc51819f4d9b'
CF.Key.set(KEY)

BASE_URL = 'https://koreacentral.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,emotion&recognitionModel=recognition_01&returnRecognitionModel=false '  # Replace with your regional Base URL
CF.BaseUrl.set(BASE_URL)


f = open('C:/Users/Oh YJ/Downloads/image_face/2.png', 'rb')
str_data = f.read()
print(str_data)
f.close()

result = open('./test2.jpg', 'wb')
result.write(str_data)
result.close()

data = open('./test2.jpg','rb')

faces = CF.face.detect(data)

print(faces)
