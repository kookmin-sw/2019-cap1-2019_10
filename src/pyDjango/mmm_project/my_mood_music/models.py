from django.db import models
from django.utils.encoding import python_2_unicode_compatible

# Create your models here.

@python_2_unicode_compatible
class Emotion_Information(models.Model):
	emotion_name = models.CharField(max_length=200)
	id_e = models.IntegerField
	
	def __str__(self):
		return self.emotion_name, self.id_e
		

class User_Information(models.Model):
        id_u = models.IntegerField
        nickname = models.CharField(max_length=200)
        password = models.CharField(max_length=200)
        research = models.IntegerField
	#emotion_id = models.ForeignKey(Emotion,on_delete=models.CASCADE)
	#music_name = models.CharField(max_length=300)
        #age = models.IntegerField(default=0)
	
	
        def __str__(self):
                return self.id_u, self.nickname, self.password, self.research
