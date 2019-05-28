# 클래스 기반 뷰
from django.http import Http404, HttpResponseNotFound
from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status
from django.db.models import Max

from django.http.response import HttpResponse
import random

from .models import *
from .serializers import *

import operator
import json

class Recommendation(APIView):
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self._table_idx = ["anger", "disgust", "fear", "happiness", "sadness", "surprise"]
        self.tone_res = ''
        self.music_list = []
        self.age = 20 #default. will be removed
        
    def get(self, request, format=None):
        try:
            music_url_list = []
            self.music_list = self.get_random3(Child, music_url_list)
            #self.music_list = self.recommend_music(self.age)
                
            result = json.dumps(self.music_list, ensure_ascii = False)
                
        except Exception as e:
            print(e)
              
        return Response(result)
        
        
    # randomly get three music of DB
    def get_random3(self, table, music_list):
        try:
            Music = { }
            max_id = table.objects.all().aggregate(max_id=Max("id"))['max_id']
            music_list = []
            count = 0
            while True:
                if count == 3 :
                    break
                pk = random.randint(1, max_id) # pk can equal, have to correct
                print(pk)
                music = table.objects.filter(id=pk).first()
                print('music.music : ' , music.music)
                print('music.link : ', music.link)
                if music:
                    print('loop start')
                    Music['music_{}'.format(count+1)] = music.music
                    Music['link_{}'.format(count+1)] = music.link
                    count += 1
            music_list.append(Music) # list of dictionary
            print(music_list)
            return music_list
        except Exception as e :
            print(e)
    
    def get_table_name(self, face, tone):

        #테이블 인덱스를 리스트에 저장한다.
        table1 = 100
        table2 = 100
        table3 = 100
        
        #tone 결과를 face 결과형식와 맞춰줌 : angry->anger / calm->neutral / fearful->fear / happy->happiness / sad->sadness
        if tone == "angry" :
            self.tone_res = "anger"
        elif tone == "calm" :
            self.tone_res = "neutral"
        elif tone == "fearful" :
            self.tone_res = "fear"
        elif tone == "happy" :
            self.tone_res = "happiness"
        elif tone == "sad" :
            self.tone_res = "sadness"
        else :
            return HttpResponse("tone_res err")

        #parse json data (input format : .json file)
        json_load = json.load(face)
        dic = json_load['faceAttributes']['emotion']
        
        anger = dic['anger']
        contempt = dic['contempt']
        disgust = dic['disgust']
        fear = dic['fear']
        happiness = dic['happiness']
        neutral = dic['neutral']
        sadness = dic['sadness']
        surprise = dic['surprise']

        #전처리 - 6개의 감정으로 만들기 위해, 우선 contempt와 disgust를 합친다. 
        contempt_disgust = contempt + disgust
        dic["disgust"] = contempt_disgust #? 
        #print(dic)

        #중립이 0.8 이상이면 3개의 랜덤 테이블을 선택한다. (0에서 5까지 중 랜덤 정수)
        if neutral > 0.8 :
            table1 = random.randint(0,6) 
            table2 = random.randint(0,6) 
            table3 = random.randint(0,6)
            return table1, table2, table3

        #전처리 - 0.05미만인 값은 0.0으로 스무딩한다. 
        else :
            for emotion, rate in dic.items() :
                if rate < 0.05 :
                    dic[emotion] = 0.0 
            #print(dic)

        #전처리 - 거짓말인 경우, 거짓말 테이블 선택
        if tone_res == "sadness" and happiness > 0 :
            print("행복 얼굴로 슬픈 목소리를 내는 거짓말쟁이1")
            table1 = 6
            table2 = 6
            table3 = 6 
            return table1, table2, table3

        elif tone_res == "happiness" and sadness > 0 :
            print("슬픈 얼굴로 행복 목소리를 내는 거짓말쟁이2")
            table1 = 6
            table2 = 6
            table3 = 6
            return table1, table2, table3

        #전처리 (1) 중립에 의한 랜덤 테이블 (2) 거짓말테이블 -> 둘 다 해당하지 않으면 1,2,3순위 감정에 따른 알고리즘 처리 적용  
        else :
            del dic['neutral']
            del dic["contempt"]

            emotion_res = sorted(dic.items(), key=operator.itemgetter(1), reverse=True) #value 기준 내림차순 정렬
            #print("정렬후")
            #print(emotion_res)
            rank_emotion = [] #인덱스 순서대로 높은 순위 감정이다. 3순위 감정까지 저장한다.
            rank_emotion_cnt = 0
            for emotion, rate in emotion_res :
                if rate > 0 :
                    rank_emotion.append(emotion)
                    rank_emotion_cnt += 1
                    if rank_emotion_cnt > 3 :
                        break

            #print(rank_emotion)
            if rank_emotion_cnt == 1 :
                rank_emotion.append(rank_emotion[0])
                rank_emotion.append(rank_emotion[0])
            elif rank_emotion_cnt == 2:
                rank_emotion.append(rank_emotion[0])

            #print(rank_emotion)

            if rank_emotion[0] == "anger" :
                table1 = 41 #4:슬픔테이블 41:슬픔(1)음악(잔잔)  
                table2 = 0  #0:화남테이블

                #table3 : '2순위 감정'에 해당하는 감정 테이블
                table3 = table_idx.index(rank_emotion[1])
                if table3 == 4 : #2순위가 SADNESS라면, 41인 슬픔(1)음악(잔잔)을 선택한다. 
                    table3 = 41

            elif rank_emotion[0] == "fear" :
                table1 = 2 #2:공포테이블
                table2 = 41 #슬픔(1)음악(잔잔)

                #table3 : 2순위 감정에 해당하는 감정 테이블
                table3 = table_idx.index(rank_emotion[1])
                if table3 == 4 :
                    table3 = 41

            elif rank_emotion[0] == "happiness" :
                if happiness >= 0.8 :
                    table1 = 3 #3:행복테이블
                    table2 = 3
                    table3 = 3 
                else :
                    table1 = 3

                    #table2 : 2순위 감정에 해당하는 감정 테이블
                    table2 = table_idx.index(rank_emotion[1])
                    if table2 == 4 :
                        table2 = 41

                    #table3 : 3순위 감정에 해당하는 감정 테이블
                    table3 = table_idx.index(rank_emotion[2])
                    if table3 == 4 :
                        table3 = 41

            elif rank_emotion[0] == "disgust" :
                table1 = 41 #슬픔(1)음악(잔잔)
                table2 = 1  #1:경멸_역겨움 테이블

                #table3 : 2순위 감정에 해당하는 감정 테이블
                table3 = table_idx.index(rank_emotion[1])
                if table3 == 4 :
                    table3 = 41

            elif rank_emotion[0] == "sadness" :
                table1 = 42 #슬픔(2)음악(너무슬퍼서 울고싶은)
                table2 = 41 #슬픔(1)음악(잔잔)
                table3 = 43 #슬픔(3)음악(기분전환)

            elif rank_emotion[0] == "surprise" :
                if SURPRISE >= 0.8 :
                    table1 = 5 #5:놀람테이블
                    table2 = 5
                    table3 = 5 
                else :
                    table1 = 5

                    #table2 : 2순위 감정에 해당하는 감정 테이블
                    table2 = table_idx.index(rank_emotion[1])
                    if table2 == 4 :
                        table2 = 41

                    #table3 : 3순위 감정에 해당하는 감정 테이블
                    table3 = table_idx.index(rank_emotion[2])
                    if table3 == 4 :
                        table3 = 41

        return table1, table2, table3

    def recommend_music(self, age):
            music_list = []
            #10세 미만이면 어린이 테이블로 가서 랜덤으로 3개의 동요를 뽑고 바로 종료한다. 
            if age<10 : 
                music_list = self.get_random3(Child, music_list)

            #어린이 나이가 아니라면, 추천 알고리즘에 의해 감정까지 고려한 노래 3개를 뽑는다.
            else :
                #연령대에 따라서 age를 결정한다. 
                if age>=10 and age<=29 : 
	                age = 20
                elif age>=30 and age<=39 : 
	                age = 30
                else :
	                age = 40

                #추천알고리즘에서 리턴하는 테이블 인덱스를 리스트 인덱스로 접근할 수 있도록 table_index 리스트 생성 
                table_index = [Anger, Disgust, Fear, Happiness, Sadness, Surprise, Lie]

                #추천알고리즘에서 리턴하는 테이블에 대한 정보 (노래 3개->3개의 요소)
                t_info = list(self.get_table_name(face, tone))

                for tableN in t_info : 
	                if tableN in [41, 42, 43] : #Sad테이블에 접근해야하는 경우
		                table = table_index[4] #Sad
		                subclass_sad = tableN % 40 # subclass : 1 or 2 or 3
		                music = table.objects.filter(subclass_s = subclass_sad).random() #Sad테이블 중 subclass_s가 subclass인 노래들 중 랜덤 선택
		                
	                else : #Sad테이블 이외의 테이블에 접근해야하는 경우
		                table = table_index[tableN]

	                music = table.objects.filter(age = age).random() #해당 연령 범위의 노래로 추림
	                music_list.append(music)

             

            return music_list


