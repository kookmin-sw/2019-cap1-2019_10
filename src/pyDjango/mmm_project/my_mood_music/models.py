from django.db import models
from django.utils.encoding import python_2_unicode_compatible

# Create your models here.

@python_2_unicode_compatible
class Emotion(models.Model):
	emotion = models.CharField(max_length=200)
	
	def __str__(self):
		return self.emotion
		

class Music(models.Model):
	emotion_id = models.ForeignKey(Emotion,on_delete=models.CASCADE)
	music_name = models.CharField(max_length=300)
	age = models.IntegerField(default=0)
	
	
	def __str__(self):
		return self.music_name, self.age
