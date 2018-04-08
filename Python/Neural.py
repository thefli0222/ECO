from tensorflow.python.keras.layers import Activation, Dense #import för layer egenskaper
from tensorflow.python.keras.models import Sequential #skapar ett objekt för att bygga det neurala nätverket på
from tensorflow.python.keras.optimizers import SGD #SGD=stochastic gradient descent, mer effektiv än vanlig gradient descent, se https://www.tensorflow.org/api_docs/python/tf/keras/optimizers för samtliga optimizers
import numpy as np #För linjär algebra
from sklearn.model_selection import train_test_split #scikitlearn metod som randomizar datan för training vs testing i 4 vektorer

path_to_data = "404lines.txt"

classification = True #klassificering eller regression
hidden_layers = 1  #antal hidden layers

with open(path_to_data) as f: #Antalet klasser utläses i första raden på inputfilen, se 404lines.txt för exempel.
    number_of_classes = int(f.readline()) 

features = np.array(np.loadtxt(path_to_data, usecols = (range(0, 10)), skiprows = 1)) #skapar features och labels, usecols för att separera kolumner.

new_features = np.zeros((len(features), 2*number_of_classes)) #skapar nya features med binära inputs för existens av klass.
for i in range(0, len(features)):
	for j in range(0, 10):
		if(j < 5):
			new_features[i,features[i,j].astype(np.int64)] += 0.2
		else:
			new_features[i,features[i,j].astype(np.int64)+12] += 0.2

with open("newfeatures.txt", "w") as f: # skapar fil med nya inputs för inspektion
	f.write("".join(str(e) for e in new_features.tolist()))

labels = np.array(np.loadtxt(path_to_data, usecols = (10, 11), skiprows = 1)) #skapar labelsen
a_wins = []
b_wins = []

#Regression START
if(classification == False):
	output_activation = "softmax"
	loss_function = "mean_squared_error"

	for x in range(0, len(labels)):
		a_wins.append(labels[x, 0] / (labels[x, 1] + labels[x, 0]))
		b_wins.append(labels[x, 1] / (labels[x, 1] + labels[x, 0]))
#Regression END

#Klassificering START
if(classification == True):
	output_activation = "sigmoid"
	loss_function = "binary_crossentropy"

	for x in range(0, len(labels)):
		if(labels[x, 0] / (labels[x , 1] + labels[x, 0]) > 0.5):
			a_wins.append(1)
			b_wins.append(0)
		else:
			a_wins.append(0)
			b_wins.append(1)
#Klassificering END

win_percent = np.c_[a_wins, b_wins] #mergar a_wins och b_wins kolumnvis

(training_data, testing_data, training_labels, testing_labels) = train_test_split(new_features, win_percent, test_size=0.25, random_state=1) #Se import ovan, random_state seedar randomiseringen för upprepbara tester.

neural_network = Sequential() #Se import ovan

neural_network.add(Dense(number_of_classes+1, input_shape=(2*number_of_classes,))) #input layer
neural_network.add(Activation("relu"))

if(hidden_layers > 1):

	for i in range(1,hidden_layers):
		neural_network.add(Dense(number_of_classes+1))
		neural_network.add(Activation("relu"))

neural_network.add(Dense(2)) #output layer för binär klassificering/regression
neural_network.add(Activation(output_activation))

optimizer = SGD(lr = 0.01, momentum = 0, decay = 0) #Som sagt optimizern, se import ovan för mer information

neural_network.compile(loss = loss_function, optimizer = optimizer, metrics = ["accuracy"]) #kompilerar modellen och lägger till 'accuracy' i printouten i nästa steg.

neural_network.fit(training_data, training_labels, epochs = 300, batch_size = 32, verbose = 2) #tränar nätverket, 300 iterationer, ändra verbose till 0 för att inte printa alls, 1 för cool progress bar!
(loss, accuracy) = neural_network.evaluate(testing_data, testing_labels, batch_size = 64, verbose = 2) #testar nätverket & printar progressen

print("\n Loss function on test data = {:.4f}, Accuracy on test data = {:.4f}%".format(loss, accuracy * 100)) #printar ut cost/accuracyn på testvektorerna
neural_network.summary() #printar ut nätverkets noder och kanter


#prediction = neural_network.predict(testing_data) #printar prediction vs result, varning kluddrigt...
#for i in range(0,len(testing_data)):
#	print("Prediction #", i+1,":")
#	print(prediction[i])
#	print("Result #", i+1,":")
#	print(testing_labels[i])
