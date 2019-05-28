from keras.utils import np_utils
from sklearn.preprocessing import LabelEncoder
import librosa
import numpy as np
import pandas as pd
import glob
import tensorflow as tf
from keras.models import model_from_json

def extract_feature(file_name):
    X, sample_rate = librosa.load(file_name, res_type='kaiser_fast', duration=3, offset=0.5)
    sample_rate = np.array(sample_rate)
    mfccs = np.mean(librosa.feature.mfcc(y=X, n_mfcc=25),axis=0)
    mfccs = -(mfccs / 100)
    print("mfccs :", mfccs,"\n",mfccs.shape)
    return mfccs
# extract_feature('./angry/angry.wav')

lb = LabelEncoder()
label = [
"female_angry",
"female_calm",
"female_fearful",
"female_happy",
"female_sad",
"male_angry",
"male_calm",
"male_fearful",
"male_happy",
"male_sad"
]

json_file = open('model.json', 'r')
loaded_model_json = json_file.read()
json_file.close()
loaded_model = model_from_json(loaded_model_json)
# load weights into new model
loaded_model.load_weights("Emotion_Voice_Detection_Model.h5")
print("Loaded model from disk")

X, sample_rate = librosa.load('화남.wav', res_type='kaiser_fast', duration=2.5,
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
print(label[int(liveabc)])
# 0 - female_angry
# 1 - female_calm
# 2 - female_fearful
# 3 - female_happy
# 4 - female_sad
# 5 - male_angry
# 6 - male_calm
# 7 - male_fearful
# 8 - male_happy
# 9 - male_sad
