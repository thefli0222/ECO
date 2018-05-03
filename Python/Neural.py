from tensorflow.python.keras.layers import Activation, Dense, Dropout #import för layer egenskaper
from tensorflow.python.keras.models import Sequential #skapar ett objekt för att bygga det neurala nätverket på
from tensorflow.python.keras.optimizers import SGD, Adagrad, Adam #SGD=stochastic gradient descent, mer effektiv än vanlig gradient descent, se https://www.tensorflow.org/api_docs/python/tf/keras/optimizers för samtliga optimizers
from tensorflow.python.keras.callbacks import EarlyStopping
import matplotlib.pyplot as plt #Plots
import numpy as np #För linjär algebra


path_to_data = "4004lines.txt"
path_to_validation_data = "validationData.txt"

classification = True #Klassificering eller regression
hidden_layers = 2  #Antal hidden layers


with open(path_to_data) as f: #Antalet klasser utläses i första raden på inputfilen, se 404lines.txt för exempel.
    number_of_classes = int(f.readline()) 

def createNeuralNetwork(activation, cost, optimization):
	neural_network = Sequential() #Se import ovan

	neural_network.add(Dense(number_of_classes+1, input_shape=(2*number_of_classes,))) #input layer
	neural_network.add(Activation("relu"))

	if(hidden_layers > 1):

		for i in range(1,hidden_layers):
			neural_network.add(Dense(number_of_classes+1))
			neural_network.add(Dropout(0.13))
			neural_network.add(Activation("relu"))

	neural_network.add(Dense(2)) #output layer för binär klassificering/regression
	neural_network.add(Activation(activation))

	neural_network.compile(loss = cost, optimizer = optimization, metrics = ["accuracy"]) #kompilerar modellen och lägger till 'accuracy' i printouten i nästa steg.
	return neural_network

def parseData():

	data = np.array(np.loadtxt(path_to_data, skiprows = 1)) # Läs in data
	validation_data = np.loadtxt(path_to_validation_data)
	indices = []

	for i in range(len(data)):
		for j in range(len(data[1])):
			if(data[i][j] > 30 or data[i][j] < 0 or not isinstance(data[i][j], np.float64)):
				indices.append(i)
				break


	data = np.delete(data, indices, axis=0)

	np.random.shuffle(data) #randomiza datan för att slumpa training/testing/validation data

	features = data[:, :-2]
	labels = data[:, (-2,-1)]

	validation_features = validation_data[:, :-2]
	validation_labels = validation_data[:, (-2,-1)]

	matches = len(features)

	new_features = np.zeros((matches, 2*number_of_classes)) #skapar nya features med binära inputs för existens av klass.

	for i in range(0, matches):
		for j in range(0, 10):
			if(j < 5):
				new_features[i,features[i,j].astype(np.int64)] += 0.2
			else:
				new_features[i,features[i,j].astype(np.int64)+number_of_classes] += 0.2

	validation_matches = len(validation_features)
	validation_data = np.zeros((validation_matches, 2*number_of_classes)) #skapar nya features med binära inputs för existens av klass.

	for i in range(0, validation_matches):
		for j in range(0, 10):
			if(j < 5):
				validation_data[i,validation_features[i,j].astype(np.int64)] += 0.2
			else:
				validation_data[i,validation_features[i,j].astype(np.int64)+number_of_classes] += 0.2

	#with open("newfeatures.txt", "w") as f: # skapar fil med nya inputs för inspektion
	#	f.write("".join(str(e) for e in new_features.tolist()))

	training_data = new_features[:int(matches*0.75)]
	testing_data = new_features[int(matches*0.75):]

	return (matches, validation_matches, labels, validation_labels, training_data, testing_data, validation_data)

def parseLabels(matches, validation_matches, labels, validation_labels):
	a_wins = []
	b_wins = []

	validation_a_wins = []
	validation_b_wins = []

	#Regression START
	if(classification == False):
		output_activation = "softmax"
		loss_function = "mean_squared_error"

		for x in range(0, matches):
			a_wins.append(labels[x, 0] / (labels[x, 1] + labels[x, 0]))
			b_wins.append(labels[x, 1] / (labels[x, 1] + labels[x, 0]))

		for x in range(0, validation_matches):
			validation_a_wins.append(validation_labels[x, 0] / (validation_labels[x, 1] + validation_labels[x, 0]))
			validation_b_wins.append(validation_labels[x, 1] / (validation_labels[x, 1] + validation_labels[x, 0]))
	#Regression END

	#Klassificering START
	if(classification == True):
		output_activation = "sigmoid"
		loss_function = "binary_crossentropy"

		for x in range(0, matches):
			if(labels[x, 0] / (labels[x , 1] + labels[x, 0]) > 0.5):
				a_wins.append(1)
				b_wins.append(0)
			else:
				a_wins.append(0)
				b_wins.append(1)

		for x in range(0, validation_matches):
			if(validation_labels[x, 0] / (validation_labels[x , 1] + validation_labels[x, 0]) > 0.5):
				validation_a_wins.append(1)
				validation_b_wins.append(0)
			else:
				validation_a_wins.append(0)
				validation_b_wins.append(1)
	#Klassificering END

	win_percent = np.c_[a_wins, b_wins] #mergar a_wins och b_wins kolumnvis

	training_labels = win_percent[:int(matches*0.75)]
	testing_labels = win_percent[int(matches*0.75):]
	validation_labels = np.c_[validation_a_wins, validation_b_wins]

	return (training_labels, testing_labels, validation_labels, output_activation, loss_function)

def main():
	(matches, validation_matches, labels, validation_labels, training_data, testing_data, validation_data) = parseData()
	(training_labels, testing_labels, validation_labels, output_activation, loss_function) = parseLabels(matches, validation_matches, labels, validation_labels)
	early_stop = EarlyStopping(monitor='val_loss',min_delta=0,patience=1,verbose=0, mode='auto')

	#optimizer_function = Adagrad(lr = 0.14, momentum = 0.5, decay = 0, nesterov = True) #Som sagt optimizern, se import ovan för mer information
	#optimizer_function = Adagrad(lr = 0.01) #Som sagt optimizern, se import ovan för mer information
	pwr = np.random.uniform(-6, -1,10)
	#for i in range(1,10):
	optimizer_function = Adam(lr = 10 ** pwr[1]) #Som sagt optimizern, se import ovan för mer information
	neural_net = createNeuralNetwork(output_activation, loss_function, optimizer_function)

	training = neural_net.fit(training_data, training_labels, validation_data = (validation_data, validation_labels), epochs = 50, batch_size = 64, verbose = 2) #tränar nätverket, 300 iterationer, ändra verbose till 0 för att inte printa alls, 1 för cool progress bar!
	(testing_loss, testing_accuracy) = neural_net.evaluate(testing_data, testing_labels, batch_size = 16, verbose = 1) #testar nätverket & printar progressen
	print("\n Loss function on test data = {:.4f}, Accuracy on test data = {:.4f}%".format(testing_loss, testing_accuracy * 100)) #printar ut cost/accuracyn på testvektorerna

	neural_net.summary() #printar ut nätverkets noder och kanter
	print(training.history.keys())

	plt.plot(training.history["acc"])
	plt.plot(training.history["val_acc"])
	plt.plot(training.history["loss"])
	plt.plot(training.history["val_loss"])

	plt.title("Accuracy and Losses for training and validation data over epochs.")
	plt.xlabel("Epoch")
	plt.ylabel("Accuracy and Loss")
	plt.legend(["Training accuracy", "Validation accuracy", "Training loss", "Validation loss"], loc="lower right")

	plt.show()

	#prediction = neural_network.predict(testing_data) #printar prediction vs result, varning kluddrigt...
	#for i in range(0,len(testing_data)):
	#	print("Prediction #", i+1,":")
	#	print(prediction[i])
	#	print("Result #", i+1,":")
	#	print(testing_labels[i])

if __name__ == "__main__":
    main()
