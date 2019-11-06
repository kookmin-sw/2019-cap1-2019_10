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
from rest_framework.authtoken import views
from rest_framework_swagger.views import get_swagger_view
from django.contrib import admin
from rest_framework import routers
from django.urls import path, include
from django.conf.urls import url
import os
import sys

sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

router = routers.DefaultRouter(trailing_slash=False)

schema_view = get_swagger_view(title='My_Mood_Music API Manual')


urlpatterns = [
    path('admin/', admin.site.urls),
    path('api/', include('my_mood_music.urls')),
    path('', schema_view),
]

