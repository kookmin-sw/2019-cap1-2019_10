from django.shortcuts import render
from django.contrib.auth.models import User

from django import forms

from rest_framework import viewsets
from .serializers import *
from .models import *
from rest_framework import permissions
from rest_framework.generics import DestroyAPIView, GenericAPIView, ListAPIView, ListCreateAPIView, UpdateAPIView, \
    CreateAPIView
from rest_framework.authtoken.serializers import AuthTokenSerializer
from rest_framework import parsers
from rest_framework import renderers
from rest_framework.response import Response


# 함수형 뷰 
from rest_framework import status
from rest_framework.decorators import api_view
from rest_framework.response import Response


# 클래스 기반 뷰
from django.http import Http404
from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status

'''
# Use the Mixin
from rest_framework import mixins
from rest_framework import generics
'''

from rest_framework import generics

from django.http.response import HttpResponse
from django.views.decorators.csrf import csrf_exempt
import cognitive_face as CF

# # viewSet classes
# class UserViewSet(viewsets.ModelViewSet):
#     """
#     API endpoint that allows users to be viewed or edited.
#     """
#     queryset = User.objects.all().order_by('-date_joined')
#     serializer_class = UserSerializer
#
#
#     def get(self, request, *args, **kwargs):
#         return self.list(request, *args, **kwargs)
#
#
#
# class EmotionViewSet(viewsets.ModelViewSet):
#     queryset = Emotion_Information.objects.all().order_by('id')
#     serializer_class = EmotionSerializer
#     permision_class = (permissions.IsAuthenticated,)
#     lookup_fields = 'pk'
#
#     def perform_create(self, serializer):
#         serializer.save(user=self.request.user)


'''
api뷰를 래퍼로 감싸기 
class EmotionList(APIView):
	"""
	코드 조각을 모두 보여주거나 새 코드 조각을 만듭니다.
	"""
	def get(self, request, format=None):
		emotions = Emotion.objects.all()
		serializer = EmotionSerializer(emotions, many=True)
		return Response(serializer.data)

	def post(self, request, format=None):
		serializer = EmotionSerializer(data=request.data)
		if serializer.is_valid():
			serializer.save()
			return Response(serializer.data, status=status.HTTP_201_CREATED)
		return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)


class EmotionDetail(APIView):
	"""
	코드 조각 조회, 업데이트, 삭제
	"""
	def get_object(self, pk):
		try:
			return Emotion.objects.get(pk=pk)
		except Emotion.DoesNotExist:
			raise Http404

	def get(self, request, pk, format=None):
		emotion = self.get_object(pk)
		serializer = EmotionSerializer(emotion)
		return Response(serializer.data)

	def put(self, request, pk, format=None):
		emotion = self.get_object(pk)
		serializer = EmotionSerializer(emotion, data=request.data)
		if serializer.is_valid():
			serializer.save()
			return Response(serializer.data)
		return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

	def delete(self, request, pk, format=None):
		emotion = self.get_object(pk)
		emotion.delete()
		return Response(status=status.HTTP_204_NO_CONTENT)
'''

'''
# 믹스인 사용한 뷰 
class EmotionList(mixins.ListModelMixin,
                  mixins.CreateModelMixin,
                  generics.GenericAPIView):
    queryset = Emotion.objects.all()
    serializer_class = EmotionSerializer

    def get(self, request, *args, **kwargs):
        return self.list(request, *args, **kwargs)

    def post(self, request, *args, **kwargs):
        return self.create(request, *args, **kwargs)


        
class EmotionDetail(mixins.RetrieveModelMixin,
                    mixins.UpdateModelMixin,
                    mixins.DestroyModelMixin,
                    generics.GenericAPIView):
    queryset = Emotion.objects.all()
    serializer_class = EmotionSerializer

    def get(self, request, *args, **kwargs):
        return self.retrieve(request, *args, **kwargs)

    def put(self, request, *args, **kwargs):
        return self.update(request, *args, **kwargs)

    def delete(self, request, *args, **kwargs):
        return self.destroy(request, *args, **kwargs)

'''

from rest_framework.authentication import TokenAuthentication
from rest_framework.permissions import IsAuthenticated
from .permissions import IsOwnerOrReadOnly


class EmotionList(generics.ListCreateAPIView):
    queryset = Emotion_Information.objects.all()
    serializer_class = EmotionSerializer


class EmotionDetail(generics.RetrieveUpdateDestroyAPIView):
    authentication_classes = (TokenAuthentication,)
    permission_classes = (IsAuthenticated, IsOwnerOrReadOnly)  # 권한 옵션
    # 기존에는 토큰이 있는 사용자만 조회할 수 있었다.
    # 거기에 더해 소유자가 아닐 경우 수정은 불가능 하도록!

    queryset = Emotion_Information.objects.all()
    serializer_class = EmotionSerializer

import logging
logger = logging.getLogger(__name__)

# function views
def show_table(request):
    emotion_list = Emotion_Information.objects.all()
    result_str = ''
    for i in emotion_list:
        result_str += '<p>' + i.emotion_name

    return HttpResponse(result_str)


import operator
import json

face_api_result_emotion = ''
face_api_result_age = 0.0
def get_final_emotion(face_api_result , speech_to_emotion_result):
    return HttpResponse("TODO : 최종결과도출")

class requestFaceAPI(APIView):
    # authentication_classes = (TokenAuthentication,)
    # permission_classes = (IsAuthenticated, IsOwnerOrReadOnly)  # 권한 옵션

    def post(self, request, format=None):
        KEY = 'e006eb023fb544eaab785e41fdd65865'
        CF.Key.set(KEY)

        BASE_URL = 'https://koreacentral.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,emotion&recognitionModel=recognition_01&returnRecognitionModel=false '  # Replace with your regional Base URL
        CF.BaseUrl.set(BASE_URL)

        # You can use this example JPG or replace the URL below with your own URL to a JPEG image.
        # data = open('C:/Users/Oh YJ/Downloads/image_face/2.png', 'rb')

        print('get the data')

        # 디버깅 용
        # print('request.POST: ', request.POST)
        # print('request.body: ', request.body)
        # print('request.content-type: ', request.content_type)
        # print('request.content-params: ', request.content_params)
        # print('request.FILES: ', request.FILES)
        print('request.headers: ', request.headers)

        data_test = request.POST.get('photo', '')  # str
        print('type of data_test is : ', type(data_test))
        print('finish to get ')
        # data_test = data_test.decode('utf-8').encode('euc_kr','replace')

        # f = open('./test.jpg', 'w', encoding='UTF-8')
        f = open('./test.jpg', 'wb')
        f.write(data_test.encode())
        f.close()

        data = open('./test.jpg', 'rb').read()
        # data_in = data.read()
        # data.close()

        # q = request.data.dict() # QueryDict to dict
        #
        # str = json.dumps(q)
        # data = ' '.join(format(ord(letter), 'b') for letter in str)  # dict to binary

        faces = CF.face.detect(data)

        jsonString = str(faces)
        dict_data = json.loads(jsonString)  # Unicode decode Error
        emotions = dict_data['faceAttributes']['emotion']
        sorted_x = sorted(emotions.items(), key=operator.itemgetter(1))

        # 결과값이 크게 나온 emotion 3가지를 구한다
        result_emotion = sorted_x[-3:]

        # 종합적인 최종 감정 결과를 도출해내기 위해 전역변수에 할당시킴.
        face_api_result_emotion = result_emotion

        # 추천 알고리즘에 적용할 age 추출
        result_age = dict_data['faceAttributes']['age']
        face_api_result_age = result_age

        # 어플에 result_emotion 3가지를 날린다. 만약 0 이하라면 날리지 않음.
        # 이 일은 최종 감정 분석 결과를 도출해낸 후에 넣어야 함.

        return HttpResponse("post")  # 이건 그냥 결과 확인하기 위한 용도. 나중에 지울 것임.

    def get(self, request, format=None):
        KEY = 'e006eb023fb544eaab785e41fdd65865'
        CF.Key.set(KEY)

        BASE_URL = 'https://koreacentral.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,emotion&recognitionModel=recognition_01&returnRecognitionModel=false '  # Replace with your regional Base URL
        CF.BaseUrl.set(BASE_URL)

        # You can use this example JPG or replace the URL below with your own URL to a JPEG image.
        data = open('C:/Users/Oh YJ/Downloads/image_face/2.png', 'rb')
        faces = CF.face.detect(data)

        return HttpResponse(faces)

'''
# MS Face api 사용
@api_view(['GET','POST'])
def requestFaceAPI(request):
    logger.debug('debug requestFaceAPI for 500 error')

    if request.method == 'POST':
        KEY = 'e006eb023fb544eaab785e41fdd65865'
        CF.Key.set(KEY)

        BASE_URL = 'https://koreacentral.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,emotion&recognitionModel=recognition_01&returnRecognitionModel=false '  # Replace with your regional Base URL
        CF.BaseUrl.set(BASE_URL)

        # You can use this example JPG or replace the URL below with your own URL to a JPEG image.
        #data = open('C:/Users/Oh YJ/Downloads/image_face/2.png', 'rb')

        data = open('./test.jpg', 'wb')
        data.write(request.raw_post_data)
        print(request.raw_post_data)
        data.close()

        faces = CF.face.detect(data)
        dict_data = json.loads(faces) # Unicode decode Error
        emotions = dict_data['faceAttributes']['emotion']
        sorted_x = sorted(emotions.items(), key=operator.itemgetter(1))

        # 결과값이 크게 나온 emotion 3가지를 구한다
        result_emotion = sorted_x[-3:]

        # 종합적인 최종 감정 결과를 도출해내기 위해 전역변수에 할당시킴.
        face_api_result = result_emotion

        # 어플에 result_emotion 3가지를 날린다. 만약 0 이하라면 날리지 않음.
        # 이 일은 최종 감정 분석 결과를 도출해낸 후에 넣어야 함.

        return HttpResponse(faces) # 이건 그냥 결과 확인하기 위한 용도. 나중에 지울 것임.

    return HttpResponse("This is not POST message")

    # KEY = 'e006eb023fb544eaab785e41fdd65865'
    # CF.Key.set(KEY)
    #
    # BASE_URL = 'https://koreacentral.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,emotion&recognitionModel=recognition_01&returnRecognitionModel=false '  # Replace with your regional Base URL
    # CF.BaseUrl.set(BASE_URL)
    #
    # # You can use this example JPG or replace the URL below with your own URL to a JPEG image.
    # data = open('C:/Users/Oh YJ/Downloads/image_face/2.png', 'rb')
    #
    # faces = CF.face.detect(data)
    # # jsonString = str(faces[0])
    # # dict = json.loads(jsonString)  # Unicode Decode Error 처리해야 함.
    # return HttpResponse(faces)
    #
    # # return HttpResponse(check)

'''

# # Token 주기
# class GetAuthToken(GenericAPIView):
#     throttle_classes = ()
#     permission_classes = ()
#     parser_classes = (parsers.FormParser, parsers.MultiPartParser, parsers.JSONParser,)
#     renderer_classes = (renderers.JSONRenderer,)
#
#     def post(self, request):
#         serializer = AuthTokenSerializer(data=request.data)
#         serializer.is_valid(raise_exception=True)
#         user = serializer.validated_data['user']
#         token, created = Token.objects.get_or_create(user=user)
#         return Response({'token': token.key})
#

'''
azure 키 

1. e006eb023fb544eaab785e41fdd65865

2. 06df8da036924acbb5c508e02d7a1226
'''
