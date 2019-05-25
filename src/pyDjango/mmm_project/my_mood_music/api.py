
# 클래스 기반 뷰
from django.http import Http404
from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status


from django.http.response import HttpResponse
import cognitive_face as CF

from keras.utils import np_utils
from sklearn.preprocessing import LabelEncoder
import librosa
import numpy as np
import pandas as pd
import glob
import tensorflow as tf
from keras.models import model_from_json


import operator
import json


Sadness = []
Happiness = []
Surprise = []
Fear = []
Anger = []
Disgust = []

'''
어플 시나리오 
1. 베이맥스가 인사한다.
2. 사진을 찍는다 -> 서버에 보냄
3. 음성을 녹음한다 -> 서버에 보냄
4. 서버에서 제일 큰 감정 3개를 받는다.
5. 서버에서 youtube url을 받는다.  + music
'''


def get_final_emotion(face_api_result , speech_to_emotion_result):
    return HttpResponse("TODO : 최종결과도출")


class requestFaceAPI(APIView):
    # authentication_classes = (TokenAuthentication,)
    # permission_classes = (IsAuthenticated, IsOwnerOrReadOnly)  # 권한 옵션

    def get_data_from_faces(self, faces):
        json_string = str(faces).replace("\"","\'")
        dict_data = json.loads(json_string)  # Unicode decode Error
        emotions = dict_data['faceAttributes']['emotion']
        sorted_x = sorted(emotions.items(), key=operator.itemgetter(1))

        # 결과값이 크게 나온 emotion 3가지를 구한다
        result_emotion = sorted_x[-3:]
        self.result_emotion = result_emotion

        # 추천 알고리즘에 적용할 age 추출
        result_age = dict_data['faceAttributes']['age']
        self.result_age = result_age


    def post(self, request, format=None):
        KEY = '86ad6a50a2af46189c45fc51819f4d9b'
        CF.Key.set(KEY)

        BASE_URL = 'https://koreacentral.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,emotion&recognitionModel=recognition_01&returnRecognitionModel=false '  # Replace with your regional Base URL
        CF.BaseUrl.set(BASE_URL)

        print('get the data')

        # 디버깅 용
        # print('request.POST: ', request.POST)
        # print('request.FILES: ', request.FILES)
        # print('request.headers: ', request.headers)

        data_test = request.FILES.get('photo', '')

        print('data test is : ', data_test)
        print('type of data_test is : ', type(data_test))
        print('length of data_test is : ', len(data_test))
        print('finish to get ')

        faces = CF.face.detect(data_test)
        # print(faces)

        #self.get_data_from_faces(faces)

        return HttpResponse("clear")

    def get(self, request, format=None):

        return HttpResponse("Using Microsoft Face API")





lb = LabelEncoder()
label = [
    "angry",
    "calm",
    "fearful",
    "happy",
    "sad",
    "angry",
    "calm",
    "fearful",
    "happy",
    "sad"
]



class call(APIView):

	def post(self, request, format=None):
		audio_file = request.FILES.get('audio','')
		path = default_storage.save('file.wav', ContentFile(audio_file.read()))
		return HttpResponse(labelfrommodel('file.wav'))
		
	def get(self, request, format=None):
		return HttpResponse("Using Speech-Emotion-Model")

	def labelfrommodel(self, filename):
		json_file = open('model.json', 'r')
		loaded_model_json = json_file.read()
		json_file.close()
		loaded_model = model_from_json(loaded_model_json)
		# load weights into new model
		loaded_model.load_weights("Emotion_Voice_Detection_Model.h5")

		X, sample_rate = librosa.load(filename, res_type='kaiser_fast', duration=2.5,
		                              sr=22050 * 2, offset=0.5)
		sample_rate = np.array(sample_rate)
		mfccs = np.mean(librosa.feature.mfcc(y=X, sr=sample_rate, n_mfcc=13), axis=0)

		featurelive = mfccs
		livedf2 = featurelive
		livedf2 = pd.DataFrame(data=livedf2)
		livedf2 = livedf2.stack().to_frame().T
		twodim = np.expand_dims(livedf2, axis=2)
		livepreds = loaded_model.predict(twodim, batch_size=32, verbose=1)
		livepreds1 = livepreds.argmax(axis=1)
		liveabc = livepreds1.astype(int).flatten()

		return label[int(liveabc)]


'''
azure 키 
1. 86ad6a50a2af46189c45fc51819f4d9b
2. b97ea531a0b04569856d75fba76141d7
'''
