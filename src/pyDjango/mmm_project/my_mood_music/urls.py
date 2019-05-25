
from rest_framework.urlpatterns import format_suffix_patterns
from .views import *
from .api import *

from django.urls import path, include
from rest_framework.routers import DefaultRouter

router = DefaultRouter()
router.register(r'users', UserViewSet)

urlpatterns =[
	path('',include(router.urls)),
    path('auth/', include('rest_framework.urls', namespace='rest_framework')),
    path('face/', requestFaceAPI.as_view(), name='Microsoft Face API'),
    path('speech/', call.as_view(), name='speech_to_emotion'),
    path('user/', UserAPI.as_view()),
    path('getauthtoken/', GetAuthToken.as_view()),
]
