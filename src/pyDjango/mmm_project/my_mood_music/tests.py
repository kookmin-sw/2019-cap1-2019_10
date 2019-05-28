from django.test import TestCase
import operator


# Create your tests here.
import json

json_string ='{"faceId": "c8a2f7ff-316b-4440-a051-f1bdebe7bebf", "faceRectangle": {"top": 0, "left": 35, "width": 245, "height": 199}, "faceAttributes": {"age": 25.0, "emotion": {"anger": 0.0, "contempt": 0.099, "disgust": 0.073, "fear": 0.0, "happiness": 0.0, "neutral": 0.012, "sadness": 0.816, "surprise": 0.0}}}'

dict = json.loads(json_string)

print(dict['faceAttributes']['emotion'])
print(type(dict['faceAttributes']['age'])) # float

emotions = dict['faceAttributes']['emotion']
sorted_emotions = sorted(emotions.items(), key=operator.itemgetter(1)) # dictionary를 value값으로 sorting

print(sorted_emotions) #tuple

print(sorted_emotions[-3:])

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
