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
import os
import sys

sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
# 'C:/Users/Oh YJ/Documents/GitHub/2019-cap1-2019_10/src/pyDjango/mmm_project/my_mood_music'
from django.urls import path, include
from rest_framework import routers

router = routers.DefaultRouter(trailing_slash=False)
# router = routers.DefaultRouter()
# router.register(r'users', views.UserViewSet)
# router.register(r'emotions', views.EmotionViewSet)

from django.contrib import admin
from rest_framework_swagger.views import get_swagger_view
from rest_framework.authtoken import views

schema_view = get_swagger_view(title='My_Mood_Music API Manual')
# url(r'^$', schema_view),

urlpatterns = [

    path('admin/', admin.site.urls),

    # my_mood_music app
    path('api/', include(router.urls)),
    path('', include('my_mood_music.urls')),
    path('api/doc', schema_view),
    path('api/get_token', views.obtain_auth_token),

]

# url(r'^my_mood_music/', include('my_mood_music.urls', namespace ="my_mood_music")),
#    url(r'^api-auth/', include('rest_framework.urls', namespace='rest_framework')),
#  url(r'^my_mood_music/', include("my_mood_music.urls"), name = 'my_mood_music'),
