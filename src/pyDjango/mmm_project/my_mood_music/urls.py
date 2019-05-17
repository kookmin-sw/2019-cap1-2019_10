from django.conf.urls import include, url
from rest_framework.urlpatterns import format_suffix_patterns
from .views import *

from django.urls import path

urlpatterns = format_suffix_patterns([
    path('auth/', include('rest_framework.urls', namespace='rest_framework')),
    path('', requestFaceAPI.as_view(), name='Microsoft Face API'),
    path('speech/', call.as_view(), name='speech_to_emotion'),
    path('api/user', UserAPI.as_view()),
    path('api/getauthtoken', GetAuthToken.as_view()),
    path('emotions/', EmotionList.as_view()),
    path('emotions/<int:pk>/', EmotionDetail.as_view()),
])
