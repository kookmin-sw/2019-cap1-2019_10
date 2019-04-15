from django.shortcuts import render
from django.contrib.auth.models import User

from rest_framework import viewsets
from .serializers import UserSerializer, EmotionSerializer
from my_mood_music.models import *
from rest_framework import permissions
'''
# 함수형 뷰 
from rest_framework import status
from rest_framework.decorators import api_view
from rest_framework.response import Response
'''
'''
# 클래스 기반 뷰
from django.http import Http404
from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status
'''
'''
# Use the Mixin
from rest_framework import mixins
from rest_framework import generics
'''

from rest_framework import generics

from django.http.response import HttpResponse
from django.views.decorators.csrf import csrf_exempt
import cognitive_face as CF

# viewSet classes 
class UserViewSet(viewsets.ModelViewSet):
    """
    API endpoint that allows users to be viewed or edited.
    """
    queryset = User.objects.all().order_by('-date_joined')
    serializer_class = UserSerializer
    
class EmotionViewSet(viewsets.ModelViewSet):
	queryset = Emotion_Information.objects.all().order_by('id')
	serializer_class = EmotionSerializer
	permision_class = (permissions.IsAuthenticated,)
	lookup_fields = 'pk'
	
	def perform_create(self, serializer):
		sereializer.save(user=self.request.user)
		
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

class EmotionList(generics.ListCreateAPIView):
    queryset = Emotion_Information.objects.all()
    serializer_class = EmotionSerializer


class EmotionDetail(generics.RetrieveUpdateDestroyAPIView):
    queryset = Emotion_Information.objects.all()
    serializer_class = EmotionSerializer
    


# function views
def show_table(request):
	emotion_list = Emotion_Information.objects.all()
	result_str = ''
	for i in emotion_list:
		result_str += '<p>'+ i.emotion_name
		
	return HttpResponse(result_str)

# MS Face api 사용
def requestFaceAPI(request):
	
	KEY = 'fc9f7f1e776d405cbd87fd787dc1cc54'
	CF.Key.set(KEY)

	BASE_URL = 'https://koreacentral.api.cognitive.microsoft.com/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,emotion&recognitionModel=recognition_01&returnRecognitionModel=false '# Replace with your regional Base URL
	CF.BaseUrl.set(BASE_URL)

	# You can use this example JPG or replace the URL below with your own URL to a JPEG image.
	data = open('/home/daeng/1553010798453.jpg', 'rb')
#	img_url = 'https://imagizer.imageshack.com/img924/833/h2VkhM.jpg'
	faces = CF.face.detect(data)
	return HttpResponse(faces)






# 실질적으로 Queryset을 컨트롤하고 데이터를 조작해 Serializer을 통해 매핑시켜주는 View를 작성
# CBV를 이용해 여러개의 뷰를 작성하지 않고, Viewset을 이용해 Model 하나를 컨트롤하는 CRUD를 1개의 View로 구현
'''
from rest_framework import viewsets
from .serializers import MMMSerializer
from rest_framework import permissions


class MyMoodMusicView(viewsets.ModelViewSet):
    queryset = Emotion.objects.all()
    serializer_class = MMMSerializer
    permission_classes = (permissions.IsAuthenticated,)

    def perform_create(self, serializer):
        serializer.save(user=self.request.user)
	'''

'''

import http.client, urllib.request, urllib.parse, urllib.error, base64

	headers = {
		# Request headers
		'Content-Type': 'application/json',
		'Ocp-Apim-Subscription-Key': 'd0b59c05aee14136af3d8dc739689e4a',
	}

	params = urllib.parse.urlencode({
		# Request parameters
		'returnFaceId': 'true',
		'returnFaceLandmarks': 'false',
		'returnFaceAttributes': 'age, emotion',
	})

	try:
		conn = http.client.HTTPSConnection('westus.api.cognitive.microsoft.com')
		conn.request("POST", "/face/v1.0/detect?%s" % params, "{body}", headers)
		response = conn.getresponse()
		data = response.read()
		print(data)
		conn.close()
	except Exception as e:
		print("[Errno {0}] {1}".format(e.errno, e.strerror))
'''



'''
API Key
d0b59c05aee14136af3d8dc739689e4a
0f45d74f60c14ebeae161adb16fb5bd1
'''
