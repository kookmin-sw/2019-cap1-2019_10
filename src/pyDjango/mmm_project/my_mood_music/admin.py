from django.contrib import admin
from my_mood_music.models import Emotion_Information, User_Information

# Register your models here.


#Emotion 클래스가 Admin사이트에서 어떤 모습으로 보여줄지를 정의
class EmotionAdmin(admin.ModelAdmin):
    list_display = ('id_e', 'emotion_name')

class UserAdmin(admin.ModelAdmin):
	list_display = ('id_u','nickname','password', 'research')

    
admin.site.register(Emotion_Information,EmotionAdmin)
admin.site.register(User_Information,UserAdmin)
