from django.shortcuts import render
from django.http import HttpResponse
from keras.utils import np_utils
from sklearn.preprocessing import LabelEncoder
import librosa
import numpy as np
import pandas as pd
import glob
import os
import tensorflow as tf
from keras.models import model_from_json

from rest_framework.views import APIView
#from pprint import pprint
#import scipy.io.wavfile

from django.core.files.storage import default_storage
from django.core.files.base import ContentFile

class call(APIView):

    def post(self, request, format=None):
        #과제!! PSOT가 아닌 file로 어플리케이션에서 받아올 수 있도록 해야한다.
        #file = request.POST.get('my file data')
        #return HttpResponse(labelfrommodel(file))

        #과제!! 어플에서 byte array로 audiorecord file을 준다. 그것을 .wav로
        audio_file = request.FILES.get('audio','')
      
        #파일의 이름이 중복될때 다른이름으로 저장되지 않도록 기존의 file.wav를 삭제
        if os.path.isfile('./media/file.wav'):
          os.remove('./media/file.wav')
        else :
            path = default_storage.save('file.wav', ContentFile(audio_file.read()))
        #file.wav로 저장

        #b = numpy.array(audio_file, dtype=numpy.int32)///시도했지만 계속 typeError가 났음
        #file= scipy.io.wavfile.write(r"C:\Users\eunch\source\repos\DjangoWebProject1\DjangoWebProject1\myfile.wav", 
        #44100, b)
        #어플에서 보내는 audio파일의 frequency가 44100
        json_file = open('model.json', 'r')
        loaded_model_json = json_file.read()
        json_file.close()
        loaded_model = model_from_json(loaded_model_json)
        # load weights into new model
        loaded_model.load_weights("Emotion_Voice_Detection_Model.h5")
        return HttpResponse(labelfrommodel('file.wav',loaded_model))
        
    
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
    #원래 성별이 있지만 이미지인식으로 더 정확한 성별결과가 나오므로 음성에서는 굳이 제외
    #label index값을 지키기위해 중복되게 둠
]

def labelfrommodel(filename, load_model):
    
   

    X, sample_rate = librosa.load( filename, res_type='kaiser_fast', duration=2.5,
                                  sr=22050*2, offset=0.5)
    sample_rate = np.array(sample_rate)
    mfccs = np.mean(librosa.feature.mfcc(y=X, sr=sample_rate, n_mfcc=13), axis=0)
    #
    featurelive =mfccs
    livedf2 = featurelive
    livedf2 = pd.DataFrame(data=livedf2)
    livedf2 = livedf2.stack().to_frame().T
    twodim = np.expand_dims(livedf2, axis=2)
    livepreds = load_model.predict(twodim, batch_size=32, verbose = 1)
    livepreds1 = livepreds.argmax(axis=1)
    liveabc = livepreds1.astype(int).flatten()
    
    return label[int(liveabc)]






