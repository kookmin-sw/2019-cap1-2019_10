# -*- coding: cp949 -*-
import json
from random import *
import operator

'''parameter format (1) face : dictionary
                        anger neutral fear happiness sadness / surprise contempt disgust
                    (2) tone : string
                        angry calm fearful happy sad 
    *거짓말인 경우 : 행복<->슬픔만 넣을것인지
                   음성결과 1개가 얼굴 결과에 없으면 (중립이 문제가 될 것 같음)
                   하드코딩으로 행복 = [ㅁ,ㅁ] , 공포 = [ㅁ, ㅁ] ,,, 이렇게 케이스 다 넣어놓기
                   음성결과 1개랑 얼굴 결과 1순위를 비교하면 ?
                => 우리가 정하기 나름이긴 한데, 일단 행복 <->슬픔만 넣고 테스트하기로  

    * 0 : 감정 테이블 중 ANGER
      1 : 감정 테이블 중 CONTEMPT_DISGUST
      2 : 감정 테이블 중 FEAR
      3 : 감정 테이블 중 HAPPINESS
      4 : 감정 테이블 중 SADNESS
      5 : 감정 테이블 중 SURPRISE
      6 : 거짓말 테이블
     ** 동요테이블은 연령만 필요하므로, 파이썬 서버에서 맨 먼저 연령을 검사하고 영유아 연령이면 추천알고리즘이 필요없다.
'''

def recom(face, tone):
    #테이블 인덱스를 리스트에 저장한다.
    table_idx = ["ANGER", "CONTEMPT_DISGUST", "FEAR", "HAPPINESS", "SADNESS", "SURPRISE"]
    table1 = 100
    table2 = 100
    table3 = 100
 
    #tone 결과를 face 결과형식와 맞춰줌 : angry->anger / calm->neutral / fearful->fear / happy->happiness / sad->sadness
    tone_res = ""
    if tone == "angry" :
        tone_res = "ANGER"
    elif tone == "calm" :
        tone_res = "NEUTRAL"
    elif tone == "fearful" :
        tone_res = "FEAR"
    elif tone == "happy" :
        tone_res = "HAPPINESS"
    elif tone == "sad" :
        tone_res = "SADNESS"
    else :
        print("tone_res err")

    #parse json data (input format : .json file)
    with open("test.json") as json_file: 
        dic = json.load(json_file)

        anger = dic["ANGER"]
        happiness = dic["HAPPINESS"]
        sadness = dic["SADNESS"]
        neutral = dic["NEUTRAL"]
        fear = dic["FEAR"]
        surprise = dic["SURPRISE"]
        contempt = dic["CONTEMPT"]
        disgust = dic["DISGUST"]

        print(dic)
 
    #전처리 - 6개의 감정으로 만들기 위해, 우선 contempt와 disgust를 합친다. 
    contempt_disgust = contempt + disgust
    dic["CONTEMPT_DISGUST"] = contempt_disgust
    #print(dic)

    #중립이 0.8 이상이면 3개의 랜덤 테이블을 선택한다. (0에서 5까지 중 랜덤 정수)
    if neutral > 0.8 :
        table1 = random.randrange(0,6) 
        table2 = random.randrange(0,6) 
        table3 = random.randrange(0,6)
        return table1, table2, table3
        
    #전처리 - 0.05미만인 값은 0.0으로 스무딩한다. 
    else :
        for emotion, rate in dic.items() :
            if rate < 0.05 :
                dic[emotion] = 0.0 
        #print(dic)

    #전처리 - 거짓말인 경우, 거짓말 테이블 선택
    if tone_res == "SADNESS" and happiness > 0 :
        print("행복 얼굴로 슬픈 목소리를 내는 거짓말쟁이1")
        table1 = 6
        table2 = 6
        table3 = 6 
        return table1, table2, table3
        
    elif tone_res == "HAPPINESS" and sadness > 0 :
        print("슬픈 얼굴로 행복 목소리를 내는 거짓말쟁이2")
        table1 = 6
        table2 = 6
        table3 = 6
        return table1, table2, table3

    #전처리 (1) 중립에 의한 랜덤 테이블 (2) 거짓말테이블 -> 둘 다 해당하지 않으면 1,2,3순위 감정에 따른 알고리즘 처리 적용  
    else :
        del dic['NEUTRAL']
        del dic["CONTEMPT"]
        del dic["DISGUST"]

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

        if rank_emotion[0] == "ANGER" :
            table1 = 41 #4:슬픔테이블 41:슬픔(1)음악(잔잔)  
            table2 = 0  #0:화남테이블
            
            #table3 : '2순위 감정'에 해당하는 감정 테이블
            table3 = table_idx.index(rank_emotion[1])
            if table3 == 4 : #2순위가 SADNESS라면, 41인 슬픔(1)음악(잔잔)을 선택한다. 
                table3 = 41

        elif rank_emotion[0] == "FEAR" :
            table1 = 2 #2:공포테이블
            table2 = 41 #슬픔(1)음악(잔잔)
            
            #table3 : 2순위 감정에 해당하는 감정 테이블
            table3 = table_idx.index(rank_emotion[1])
            if table3 == 4 :
                table3 = 41

        elif rank_emotion[0] == "HAPPINESS" :
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

        elif rank_emotion[0] == "CONTEMPT_DISGUST" :
            table1 = 41 #슬픔(1)음악(잔잔)
            table2 = 1  #1:경멸_역겨움 테이블
            
            #table3 : 2순위 감정에 해당하는 감정 테이블
            table3 = table_idx.index(rank_emotion[1])
            if table3 == 4 :
                table3 = 41

        elif rank_emotion[0] == "SAD" :
            table1 = 42 #슬픔(2)음악(너무슬퍼서 울고싶은)
            table2 = 41 #슬픔(1)음악(잔잔)
            table3 = 43 #슬픔(3)음악(기분전환)

        elif rank_emotion[0] == "SURPRISE" :
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

if __name__ == "__main__" :
    #recom(1,1)
    db_table1, db_table2, db_table3 = recom(1,"angry") # json파일을 첫번째 파라미터로 넣게 수정 
    print(db_table1)
    print(db_table2)
    print(db_table3)
    
