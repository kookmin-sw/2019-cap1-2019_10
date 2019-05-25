
from .views import *
from .api import *

from django.urls import path, include
from rest_framework.routers import DefaultRouter

# 라우터를 생성하고 뷰셋을 등록합니다
router = DefaultRouter(trailing_slash=False)
router.register(r'users', UserViewSet)

urlpatterns = [
    path('auth/', include('rest_framework.urls', namespace='rest_framework')),
    path('', include(router.urls)),
    path('face/', RequestFaceAPI.as_view(), name='Microsoft Face API'),
    path('speech/', Call.as_view(), name='speech_to_emotion'),
    path('api/user', UserAPI.as_view()),
    path('api/getauthtoken', GetAuthToken.as_view()),
]
