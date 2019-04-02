"""mysite URL Configuration

The `urlpatterns` list routes URLs to views. For more information please see:
    https://docs.djangoproject.com/en/2.1/topics/http/urls/
Examples:
Function views
    1. Add an import:  from my_app import views
    2. Add a URL to urlpatterns:  path('', views.home, name='home')
Class-based views
    1. Add an import:  from other_app.views import Home
    2. Add a URL to urlpatterns:  path('', Home.as_view(), name='home')
Including another URLconf
    1. Import the include() function: from django.urls import include, path
    2. Add a URL to urlpatterns:  path('blog/', include('blog.urls'))
"""


from django.conf.urls import url, include
from rest_framework import routers

router = routers.DefaultRouter()
'''
from my_mood_music import views

router = routers.DefaultRouter()
router.register(r'users', views.UserViewSet)
router.register(r'groups', views.GroupViewSet)
'''

from django.contrib import admin
from rest_framework_swagger.views import get_swagger_view

schema_view = get_swagger_view(title='Pastebin API')

from bookmark.views import BookmarkLV, BookmarkDV
from my_mood_music.views import *

#  url(r'^$', schema_view),
urlpatterns = [
   
	url(r'^admin/', admin.site.urls),

	# Class-based views for Bookmark app
	# url(regex, view, kwargs=None, name=None, prefex='')
	
	url(r'^bookmark/$', BookmarkLV.as_view(), name='index'),
	url(r'^bookmark/(?P<pk>\d+)/$', BookmarkDV.as_view(), name='detail'),
	
	# 우리가 만든 API를 자동으로 라우팅합니다.
	# 그리고 API 탐색을 위해 로그인 URL을 추가했습니다.


	url(r'^', include(router.urls)),
    
    url(r'^my_mood_music/', include("my_mood_music.urls"), name = 'my_mood_music')
    # Class-vased views for my_mood_music app
	
	
]

#url(r'^my_mood_music/', include('my_mood_music.urls', namespace ="my_mood_music")),
#    url(r'^api-auth/', include('rest_framework.urls', namespace='rest_framework')),
