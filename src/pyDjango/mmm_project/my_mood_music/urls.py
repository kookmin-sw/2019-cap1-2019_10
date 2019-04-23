from django.conf.urls import include, url
from rest_framework.urlpatterns import format_suffix_patterns
from . import views


from django.urls import path, include


urlpatterns = format_suffix_patterns([
	path('auth/', include('rest_framework.urls', namespace='rest_framework')),
	path('', views.requestFaceAPI, name='show_table'),
	# path('emotion_analysis', views.requestFaceAPI, name ='api_response'),
	path('emotions/', views.EmotionList.as_view()),
	url(r'emotions/(?P<pk>[0-9]+)/$', views.EmotionDetail.as_view()),
])
