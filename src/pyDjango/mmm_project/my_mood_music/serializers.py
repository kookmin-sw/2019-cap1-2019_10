from django.contrib.auth.models import User
from rest_framework import serializers

class UserSerializer(serializers.HyperlinkedModelSerializer):
	class Meta:
		model = User
		fields = ('url', 'username', 'email')

class CreateUserSerializer(serializers.ModelSerializer):
    email = serializers.EmailField(required=True)

    class Meta:
        model = User
        fields = ('id', 'email', 'username', 'password')
        extra_kwargs = {'password': {'write_only': True}}

    def create(self, validated_data):
        user = User(
            email=validated_data['email'],
            username=validated_data['username']
        )
        user.set_password(validated_data['password'])
        user.save()
        return user


from .models import *

# class EmotionSerializer(serializers.ModelSerializer):
#     class Meta:
#         model = Emotion_Information
#         fields = '__all__'

class AngerSerializer(serializers.ModelSerializer):
    class Meta:
        model = Anger
        fields = '__all__'

class FearSerializer(serializers.ModelSerializer):
    class Meta:
        model = Fear
        fields = '__all__'

class HappinessSerializer(serializers.ModelSerializer):
    class Meta:
        model = Happiness
        fields = '__all__'


class SurpriseSerializer(serializers.ModelSerializer):
    class Meta:
        model = Surprise
        fields = '__all__'

class DisgustSerializer(serializers.ModelSerializer):
    class Meta:
        model = Disgust
        fields = '__all__'

class SadnessSerializer(serializers.ModelSerializer):
    class Meta:
        model = Sadness
        fields = '__all__'

class Subclass_SadSerializer(serializers.ModelSerializer):
    class Meta:
        model = Subclass_Sad
        fields = '__all__'

class LieSerializer(serializers.ModelSerializer):
    class Meta:
        model = Lie
        fields = '__all__'

class ChildSerializer(serializers.ModelSerializer):
    class Meta:
        model = Child
        fields = ('music', 'link',)

class Analysis_ResultSerializer(serializers.ModelSerializer):
    class Meta:
        model = Analysis_Result
        fields = ('music_r1', 'music_r2', 'music_r3',)

