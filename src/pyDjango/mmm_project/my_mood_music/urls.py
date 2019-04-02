from django.conf.urls import include, url
from my_mood_music.views import *


from django.urls import path, include
from rest_framework.urlpatterns import format_suffix_patterns

emotion_list = MyMoodMusicView.as_view({
    'post': 'create',
    'get': 'list'
})

emotion_detail = MyMoodMusicView.as_view({
    'get': 'retrieve',
    'put': 'update',
    'patch': 'partial_update',
    'delete': 'destroy'
})

urlpatterns = format_suffix_patterns([
    path('auth/', include('rest_framework.urls', namespace='rest_framework')),
    path('emotions/', emotion_list, name='emotion_list'),
    path('emotions/<int:pk>/', emotion_detail, name='emotion_detail'),
])