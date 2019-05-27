from django.db import models
from django.utils.encoding import python_2_unicode_compatible
from django.contrib.auth.models import User
from django.db.models.signals import post_save
from django.dispatch import receiver


# Create your models here.

@python_2_unicode_compatible

class Analysis_Result(models.Model):
    id = models.IntegerField(primary_key=True)
    user_id = models.CharField(max_length=300, null=True)
    music_r1 = models.CharField(max_length=500)
    music_r2 = models.CharField(max_length=500)
    music_r3 = models.CharField(max_length=500)

    def __str__(self):
        return '{} {} {} {} {}'.format(self.id, self.user_id, self.music_r1, self.music_r2, self.music_r3)


class Happiness(models.Model):
    id = models.IntegerField(primary_key=True)
    music_h = models.CharField(max_length=500)
    age_h = models.IntegerField(default=0)
    link_h = models.URLField()
    tag_h1 = models.CharField(max_length=500, null=True)
    tag_h2 = models.CharField(max_length=500, null=True)

    def __str__(self):
        return '{} {} {} {} {} {}'.format(self.id, self.music_h, self.age_h, self.link_h, self.tag_h1, self.tag_h2)


class Anger(models.Model):
    id = models.IntegerField(primary_key=True)
    music_a = models.CharField(max_length=500)
    age_a = models.IntegerField(default=0)
    link_a = models.URLField()
    tag_a1 = models.CharField(max_length=500, null=True)
    tag_a2 = models.CharField(max_length=500, null=True)

    def __str__(self):
        return '{} {} {} {} {} {}'.format(self.id, self.music_a, self.age_a, self.link_a, self.tag_a1, self.tag_a2)


class Fear(models.Model):
    id = models.IntegerField(primary_key=True)
    music_f = models.CharField(max_length=500)
    age_f = models.IntegerField(default=0)
    link_f = models.URLField()
    tag_f1 = models.CharField(max_length=500, null=True)
    tag_f2 = models.CharField(max_length=500, null=True)

    def __str__(self):
        return '{} {} {} {} {} {}'.format(self.id, self.music_f, self.age_f, self.link_f, self.tag_f1, self.tag_f2)


class Surprise(models.Model):
    id = models.IntegerField(primary_key=True)
    music_su = models.CharField(max_length=500)
    age_su = models.IntegerField(default=0)
    link_su = models.URLField()
    tag_su1 = models.CharField(max_length=500, null=True)
    tag_su2 = models.CharField(max_length=500, null=True)

    def __str__(self):
        return '{} {} {} {} {} {}'.format(self.id, self.music_su, self.age_su, self.link_su, self.tag_su1, self.tag_su2)


class Disgust(models.Model):
    id = models.IntegerField(primary_key=True)
    music_d = models.CharField(max_length=500)
    age_d = models.IntegerField(default=0)
    link_d = models.URLField()
    tag_d1 = models.CharField(max_length=500, null=True)
    tag_d2 = models.CharField(max_length=500, null=True)

    def __str__(self):
        return '{} {} {} {} {} {}'.format(self.id, self.music_d, self.age_d, self.link_d, self.tag_d1, self.tag_d2)


class Sadness(models.Model):
    id = models.IntegerField(primary_key=True)
    music_s = models.CharField(max_length=500)
    age_s = models.IntegerField(default=0)
    link_s = models.URLField()
    subclass_s = models.IntegerField(default=0)
    tag_s1 = models.CharField(max_length=500, null=True)
    tag_s2 = models.CharField(max_length=500, null=True)

    def __str__(self):
        return '{} {} {} {} {} {} {}'.format(self.id, self.music_s, self.age_s, self.link_s, self.subclass_s,
                                             self.tag_s1, self.tag_s2)


class Subclass_Sad(models.Model):
    id = models.IntegerField(primary_key=True)
    subclass = models.CharField(max_length=500)

    def __str__(self):
        return '{} {}'.format(self.id, self.subclass)


class Lie(models.Model):
    id = models.IntegerField(primary_key=True)
    music_l = models.CharField(max_length=500)
    link_l = models.URLField()

    def __str__(self):
        return '{} {} {}'.format(self.id, self.music_l, self.link_l)


class Child(models.Model):
    id = models.IntegerField(primary_key=True)
    music_c = models.CharField(max_length=500)
    link_c = models.URLField()

    def __str__(self):
        return '{} {} {}'.format(self.id, self.music_c, self.link_c)