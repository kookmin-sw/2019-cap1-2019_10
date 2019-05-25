python manage.py shell #open shell

import csv
import os
#path = "Desktop/2019-cap1-2019_10/src/DB"  # Set path of new directory here
#os.chdir(path) # changes the directory
from my_mood_music.models import Lie
with open('lie.csv', encoding='utf-8') as csvfile:
    reader = csv.DictReader(csvfile)
    for row in reader :
        p = Lie(music_l=row['music_l'], link_l=row['link_l'])
        p.save()

exit()
