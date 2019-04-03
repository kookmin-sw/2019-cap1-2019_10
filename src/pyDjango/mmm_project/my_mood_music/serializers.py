from django.contrib.auth.models import User
from rest_framework import serializers


class UserSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = User
        fields = ('url', 'username', 'email', 'groups')


'''
from my_mood_music.models import Emotion, Music


class MMMSerializer(serializers.ModelSerializer):

    user = UserSerializer(read_only=True)

    class Meta:
        model = Emotion
        fields = [
            'emotion',
        ]
        read_only_fields = ('created_at',)
'''
