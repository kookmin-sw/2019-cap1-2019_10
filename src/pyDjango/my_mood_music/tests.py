import csv
import os
path = 'C:/Users/X58/Documents/GitHub/2019-cap1-2019_10/src/DB' # replace DB/ path
os.chdir(path)
from my_mood_music.models import Anger
with open('anger.csv', encoding='utf-8') as csvfile:
    reader = csv.DictReader(csvfile)
    for row in reader :
        p = Anger(music=row['music'], age=row['age_a'],link=row['link_a'], tag_1=row['tag_a1'], tag_2=row['tag_a2'])
        p.save()



import csv
import os
path = 'C:/Users/X58/Documents/GitHub/2019-cap1-2019_10/src/DB' 
os.chdir(path)
from my_mood_music.models import Child
with open('child.csv', encoding='utf-8') as csvfile:
    reader = csv.DictReader(csvfile)
    for row in reader :
        p = Child(music=row["music_c"],link=row["link_c"])
        p.save()

import csv
import os
path = 'C:/Users/X58/Documents/GitHub/2019-cap1-2019_10/src/DB'
os.chdir(path)
from my_mood_music.models import Lie
with open('lie.csv', encoding='utf-8') as csvfile:
    reader = csv.DictReader(csvfile)
    for row in reader :
        p = Lie(music=row["music_l"], link=row["link_l"])
        p.save()

import csv
import os
path = 'C:/Users/X58/Documents/GitHub/2019-cap1-2019_10/src/DB'
os.chdir(path)
from my_mood_music.models import Surprise
with open('surprise.csv', encoding='utf-8') as csvfile:
    reader = csv.DictReader(csvfile)
    for row in reader :
        p = Surprise(music=row["music"],age=row["age_su"],link=row["link_su"], tag_1=row["tag_su1"], tag_2=row["tag_su2"])
        p.save()

import csv
import os
path = 'C:/Users/X58/Documents/GitHub/2019-cap1-2019_10/src/DB'
os.chdir(path)
from my_mood_music.models import Disgust
with open('disgust.csv', encoding='utf-8') as csvfile:
    reader = csv.DictReader(csvfile)
    for row in reader :
        p = Disgust(music=row["music"],age=row["age_d"],link=row["link_d"], tag_1=row["tag_d1"], tag_2=row["tag_d2"])
        p.save()

import csv
import os
path = 'C:/Users/X58/Documents/GitHub/2019-cap1-2019_10/src/DB'
os.chdir(path)
from my_mood_music.models import Fear
with open('fear.csv', encoding='utf-8') as csvfile:
    reader = csv.DictReader(csvfile)
    for row in reader :
        p = Fear(music=row["music"],age=row["age_f"],link=row["link_f"], tag_1=row["tag_f1"], tag_2=row["tag_f2"])
        p.save()

import csv
import os
path = 'C:/Users/X58/Documents/GitHub/2019-cap1-2019_10/src/DB'
os.chdir(path)
from my_mood_music.models import Happiness
with open('happiness.csv', encoding='utf-8') as csvfile:
    reader = csv.DictReader(csvfile)
    for row in reader :
        p = Happiness(music=row["music_h"],age=row["age_h"],link=row["link_h"], tag_1=row["tag_h1"], tag_2=row["tag_h2"])
        p.save()

import csv
import os
path = 'C:/Users/X58/Documents/GitHub/2019-cap1-2019_10/src/DB'
os.chdir(path)
from my_mood_music.models import Sadness
with open('sadness.csv', encoding='utf-8') as csvfile:
    reader = csv.DictReader(csvfile)
    for row in reader :
        p = Sadness(music=row["music"],age=row["age_s"],link=row["link_s "], subclass_s=row["subclass_s"], tag_1=row["tag_s1"], tag_2=row["tag_s2"])
        p.save()
