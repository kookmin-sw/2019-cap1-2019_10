from django.conf.urls import include, url
from my_mood_music.views import *


from django.urls import path, include

'''
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
'''


urlpatterns = [
    url('auth/', include('rest_framework.urls', namespace='rest_framework')),
]

