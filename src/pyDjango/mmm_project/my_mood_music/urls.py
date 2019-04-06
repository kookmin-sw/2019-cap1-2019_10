from django.conf.urls import include, url
from . import views


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

	path('', views.show_table, name='show_table'),
	path('emotion_analysis', views.requestFaceAPI, name ='api_response'),
]	

