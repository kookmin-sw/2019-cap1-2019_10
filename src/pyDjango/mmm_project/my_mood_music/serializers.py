from django.contrib.auth.models import User
from rest_framework import serializers


class UserSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = User
        fields = ('url', 'username', 'email', 'groups')



from my_mood_music.models import *


class EmotionSerializer(serializers.ModelSerializer):


    class Meta:
        model = Emotion_Information
        fields = [
            'emotion_name',
        ]


