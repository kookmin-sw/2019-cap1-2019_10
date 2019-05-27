from django.contrib import admin
from .models import Analysis_Result, Happiness, Anger, Fear, Surprise, Disgust, Sadness, Subclass_Sad, Lie, \
    Child


# Register your models here.


# Emotion 클래스가 Admin사이트에서 어떤 모습으로 보여줄지를 정의
# class UserAdmin(admin.ModelAdmin):
# list_display = ('id_u','email','password', 'research')

class ResultAdmin(admin.ModelAdmin):
    list_display = ('id', 'user_id', 'music_r1', 'music_r2', 'music_r3')


class HappinessAdmin(admin.ModelAdmin):
    list_display = ('id', 'music_h', 'age_h', 'link_h', 'tag_h1', 'tag_h2')


class AngerAdmin(admin.ModelAdmin):
    list_display = ('id', 'music_a', 'age_a', 'link_a', 'tag_a1', 'tag_a2')


class FearAdmin(admin.ModelAdmin):
    list_display = ('id', 'music_f', 'age_f', 'link_f', 'tag_f1', 'tag_f2')


class SurpriseAdmin(admin.ModelAdmin):
    list_display = ('id', 'music_su', 'age_su', 'link_su', 'tag_su1', 'tag_su2')


class DisgustAdmin(admin.ModelAdmin):
    list_display = ('id', 'music_d', 'age_d', 'link_d', 'tag_d1', 'tag_d2')


class SadnessAdmin(admin.ModelAdmin):
    list_display = ('id', 'music_s', 'age_s', 'link_s', 'subclass_s', 'tag_s1', 'tag_s2')


class SubclassAdmin(admin.ModelAdmin):
    list_display = ('id', 'subclass')


class LieAdmin(admin.ModelAdmin):
    list_display = ('id', 'music_l', 'link_l')


class ChildAdmin(admin.ModelAdmin):
    list_display = ('id', 'music_c', 'link_c')


# admin.site.register(User_Information,UserAdmin)
admin.site.register(Analysis_Result, ResultAdmin)
admin.site.register(Happiness, HappinessAdmin)
admin.site.register(Anger, AngerAdmin)
admin.site.register(Fear, FearAdmin)
admin.site.register(Surprise, SurpriseAdmin)
admin.site.register(Disgust, DisgustAdmin)
admin.site.register(Sadness, SadnessAdmin)
admin.site.register(Subclass_Sad, SubclassAdmin)
admin.site.register(Lie, LieAdmin)
admin.site.register(Child, ChildAdmin)