from django.shortcuts import render
from django.http import HttpResponse
from keras.utils import np_utils
from sklearn.preprocessing import LabelEncoder
import librosa
import numpy as np
import pandas as pd
import glob
import tensorflow as tf
from keras.models import model_from_json


def index(request):
    return HttpResponse("hello django")

def call(request):
    return HttpResponse(labelfrommodel('input.wav'))
    
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

def labelfrommodel(filename):
    
    json_file = open('model.json', 'r')
    loaded_model_json = json_file.read()
    json_file.close()
    loaded_model = model_from_json(loaded_model_json)
    # load weights into new model
    loaded_model.load_weights("Emotion_Voice_Detection_Model.h5")

    X, sample_rate = librosa.load( filename, res_type='kaiser_fast', duration=2.5,
                                  sr=22050*2, offset=0.5)
    sample_rate = np.array(sample_rate)
    mfccs = np.mean(librosa.feature.mfcc(y=X, sr=sample_rate, n_mfcc=13), axis=0)

    featurelive =mfccs
    livedf2 = featurelive
    livedf2 = pd.DataFrame(data=livedf2)
    livedf2 = livedf2.stack().to_frame().T
    twodim = np.expand_dims(livedf2, axis=2)
    livepreds = loaded_model.predict(twodim, batch_size=32, verbose = 1)
    livepreds1 = livepreds.argmax(axis=1)
    liveabc = livepreds1.astype(int).flatten()
    
    return label[int(liveabc)]






