age = get(user.age) #연령 체크

#10세 미만이면 어린이 테이블로 가서 랜덤으로 3개의 동요를 뽑고 바로 종료한다. 
if age<10 : 
	get random 3 songs in 'Child Table'
	music_list.append(those 3 songs)

#어린이 나이가 아니라면, 추천 알고리즘에 의해 감정까지 고려한 노래 3개를 뽑는다.
else :
	#연령대에 따라서 age를 결정한다. 
	if age>=10 and age<=29 : 	
		age = 20
	elif age>=30 and age<=39 : 
		age = 30
	else :
		age = 40

	#추천알고리즘에서 리턴하는 테이블 인덱스를 리스트 인덱스로 접근할 수 있도록 table_index 리스트 생성 
	table_index = [Anger, Disgust, Fear, Happiness, Sadness, Surprise, Lie]

	#추천알고리즘에서 리턴하는 테이블에 대한 정보 (노래 3개->3개의 요소)
	t_info = [table1, table2, table3] 

	for tableN in t_info : 
		if tableN in [41, 42, 43] : #Sad테이블에 접근해야하는 경우
			table = table_index[4] #Sad
			subclass_sad = tableN % 40 # subclass : 1 or 2 or 3
			music = table.objects.filter(subclass_s = subclass_sad).random() #Sad테이블 중 subclass_s가 subclass인 노래들 중 랜덤 선택
			
		else : #Sad테이블 이외의 테이블에 접근해야하는 경우
			table = table_index[tableN]
			
		music = table.objects.filter(age_s = age).random() #해당 연령 범위의 노래로 추림
		music_list.append(music)

return music_list








