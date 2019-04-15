from django.contrib import admin
from my_mood_music.models import Emotion_Information, User_Information, Analysis_Result, Comments_List, Happiness, Anger, Fear, Surprise, Disgust, Sadness, Subclass_Sad    

# Register your models here.


#Emotion 클래스가 Admin사이트에서 어떤 모습으로 보여줄지를 정의
class EmotionAdmin(admin.ModelAdmin):
    list_display = ('id_e','emotion_name')

class UserAdmin(admin.ModelAdmin):
	list_display = ('id_u','nickname','password', 'research')

class ResultAdmin(admin.ModelAdmin):
	list_display = ('id_r','nickname','music_r1', 'music_r2', 'music_r3', 'comments_r')
	
class CommentsAdmin(admin.ModelAdmin):
	list_display = ('id_cm','comments','emotion_r1', 'emotion_r2')

class HappinessAdmin(admin.ModelAdmin):
	list_display = ('id_h','music_h','age_h', 'link_h')

class AngerAdmin(admin.ModelAdmin):
	list_display = ('id_a','music_a','age_a', 'link_a')

class FearAdmin(admin.ModelAdmin):
	list_display = ('id_f','music_f','age_f', 'link_f')

class SurpriseAdmin(admin.ModelAdmin):
	list_display = ('id_su','music_su','age_su', 'link_su')

class DisgustAdmin(admin.ModelAdmin):
	list_display = ('id_d','music_d','age_d', 'link_d')

class SadnessAdmin(admin.ModelAdmin):
	list_display = ('id_s','music_s','age_s', 'link_s','subclass_s')

class SubclassAdmin(admin.ModelAdmin):
	list_display = ('id_sc','subclass')	
	
	
admin.site.register(Emotion_Information,EmotionAdmin)
admin.site.register(User_Information,UserAdmin)
admin.site.register(Analysis_Result,ResultAdmin)
admin.site.register(Comments_List,CommentsAdmin)
admin.site.register(Happiness,HappinessAdmin)
admin.site.register(Anger,AngerAdmin)
admin.site.register(Fear,FearAdmin)
admin.site.register(Surprise,SurpriseAdmin)
admin.site.register(Disgust,DisgustAdmin)
admin.site.register(Sadness,SadnessAdmin)
admin.site.register(Subclass_Sad,SubclassAdmin)
