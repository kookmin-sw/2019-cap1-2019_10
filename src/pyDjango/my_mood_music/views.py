import logging
from django.contrib.auth.models import User

from rest_framework import viewsets
from rest_framework.authtoken.models import Token
from rest_framework.authtoken.serializers import AuthTokenSerializer
from rest_framework import parsers
from rest_framework import renderers
from rest_framework.response import Response
from rest_framework import status

from rest_framework.generics import DestroyAPIView, UpdateAPIView, CreateAPIView
from rest_framework.views import APIView

from .serializers import *
from .models import *

logger = logging.getLogger(__name__)

class UserViewSet(viewsets.ModelViewSet):
    queryset = User.objects.all()
    serializer_class = UserSerializer

    def get(self, request, *args, **kwargs):
        return self.list(request, *args, **kwargs)


class UserAPI(DestroyAPIView, CreateAPIView, UpdateAPIView):
    """
    for login
    """
    queryset = User.objects.all()
    serializer_class = CreateUserSerializer

    def perform_destroy(self, instance):
        user = User.objects.get(username=self.request.data['username'], email=self.request.data['email'])
        if user.check_password(self.request.data['password']) is False:
            return Response('You are not authorized to do that.', status=status.HTTP_401_UNAUTHORIZED)
        instance.delete()


logger = logging.getLogger(__name__)

class GetAuthToken(APIView):
    """
    for singup
    """
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

