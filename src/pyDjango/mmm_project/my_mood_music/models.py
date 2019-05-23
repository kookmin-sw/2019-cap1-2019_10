from django.db import models
from django.utils.encoding import python_2_unicode_compatible

# Create your models here.

@python_2_unicode_compatible
		
class User_Information(models.Model):
        id_u = models.IntegerField
        email = models.CharField(max_length=200)
        password = models.CharField(max_length=200)
        research = models.IntegerField
	#emotion_id = models.ForeignKey(Emotion,on_delete=models.CASCADE)
	#music_name = models.CharField(max_length=300)
        #age = models.IntegerField(default=0)
	
	
        def __str__(self):
                return '{} {} {} {}'.format(self.id_u, self.email, self.password, self.research)


''' onetoonefield 


class User_Table(models.Model):
        user_information = models.OneToOneField(
                User_Information,
                on_delete = models.CASCADE,
                primary_key = True,
        )
        def __str__(self):
                return '{}'.format(self.user_information)
                
''' 
        
class Analysis_Result(models.Model):
        id_r = models.IntegerField
        email = models.CharField(max_length=200)
        music_r1 = models.IntegerField
        music_r2 = models.IntegerField
        music_r3 = models.IntegerField


        def __str__(self):
                return '{} {} {} {} {}'.format(self.id_r, self.email, self.music_r1, self.music_r2, self.music_r3)
        
        
class Happiness(models.Model):
        id_h = models.IntegerField
        music_h = models.CharField(max_length=500)
        age_h = models.IntegerField
        link_h = models.URLField()
        tag_h1 = models.CharField(max_length=500)
        tag_h2 = models.CharField(max_length=500)

        def __str__(self):
                return '{} {} {} {} {} {}'.format(self.id_h, self.music_h, self.age_h, self.link_h, self.tag_h1, self.tag_h2)

               
class Anger(models.Model):
        id_a = models.IntegerField
        music_a = models.CharField(max_length=500)
        age_a = models.IntegerField
        link_a = models.URLField()
        tag_a1 = models.CharField(max_length=500)
        tag_a2 = models.CharField(max_length=500)
        
        def __str__(self):
                return '{} {} {} {} {} {}'.format(self.id_a, self.music_a, self.age_a, self.link_a, self.tag_a1, self.tag_a2)

                
class Fear(models.Model):
        id_f = models.IntegerField
        music_f = models.CharField(max_length=500)
        age_f = models.IntegerField
        link_f = models.URLField()
        tag_f1 = models.CharField(max_length=500)
        tag_f2 = models.CharField(max_length=500)
        
        def __str__(self):
                return '{} {} {} {} {} {}'.format(self.id_f, self.music_f, self.age_f, self.link_f, self.tag_f1, self.tag_f2)                


class Surprise(models.Model):
        id_su = models.IntegerField
        music_su = models.CharField(max_length=500)
        age_su = models.IntegerField
        link_su = models.URLField()
        tag_su1 = models.CharField(max_length=500)
        tag_su2 = models.CharField(max_length=500)
        
        def __str__(self):
                return '{} {} {} {} {} {}'.format(self.id_su, self.music_su, self.age_su, self.link_su, self.tag_su1, self.tag_su2)

                
class Disgust(models.Model):
        id_d = models.IntegerField
        music_d = models.CharField(max_length=500)
        age_d = models.IntegerField
        link_d = models.URLField()
        tag_d1 = models.CharField(max_length=500)
        tag_d2 = models.CharField(max_length=500)
        
        def __str__(self):
                return '{} {} {} {} {} {}'.format(self.id_d, self.music_d, self.age_d, self.link_d, self.tag_d1, self.tag_d2)
                
class Sadness(models.Model):
        id_s = models.IntegerField
        music_s = models.CharField(max_length=500)
        age_s = models.IntegerField
        link_s = models.URLField()
        subclass_s = models.IntegerField
        tag_s1 = models.CharField(max_length=500)
        tag_s2 = models.CharField(max_length=500)
        
        def __str__(self):
                return '{} {} {} {} {} {} {}'.format(self.id_s, self.music_s, self.age_s, self.link_s, self.subclass_s, self.tag_s1, self.tag_s2)
                
class Subclass_Sad(models.Model):
        id_sc = models.IntegerField
        subclass = models.CharField(max_length=500)

        def __str__(self):
                return '{} {}'.format(self.id_sc, self.subclass)

class Lie(models.Model):
        id_l = models.IntegerField
        music_l = models.CharField(max_length=500)
        link_l = models.URLField()

        def __str__(self):
                return '{} {} {}'.format(self.id_l, self.music_l, self.link_l)

class Child(models.Model):
        id_c = models.IntegerField
        music_c = models.CharField(max_length=500)
        link_c = models.URLField()

        def __str__(self):
                return '{} {} {}'.format(self.id_c, self.music_c, self.link_c)      
