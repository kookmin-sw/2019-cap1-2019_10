from django.urls import path, include
from rest_framework.routers import DefaultRouter

from .views import UserAPI, GetAuthToken, UserViewSet
from .api import RequestFaceAPI, Call, RecommendationMusic

router = DefaultRouter()
router.register(r'users', UserViewSet)

urlpatterns = [
    path('', include(router.urls)),
    path('face/', RequestFaceAPI.as_view(), name='Microsoft Face API'),
    path('speech/', Call.as_view(), name='speech_to_emotion'),
    path('user/', UserAPI.as_view()),
    path('getauthtoken/', GetAuthToken.as_view()),
    path('recommand/', RecommendationMusic.as_view()),
]
