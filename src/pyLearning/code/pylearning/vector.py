import glob
import librosa  #librosa를 이용하여 39차 MFCCs벡터를 추출한다.
import numpy as np

def extract_feature(file_name):
    X, sample_rate = librosa.load(file_name)
    mfccs = librosa.feature.mfcc(y=X, sr=sample_rate, n_mfcc=39,
                                hop_length = int(sample_rate*0.01),
                                n_fft= int(sample_rate*0.02)).T
    print("mfcc :", mfccs, "\n", mfccs.shape)
    return mfccs

wav_files = []

for i in range(8):
    all_mfccs = []
    files = glob.glob('~~~.wav' % i)#내 파일명형식은?? wav파일 다 불러오기
    col = len(files)
    all_mfccs = np.ndarray(shape=[0,39], dtype=np.float32)
    print(all_mfccs.shape)
    print(files)

    for isfile in files :
        print(isfile)
        feature = extract_feature(isfile)
        print(feature.shape)
        all_mfccs = np.append(all_mfccs, feature, axis=0)
        print(isfile, "is done")

    print(all_mfccs.shape)
    np.savez('%d' %i, X=all_mfccs)


