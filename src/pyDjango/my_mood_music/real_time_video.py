from keras.preprocessing.image import img_to_array
import imutils
import cv2
from keras.models import load_model
import numpy as np
from keras.backend import clear_session
import tensorflow as tf
import logging
from mmm_project.core import httpError
logger = logging.getLogger('default')

# parameters for loading data and images
detection_model_path = 'C:/Users/X58/Documents/GitHub/2019-cap1-2019_10/src/pyDjango/haarcascade_frontalface_default.xml' # replace path
emotion_model_path = 'C:/Users/X58/Documents/GitHub/2019-cap1-2019_10/src/pyDjango/_mini_XCEPTION.102-0.66.hdf5' # replace path

# hyper-parameters for bounding boxes shape
# loading models
face_detection = cv2.CascadeClassifier(detection_model_path)
emotion_classifier = load_model(emotion_model_path, compile=False)
EMOTIONS = ["angry" ,"disgust","scared", "happy", "sad", "surprised",
 "neutral"]

def load_model():
    global emotion_classifier
    emotion_classifier = tf.keras.models.load_model(emotion_model_path)
    global graph
    graph = tf.get_default_graph()

def face_recognition(request,image):
    load_model()
    # clear_session()
    frame = cv2.imread(image, cv2.IMREAD_COLOR)
    #reading the frame
    try:
        frame = imutils.resize(frame,width=500)
        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    except Exception as e:
        logger.error(e)
        httpError.serverError(request, "Can't Frame Resize ")
        
    with graph.as_default():

        faces = face_detection.detectMultiScale(gray,scaleFactor=1.1,minNeighbors=5,minSize=(30,30),flags=cv2.CASCADE_SCALE_IMAGE)
        
        canvas = np.zeros((250, 300, 3), dtype="uint8")
        frameClone = frame.copy()
        if len(faces) > 0:
            faces = sorted(faces, reverse=True,
            key=lambda x: (x[2] - x[0]) * (x[3] - x[1]))[0]
            (fX, fY, fW, fH) = faces
                        # Extract the ROI of the face from the grayscale image, resize it to a fixed 28x28 pixels, and then prepare
                # the ROI for classification via the CNN
            roi = gray[fY:fY + fH, fX:fX + fW]
            roi = cv2.resize(roi, (64, 64))
            roi = roi.astype("float") / 255.0
            roi = img_to_array(roi)
            roi = np.expand_dims(roi, axis=0)
            
            
            preds = emotion_classifier.predict(roi)[0]
            emotion_probability = np.max(preds)
            label = EMOTIONS[preds.argmax()]

        emotion_result = {}
        for (i, (emotion, prob)) in enumerate(zip(EMOTIONS, preds)):
                    # construct the label text
                    text = "{}: {:.2f}%".format(emotion, prob * 100)
                    emotion_result[emotion] = prob

    return emotion_result