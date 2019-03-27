from django.contrib import admin
from my_mood_music.models import Emotion, Music

# Register your models here.


#Emotion 클래스가 Admin사이트에서 어떤 모습으로 보여줄지를 정의
class EmotionAdmin(admin.ModelAdmin):
    list_display = ('id', 'emotion')

class MusicAdmin(admin.ModelAdmin):
	list_display = ('emotion_id','music_name','age')

    
admin.site.register(Emotion,EmotionAdmin)
admin.site.register(Music,MusicAdmin)
