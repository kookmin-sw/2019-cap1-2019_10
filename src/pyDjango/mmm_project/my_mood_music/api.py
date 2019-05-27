# 클래스 기반 뷰
from django.http import Http404
from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status

from django.http.response import HttpResponse
from django.db.models import Max
import random
import cognitive_face as CF
from .models import *



from sklearn.preprocessing import LabelEncoder
import librosa
import numpy as np
import pandas as pd
from keras.models import model_from_json

from rest_framework.authentication import TokenAuthentication
from rest_framework.permissions import IsAuthenticated
from .permissions import IsOwnerOrReadOnly

from django.core.files.storage import default_storage
from django.core.files.base import ContentFile

import operator
import json

'''
어플 시나리오 
1. 베이맥스가 인사한다.
2. 사진을 찍는다 -> 서버에 보냄
3. 음성을 녹음한다 -> 서버에 보냄
4. 서버에서 제일 큰 감정 3개를 받는다.
5. 서버에서 youtube url을 받는다.  + music
'''
from django.db import connection

class Test(APIView):
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.music_list = []
        self.emotion_dict = {0: Anger, 1: Sadness,}

    def get_random3(self):
        max_id = Happiness.objects.all().aggregate(max_id=Max("id"))['max_id']
        music_list = []
        for i in range(0, 3):
            pk = random.randint(1, max_id)
            music = Happiness.objects.filter(pk=pk).first()
            if music:
                music_list.append(music)
        self.music_list = music_list

    def create_Analysis_Result(self):
        queryset = Analysis_Result.objects.create(email = "", music_r1=self.music_list[0], music_r2=self.music_list[1],
                                                  music_r3=self.music_list[2])
        # id를 받아오는 걸 짜야 되는데 그 상대의 id를 어떻게 받지?

        print(connection.queries[-1]) # 디버깅 용
        # 모델 클래스의 오브젝트 갯수확인
        Analysis_Result.objects.all().count()

        # 지정 조건의 데이터 Row를 순회
        for model_instance in queryset:
            print(model_instance)

        # 특정 조건의 데이터 Row 1개 Fetch (1개!! 2개이상말고 1개!! 0개말고 1개!!) model_instance = queryset.get(id=1)
        model_instance = queryset.get(title='my title')
        model_instance = queryset.first()  # model instance 또는 None
        model_instance = queryset.last()  # model instance 또는 None

    def get(self, request, format=None):

        self.get_random3()
        self.create_Analysis_Result()
        return HttpResponse(self.music_list)


class RequestFaceAPI(APIView):

    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.result_age = 0.0
        self.result_emotion = []
        self.music_list = []

    # 추천 알고리즘에서 DB 테이블을 주면 거 테이블로 가서 랜덤으로 가져온다.
    def get_random3(self):
        max_id = Happiness.objects.all().aggregate(max_id=Max("id"))['max_id']
        music_list = []
        for i in range(0, 3):
            pk = random.randint(1, max_id)
            music = Happiness.objects.filter(pk=pk).first()
            if music:
                music_list.append(music)

        self.music_list = music_list

    def create_Analysis_Result(self):
        new_post = Analysis_Result.objects.create(music_r1=self.music_list[0], music_r2=self.music_list[1],
                                                  music_r3=self.music_list[2])

    def get_data_from_faces(self, faces):
        json_string = str(faces).replace("\"", "\'")

        dict_data = json.loads(json_string)  # Unicode decode Error
        emotions = dict_data['faceAttributes']['emotion']
        sorted_emotions = sorted(emotions.items(), key=operator.itemgetter(1), reverse=True)

        # 결과값이 0.0 이상인 tuple 만 list 에 저장한다.
        result_emotion = [emotion for emotion, value in sorted_emotions if value > 0]
        self.result_emotion = result_emotion

        # 추천 알고리즘에 적용할 age 추출
        result_age = dict_data['faceAttributes']['age']
        self.result_age = result_age

        # DB 테이블에서 랜덤으로 값 가져와서 그 결과값 Analysis_Result에 쌓기

    def post(self, request, format=None):
        try:
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

            if faces==[]:
                return HttpResponse("please try again")
            else:
                self.get_data_from_faces(faces[0])
                print(self.result_emotion)
                return HttpResponse(self.result_emotion)
        except Exception as e :
            print(e)



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


class Call(APIView):

    def post(self, request, format=None):
        try:
            audio_file = request.FILES.get('audio', '')
            path = default_storage.save('file.wav', ContentFile(audio_file.read()))
        except Exception as e:
            print(e)
        return HttpResponse(self.labelfrommodel('file.wav'))

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
