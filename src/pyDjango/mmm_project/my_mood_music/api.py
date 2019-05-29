"""

어플의 전체적인 흐름에 관여하는 코드입니다.

"""
# 클래스 기반 뷰
from django.http import Http404, HttpResponseNotFound
from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status

from django.http.response import HttpResponse
from django.db.models import Max
import random
import cognitive_face as CF
from .serializers import *

import librosa
import numpy as np
import pandas as pd
from keras.models import model_from_json
from .models import *

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
4. 서버에서 결과가 0 이상인 감정결과를 받는다.
5. 서버에서 youtube url과 노래제목,가수이름과 tag를 받는다.
'''

from django.db import connection
from django.contrib.auth.models import User

# face_api_emotion = ''
# face_api_age = 0.0
# speech_api_emotion = ''

# # 이미지, 음성 감정분석 결과를 파일에 저장하거나 읽어온다.
# def process_file(self, filename, mode):
#     f = open(filename, mode)
#     if mode == 'r':
#         result = f.read()
#         return result
#     elif mode == 'w':
#         f.write()
#         f.close()


# 최종적으로 음악을 추천해주는 클래스 뷰
class RecommendationMusic(APIView):
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.table_idx = ["anger", "disgust", "fear", "happiness", "sadness", "surprise"]
        self.tone_res = ''
        self.music_list = [] # 리스트 안의 dictionary 형태로 들어온다. (music, url)
        self.age = random.randint(0, 150)
        # self.age = 10

        self.face = '{"faceId": "c8a2f7ff-316b-4440-a051-f1bdebe7bebf", "faceRectangle": {"top": 0, "left": 35, "width": 245, "height": 199}, "faceAttributes": {"age": 25.0, "emotion": {"anger": 0.0, "contempt": 0.099, "disgust": 0.073, "fear": 0.0, "happiness": 0.0, "neutral": 0.012, "sadness": 0.816, "surprise": 0.0}}}'
        self.speech = 'angry'

    # GET request
    def get(self, request, format=None):
        # global face_api_age
        # print('face_api_age : ', face_api_age)

        # self.age = process_file('face_api_age', 'r')
        try:
            f = open('face_api_age.txt', 'r')
            self.age = f.read()
            if not self.age : self.age = 24
            f.close()
            self.music_list = self.get_random3_test(Child)
            #self.music_list = self.recommand_music(self.age)

            print('self.music_list is : ', self.music_list)

            result = json.dumps(self.music_list, ensure_ascii=False)
            return Response(result)

        except Exception as e:
            return HttpResponse('please try again')

    # 어플에서 받은 id값과 데이터베이스에서 받은 값(music list)을 INSERT 한다.
    # 근데 이게 여기에 있으면 안 되잖아.
    def create_Analysis_Result(self, music_list):
        try:
            max_id = Analysis_Result.objects.all().aggregate(max_id=Max("id"))['max_id']
            if max_id:
                queryset = Analysis_Result.objects.create(id=max_id + 1, user_id=self.user_id,
                                                          music_r1=music_list[0],
                                                          music_r2=music_list[1], music_r3=music_list[2])
            else:
                queryset = Analysis_Result.objects.create(id=1, user_id=self.user_id, music_r1=music_list[0],
                                                          music_r2=music_list[1], music_r3=music_list[2])
            queryset.save()

            print(connection.queries[-1])  # 디버깅 용
            # 모델 클래스의 오브젝트 갯수확인
            print(Analysis_Result.objects.all().count())

        except Exception as e:
            return HttpResponse('please try again')

    # 하나의 데이터베이스에서 랜덤으로 레코드 3개를 받아온다.
    def get_random3(self, table):
        try:
            Music = {}
            max_id = table.objects.all().aggregate(max_id=Max("id"))['max_id']
            dictionary_list = []
            music_list = []
            link_list = []
            tag_list = []
            pk_list = []
            count = 0
            while True:
                # print('loop start')
                if count == 3:
                    break
                # print('pk before')
                pk = random.randint(1, max_id)  # pk can equal, have to correct
                # print('pk after')
                # print(pk)
                if pk in pk_list : continue
                pk_list.append(pk)
                # print(pk_list)
                music = table.objects.filter(id=pk).first()
                print('music.music : ', music.music)
                print('music.link : ', music.link)
                if music:
                    music_list.append(music.music)
                    link_list.append(music.link)
                    tag_list.append((music.tag1, music.tag2))
                    count += 1

            self.create_Analysis_Result(music_list)

            Music['music'] = music_list
            Music['link'] = link_list
            Music['tag'] = tag_list
            dictionary_list.append(Music)  # list of dictionary
            print(dictionary_list)
            return dictionary_list
        except Exception as e:
            return HttpResponse('please try again')

    # randomly get three music of DB (test)
    def get_random3_test(self, table):
        try:
            Music = {}
            max_id = table.objects.all().aggregate(max_id=Max("id"))['max_id']
            dictionary_list = []
            pk_list = []
            count = 0
            while True:
                print('loop start')
                if count == 3:
                    break
                print('pk before')
                pk = random.randint(1, max_id)  # pk can equal, have to correct
                print('pk after')
                print(pk)
                if pk in pk_list : continue
                pk_list.append(pk)
                print(pk_list)
                music = table.objects.filter(id=pk).first()
                print('music.music : ', music.music)
                print('music.link : ', music.link)
                if music:
                    Music['music_{}'.format(len(pk_list))] = music.music
                    Music['link_{}'.format(len(pk_list))] = music.link
                    count += 1
            
            dictionary_list.append(Music)  # list of dictionary
            print(dictionary_list)
            return dictionary_list
        except Exception as e:
            return HttpResponse('please try again')

    # 추천 알고리즘을 통해 얻은 3개의 레코드를 받아온다.
    def recommand_music(self, age):
        try:
            # global face_api_emotion
            # global speech_api_emotion
            # print('face_api_emotion : ', face_api_emotion)
            # print('speech_api_emotion : ', speech_api_emotion)

            dictionary_list = []
            music_list = []
            # 10세 미만이면 어린이 테이블로 가서 랜덤으로 3개의 동요를 뽑고 바로 종료한다.
            if age < 10:
                print('get_random3 function before')
                dictionary_list = self.get_random3(Child)
                print('get_random3 function finish')

            # 어린이 나이가 아니라면, 추천 알고리즘에 의해 감정까지 고려한 노래 3개를 뽑는다.
            else:
                # 연령대에 따라서 age를 결정한다.
                if age >= 10 and age <= 29:
                    age = 20
                elif age >= 30 and age <= 39:
                    age = 30
                else:
                    age = 40

                # 추천알고리즘에서 리턴하는 테이블 인덱스를 리스트 인덱스로 접근할 수 있도록 table_index 리스트 생성
                table_index = [Anger, Disgust, Fear, Happiness, Sadness, Surprise, Lie]

                # self.face = process_file('face_api_emotion', 'r')
                # self.speech = process_file('speech_api_emotion', 'r')

                f = open('face_api_emotion.txt', 'r')
                self.face = f.read()

                f.close()

                f = open('speech_api_emotion.txt', 'r')
                self.speech = f.read()
                f.close()

                # 추천알고리즘에서 리턴하는 테이블에 대한 정보 (노래 3개->3개의 요소)
                print('get_table_name function before')
                t_info = list(self.get_table_name(self.face, self.speech))  # should modify
                print('get_table_name function finish')
                print('t_info is : ', t_info)

                for tableN in t_info:
                    print('loop start')
                    if tableN in [41, 42, 43]:  # Sad테이블에 접근해야하는 경우
                        table = table_index[4]  # Sad
                        subclass_sad = tableN % 40  # subclass : 1 or 2 or 3
                        music = table.objects.filter(subclass_s=subclass_sad).order_by(
                            '?').first()  # Sad테이블 중 subclass_s가 subclass인 노래들 중 랜덤 선택

                    else:  # Sad테이블 이외의 테이블에 접근해야하는 경우
                        table = table_index[tableN]

                    music = table.objects.filter(age=age).order_by('?').first()  # 해당 연령 범위의 노래로 추림
                    print('music is: ', music)
                    print('music.music is : ', music.music)
                    music_list.append(music.music)
                    dictionary_list.append(music)

            self.create_Analysis_Result(music_list)

            return dictionary_list
        except Exception as e:
            return HttpResponse('please try again')

    # 이미지, 음성 감정 분석 결과를 이용해 접근할 테이블을 결정한다.
    def get_table_name(self, face, tone):
        try:
            # 테이블 인덱스를 리스트에 저장한다.
            table1 = 100
            table2 = 100
            table3 = 100

            # tone 결과를 face 결과형식와 맞춰줌 : angry->anger / calm->neutral / fearful->fear / happy->happiness / sad->sadness
            if tone == "angry":
                self.tone_res = "anger"
            elif tone == "calm":
                self.tone_res = "neutral"
            elif tone == "fearful":
                self.tone_res = "fear"
            elif tone == "happy":
                self.tone_res = "happiness"
            elif tone == "sad":
                self.tone_res = "sadness"
            else:
                return HttpResponse("tone_res err")

            # parse json data (input format : .json file)
            json_string = str(face).replace("\'", "\"")
            json_load = json.loads(json_string)  # Unicode decode Error

            dic = json_load['faceAttributes']['emotion']
            print('dictionary : ', dic)

            anger = dic['anger']
            contempt = dic['contempt']
            disgust = dic['disgust']
            fear = dic['fear']
            happiness = dic['happiness']
            neutral = dic['neutral']
            sadness = dic['sadness']
            surprise = dic['surprise']

            # 전처리 - 6개의 감정으로 만들기 위해, 우선 contempt와 disgust를 합친다.
            contempt_disgust = contempt + disgust
            dic["disgust"] = contempt_disgust
            # print('modified dictionary : ', dic)

            # 중립이 0.8 이상이면 3개의 랜덤 테이블을 선택한다. (0에서 5까지 중 랜덤 정수)
            if neutral > 0.8:
                table1 = random.randint(0, 6)
                table2 = random.randint(0, 6)
                table3 = random.randint(0, 6)
                return table1, table2, table3

            # 전처리 - 0.05미만인 값은 0.0으로 스무딩한다.
            else:
                for emotion, rate in dic.items():
                    if rate < 0.05:
                        dic[emotion] = 0.0
                        # print('smoothing dictionary : ', dic)

            # 전처리 - 거짓말인 경우, 거짓말 테이블 선택
            if self.tone_res == "sadness" and happiness > 0:
                print("행복 얼굴로 슬픈 목소리를 내는 거짓말쟁이1")
                table1 = 6
                table2 = 6
                table3 = 6
                return table1, table2, table3

            elif self.tone_res == "happiness" and sadness > 0:
                print("슬픈 얼굴로 행복 목소리를 내는 거짓말쟁이2")
                table1 = 6
                table2 = 6
                table3 = 6
                return table1, table2, table3

            # 전처리 (1) 중립에 의한 랜덤 테이블 (2) 거짓말테이블 -> 둘 다 해당하지 않으면 1,2,3순위 감정에 따른 알고리즘 처리 적용
            else:
                del dic['neutral']
                del dic["contempt"]

                emotion_res = sorted(dic.items(), key=operator.itemgetter(1), reverse=True)  # value 기준 내림차순 정렬
                # print("정렬후")
                # print('sorted dictionary to list of tuple : ', emotion_res)

                rank_emotion = []  # 인덱스 순서대로 높은 순위 감정이다. 3순위 감정까지 저장한다.
                rank_emotion_cnt = 0
                for emotion, rate in emotion_res:
                    if rate > 0:
                        rank_emotion.append(emotion)
                        rank_emotion_cnt += 1
                        if rank_emotion_cnt > 3:
                            break

                # print(rank_emotion)
                if rank_emotion_cnt == 1:
                    rank_emotion.append(rank_emotion[0])
                    rank_emotion.append(rank_emotion[0])
                elif rank_emotion_cnt == 2:
                    rank_emotion.append(rank_emotion[0])

                # print(rank_emotion)

                if rank_emotion[0] == "anger":
                    table1 = 41  # 4:슬픔테이블 41:슬픔(1)음악(잔잔)
                    table2 = 0  # 0:화남테이블

                    # table3 : '2순위 감정'에 해당하는 감정 테이블
                    table3 = self.table_idx.index(rank_emotion[1])
                    if table3 == 4:  # 2순위가 SADNESS라면, 41인 슬픔(1)음악(잔잔)을 선택한다.
                        table3 = 41

                elif rank_emotion[0] == "fear":
                    table1 = 2  # 2:공포테이블
                    table2 = 41  # 슬픔(1)음악(잔잔)

                    # table3 : 2순위 감정에 해당하는 감정 테이블
                    table3 = self.table_idx.index(rank_emotion[1])
                    if table3 == 4:
                        table3 = 41

                elif rank_emotion[0] == "happiness":
                    if happiness >= 0.8:
                        table1 = 3  # 3:행복테이블
                        table2 = 3
                        table3 = 3
                    else:
                        table1 = 3

                        # table2 : 2순위 감정에 해당하는 감정 테이블
                        table2 = self.table_idx.index(rank_emotion[1])
                        if table2 == 4:
                            table2 = 41

                        # table3 : 3순위 감정에 해당하는 감정 테이블
                        table3 = self.table_idx.index(rank_emotion[2])
                        if table3 == 4:
                            table3 = 41

                elif rank_emotion[0] == "disgust":
                    table1 = 41  # 슬픔(1)음악(잔잔)
                    table2 = 1  # 1:경멸_역겨움 테이블

                    # table3 : 2순위 감정에 해당하는 감정 테이블
                    table3 = self.table_idx.index(rank_emotion[1])
                    if table3 == 4:
                        table3 = 41

                elif rank_emotion[0] == "sadness":
                    table1 = 42  # 슬픔(2)음악(너무슬퍼서 울고싶은)
                    table2 = 41  # 슬픔(1)음악(잔잔)
                    table3 = 43  # 슬픔(3)음악(기분전환)

                elif rank_emotion[0] == "surprise":
                    if surprise >= 0.8:
                        table1 = 5  # 5:놀람테이블
                        table2 = 5
                        table3 = 5
                    else:
                        table1 = 5

                        # table2 : 2순위 감정에 해당하는 감정 테이블
                        table2 = self.table_idx.index(rank_emotion[1])
                        if table2 == 4:
                            table2 = 41

                        # table3 : 3순위 감정에 해당하는 감정 테이블
                        table3 = self.table_idx.index(rank_emotion[2])
                        if table3 == 4:
                            table3 = 41

            return table1, table2, table3
        except Exception as e:
            return HttpResponse('please try again')


# Serializer를 이용한 Json 데이터 Response
class TestJson(APIView):
    def get_object(self, pk):
        try:
            return Analysis_Result.objects.get(pk=pk)
        except Exception as e:
            raise Http404

    def get(self, request, format=None):
        try:
            '''
            child_1 = self.get_object(24)
            child_2 = self.get_object(25)
            serializer = ChildSerializer(child_1)
            serializer = ChildSerializer(child_2)
            '''
            analysis_result = self.get_object(1)
            serializer = Analysis_ResultSerializer(analysis_result)
            return Response(serializer.data)

        except Exception as e:
            return HttpResponse('please try again')


# Microsoft Face API를 이용한 감정분석
class RequestFaceAPI(APIView):

    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.result_age = 0.0
        self.result_emotion = []
        self.user_id = ''

    def get_data_from_faces(self, faces):
        #global face_api_age

        json_string = str(faces).replace("\'", "\"")

        dict_data = json.loads(json_string)  # Unicode decode Error
        emotions = dict_data['faceAttributes']['emotion']
        sorted_emotions = sorted(emotions.items(), key=operator.itemgetter(1), reverse=True)

        # 결과값이 0.0 이상인 tuple 만 list 에 저장한다.
        result_emotion = [emotion for emotion, value in sorted_emotions if value > 0]
        self.result_emotion = result_emotion

        # 추천 알고리즘에 적용할 age 추출
        result_age = dict_data['faceAttributes']['age']
        # self.result_age = result_age

        #face_api_age = result_age
        f = open('face_api_age.txt', 'w')
        json_val = json.dumps(result_age)
        f.write(json_val)
        f.close()

    # 어플에서 이미지를 받았을 때
    def post(self, request, format=None):
        # global face_api_emotion
        #self.user_name = request.POST.get('username', '') id 받아서 Analysis_Result에 넣어줘야 함.
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
            print(faces)
            #face_api_emotion = faces

            f = open('face_api_emotion.txt','w')
            json_val = json.dumps(faces[0])
            f.write(json_val)
            f.close()
            

            # process_file('face_api_emotion','w')

            if not faces:
                return HttpResponse('please try again')
            else:
                self.get_data_from_faces(faces[0])
                print('self.result_emotion : ', str(self.result_emotion))
                return HttpResponse(str(self.result_emotion))

        except Exception as e:
            print(e)
            return HttpResponse('please try again')

    def get(self, request, format=None):
        return HttpResponse("Using Microsoft Face API")


class Call(APIView):

    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.label = ["angry", "calm", "fearful", "happy", "sad", "angry", "calm", "fearful", "happy", "sad"]

    def post(self, request, format=None):
        try:
            audio_file = request.FILES.get('audio', '')
            path = default_storage.save('file.wav', ContentFile(audio_file.read()))
            print(path)
            print(self.labelfrommodel('./media/{}'.format(path)))
        except Exception as e:
            return HttpResponse('please try again')
        return HttpResponse(self.labelfrommodel('./media/{}'.format(path)))

    def get(self, request, format=None):
        return HttpResponse("Using Speech-Emotion-Model")

    def labelfrommodel(self, filename):
        #global speech_api_emotion
        json_file = open('model.json', 'r')
        loaded_model_json = json_file.read()
        json_file.close()
        loaded_model = model_from_json(loaded_model_json)
        # load weights into new model
        try:
            loaded_model.load_weights("Emotion_Voice_Detection_Model.h5")
        except Exception as e:
            pass

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

        #speech_api_emotion = self.label[int(liveabc)]
        
        f=open('speech_api_emotion.txt', 'w')
        json_val = json.dumps(self.label[int(liveabc)])
        f.write(json_val)
        f.close()

        return self.label[int(liveabc)]


'''
azure 키 
1. 86ad6a50a2af46189c45fc51819f4d9b
2. b97ea531a0b04569856d75fba76141d7
'''
