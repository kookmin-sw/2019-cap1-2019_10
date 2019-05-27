from django.test import TestCase
import operator


# Create your tests here.
import json

json_string ='{"faceId": "c8a2f7ff-316b-4440-a051-f1bdebe7bebf", "faceRectangle": {"top": 0, "left": 35, "width": 245, "height": 199}, "faceAttributes": {"age": 25.0, "emotion": {"anger": 0.0, "contempt": 0.099, "disgust": 0.073, "fear": 0.0, "happiness": 0.0, "neutral": 0.012, "sadness": 0.816, "surprise": 0.0}}}'

dict = json.loads(json_string)

print(type(dict['faceAttributes']['emotion']))
print(type(dict['faceAttributes']['age'])) # float

emotions = dict['faceAttributes']['emotion']
sorted_emotions = sorted(emotions.items(), key=operator.itemgetter(1), reverse=True) # dictionary를 value값으로 sorting

print('sorted_emotions : ', sorted_emotions) # tuple list

#
# sorted_emotion_names = []
# for i in sorted_emotions:
#     if i[1]>0.0:
#         sorted_emotion_names.append(i)

sorted_emotion_names = [x for x,y in sorted_emotions if y>0]

print(sorted_emotion_names)
print(len(sorted_emotion_names))

print(sorted_emotion_names[:2])

anger = emotions['anger']
# contempt = emotions['contempt']
disgust = emotions['disgust'] + emotions['contempt']
fear = emotions['fear']
happiness = emotions['happiness']
neutral = emotions['neutral']
sadness = emotions['sadness']
surprise = emotions['surprise']

print(disgust)



# if neutral==1.0  : 뮤직에서 아무거나 랜덤으로 뽑는다.

#
# print(contempt)

# import cognitive_face as CF
#
# KEY = '86ad6a50a2af46189c45fc51819f4d9b'
# CF.Key.set(KEY)
#
# BASE_URL = 'https://koreacentral.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,emotion&recognitionModel=recognition_01&returnRecognitionModel=false '  # Replace with your regional Base URL
# CF.BaseUrl.set(BASE_URL)
#
# # You can use this example JPG or replace the URL below with your own URL to a JPEG image.
# data = open('C:/Users/Oh YJ/Downloads/image_face/2.png', 'rb')
# faces = CF.face.detect(data)
#
# print(faces)