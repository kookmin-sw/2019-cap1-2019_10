
from .serializers import *
from .models import *
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

from rest_framework.authtoken.models import Token


class UserAPI(DestroyAPIView, CreateAPIView):
    queryset = User.objects.all()
    serializer_class = CreateUserSerializer

    def perform_destroy(self, instance):
        user = User.objects.get(username=self.request.data['username'], email=self.request.data['email'])
        if user.check_password(self.request.data['password']) is False:
            return Response('You are not authorized to do that.', status=status.HTTP_401_UNAUTHORIZED)
        instance.delete()

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
