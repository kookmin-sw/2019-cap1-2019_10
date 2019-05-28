"""
Definition of urls for DjangoWebProject1.
"""

from django.conf.urls import include, url
import DjangoApp.views
from DjangoApp.views import *

urlpatterns = [
    url(r'^$', DjangoApp.views.call.as_view(), name='call'),
    url(r'^speech/', call.as_view(), name='speech_to_emotion'),
]
    

