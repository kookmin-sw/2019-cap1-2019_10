#-*- coding: utf-8 -*-

import tensorflow as tf
import numpy as np

n_dim = 39  #mfcc 벡터
n_classes = 8   #8가지 음성을 분류함.
n_hidden1 = 300
n_hidden2 = 200
n_hidden3 = 100

training_times = 5000   #훈련횟수
learning_rate = 0.01    #매개변수가 변동되는 속도
sd = 1 / np.sqrt(n_dim)

# 입력 데이터, 정답 데이터 파라미터 생성.
X = tf.placeholder(tf.float32, [None, n_dim])
Y = tf.placeholder(tf.float32, [None, n_classes])

#1차 hidden layer
W_1 = tf.Variable(tf.random_normal([n_dim, n_hidden1], mean = 0, stddev = sd), name="w1")
b_1 = tf.Variable(tf.random_normal([n_hidden1], mean = 0, stddev = sd), name="b1")
h_1 = tf.nn.relu(tf.matmul(X, W_1)+b_1)
#시그모이드함수, 탄젠트하이퍼볼릭과 비교

#2차 hidden layer
W_2 = tf.Variable(tf.random_normal([n_hidden1, n_hidden2], mean=0, stddev=sd), name="w2")
b_2 = tf.Variable(tf.random_normal([n_hidden2], mean=0, stddev=sd), name="b2")
h_2 = tf.nn.tanh(tf.matmul(h_1, W_2)+b_2)
#왜 relu와 시그모이드를 사용하지 않았는가??

#3차 hidden layer
W_3 = tf.Variable(tf.random_normal([n_hidden2,n_hidden3], mean=0, stddev=sd),name="w3")
b_3 = tf.Variable(tf.random_normal([n_hidden3], mean=0, stddev=sd), name="b3")
h_3 = tf.nn.relu(tf.matmul(h_2, W_3)+b_3)
#시그모이드함수, 탄젠트하이퍼볼릭과 비교

#드롭아웃 과정
keep_prob = tf.placeholder(tf.float32)
h_3_drop = tf.nn.dropout(h_3, keep_prob)

W = tf.Variable(tf.random_normal([n_hidden3, n_classes], mean=0, stddev=sd), name="w")
b = tf.Variable(tf.random_normal([n_classes], mean=0, stddev=sd), name="b")
y_ = tf.nn.softmax(tf.matmul(h_3_drop, W)+b)
#one hot encoder를 할 것인가?
#기계가 추측한 결과 y_, 실제 정답 Y

#기대하는값과 실제 출력값 사이의 오차를 재기위함. cross entropy error
cross_entropy = -tf.reduce_sum(Y*tf.log(y_))

#경사하강법.
train_step = tf.train.GradientDescentOptimizer(learning_rate).minimize(cross_entropy)
#adam 최적화 알고리즘과 비교!!

#정확도 계산
correct_prediction = tf.equal(tf.argmax(y_, 1), tf.argmax(Y, 1)) #예측값과 실제 레이블 비교.
accuracy = tf.reduce_mean(tf.cast(correct_prediction, tf.float32))#boolean값을 수치값으로 변경.

#학습된 데이터 저장
saver = tf.train.Saver()

#학습 실행!!
sess = tf.Session()

sess.run(tf.global_variables_initializer())

#파일을 dictionary형태로 가져오는거 테스트해보자
#중간서버에 음성파일이 어떤형태로 전송되는가??




