from django.shortcuts import render
from django.contrib.auth.models import User

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

# 클래스 기반 뷰
from django.http import Http404
from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status

from rest_framework import generics

from django.http.response import HttpResponse
from django.views.decorators.csrf import csrf_exempt
import cognitive_face as CF
from rest_framework.authtoken.models import Token


from rest_framework.authentication import TokenAuthentication
from rest_framework.permissions import IsAuthenticated
from .permissions import IsOwnerOrReadOnly


# class EmotionList(generics.ListCreateAPIView):
#     queryset = Emotion_Information.objects.all()
#     serializer_class = EmotionSerializer
#
#
# class EmotionDetail(generics.RetrieveUpdateDestroyAPIView):
#     authentication_classes = (TokenAuthentication,)
#     permission_classes = (IsAuthenticated, IsOwnerOrReadOnly)  # 권한 옵션
#     # 기존에는 토큰이 있는 사용자만 조회할 수 있었다.
#     # 거기에 더해 소유자가 아닐 경우 수정은 불가능 하도록!
#
#     queryset = Emotion_Information.objects.all()
#     serializer_class = EmotionSerializer

import logging
logger = logging.getLogger(__name__)

class UserViewSet(viewsets.ModelViewSet):
	queryset = User.objects.all()
	serializer_class = UserSerializer
	
	def get(self, request, *args, **kwargs):
		return self.list(request, *args, **kwargs)


class UserAPI(DestroyAPIView, CreateAPIView):
    queryset = User.objects.all()
    serializer_class = CreateUserSerializer

    def perform_destroy(self, instance):
        user = User.objects.get(username=self.request.data['username'], email=self.request.data['email'])
        if user.check_password(self.request.data['password']) is False:
            return Response('You are not authorized to do that.', status=status.HTTP_401_UNAUTHORIZED)
        instance.delete()


import logging
logger = logging.getLogger(__name__)

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

