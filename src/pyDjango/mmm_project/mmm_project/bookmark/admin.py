from django.contrib import admin
from bookmark.models import Bookmark

# Register your models here.

#Bookmark 클래스가 Admin사이트에서 어떤 모습으로 보여줄지를 정의
class BookmarkAdmin(admin.ModelAdmin):
    list_display = ('title' , 'url')
    
admin.site.register(Bookmark, BookmarkAdmin)
