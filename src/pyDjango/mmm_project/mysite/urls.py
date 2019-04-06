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
from my_mood_music import views

router = routers.DefaultRouter()
router.register(r'users', views.UserViewSet)
router.register(r'emotions', views.EmotionViewSet)


from django.contrib import admin
# from rest_framework_swagger.views import get_swagger_view

# schema_view = get_swagger_view(title='Pastebin API')
#  url(r'^$', schema_view),

from bookmark.views import BookmarkLV, BookmarkDV
from my_mood_music.views import *
import my_mood_music.urls

urlpatterns = [
   
	url(r'^admin/', admin.site.urls),

	# Class-based views for Bookmark app
	# url(regex, view, kwargs=None, name=None, prefex='')
	
	url(r'^bookmark/$', BookmarkLV.as_view(), name='index'),
	url(r'^bookmark/(?P<pk>\d+)/$', BookmarkDV.as_view(), name='detail'),
	
	# my_mood_music app
    
    url(r'^',include(router.urls)),	

	url(r'^mmm/', include(my_mood_music.urls)),
	
	 url(r'^api-auth/', include('rest_framework.urls', namespace='rest_framework')),
	
	
]

#url(r'^my_mood_music/', include('my_mood_music.urls', namespace ="my_mood_music")),
#    url(r'^api-auth/', include('rest_framework.urls', namespace='rest_framework')),
#  url(r'^my_mood_music/', include("my_mood_music.urls"), name = 'my_mood_music'),
