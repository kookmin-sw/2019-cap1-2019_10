from django.db import models
from django.utils.encoding import python_2_unicode_compatible

# Create your models here.

@python_2_unicode_compatible
        
class Analysis_Result(models.Model):
        id = models.AutoField(primary_key=True)
        user_id = models.CharField(max_length=300, null=True)
        music_r1 = models.CharField(max_length=500)
        music_r2 = models.CharField(max_length=500)
        music_r3 = models.CharField(max_length=500)

        def __str__(self):
                return '{} {} {} {} {}'.format(self.id, self.user_id, self.music_r1, self.music_r2, self.music_r3)
        
        
class Happiness(models.Model):
        id = models.AutoField(primary_key=True)
        music = models.CharField(max_length=500)
        age = models.IntegerField(default=0)
        link = models.URLField()
        tag_1 = models.CharField(max_length=500, null=True)
        tag_2 = models.CharField(max_length=500, null=True)

        def __str__(self):
                return '{} {} {} {} {} {}'.format(self.id, self.music, self.age, self.link, self.tag_1, self.tag_2)

               
class Anger(models.Model):
        id = models.AutoField(primary_key=True)
        music = models.CharField(max_length=500)
        age = models.IntegerField(default=0)
        link = models.URLField()
        tag_1 = models.CharField(max_length=500, null=True)
        tag_2 = models.CharField(max_length=500, null=True)
        
        def __str__(self):
                return '{} {} {} {} {} {}'.format(self.id, self.music, self.age, self.link, self.tag_1, self.tag_2)

                
class Fear(models.Model):
        id = models.AutoField(primary_key=True)
        music = models.CharField(max_length=500)
        age = models.IntegerField(default=0)
        link = models.URLField()
        tag_1 = models.CharField(max_length=500, null=True)
        tag_2 = models.CharField(max_length=500, null=True)
        
        def __str__(self):
                return '{} {} {} {} {} {}'.format(self.id, self.music, self.age, self.link, self.tag_1, self.tag_2)                


class Surprise(models.Model):
        id = models.AutoField(primary_key=True)
        music = models.CharField(max_length=500)
        age = models.IntegerField(default=0)
        link = models.URLField()
        tag_1 = models.CharField(max_length=500, null=True)
        tag_2 = models.CharField(max_length=500, null=True)
        
        def __str__(self):
                return '{} {} {} {} {} {}'.format(self.id, self.music, self.age, self.link, self.tag_1, self.tag_2)

                
class Disgust(models.Model):
        id = models.AutoField(primary_key=True)
        music = models.CharField(max_length=500)
        age = models.IntegerField(default=0)
        link = models.URLField()
        tag_1 = models.CharField(max_length=500, null=True)
        tag_2 = models.CharField(max_length=500, null=True)
        
        def __str__(self):
                return '{} {} {} {} {} {}'.format(self.id, self.music, self.age, self.link, self.tag_1, self.tag_2)

                
class Sadness(models.Model):
        id = models.AutoField(primary_key=True)
        music = models.CharField(max_length=500)
        age = models.IntegerField(default=0)
        link = models.URLField()
        subclass_s = models.IntegerField(default=0)
        tag_1 = models.CharField(max_length=500, null=True)
        tag_2 = models.CharField(max_length=500, null=True)
        
        def __str__(self):
                return '{} {} {} {} {} {} {}'.format(self.id, self.music, self.age, self.link, self.subclass_s, self.tag_1, self.tag_2)

                
class Subclass_Sad(models.Model):
        id = models.AutoField(primary_key=True)
        subclass = models.CharField(max_length=500)

        def __str__(self):
                return '{} {}'.format(self.id, self.subclass)


class Lie(models.Model):
        id = models.AutoField(primary_key=True)
        music = models.CharField(max_length=500)
        link = models.URLField()

        def __str__(self):
                return '{} {} {}'.format(self.id, self.music, self.link)


class Child(models.Model):
        id = models.AutoField(primary_key=True)
        music = models.CharField(max_length=500)
        link = models.URLField()

        def __str__(self):
                return '{} {} {}'.format(self.id, self.music, self.link)      

