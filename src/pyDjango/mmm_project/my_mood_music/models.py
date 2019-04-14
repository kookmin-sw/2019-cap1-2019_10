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

        
class Analysis_Result(models.Model):
        id_r = models.IntegerField
        nickname = models.CharField(max_length=200)
        music_r1 = models.IntegerField
        music_r2 = models.IntegerField
        music_r3 = models.IntegerField
        comments_r = models.IntegerField

        def __str__(self):
                return self.id_r, self.nickname, self.music_r1, self.music_r2, self.music_r3, self.comments_r
        
class Comments_List(models.Model):
        id_cm = models.IntegerField
        comments = models.CharField(max_length=300)
        emotion_r1 = models.IntegerField
        emotion_r2 = models.IntegerField

        def __str__(self):
                return self.id_cm, self.comments, self.emotion_r1, self.emotion_r2
        
class Happiness(models.Model):
        id_h = models.IntegerField
        music_h = models.CharField(max_length=500)
        age_h = models.IntegerField
        link_h = models.CharField(max_length=500)

        def __str__(self):
                return self.id_h, self.music_h, self.age_h, self.link_h
                
class Anger(models.Model):
        id_a = models.IntegerField
        music_a = models.CharField(max_length=500)
        age_a = models.IntegerField
        link_a = models.CharField(max_length=500)

        def __str__(self):
                return self.id_a, self.music_a, self.age_a, self.link_a
                
class Fear(models.Model):
        id_f = models.IntegerField
        music_f = models.CharField(max_length=500)
        age_f = models.IntegerField
        link_f = models.CharField(max_length=500)

        def __str__(self):
                return self.id_f, self.music_f, self.age_f, self.link_f
                
class Surprise(models.Model):
        id_su = models.IntegerField
        music_su = models.CharField(max_length=500)
        age_su = models.IntegerField
        link_su = models.CharField(max_length=500)

        def __str__(self):
                return self.id_su, self.music_su, self.age_su, self.link_su
                
class Disgust(models.Model):
        id_d = models.IntegerField
        music_d = models.CharField(max_length=500)
        age_d = models.IntegerField
        link_d = models.CharField(max_length=500)

        def __str__(self):
                return self.id_d, self.music_d, self.age_d, self.link_d
                
class Sadness(models.Model):
        id_s = models.IntegerField
        music_s = models.CharField(max_length=500)
        age_s = models.IntegerField
        link_s = models.CharField(max_length=500)
        subclass_s = models.IntegerField

        def __str__(self):
                return self.id_s, self.music_s, self.age_s, self.link_s, self.subclass_s
                
class Subclass_Sad(models.Model):
        id_sc = models.IntegerField
        subclass = models.CharField(max_length=500)

        def __str__(self):
                return self.id_sc, self.subclass
