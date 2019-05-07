from django.shortcuts import render
from django.contrib.auth.models import User


from django import forms

from rest_framework import viewsets
from .serializers import *
from .models import *
from rest_framework import permissions
from rest_framework.generics import DestroyAPIView, GenericAPIView, ListAPIView, ListCreateAPIView, UpdateAPIView, \
    CreateAPIView
from rest_framework.authtoken.models import Token
from rest_framework.authtoken.serializers import AuthTokenSerializer
from rest_framework import parsers
from rest_framework import renderers
from rest_framework.response import Response

import json

'''
# 함수형 뷰 
from rest_framework import status
from rest_framework.decorators import api_view
from rest_framework.response import Response
'''

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
    permission_classes = (IsAuthenticated, IsOwnerOrReadOnly) #권한 옵션
    # 기존에는 토큰이 있는 사용자만 조회할 수 있었다.
    # 거기에 더해 소유자가 아닐 경우 수정은 불가능 하도록!

    queryset = Emotion_Information.objects.all()
    serializer_class = EmotionSerializer


# function views
def show_table(request):
    emotion_list = Emotion_Information.objects.all()
    result_str = ''
    for i in emotion_list:
        result_str += '<p>' + i.emotion_name

    return HttpResponse(result_str)


# MS Face api 사용
def requestFaceAPI(request):
    check = "It is not POST"
    # Unity에서 파일을 받는 코드
    # if request.method == 'POST':
    #     if request.body is not None:
    #         check = request.body # request.POST.get()함수로 바꿔야 함.
    #         # print(check)
    #


    KEY = 'e006eb023fb544eaab785e41fdd65865'
    CF.Key.set(KEY)


    BASE_URL = 'https://koreacentral.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,emotion&recognitionModel=recognition_01&returnRecognitionModel=false '  # Replace with your regional Base URL
    CF.BaseUrl.set(BASE_URL)

    # You can use this example JPG or replace the URL below with your own URL to a JPEG image.
    data = open('C:/Users/Oh YJ/Downloads/image_face/2.png', 'rb')

    faces = CF.face.detect(data)
    # jsonString = str(faces[0])
    # dict = json.loads(jsonString)  # Unicode Decode Error 처리해야 함.
    return HttpResponse(faces)


    # return HttpResponse(check)
'''
    KEY = 'e006eb023fb544eaab785e41fdd65865'
    CF.Key.set(KEY)

    BASE_URL = 'https://koreacentral.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,emotion&recognitionModel=recognition_01&returnRecognitionModel=false '  # Replace with your regional Base URL
    CF.BaseUrl.set(BASE_URL)

    # You can use this example JPG or replace the URL below with your own URL to a JPEG image.
    data = open('C:/Users/Oh YJ/Downloads/image_face/2.png', 'rb')
    # data = open('./test.jpy', 'rb')

    #	img_url = 'https://imagizer.imageshack.com/img924/833/h2VkhM.jpg'
    faces = CF.face.detect(data)
    # faces = CF.face.detect(data)

    # 이 데이터를 슬라이스해서..........슬라이스그냥하지말까.


    return HttpResponse(faces)
'''

# Token 주기
class GetAuthToken(GenericAPIView):
    throttle_classes = ()
    permission_classes = ()
    parser_classes = (parsers.FormParser, parsers.MultiPartParser, parsers.JSONParser,)
    renderer_classes = (renderers.JSONRenderer,)

    def post(self, request):
        serializer = AuthTokenSerializer(data=request.data)
        serializer.is_valid(raise_exception=True)
        user = serializer.validated_data['user']
        token, created = Token.objects.get_or_create(user=user)
        return Response({'token': token.key})



'''
azure 키 

1. e006eb023fb544eaab785e41fdd65865

2. 06df8da036924acbb5c508e02d7a1226
'''
