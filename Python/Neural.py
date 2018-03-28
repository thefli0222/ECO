from tensorflow.python.keras.layers import Activation, Dense #import för layer egenskaper
from tensorflow.python.keras.models import Sequential #skapar ett objekt för att bygga det neurala nätverket på
from tensorflow.python.keras.optimizers import SGD #SGD=stochastic gradient descent, mer effektiv än vanlig gradient descent, se https://www.tensorflow.org/api_docs/python/tf/keras/optimizers för samtliga optimizers
import numpy as np #För linjär algebra
from sklearn.model_selection import train_test_split #scikitlearn metod som randomizar datan för training vs testing i 4 vektorer

path_to_data = "404lines.txt"

features = np.array(np.loadtxt(path_to_data, usecols = (range(0, 10)))) #skapar features och labels, usecols för att separera kolumner.
labels = np.array(np.loadtxt(path_to_data, usecols = (10, 11)))
a_wins = []
b_wins = []
#for x in range(0, len(labels)): #avkommentera och kommentera andra loopen för att få varje output som antal vinster för lag a/b dividerat med totala antal matcher spelade. NOTERA förändring i activation och loss_function
#	winRateA.append(labels[x, 0] / (labels[x, 1] + labels[x, 0]))
#	winRateB.append(labels[x, 1] / (labels[x, 1] + labels[x, 0]))
for x in range(0, len(labels)): #avkommentera och kommentera andra loopen för att få varje output som vektorer på formen [1 0]/[0 1] för a vinst/b vinst. NOTERA förändring i activation och loss_function
	if(labels[x, 0] / (labels[x , 1] + labels[x, 0]) > 0.5):
		a_wins.append(1)
		b_wins.append(0)
	else:
		a_wins.append(0)
		b_wins.append(1)
winPercent = np.c_[a_wins, b_wins] #mergar a_wins och b_wins kolumnvis

(training_data, testing_data, training_labels, testing_labels) = train_test_split(features, winPercent, test_size=0.2, random_state=1) #Se import ovan, random_state seedar randomiseringen för upprepbara tester.

neural_network = Sequential() #Se import ovan


neural_network.add(Dense(6, input_shape=(10,))) #Första layeret måste specificera input shape, dvs antal inputnoder
neural_network.add(Activation('relu')) #relu=rectified linear unit > sigmoid för hidden layers

#neural_network.add(Dense(4))  #Ta bort kommentarer för ett andra hidden layer
#neural_network.add(Activation('relu'))

neural_network.add(Dense(2)) #output layer för binär klassificering/regression
neural_network.add(Activation('sigmoid')) #Byt sigmoid till softmax om inte klassificering

optimizer = SGD(lr = 0.01, momentum = 0, decay = 0) #Som sagt optimizern, se import ovan för mer information
loss_function = 'binary_crossentropy' #costfunktionen, kan ändras till bla. mean_squared_error men denna funkar bättre de flesta fall. Se https://www.tensorflow.org/api_docs/python/tf/keras/losses för samtliga costfunktioner.

neural_network.compile(loss = loss_function, optimizer = optimizer, metrics = ['accuracy']) #kompilerar modellen och lägger till 'accuracy' i printouten i nästa steg.

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