"""

어플의 전체적인 흐름에 관여하는 코드입니다.

어플 시나리오 
1. 베이맥스가 인사한다.
2. 사진을 찍는다 -> 서버에 보냄
3. 음성을 녹음한다 -> 서버에 보냄
4. 서버에서 결과가 0 이상인 감정결과를 받는다.
5. 서버에서 youtube url과 노래제목,가수이름과 tag를 받는다.

"""

import logging
import random
import operator
import json
import os
import tensorflow as tf

from django.http.response import HttpResponse
from django.db.models import Max
from django.db import connection
from django.core.files.storage import default_storage
from django.core.files.base import ContentFile
from django.conf.urls import *
from django.forms.models import model_to_dict

from rest_framework.views import APIView
from rest_framework.response import Response

#import cognitive_face as CF
import librosa
import numpy as np
import pandas as pd
from keras.models import model_from_json

from .serializers import *
from .models import *
from .real_time_video import *

from mmm_project.core import httpResponse, httpError
from mmm_project.core import fileIO

logger = logging.getLogger('default')


class RequestFaceAPI(APIView):
    """
    Microsoft Face API를 이용한 감정분석
    """

    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        # self.result_age = 0.0
        self.result_emotion = []
        self.user_id = ''

    # api로부터 받은 결과로부터 데이터 추출
    def get_data_from_faces(self, request, faces):
        try:
            json_string = str(faces).replace("\'", "\"")

            dict_data = json.loads(json_string)
            # emotions = dict_data['faceAttributes']['emotion']
            emotions = dict_data
            logger.debug(emotions)
            sorted_emotions = sorted(
                emotions.items(), key=operator.itemgetter(1), reverse=True)

            # 결과값이 0.0 이상인 tuple 만 list 에 저장
            result_emotion = [emotion for emotion,
                            value in sorted_emotions if value > 0]
            self.result_emotion = result_emotion
            logger.debug(self.result_emotion)

            # 추천 알고리즘에 적용할 age 추출
            ### result_age = dict_data['faceAttributes']['age']
            # self.result_age = result_age

            # if (!result_emotion || !result_age):
            #     return httpError

            #fileIO.write_file(request, 'face_api_age.txt', result_age)
        except Exception as e:
            logger.error(e)
            return httpError.serverError(reqeust, "Can't Get Data From Faces")

    def post(self, request):
        """
        어플에서 이미지를 받았을 때
        """
        try:
            """
            Microsoft Face API에서 https://github.com/omar178/Emotion-recognition 로 face api 변경
            """
        #     KEY = 'ecd8a803ef3c4a57bbd01b909e90e151'  # API_KEY
        #     CF.Key.set(KEY)

        #     BASE_URL = 'https://koreacentral.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,emotion&recognitionModel=recognition_01&returnRecognitionModel=false '  # Replace with your regional Base URL
        #     CF.BaseUrl.set(BASE_URL)

        #     data_test = request.FILES.get('photo', '')

        #     try:
        #         faces = CF.face.detect(data_test)
        #         logger.debug(faces)
        #     except Exception as e:
        #         logger.error(e)
        #         return httpError.serverError(request, "FaceAPI Connection Error")
            
            image_file = request.FILES.get('photo', '')

            try:
                path = default_storage.save(
                    'image.png', ContentFile(image_file.read()))
                logger.debug(path)
                faces = face_recognition(request, './media/{}'.format(path))  # replace path
                logger.debug(faces)

                os.remove('./media/{}'.format(path))
            except Exception as e:
                logger.error(e)
                return httpError.serverError(request, "FaceAPI Connection Error")

            fileIO.write_file(request, 'face_api_emotion.txt', str(faces))

            if not faces:
                return httpResponse.noContent(request, 'Face is not detected')
            else:
                self.get_data_from_faces(request, faces)
                return httpResponse.ok(request, str(self.result_emotion).replace('\"',''))

        except Exception as e:
            logger.error(e)
            return httpError.serverError(request, 'Face API Error')

    def get(self, request):
        return httpResponse.ok(request, "Using Microsoft Face API")


def load_model():
    global loaded_model
    json_file = open('model.json', 'r')
    loaded_model_json = json_file.read()
    json_file.close()
    loaded_model = tf.keras.models.model_from_json(loaded_model_json)
    global graph
    graph = tf.get_default_graph()

class Call(APIView):
    """
    Speech-Emotion-Analyzer 를 이용한 감정분석
    """

    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.label = ["angry", "calm", "fearful", "happy",
                      "sad", "angry", "calm", "fearful", "happy", "sad"]  # 감정 분류

    def post(self, request):
        try:
            audio_file = request.FILES.get('audio', '')
            path = default_storage.save(
                'file.wav', ContentFile(audio_file.read()))
            logger.debug(path)
            label = self.labelfrommodel(request, './media/{}'.format(path))

            logger.debug(label)
            os.remove('./media/{}'.format(path))
        except Exception as e:
            logger.error(e)
            return httpError.serverError(request, 'SEA getting label Error')
        return httpResponse.created(request, label)

    def get(self, request):
        return httpResponse.ok(request, "Using Speech-Emotion-Model")

    def labelfrommodel(self, request, filename):
        # json_file = open('model.json', 'r')
        # loaded_model_json = json_file.read()
        # json_file.close()
        # loaded_model = model_from_json(loaded_model_json)
        load_model()
    
        try:
            # load weights into new model
            # if not .wav file, throw Exception
            loaded_model.load_weights("Emotion_Voice_Detection_Model.h5")
        except Exception as e:
            logger.error(e)
            return httpError.serverError(request, "SEA Model Error")

        with graph.as_default():
            try: 
                X, sample_rate = librosa.load(filename, res_type='kaiser_fast', duration=2.5,
                                            sr=22050 * 2, offset=0.5)
                sample_rate = np.array(sample_rate)
                mfccs = np.mean(librosa.feature.mfcc(
                    y=X, sr=sample_rate, n_mfcc=13), axis=0)

                featurelive = mfccs
                livedf2 = featurelive
                livedf2 = pd.DataFrame(data=livedf2)
                livedf2 = livedf2.stack().to_frame().T
                twodim = np.expand_dims(livedf2, axis=2)
                livepreds = loaded_model.predict(twodim, batch_size=32, verbose=1)
                livepreds1 = livepreds.argmax(axis=1)
                liveabc = livepreds1.astype(int).flatten()

                fileIO.write_file(request, 'speech_api_emotion.txt',
                                self.label[int(liveabc)])

                return self.label[int(liveabc)]
            except Exception as e:
                logger.error(e)
                return httpError.serverError(request, "Tensor graph Error")


class RecommendationMusic(APIView):
    """
    최종적으로 음악을 추천
    """

    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.table_idx = ["angry", "disgust", "scared",
                          "happy", "sad", "surprised"]
        self.tone_res = ''
        self.music_list = []  # 리스트 안의 dictionary 형태로 들어온다. (music, url)
        self.age = random.randint(0, 150)
        self.user_id = ''
        self.recommand_table = []

    def post(self, request):
        """
        recommand_music 함수를 호출해 추천받은 노래를 Response 해주는 함수\
        face api를 변경하여 age값은 추출하지 않으므로 랜덤으로 돌린다.
        """
        try:
            self.user_id = request.POST.get('recommand','')
            # self.age = fileIO.read_file(request, 'face_api_age.txt')
            self.music_list = self.recommand_music(request, self.age)
            #self.music_list = self.recommand_music(request, 24)

            logger.debug(self.music_list)

            if(self.age <10):
                obj1= dict({
                    "music": self.music_list[0]["music"][0],
                    "link": self.music_list[0]["link"][0],
                })
                obj2 = dict({
                    "music": self.music_list[0]["music"][1],
                    "link": self.music_list[0]["link"][1],
                })
                obj3 = dict({
                    "music": self.music_list[0]["music"][2],
                    "link": self.music_list[0]["link"][2],
                })
                result = []
                result.append(obj1)
                result.append(obj2)
                result.append(obj3)
                print(result)
                
                # response_data = ChildSerializer(self.music_list, many=True)
                return httpResponse.ok(request, result)

            else :
                result = []
                result.append(model_to_dict(self.music_list[0]))
                result.append(model_to_dict(self.music_list[1]))
                result.append(model_to_dict(self.music_list[2]))

                # response_data = AngerSerializer(self.music_list, many=True)
                # result = json.dumps(self.music_list, ensure_ascii=False)
                return httpResponse.ok(request, result)

        except Exception as e:
            logging.error(e)
            return httpError.serverError(request, 'Recommandation Error')

    def create_Analysis_Result(self, request,  music_list):
        """
        어플에서 받은 id값과 데이터베이스에서 받은 값(music list)을 INSERT 한다.
        """
        try:
            max_id = Analysis_Result.objects.all().aggregate(
                max_id=Max("id"))['max_id']
            if max_id:
                queryset = Analysis_Result.objects.create(id=max_id + 1, user_id=self.user_id,
                                                          music_1=music_list[0],music_2=music_list[1], music_3=music_list[2],
                                                          link_1=link_list[0],link_2=link_list[1],link_3=link_list[2])
            else:
                queryset = Analysis_Result.objects.create(id=1, user_id=self.user_id,
                                                          music_1=music_list[0],music_2=music_list[1], music_3=music_list[2],
                                                          link_1=link_list[0],link_2=link_list[1],link_3=link_list[2])
            queryset.save()

            logger.debug(connection.queries[-1])
            # 모델 클래스의 오브젝트 갯수확인
            logger.debug(Analysis_Result.objects.all().count())

        except Exception as e:
            logger.error(e)
            return httpError.serverError(request, "Can't Create Analysis_Result")

    def get_random3(self, request, table):
        """
        하나의 데이터베이스에서 랜덤으로 레코드 3개를 받아온다.
        """
        try:
            Music = {}
            max_id = table.objects.all().aggregate(max_id=Max("id"))['max_id']
            dictionary_list = []
            music_list = []  # 노래제목-가수 리스트
            link_list = []  # Youtub 링크 리스트
            tag_list = []  # 노래에 해당하는 태그 리스트
            # pk를 랜덤으로 가져오면 겹칠 수 있으므로, 뽑은 pk를 list에 저장하고 그 list에 없는 값만 append한다.
            pk_list = []
            count = 0
            while True:
                if count == 3:
                    break

                pk = random.randint(1, max_id)
                if pk in pk_list:
                    continue
                pk_list.append(pk)

                music = table.objects.filter(id=pk).first()
                logger.debug('music.music : {}'.format(music.music))  # 노래제목-가수
                logger.debug('music.link : {}'.format(music.link))  # Youtube 링크

                if music:
                    music_list.append(music.music)
                    link_list.append(music.link)
                    # tag_list.append((music.tag1, music.tag2))
                    count += 1

            self.create_Analysis_Result(request, music_list, link_list)

            Music['music'] = music_list
            Music['link'] = link_list
            # Music['tag'] = tag_list
            dictionary_list.append(Music)  # list of dictionary
            logger.debug('dictionary_list from get_random3 {}'.format(dictionary_list))
            return dictionary_list
        except Exception as e:
            logger.error(e)
            return httpError.serverError(request, "Can't Get Randomly 3 Music")

    # # randomly get three music of DB (test)
    # def get_random3_test(self, request, table):
    #     try:
    #         Music = {}
    #         max_id = table.objects.all().aggregate(max_id=Max("id"))['max_id']
    #         dictionary_list = []
    #         pk_list = []
    #         count = 0
    #         while True:
    #             print('loop start')
    #             if count == 3:
    #                 break
    #             print('pk before')
    #             pk = random.randint(1, max_id)  # pk can equal, have to correct
    #             print('pk after')
    #             print(pk)
    #             if pk in pk_list:
    #                 continue
    #             pk_list.append(pk)
    #             print(pk_list)
    #             music = table.objects.filter(id=pk).first()
    #             print('music.music : ', music.music)
    #             print('music.link : ', music.link)
    #             if music:
    #                 Music['music_{}'.format(len(pk_list))] = music.music
    #                 Music['link_{}'.format(len(pk_list))] = music.link
    #                 count += 1

    #         dictionary_list.append(Music)  # list of dictionary
    #         return dictionary_list
    #     except Exception as e:
    #         logger.error(e)
    #         return httpError.serverError(request, "Can't Get Random3 Test")

    def recommand_music(self, request, age):
        """
        추천 알고리즘을 통해 얻은 3개의 레코드를 받아온다.
        """
        try:
            dictionary_list = []
            music_list = []
            link_list = []
            # 10세 미만이면 어린이 테이블로 가서 랜덤으로 3개의 동요를 뽑고 바로 종료한다.
            if age < 10:
                dictionary_list = self.get_random3(request, Child)

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
                table_index = [Anger, Disgust, Fear,
                               Happiness, Sadness, Surprise, Lie]

                self.face = fileIO.read_file(request, 'face_api_emotion.txt')
                self.speech= fileIO.read_file(request,'speech_api_emotion.txt')
                
                tone = self.speech.splitlines()[0].replace("\"","")

                # 추천알고리즘에서 리턴하는 테이블에 대한 정보 (노래 3개->3개의 요소)
                t_info = list(self.get_table_name(request, self.face, tone))
                self.recommand_table = t_info
                print('t_info is : ', t_info)

                if t_info=="tone_res err":
                    return httpError.serverError(request, "Don't Match Sppech Result")

                for tableN in t_info:
                    if tableN in [41, 42, 43]:  # Sad테이블에 접근해야하는 경우
                        table = table_index[4]  # Sad
                        subclass_sad = tableN % 40  # subclass : 1 or 2 or 3
                        music = table.objects.filter(subclass_s=subclass_sad).order_by(
                            '?').first()  # Sad테이블 중 subclass_s가 subclass인 노래들 중 랜덤 선택

                    elif tableN == 6:
                        table = table_index[6]

                    else:  # Sad테이블 이외의 테이블에 접근해야하는 경우
                        table = table_index[tableN]

                    music = table.objects.order_by('?').first()  # 정해진 테이블에서 랜덤으로
                    if music:
                        music_list.append(music.music)
                        link_list.append(music.link)
                        dictionary_list.append(music)
                    else:
                        httpError.serverError(request, "Can't Get Table Objects")

                self.create_Analysis_Result(request, music_list, link_list)
            return dictionary_list
        except Exception as e:
            logger.error(e)
            return httpError.serverError(request, "Can't Recommand music")

    # 이미지, 음성 감정 분석 결과를 이용해 접근할 테이블을 결정한다.
    def get_table_name(self, request, face, tone):
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
                return "tone_res err"

            # parse json data (input format : .json file)
            json_string = str(face).replace("\'", "\"")
            json_load = json.loads(json_string) 

            # dic = json_load['faceAttributes']['emotion']
            dic = json_load

            anger = dic['angry']
            # contempt = dic['contempt']
            disgust = dic['disgust']
            fear = dic['scared']
            happiness = dic['happy']
            neutral = dic['neutral']
            sadness = dic['sad']
            surprise = dic['surprised']

            ### face api 변경
            # # 전처리 - 6개의 감정으로 만들기 위해, 우선 contempt와 disgust를 합친다.
            # contempt_disgust = contempt + disgust
            # dic["disgust"] = contempt_disgust

            # 중립이 0.8 이상이면 3개의 랜덤 테이블을 선택한다. (0에서 5까지 중 랜덤 정수)
            if neutral > 0.8:
                table1 = random.randint(0, 5)
                table2 = random.randint(0, 5)
                table3 = random.randint(0, 5)
                return table1, table2, table3

            # 전처리 - 0.05미만인 값은 0.0으로 스무딩한다.
            else:
                for emotion, rate in dic.items():
                    if rate < 0.05:
                        dic[emotion] = 0.0
                        # print('smoothing dictionary : ', dic)
                
                anger = dic['angry']
                # contempt = dic['contempt']
                disgust = dic['disgust']
                fear = dic['scared']
                happiness = dic['happy']
                neutral = dic['neutral']
                sadness = dic['sad']
                surprise = dic['surprised']

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
                # del dic["contempt"]

                emotion_res = sorted(dic.items(), key=operator.itemgetter(
                    1), reverse=True)  # value 기준 내림차순 정렬
                print("정렬후")
                print('sorted dictionary to list of tuple : ', emotion_res)

                rank_emotion = []  # 인덱스 순서대로 높은 순위 감정이다. 3순위 감정까지 저장한다.
                rank_emotion_cnt = 0
                for emotion, rate in emotion_res:
                    if rate > 0:
                        rank_emotion.append(emotion)
                        rank_emotion_cnt += 1
                        if rank_emotion_cnt > 3:
                            break

                print(rank_emotion)
                if rank_emotion_cnt == 1:
                    rank_emotion.append(rank_emotion[0])
                    rank_emotion.append(rank_emotion[0])
                elif rank_emotion_cnt == 2:
                    rank_emotion.append(rank_emotion[0])

                print(rank_emotion)

                if rank_emotion[0] == "angry":
                    table1 = 41  # 4:슬픔테이블 41:슬픔(1)음악(잔잔)
                    table2 = 0  # 0:화남테이블

                    # table3 : '2순위 감정'에 해당하는 감정 테이블
                    table3 = self.table_idx.index(rank_emotion[1])
                    if table3 == 4:  # 2순위가 SADNESS라면, 41인 슬픔(1)음악(잔잔)을 선택한다.
                        table3 = 41

                elif rank_emotion[0] == "scared":
                    table1 = 2  # 2:공포테이블
                    table2 = 41  # 슬픔(1)음악(잔잔)

                    # table3 : 2순위 감정에 해당하는 감정 테이블
                    table3 = self.table_idx.index(rank_emotion[1])
                    if table3 == 4:
                        table3 = 41

                elif rank_emotion[0] == "happy":
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

                elif rank_emotion[0] == "sad":
                    table1 = 42  # 슬픔(2)음악(너무슬퍼서 울고싶은)
                    table2 = 41  # 슬픔(1)음악(잔잔)
                    table3 = 43  # 슬픔(3)음악(기분전환)

                elif rank_emotion[0] == "surprised":
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
            logger.error(e)
            return httpError.serverError(request, "Can't Get Table Name")


class ResultResponse(APIView):
    """
    Serializer를 이용한 Json 데이터 Response
    """
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.user_id = ''

    def get_object(self, request, user_id):
        try:
            return Analysis_Result.objects.filter(user_id=user_id)
        except Exception as e:
            logger.error(e)
            return httpError.notFoundError(request, "Not Found" )

    def post(self, request, format=None):
        try:
            self.user_id = request.POST.get('result','')
            analysis_result = self.get_object(request, self.user_id)
            serializer = Analysis_ResultSerializer(analysis_result, many = True)
            print('data',serializer.data)
            return httpResponse.ok(request, serializer.data)

        except Exception as e:
            logger.error(e)
            return httpError.serverError(request, 'please try again')