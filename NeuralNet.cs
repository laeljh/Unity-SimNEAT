using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;
public class NeuralNet : MonoBehaviour
{
    
    /* This script will define the structure of a neuron class and a neural net class
  * 
  * Neuron class will include it's global number, function type, sum of received inputs, list of neurons it forwards to
  * as well as methods for activation functions, methods to change the values of variables and fire function
  * 
  * Neural net class will consist mainly of a dictionary of Neurons, and lists for I/O and Deep neurons
  * the key will be the unique int number of each neuron, the value will be the actual neuron object
  * 
  * 
  * FIRE -> for output neuron should either return the raw sum of neurons or perhaps sigmoid function 
  */

    /*FUNCTION NUMBERS
     *0 - pass/identity y = x
     *1 - linear step (y = 1 for x >= 0.5 AND y = 0 for x < 0)
     *2 - sigmoid y = (1/(1+e^-x)
     *3 - tanh y = ((e^x)-(e^-x))/((e^x)+(e^-x))
     *4 - google swish y = x/(1+e^-x)
     *5 - absolute y = abs(x)
     *6 - cubic y = x^3
     *7 - sinus y = sin(x)
     *8 - square y = x^2
     *9 - squre root y = sqrt(x);
     *10 - cubic root y = x^(1/3);
     *11 - opposite y = -x
     *12 - inverse y = 1/x;
     */

    /*
     * PROCESS OF MAKING A NEURON
     * n - neuron, nn - neuralnet  <- shortcuts for methods locations
     * 1) make a new neuron with global number based on nn.PickNeuronNumber()
     * 2) decide if it's input or output using n.MakeInput() or n.MakeOutput()
     * 3) function name and other values get updated automatically
     * 
     */

    //the neuron class is fully functional, as for the moment it doesn't prevent mutations in inputs or outputs, this can be managed by the neural net
    //both Fire() and GetActivationFunctionName() use paralel switchboards for deciding the outcomes, the number of outcomes must be made always the same
    //and the results must match cases in the switch
    //also the current number of available functions should be reflected in maxActivationFunction
    public class Neuron
    {
        //must be the same as the numbers of functions
        private int maxActivationFunction = 11;
        /*FUNCTION NUMBERS
        *0 - pass/identity y = x
        *1 - linear step (y = 1 for x >= 0.5 AND y = 0 for x < 0)
        *2 - sigmoid y = (1/(1+e^-x)
        *3 - tanh y = ((e^x)-(e^-x))/((e^x)+(e^-x))
        *4 - google swish y = x/(1+e^-x)
        *5 - absolute y = abs(x)
        *6 - cubic y = x^3
        *7 - sinus y = sin(x)
        *8 - square y = x^2
        */

        private int globalNumber;
        private int functionNumber;
        private float sumOfInputs;
        public float lastSumOfInputs;
        private List<int> forwardsTo;
        private float lastCalculatedOutput;
        private bool isInput = false;
        private bool isOutput = false;
        private string activationFunctionName = "";
        
        //constructor using "new Neuron()" requires providing a neuron number, the default activation fucntion is pass
        public Neuron(int neuronNumber, int activationFuctionNumber = 0)
        {
            globalNumber = neuronNumber;
            functionNumber = activationFuctionNumber;
            sumOfInputs = 0;
            forwardsTo = new List<int>();
            GetActivFunctionName();
        }

        //methods:

        //set the neuron global number - using set is not required as neuron can't be created without providing the number
        public void SetNumber(int number)
        {
            globalNumber = number;
        }
        //gets the neuron global number 
        public int GetGlobalNumber()
        {
            return globalNumber;
        }
        //set the list of neurons this one is supposed to send signal to when fired
        public void SetForwards(List<int> destinationNeuronsList)
        {
            if(!isOutput)//output neurons don't forward signals
            {
                forwardsTo = destinationNeuronsList;
            }
            
        }
        //gets a list of neurons it forwards to
        public List<int> GetForwards()
        {
            return forwardsTo;
        }



        
        //sets function number and updates the function name in the neurons variables
        public void SetActivationFunction(int activationFunctionNumber)
        {
            if(!isInput) //input can only use pass function, so check that it's not an input before allowing to change the function!
            {
                if (activationFunctionNumber > maxActivationFunction)
                {
                    //Debug.Log("Used a function number beyond available, scope, reverting to default");
                    functionNumber = 0;
                }
                else
                {
                    functionNumber = activationFunctionNumber;
                }
            }

            GetActivFunctionName();
        }
        //saves the name of the function based on it's number into neurons variable and returns it's name
        public string GetActivFunctionName()
        {
            switch (functionNumber) //gets the function number and runs the sum of input trhoug to get the output
            {
                /*FUNCTION NUMBERS
                 *0 - pass/identity y = x
                 *1 - linear step (y = 1 for x >= 0.5 AND y = 0 for x < 0)
                 *2 - sigmoid y = (1/(1+e^-x)
                 *3 - tanh y = ((e^x)-(e^-x))/((e^x)+(e^-x))
                 *4 - google swish y = x/(1+e^-x)
                 *5 - absolute y = abs(x)
                 *6 - cubic y = x^3
                 *7 - sinus y = sin(x)
                 *8 - square y = x^2
                 *9 - squre root y = sqrt(x);
                 *10 - cubic root y = x^(1/3);
                 *11 - opposite y = -x
                 *12 - inverse y = 1/x;
                 */
                case 0:
                    activationFunctionName = "pass/identity";
                    break;
                case 1:
                    activationFunctionName = "linear step";
                    break;
                case 2:
                    activationFunctionName = "sigmoid";
                    break;
                case 3:
                    activationFunctionName = "tanh";
                    break;
                case 4:
                    activationFunctionName = "google swish";
                    break;
                case 5:
                    activationFunctionName = "absolute";
                    break;
                case 6:
                    activationFunctionName = "cubic";
                    break;
                case 7:
                    activationFunctionName = "sinus";
                    break;
                case 8:
                    activationFunctionName = "square";
                    break;
                case 9:
                    activationFunctionName = "square root";
                    break;
                case 10:
                    activationFunctionName = "cubic root";
                    break;
                case 11:
                    activationFunctionName = "opposite";
                    break;
                case 12:
                    activationFunctionName = "inverse";
                    break;
                default:
                    activationFunctionName = "DEFAULT, INDEX OUTSIDE SCOPE";
                    //Debug.Log("Given non existing function index for the neuron, falling back to using pass function");
                    break;




            }
            return activationFunctionName;
        }

        public int GetActivFunctionNumber()
        {
            return functionNumber;
        }
        //add input from a neuron to this one sum of inputs
        public void ReceiveInput(float input)
        {
            sumOfInputs += input;
        }
        //takes all the inputs and returns a value of a function, then zeroes the sum of inputs for the next turn, for output neurons it just returns the sum of inputs, zeroing it
        public float Fire()
        {

            /*FUNCTION NUMBERS
             *0 - pass/identity y = x
             *1 - linear step (y = 1 for x >= 0.5 AND y = 0 for x < 0)
             *2 - sigmoid y = (1/(1+e^-x)
             *3 - tanh y = ((e^x)-(e^-x))/((e^x)+(e^-x))
             *4 - google swish y = x/(1+e^-x)
             *5 - absolute y = abs(x)
             *6 - cubic y = x^3
             *7 - sinus y = sin(x)
             *8 - square y = x^2
             *9 - squre root y = sqrt(x);
             *10 - cubic root y = x^(1/3);
             *11 - opposite y = -x
             *12 - inverse y = 1/x;
             */

            float x = sumOfInputs;
            float output = 0;

            switch (functionNumber) //gets the function number and runs the sum of input trhoug to get the output
            {
                case 0:
                    output = x;
                    break;
                case 1:
                    if (x < 0.5) output = 0; else output = 1;
                    break;
                case 2:
                    output = (1 / (1 + Mathf.Exp(-x)));
                    break;
                case 3:
                    output = (float)System.Math.Tanh(x);                    
                    break;
                case 4:
                    output = x / (1 + Mathf.Exp(-x));
                    break;
                case 5:
                    output = Mathf.Abs(x);
                    break;
                case 6:
                    output = Mathf.Pow(x, 3);
                    break;
                case 7:
                    output = Mathf.Sin(x);
                    break;
                case 8:
                    output = Mathf.Pow(x, 2);
                    break;
                case 9:
                    if (x >= 0)
                    {
                        output = Mathf.Sqrt(x);
                    }
                    else
                    {
                        output = (-(float)Math.Sqrt(-x));
                    }
                    break;
                case 10:
                    output = Mathf.Pow(x, (1 / 3));
                    break;
                case 11:
                    output = -x;
                    break;
                case 12:
                    output = 1/x;
                    break;
                default:
                    output = x;
                    //Debug.Log("Given non existing function index for the neuron, falling back to using pass function");
                    break;


    

            }
            lastSumOfInputs = sumOfInputs;
            sumOfInputs = 0;
            lastCalculatedOutput = output;
            return output;

        }


        //sets the type of the neuron to be able to check if it's not input or output or deep, by default it's deep
        public void MakeOutput()
        {
            isOutput = true;
            isInput = false;
            forwardsTo.Clear();
        }
        public void MakeInput()
        {
            isOutput = false;
            isInput = true;
            functionNumber = 0;
            GetActivFunctionName();
            
        }
        public void MakeDeep()
        {
            isOutput = false;
            isInput = false;
        }
        //Get a string of type of neuron
        public string GetNeuronType()
        {
            if(isOutput && !isInput)
            {
                return "Output";
            }
            else if(!isOutput && isInput)
            {
                return "Input";
            }
            else if(!isInput && !isOutput)
            {
                return "Deep";
            }
            else
            {
                return "WRONG TYPE";
            }
        }
        
        //get the previously calculated output
        public float GetLastOutput()
        {
            return lastCalculatedOutput;
        }
        //get the sum of inputs in this cycle, before the fire
        public float GetLastSumOfInputs()
        {
            return lastSumOfInputs;
        }
        //gets info how many different functions there are available
        public int GetMaxFunctionNumber()
        {
            return maxActivationFunction;
        }

        //adds a connection connection
        public void AddConnection(int neuronNumber)
        {
            if (!forwardsTo.Contains(neuronNumber))
            {
                forwardsTo.Add(neuronNumber);
            }
            else
            {
                //Debug.Log($"Can't add a connection. A connection from {GetGlobalNumber()} to {neuronNumber} already existed.");
            }
        }
        //removes a connection on the list
        public void RemoveConnection(int neuronNumber)
        {
            if (forwardsTo.Contains(neuronNumber))
            {
                forwardsTo.Remove(neuronNumber);
            }
            else
            {
                //Debug.Log($"Can't remove connection. A connection from {GetGlobalNumber()} to {neuronNumber} does not exist.");
            }
        }

        //returns true or false for questions - isOutput? and isInput?
        public bool IsOutput()
        {
            return isOutput;
        }
        public bool IsInput()
        {
            return isInput;
        }
    }

    //NN class so far fully functional,    
    public class NeuralNetwork
    {
        //constructor
        public NeuralNetwork()
        {

        }
        public NeuralNetwork(Dictionary<int,Neuron> neuronDictionary)
        {
            ImportNeuronsDNA(neuronDictionary);
        }

        


        //random number generator
        static System.Random rand = new System.Random();
        //private variables
        private Dictionary<int, Neuron> neurons = new Dictionary<int, Neuron>();
        private int neuronNumberNameIterator = 0;
        

        // inputs are always the first neurons in the dicitonary, after them are outputs, this are unmutable, only after then are changable deep neurons
        //therefore these list below once created always is true
        private List<int> inputNeuronNumbers = new List<int>();
        private List<int> outputNeuronNumbers = new List<int>();
        //deep neurons list - is refreshed via method, should be refreshed whenever a new neuron is added in a mutiaton,
        //mutations happen only during reproduction thought
        private List<int> deepNeuronsList = new List<int>();

        //list of connections generated once at the start of the network, used for deciding fire order later
        private List<(int,int)> connectionsList = new List<(int, int)>();

        //fire order list - defines, once the network is made, in which order it should fire
        private List<int> fireOrder = new List<int>();



        //PRIVATE METHODS

        //uses the neuron name iterator to keep track of what neurons have already existed and gives new neurons new numbers
        private int PickNeuronNumber()
        {
            int thisNumber = neuronNumberNameIterator;
            neuronNumberNameIterator++;
            return thisNumber;
        }
        //generates connections list from each neuron, there's a public method for getting the list
        public void GenerateConnectionsList()
        {
            connectionsList.Clear();
            //iterate through every element in neurons dectionary
            foreach (KeyValuePair<int, Neuron> kvp in neurons)
            {
                //iterate through every forward it has 
                foreach (int forward in kvp.Value.GetForwards())
                {
                    //add a pair of (currentNeuronNumber, currentForward)
                    connectionsList.Add((kvp.Value.GetGlobalNumber(), forward));
                }

            }

        }

        public void RefreshEnumerator()
        {
            int highestNum = 0;
            foreach(int neuronNumber in inputNeuronNumbers.Concat(deepNeuronsList).Concat(outputNeuronNumbers).ToList())
            {
                if (neuronNumber > highestNum)
                {
                    highestNum = neuronNumber;
                }
            }
            neuronNumberNameIterator = highestNum + 1;
        }

        
        //makes inputs/outputs for the network by edfault outputs are ranged -1 to 1
        public void MakeInputsOutputs(int inputs, int outputs, bool outputsWithFunction = true, bool remake = false)
        {
            //if remake set to true, or input outs are not made yet
            if(remake || (inputNeuronNumbers.Count()==0 & outputNeuronNumbers.Count() == 0))
            {
                
                for (int i = 0; i < inputs; i++)
                {
                    Neuron n = new Neuron(PickNeuronNumber());
                    n.MakeInput();
                    neurons.Add(n.GetGlobalNumber(), n);
                    inputNeuronNumbers.Add(n.GetGlobalNumber());
                }
                for (int i = 0; i < outputs; i++)
                {
                    Neuron n = new Neuron(PickNeuronNumber());
                    n.MakeOutput();
                    if (outputsWithFunction)
                    {
                        n.SetActivationFunction(3);
                    }
                    neurons.Add(n.GetGlobalNumber(), n);
                    outputNeuronNumbers.Add(n.GetGlobalNumber());
                }
                List<string> ins = new List<string>();
                foreach(int inN in GetInputNeurons())
                {
                    ins.Add(inN.ToString());
                }
                List<string> outs = new List<string>();
                foreach (int outN in GetOutputNeurons())
                {
                    outs.Add(outN.ToString());
                }
                Debug.Log($"Made I/O layers: {string.Join(", ", ins)} input neurons, and {string.Join(", ", outs)} output neurons");
            }

        }
        //makes random connections from inputs to outputs,
        //maxConnections specify how many connections maximally can grow from each neuron, but if it tries to grow to existing connection the growth will fail
        //chance specifies what's the probability that in each iteration, the connection will try to grow, then the result also depends
        public void InitRandomConnections(int maxConnections = 1, float chance = 1)
        {
            //give as many attempts to grow a new connection as there is specified in max connections    
            for (int i = 0; i < maxConnections; i++)
            {    
                //if defined growth chance was higher than random from 0-1, try to grow connection            
                if (chance > rand.NextDouble())
                {
                    //uses a list of neurons that could potentially form new connections
                    List<int> neuronsToConnectFrom = inputNeuronNumbers.Concat(deepNeuronsList).ToList();
                    //picks randomly one of the neurons
                    int drawSourceNeuron = neuronsToConnectFrom[rand.Next(0, neuronsToConnectFrom.Count())];
                    
                    //from the list of possible output neurons (neurons that can receive inputs)
                    List<int> neuronsToConnectTo = deepNeuronsList.Concat(outputNeuronNumbers).ToList();
                    //pick one at random
                    int drawDestinationNeuron = neuronsToConnectTo[rand.Next(0, neuronsToConnectTo.Count())];

                    //if the connection already existed, it will fail with a message
                    neurons[drawSourceNeuron].AddConnection(drawDestinationNeuron);
                }
            }
        }
        //make dense connections from inputs to outputs
        public void InitDenseConnections()
        {
            foreach(KeyValuePair<int,Neuron> kvp in neurons)
            {
                if(!kvp.Value.IsOutput())
                {
                    kvp.Value.SetForwards(new List<int>(outputNeuronNumbers));
                }
            }
        } 
        //gets the connection list once it's generated
        public List<(int,int)> GetConnectionsList()
        {
            if (!(connectionsList.Count() > 0))
            {
                GenerateConnectionsList();
            }
            return connectionsList;
        }
        //generate list of Deep neurons and returns it
        public List<int> GenerateDeepNeuronList()
        {
            //GENERATES LIST OF DEEP NEURONS - ONES THAT AREN'T IN OR OUTS
            deepNeuronsList.Clear();
            foreach (KeyValuePair<int, Neuron> kvp in neurons)
            {
                if (!outputNeuronNumbers.Contains(kvp.Value.GetGlobalNumber()) && !inputNeuronNumbers.Contains(kvp.Value.GetGlobalNumber()))
                {
                    deepNeuronsList.Add(kvp.Value.GetGlobalNumber());
                }
                else
                {
                    
                }
            }
            //GENERATION COMPLETE
            return deepNeuronsList;
        }
        //gets list of inputs
        public List<int> GetInputNeurons()
        {
            return inputNeuronNumbers;
        }
        //gets list of outputs
        public List<int> GetOutputNeurons()
        {
            return outputNeuronNumbers;
        }
        //gets a list of neurons ordered by how soon they should fire in the chain
        public List<int> GetFireOrder()
        {
            if (fireOrder.Count() <= 0)
            {
                SetFireOrder();
            }
            //return fire order, but at the beginning insert all input neurons as they should fire first
            return fireOrder;
        }


        //read netowrk in debug messages - a lot of info! - FULLY FUNCTIONAL DISABLED FOR NOW BECAUSE OF SPAM IN UNITY LOGS
        public void ReadNetwork(bool detailed = false)
        { /*  
            Debug.Log("Reading the network:");
            Debug.Log($"There are {neurons.Count()} neurons:");
            //INPUTS INFORMATION:
            Debug.Log($"> {inputNeuronNumbers.Count()} input layer neurons:");
            Debug.Log($">>> {string.Join(", ", inputNeuronNumbers)}:");
            //DEEP INFORMATION:
            Debug.Log($"> {neurons.Count()-inputNeuronNumbers.Count()-outputNeuronNumbers.Count()} deep layer neurons:");
            Debug.Log($">>> {string.Join(", ", GenerateDeepNeuronList())}:");
            //OUTPUTS INFORMATION:
            Debug.Log($"> {outputNeuronNumbers.Count()} output layer neurons:");
            Debug.Log($">>> {string.Join(", ", outputNeuronNumbers)}:");
            //CONNECTIONS INFORMATION:
            Debug.Log($"There are {GetConnectionsList().Count()} connections between neurons:");
            Debug.Log($"{string.Join(", ",GetConnectionsList())}");
            if (detailed)
            {
                //DETAILED INFORMATION ON EACH NEURON:
                Debug.Log("Detailed information on every neuron:");
                foreach (KeyValuePair<int, Neuron> kvp in neurons)
                {
                    Debug.Log($"Neuron {kvp.Key} // Type: {kvp.Value.GetNeuronType()} // Activation funciton: {kvp.Value.GetActivFunctionName()} // ForwardsTo: {string.Join(",", kvp.Value.GetForwards())}");
                }
            }
            */
        }
        //manually adds a connection and refreshes neuron list
        public void AddConnection(int neuronSource, int neuronDestination)
        {
            if (!neurons[neuronSource].GetForwards().Contains(neuronDestination))
            {
                neurons[neuronSource].AddConnection(neuronDestination);
                GenerateConnectionsList();
            }
        }
        //MUTATIONS AND METHODS FOR MODIFICATION OF NETWORK AND NEURONS THAT FACILITATE MUTATIONS - RANDOM BASED CHANGES TO THE NETWORK

        //ADD A NEURON ON EXISTING CONNECTION
        //it takes a connection and adds a new neuron on it, the new neuron intercepts old input, and forwards it to the old output
        //if forced set to true and the connection doesn't exist but neurons do, connection will be added first, then a new neuron placed on it
        //afterwars the deep neuron list and connection lists are refreshed
        public void InsertNeuron((int, int) connection, bool forced = false)
        {
            //if it's not forced and the connection exists add the neuron
            if (GetConnectionsList().Contains(connection))
            {
                int source = connection.Item1;
                int destination = connection.Item2;
                //create the neuron
                int newNeuronsNumber = PickNeuronNumber();
                Neuron newNeuron = new Neuron(newNeuronsNumber);
                Debug.Log($"Made new neuron, number {newNeuronsNumber}, between {source} and {destination}.");
                //add the neuron to the list of neurons
                neurons.Add(newNeuronsNumber, newNeuron);
                //set added neurons destination
                neurons[newNeuronsNumber].AddConnection(destination);
                //assign a random function to the new neuron  
                RandomizeNeuronFunction(neurons[newNeuronsNumber]);
                //set it to receive from the desired source, by making it that sources forward destination
                neurons[source].AddConnection(newNeuronsNumber);
                //remove the old connection 
                neurons[source].RemoveConnection(destination);
                //remake the deep neurons list and the connections list
                GenerateDeepNeuronList();
                GenerateConnectionsList();

            }
            else if (!forced)
            {
                //Debug.Log($"Can't add neuron, the specified path {connection} on which to add the neuron doesn't exist");
            }
            //if it's forced, don't need to check if the conneciton exist, but will check later if the neurons exist
            else if (forced) //adding a neuron without checking the prequisite for exesting path on which to add the neuron
            {
                int source = connection.Item1;
                int destination = connection.Item2;
                //if both neurons are present create a neuron with all necessary connections
                if (neurons.ContainsKey(source) && neurons.ContainsKey(destination))
                {

                    //create the neuron
                    int newNeuronsNumber = PickNeuronNumber();
                    //Debug.Log($"FORCED CONNECTION: Inserting a neuron {newNeuronsNumber} between two neurons {source} and {destination}, without pre-existing connection between them");
                    Neuron newNeuron = new Neuron(newNeuronsNumber);
                    ////Debug.Log($"Made new neuron, number {newNeuronsNumber}");        
                    //add the neuron to the list of neurons
                    neurons.Add(newNeuronsNumber, newNeuron);
                    //set added neurons destination
                    neurons[newNeuronsNumber].AddConnection(destination);
                    //assign a random function to the new neuron  
                    RandomizeNeuronFunction(neurons[newNeuronsNumber]);
                    //set it to receive from the desired source, by making it that sources forward destination
                    neurons[source].AddConnection(newNeuronsNumber);
                    //remake the deep neurons list and the connections list
                    GenerateDeepNeuronList();
                    GenerateConnectionsList();
                }
                else
                {
                    //Debug.Log("Can't add neuron, the source and destination neurons don't exist!");
                }
            }


        }


        //function that takes a neuron, removes it and clears it from forwards of other neurons
        //unless forced set to true it can only remove deep neurons
        //at the end it refreshes the connection list and all neurons lists
        public void RemoveNeuron(int neuronNumber,bool forced = false, bool refreshDeepNeuronsAndConnectionsList = true)
        {
            //Debug.Log($"Trying to remove a neuron {neuronNumber}");
            if (neurons.ContainsKey(neuronNumber))
            {
                //if the neuron is NEITHER input or output AND forced is false, then get TRUE && TRUE, 
                //if it is input
                if(!(neurons[neuronNumber].IsOutput() || neurons[neuronNumber].IsInput()) || !forced)
                {
                    neurons.Remove(neuronNumber);
                    foreach (KeyValuePair<int, Neuron> kvp in neurons)
                    {
                        if (kvp.Value.GetForwards().Contains(neuronNumber))
                        {
                            kvp.Value.RemoveConnection(neuronNumber);
                            //Debug.Log($"Neuron sucessfully removed.");
                        }
                    }
                    //clears input and output lists, deep neuron lists and remakes the connection list
                    if (forced)
                    {
                        if (inputNeuronNumbers.Contains(neuronNumber))
                        {
                            inputNeuronNumbers.Remove(neuronNumber);
                        }
                        if (outputNeuronNumbers.Contains(neuronNumber))
                        {
                            outputNeuronNumbers.Remove(neuronNumber);
                        }
                    }
                    if (refreshDeepNeuronsAndConnectionsList)
                    {
                        GenerateDeepNeuronList();
                        GenerateConnectionsList();
                    }
                    
                }
                else
                {
                    //Debug.Log("Can't remove input output neuron without forced=true, no changes made to the net");
                }
  


            }

            
        }

        //Clean the network from disconnected neurons
        public void CleanDisconnectedNeurons()
        {
            //Debug.Log("Removing disconnected neurons");
            //a list of all paths made by a recursive method <3 
            List<List<int>> allPaths = GetPathEveryInToOut();
            
            List<int> deepNeurons = GenerateDeepNeuronList();
            //temporary list to get all the connected neurons
            List<int> connectedNeurons = new List<int>();
            foreach(List<int> path in allPaths)
            {   
                foreach(int connectedNeuron in path)
                {
                    //add the neuron to the list only if it's not included
                    if (!connectedNeurons.Contains(connectedNeuron))
                    {
                        connectedNeurons.Add(connectedNeuron);
                    }
                }
                //on the last loop it finished adding each connected neuron once to the list
            }
            //check every DEEP neuron in the network, if it's not in the list save its number to a list of neurons that got to be removed
            List<int> neuronsToRemove = new List<int>();
            foreach(int deepNeuron in deepNeurons)
            {
                //if deep neuron wasn't on any path add it to remove list
                if (!connectedNeurons.Contains(deepNeuron))
                {
                    neuronsToRemove.Add(deepNeuron);
                }
            }
            //now cycle through all neurons on remove list and remove them from actual neural net
            foreach(int neuronToRemove in neuronsToRemove)
            {
                RemoveNeuron(neuronToRemove, false, false); //diasbled automatic rebuiding of deepNeuronList and Connection list
            }

            //CAN OPTIMIZE THIS, MAKE ONE LSIT OF ALL NEURONS THAT WERE DELETED AND JUST REMOVE THESE FROM EVERY NEURON'S FORWARD
            //depending of whether there's more neurons to remove or more deep neurons left should either loop through one or the other
            //i'm assuming that by the time the network has any significant size, there will be less neurons removed than left, so cycle through removed

            //if any neuron has forwards that are among the deleted neurons remove them also
            foreach(KeyValuePair<int,Neuron> kvp in neurons)
            {
                List<int> forwardsToRemove = new List<int>();
                //cyle throug all its forwards and save those that need to removed
                foreach(int forward in kvp.Value.GetForwards())
                {
                    if (neuronsToRemove.Contains(forward))
                    {
                        forwardsToRemove.Add(forward);
                    }

                }
                //remove the found non-existing forwards
                foreach(int forwardToRemove in forwardsToRemove)
                {
                    kvp.Value.RemoveConnection(forwardToRemove);
                }
            }
            if (neuronsToRemove.Count() > 0)
            {
                //Debug.Log($"Removed following disconnected neurons: {String.Join(", ", neuronsToRemove)}");
            }
            else
            {
                //Debug.Log("No disconnected neurons had to be removed.");
            }

            //since removed neurons with false, false parameter 
            GenerateDeepNeuronList();
            GenerateConnectionsList();
        }

        //RANDOMIZE FUNCTION - can modify this function to include bool check of a global variable, to decide if in and out neurons also should be mutable this way
        //it's not a mutation, as it doesn't choose which neuron to mutate
        public void RandomizeNeuronFunction(Neuron neuron)
        {
            if (!neuron.IsInput() && !neuron.IsOutput())
            {
                //set activation function to something from range (0, maxNumberOfFunctions), there's plus 1, because the second number in range is non-inclusive
                neuron.SetActivationFunction(rand.Next(0, neuron.GetMaxFunctionNumber() + 1));
                //Debug.Log($"Neuron {neuron.GetGlobalNumber()} activation set to {neuron.GetActivFunctionName()}");
            }
            else
            {
                //Debug.Log("Can't mutate this neuron, it's input or output");
            }
        }

        //GET PATHS RECURSIVE FUNCTIONS AND READING TABLE FUNCTIONS
        //Given the list of neuron numbers (each encapsulated in it's own lsit) it will return
        //every possible path from every one of these neurons to output 
        public List<List<int>> ReturnEveryPathToEndOfChain(List<List<int>> path)
        {
            //create temporary list of lists to hold paths
            List<List<int>> tLOL = new List<List<int>>();
            //for every list of paths in methods argument, take one of the lists and:
            foreach (List<int> small_list in path)
            {
                //create a temporary  int to hold the last neuron number on each list
                int last = small_list[small_list.Count() - 1];
                //if the last neuron has at least one forward
                if (neurons[last].GetForwards().Count() > 0)
                {
                    //take that neuron number, and get it's forwards, for each forward do:
                    foreach (int forward in neurons[last].GetForwards())
                    {   //check that isn't a forward to any of the neruons that provide signal (if it's recurrent path)
                        if (!small_list.Contains(forward))
                        {   //create a temporary path
                            List<int> tP = new List<int>(small_list);
                            //ads forward to the temporary path
                            tP.Add(forward);
                            //ads temporary list to the list of list (list of paths)
                            tLOL.Add(tP);
                        }
                        else //if the forward is to self then skip it
                        {
                            continue;
                        }
                    }
                }
            }
            //if every last neuron in tLOL is either -> output neuron, or a neuron that's already included in it's own lsit, or a neuron without any forwards 
            //stop
            bool keepLooping = false;
            List<List<int>> completePaths = new List<List<int>>();
            foreach (List<int> tPath in tLOL)
            {
                //get the last neuron number in the current subpath
                int lastNeuronNumber = tPath[tPath.Count() - 1];
                //if it doesn't have any forwards, it's either output or deadend
                if (neurons.ContainsKey(lastNeuronNumber) && neurons[lastNeuronNumber].GetForwards().Count() != 0)
                {   //if it has forwards, check all of them
                    foreach (int forward in neurons[lastNeuronNumber].GetForwards())
                    {
                        //if there's still a forward not included in the current subpath
                        if (!tPath.Contains(forward))
                        {
                            //go back to looping
                            keepLooping = true;
                        }
                        else //MUST CHECK IF THERE ARE OTHER FORWARDS
                        {
                            //otherwise take the next neuron
                            //save this subpath as complete!!!
                            //completePaths.Add(tPath);
                        }
                    }
                }
                else
                {
                    completePaths.Add(tPath);
                }
            }
            if (keepLooping)
            {
                return completePaths.Concat(ReturnEveryPathToEndOfChain(tLOL)).ToList();
                //return ReturnEveryPath(tLOL);
            }
            else
            {
                return completePaths;
            }

        }

        //Get path from every input to every output
        public List<List<int>> GetPathEveryInToOut()
        {
            List<List<int>> everyPath = new List<List<int>>();
            foreach (int neuron in GetInputNeurons())
            {
                everyPath.Add(new List<int>() { neuron });
            }
            //get every path from int to the last neuron on the chain
            everyPath = ReturnEveryPathToEndOfChain(everyPath);
            //but make sure to remove dead ends on deep neurons
            List<List<int>> inToOutPaths = new List<List<int>>();
            foreach (List<int> path in everyPath)
            {
                //if the neuron on the last index on the given path is an output, then the path leads from input to output
                if (path.Count() > 0)
                {
                    if (neurons[path[path.Count() - 1]].IsOutput())
                    {
                        inToOutPaths.Add(path);
                    }
                }

            }
            return inToOutPaths;
        }

        //public return every path from every deep neuron to every output
        public List<List<int>> GetPathEveryDeepToOut()
        {
            GenerateDeepNeuronList();
            List<List<int>> everyPath = new List<List<int>>();
            foreach (int neuron in deepNeuronsList)
            {
                everyPath.Add(new List<int>() { neuron });
            }
            //get every path from int to the last neuron on the chain
            everyPath = ReturnEveryPathToEndOfChain(everyPath);
            //but make sure to remove dead ends on deep neurons
            List<List<int>> inToOutPaths = new List<List<int>>();
            foreach (List<int> path in everyPath)
            {
                //if the neuron on the last index on the given path is an output, then the path leads from input to output
                if (path.Count() > 0)
                {
                    if (neurons[path[path.Count() - 1]].IsOutput())
                    {
                        inToOutPaths.Add(path);
                    }
                }
            }
            return inToOutPaths;
        }

        //convert a list of lists of int into a list of strings
        public List<string> PathListToStringList(List<List<int>> paths)
        {
            List<string> pathsPreview = new List<string>();
            foreach (List<int> path in paths)
            {
                pathsPreview.Add(String.Join(", ", path));
            }
            return pathsPreview;
        }

        //Update an order in which the neurons should fire
        public void SetFireOrder(bool debug = false)
        {
            //list to hold neurons and their priority points
            List<(int, int)> neuronPoints = new List<(int, int)>();
            List<int> tempDeepNeurons = new List<int>(deepNeuronsList);

            //take every deep neuron
            foreach(int deepNeuron in tempDeepNeurons)
            {
                int points = 0;
                //loop trough every path deepToOut 
                foreach(List<int> path in GetPathEveryDeepToOut())
                {
                    //check if the given neuron appears on this path
                    if (path.Contains(deepNeuron))
                    {
                        //if it does give it points equal to the length of the path
                        points += path.Count();
                    }
                }
                //add a pair of neuron and it's points
                neuronPoints.Add((deepNeuron, points));
            }
            //sort list by points from highest to lowest
            neuronPoints.Sort((x, y) => y.Item2.CompareTo(x.Item2));
            //reverse the order to match the planned order
            neuronPoints.Reverse();

            //readout the fireOrder into a single dimension list
            fireOrder.Clear();
            foreach((int,int) neuronPoint in neuronPoints)
            {
                fireOrder.Add(neuronPoint.Item1);
            }
            fireOrder = inputNeuronNumbers.Concat(fireOrder).ToList();

            if (debug)
            {
                foreach((int,int) neuronPoint in neuronPoints)
                {
                    //Debug.Log($"Neuron: {neuronPoint.Item1} | Points: {neuronPoint.Item2}");
                }
            }
            //oxo             

        }

        //ACTUAL MUTATIONS - FULLY RANDOM     //ALL MUTATIONS SEEM TO WORK FINE

        //GROW RANDOM CONNECTION between two random unconnected neurons, if they are already connected, the growth fails unless
        //if untilGrown is set to true, then method will keep on trying to find a neuron that can still grow a connection, only after it checked every candidate and faild it would give up
        //after it's completed it refreshes the connection list
        public void Mutate_GrowConnection(bool untilGrown = false)
        {

            //joins both lists of neurons allowed to be used as inputs (input and deep)
            List<int> growFromList = inputNeuronNumbers.Concat(deepNeuronsList).ToList();
            //joins both lists of neurons allowed to be used as outputs (output and deep)
            List<int> growToList = outputNeuronNumbers.Concat(deepNeuronsList).ToList();

            //pick the neuron from which the connection will be drawn, pick an element from each list
            int growFrom = growFromList[rand.Next(0, growFromList.Count())];
            int growTo = growToList[rand.Next(0, growToList.Count())];

            //if we're not trying until success, just one attempt
            if (!untilGrown)
            {
                Debug.Log($"Mutation: Growing a new connection from {growFrom} to {growTo}");
                //grow connection based on the draw
                //connection from to added
                neurons[growFrom].AddConnection(growTo);
                //refresh connection list
                GenerateConnectionsList();
            }
            else if (untilGrown)
            {//if trying untill success or until there are no more options:
                //take the neuron to growFrom, if it has all possible forwards remove this neuron off the list
                //and take next neuron, if list is empty, stop, or if found suitable neuron that's not densely connected, find it a new connections
                while (untilGrown)
                {
                    //if the forward list of this neuron isn't shorther than the list of all other neurons that can recieve
                    //then it already is connected to everything, so it's not useful on our list
                    if (!(neurons[growFrom].GetForwards().Count() < growToList.Count()))
                    {
                        //remove such neuron from the possible inputs list and try again
                        growFromList.Remove(growFrom);
                        //if there are still other candidates in the list
                        if (growFromList.Count() > 0)
                        {
                            //draw a different neuron
                            growFrom = growFromList[rand.Next(0, growFromList.Count())];
                        }
                        else //if there are no more candidates break the loop
                        {
                            break;
                        }
                    }
                    else //found the candidate that can grow a connection
                    {
                        //from possible connection destinations list, remove the nodes this growFrom is already connected to
                        foreach (int forwardsTo in neurons[growFrom].GetForwards())
                        {
                            growToList.Remove(forwardsTo);
                        }
                        //now in the growToList only neurons that don't yet receive signal from growFrom remain and we can pick any of them
                        growTo = growToList[rand.Next(0, growToList.Count())];
                        //this is sure to exist so we can initialize growth
                        neurons[growFrom].AddConnection(growTo);
                        //once done, refresh connection list
                        GenerateConnectionsList();
                        untilGrown = false;
                        Debug.Log($"Mutation Forced: Growing a new connection from {growFrom} to {growTo}");
                    }
                }
                if (untilGrown)
                {
                    Debug.Log("Tried every neuron and couldn't find space for a new connection. Growth failed.");
                    //it means the loop was broken without finding the appropriate connection, it means there's no neurons that can still grow new connections
                    //aka the network dense and before it can be run again a new neuron has to be grown
                }
            }




        }

        //GROW RANDOM NEURON
        //grows a neuron on an existing connection between other two neurons
        //(InsertNeuron method has also an option to grow in paralel if the specified connection doesn't exist, but it's not used here)
        public void Mutate_GrowNeuron()
        {
            //pick a random existing connection from the connection lsit
            //it selects a random index from the list then picks that element in the list and assigns it to the variable
            if (connectionsList.Count() > 0)
            {
                (int, int) connection = GetConnectionsList()[rand.Next(0, GetConnectionsList().Count())];
                Debug.Log($"Mutation: Growing new neuron on a connection {connection}");
                InsertNeuron(connection);
            }
            else
            {
                //Debug.Log("Can't grow neuron, if there are no connections available");
            }

            //this method can always succeed 
        }

        //MUTATE ACTIVATION FUNCTION
        //selects one of the deep neurons and randomizes it's activation function
        public void Mutate_RandomizeActivation()
        {
            //pick one of the existing deep neurons 
            List<int> deepNeurons = GenerateDeepNeuronList();
            if (deepNeurons.Count() > 0)
            {
                int neuronToMutate = deepNeurons[rand.Next(0, deepNeurons.Count())];
                RandomizeNeuronFunction(neurons[neuronToMutate]);
                Debug.Log($"Mutation: Activation function for neuron {neuronToMutate} changed.");
            }
            else
            {
                //Debug.Log("Can't mutate activations, no deep neurons exist");
            }

        }

        //REMOVE A RANDOM CONNECTION
        public void Mutate_LoseConnection()
        {
            
            //get a list of connections
            List<(int,int)> currentConnections = GetConnectionsList();
            //randomize one to be lost
            if (currentConnections.Count() > 0)
            {

                (int, int) selectedConnection = currentConnections[rand.Next(0, currentConnections.Count())];
                //remove it
                if(connectionsList.Contains(selectedConnection))
                {
                    Debug.Log($"Mutation: Loosing connection {selectedConnection}");
                }
                
                neurons[selectedConnection.Item1].RemoveConnection(selectedConnection.Item2);

                //clean disconnected neurons as needed
                CleanDisconnectedNeurons();
            }
            else
            {
                //Debug.Log("No connectiosn to choose from");
            }


        }

        //NETWORK OPERATION
        //runs one cycle of signal through the whole network
        public List<float> RunCycle()
        {
            foreach(int neuron in fireOrder)
            {
                //calcuates the output for each neuron based on fire order
                float fired = neurons[neuron].Fire();
                //takes every neuron that should receive this fire
                foreach(int forward in neurons[neuron].GetForwards())
                {
                    //and ads the calculated value to it's sum of inputs 
                    if (neurons.ContainsKey(forward))
                    {
                        neurons[forward].ReceiveInput(fired);
                    }
                    else
                    {
                        Debug.Log("NO SUCH KEY, ERROR ASIGNMENT");
                    }
                }

            }
            //once the cycle is done through all input neurons and deep, fires output and returns a list
            List<float> outputValues = GetOutputValues();
            return outputValues;
        }
        public List<float> GetAllLastCalculatedOutputs()
        {
            List<float> outputValues = new List<float>();
            foreach(int neuron in inputNeuronNumbers.Concat(outputNeuronNumbers).Concat(deepNeuronsList).ToList())
            {
                outputValues.Add(neurons[neuron].GetLastOutput());
            }
            return outputValues;
        }
        //gets a list of string in format Neuron name, neuron type, last value
        public List<string> GetNeuronsLastDataStringList()
        {
            List<string> neuronsData = new List<string>();
            foreach (int neuron in inputNeuronNumbers.Concat(outputNeuronNumbers).Concat(deepNeuronsList).ToList())
            {
                string neuronData = $"{neurons[neuron].GetNeuronType()}_N{neuron}: IN: {neurons[neuron].GetLastSumOfInputs()} -->-- {neurons[neuron].GetActivFunctionName()} -->-- {neurons[neuron].GetLastOutput()} ";
                neuronsData.Add(neuronData);
            }
            return neuronsData;
        }

        //takes outputs from all output neurons and returns them as a list of loats, used inside "run cycle" to finish the cycle
        public List<float> GetOutputValues()
        {
            //temporary list to hold the values
            List<float> outputValues = new List<float>();
            //take every outputNeuron
            foreach(int outputNeuron in outputNeuronNumbers)
            {
                //then use fire function to return the sumOfInputs, as well as set it to 0 for the next cycle
                outputValues.Add(neurons[outputNeuron].Fire());
            }
            return outputValues;
        }        

        public void SendInputs(List<float> inputsList)
        {

            if (!(inputsList.Count() == inputNeuronNumbers.Count()))
            {
                Debug.Log("!!!!!!!!!!!!Inputs aren't matching the number of Input neurons !!!!!!!!!!!!!!!");
            }
            else
            {
                for(int i = 0; inputNeuronNumbers.Count()>i; i++)
                {
                    neurons[inputNeuronNumbers[i]].ReceiveInput(inputsList[i]);
                }
            }
        }

        public List<float> GenerateRandomInputs()
        {
            List<float> inputValues = new List<float>();
            foreach(int inputNeuron in inputNeuronNumbers)
            {
                inputValues.Add((float)rand.NextDouble());
            }
            return inputValues;
        }

        //export neurons dictionary
        public Dictionary<int, Neuron> ExportNeuronsDNA()
        {
            Dictionary<int, Neuron> newNeurons = new Dictionary<int, Neuron>();
            foreach (KeyValuePair<int, Neuron> kvp in neurons)
            {
                int neuronNumber = kvp.Key;
                int activationFunctionNumber = kvp.Value.GetActivFunctionNumber();
                Neuron newNeuron = new Neuron(neuronNumber, activationFunctionNumber);
                List<int> forwards = new List<int>(kvp.Value.GetForwards());
                newNeuron.SetForwards(forwards);
                if (kvp.Value.IsInput())
                {
                    newNeuron.MakeInput();
                }
                else if (kvp.Value.IsOutput())
                {
                    newNeuron.MakeOutput();
                }
                else
                {
                    newNeuron.MakeDeep();
                }
                newNeurons.Add(neuronNumber, newNeuron);
            }
            return newNeurons;
        }
        //import neurons dictionary
        public void ImportNeuronsDNA(Dictionary<int,Neuron> importedNeurons)
        {
            //create a copy of the input neuron dictionary
            neurons = new Dictionary<int, Neuron>(importedNeurons);

            //clear or data lists
            inputNeuronNumbers.Clear();
            outputNeuronNumbers.Clear();
            deepNeuronsList.Clear();
            connectionsList.Clear();
            fireOrder.Clear();

            int lastNeuronIterator = 0;
            //build inputs outputs list
            foreach (KeyValuePair<int,Neuron> kvp in neurons)
            {
                //check the highest neuron number name to keep proper iterator
                if (kvp.Key > lastNeuronIterator)
                {
                    lastNeuronIterator = kvp.Key;
                }
                //go trhoug each neuron and based on type add it to an appropriate group
                if (kvp.Value.IsInput())
                {
                    inputNeuronNumbers.Add(kvp.Value.GetGlobalNumber());
                    //inputNeuronNumbers.Add(kvp.Key);
                }
                else if (kvp.Value.IsOutput())
                {
                    outputNeuronNumbers.Add(kvp.Value.GetGlobalNumber());
                    //outputNeuronNumbers.Add(kvp.Key);
                }
                else
                {
                    deepNeuronsList.Add(kvp.Value.GetGlobalNumber());
                    //deepNeuronsList.Add(kvp.Key);
                }
            }

            RefreshEnumerator();
            GenerateConnectionsList();
            SetFireOrder();
        }

    }


    public class NN_Instance
    {


        //class used to make brains, it uses neural net of neurons, but it provides frontend for commands, also GetNN(), gives direct access to methods in NN
        public NN_Instance()
        {

        }
        public NN_Instance(NeuralNetwork importNetwork)
        {
            ImportNN(importNetwork);
        }
        //Neural network object
        private NeuralNetwork NN = new NeuralNetwork();
        //random number generator
        static System.Random rand = new System.Random();

        //methods
        //default make of NN
        public void MakeNN(int inputs, int outputs, int maxNumberOfStartingConnections=0)
        {
            NN.MakeInputsOutputs(inputs, outputs);
            NN.InitRandomConnections(maxNumberOfStartingConnections);
            NN.SetFireOrder(true);
        }
        //returns the NeuralNetwork objects
        public NeuralNetwork GetNN()
        {
            return NN;
        }
        public Dictionary<int, Neuron> GetNNDNA()
        {
            return NN.ExportNeuronsDNA();
        }
        //generates saveable string from dna
        public string StringFromNN()
        {
            string stringDNA="";
            Dictionary<int, Neuron> thisNN = new Dictionary<int, Neuron>(NN.ExportNeuronsDNA());
            foreach(KeyValuePair<int, Neuron> kvp in thisNN)
            {
                string forwards = string.Join(",", kvp.Value.GetForwards());
                string thisAllele = $"{kvp.Key}.{kvp.Value.GetNeuronType()}.{kvp.Value.GetActivFunctionNumber()}.[{forwards}]|";
                stringDNA = stringDNA+thisAllele;
                
            }
            
            return stringDNA;

        }

        //takes a string of characters from file or otherwise and loads the data into an nn
        public void ImportStringToNN(string stringNN)
        {
            Debug.Log("Importing string NeuralNet...");
            //split list into separate rows
            List<string> allelesList = stringNN.Split('|').ToList();
            //holder for the neurons
            Dictionary<int, Neuron> neurons = new Dictionary<int, Neuron>();
            Debug.Log($"There are {allelesList.Count} neurons to be imported:");
            //temp data holders
            int neuronNumber;
            string neuronType;
            int neuronActivFunction;
            List<int> neuronForwards = new List<int>();

            allelesList.RemoveAt(allelesList.Count - 1);


            foreach(string allele in allelesList)
            {
                List<string> separatedData = allele.Split('.').ToList();
                neuronNumber = Int32.Parse(separatedData[0]);
                neuronType = separatedData[1];
                neuronActivFunction = Int32.Parse(separatedData[2]);
                string tempForwards = separatedData[3];
                char[] toTrim = { '[', ']' };
                tempForwards = tempForwards.Trim(toTrim);
                if (tempForwards.Length > 0)
                {
                    foreach(string fw in tempForwards.Split(',').ToList())
                    {
                        neuronForwards.Add(Int32.Parse(fw));

                    }
                }

                //create a new neuron based on the data
                Neuron newNeuron = new Neuron(neuronNumber, neuronActivFunction);
                if (neuronType == "Input")
                {
                    newNeuron.MakeInput();
                } 
                else if (neuronType == "Output")
                {
                    newNeuron.MakeOutput();
                }
                else if (neuronType == "Deep")
                {
                    newNeuron.MakeDeep();
                }
                if (neuronForwards.Count > 0)
                {
                    foreach(int fw in neuronForwards)
                    {
                        newNeuron.AddConnection(fw);
                    }
                }
                neuronForwards.Clear();
                //ad the neuron to the dicitonary

                neurons.Add(neuronNumber, newNeuron);

            }
            NN.ImportNeuronsDNA(neurons);
            

        }


        //returns number of connections in the neuron
        public int ConnectionNumberNN()
        {
            return NN.GetConnectionsList().Count();
        }
        //returns number of deep neurons
        public int DeepNeuronsNumberNN()
        {
            return NN.GenerateDeepNeuronList().Count();
        }


        //loads a network from another neural net file- usable with GetNN()
        public void ImportNN(NeuralNetwork network)
        {
            NeuralNetwork newNN = new NeuralNetwork(network.ExportNeuronsDNA());
            NN.ImportNeuronsDNA(newNN.ExportNeuronsDNA());
        }
        //displays data about the network
        public void ReadNN()
        {
            NN.ReadNetwork();
        }
        //passes input values into the network
        public void InputsNN(List<float> inputsValues)
        {
            NN.SendInputs(inputsValues);
        }
        //returns a preview of every value that was calculated in each neuron in a previous pass
        public List<string> PreviewAllValuesNN()
        {
            return NN.GetNeuronsLastDataStringList();
        }
        //runs the network - fires neurons in the appropraite order, then returns a list of output values
        public List<float> FireNN()
        {
            return NN.RunCycle();
        }
        //generates random inputs to use to send to inputs for testing if the network passes values properly
        public List<float> Debug_GenerateRandomInputs()
        {
            return NN.GenerateRandomInputs();
        }



        ////takes a this neural network, mutates it and returns the result
        public NeuralNetwork MutateNN(float mutationChance=1, bool exclusive = false)
        {
            NeuralNetwork NN_child = new NeuralNetwork(GetNN().ExportNeuronsDNA());
            float drawChance = (float)rand.NextDouble();
            //if mutation is triggered
            if (mutationChance >= drawChance)
            {
                //Debug.Log("mutation triggered, trying to mutate...");
                drawChance = (float)rand.NextDouble()*2;
                if (drawChance > 1.5) //hapens 1 in 10 times
                {
                    //grow neuron
                    NN_child.Mutate_GrowNeuron();
                }
                else if(drawChance > 0.8) //hapens 2 in 10 times
                {
                    //grow connection
                    NN_child.Mutate_GrowConnection();
                }
                else if(drawChance > 0.3)//happens 3 in 10 times
                {
                    //mutate activation
                    NN_child.Mutate_RandomizeActivation();
                }
                else //happens 4 in 10 times
                {
                    NN_child.Mutate_LoseConnection();
                }

            }
            //nonexclusive mutations or exclusive?
            //chance to grow connection 0.3
            //chance to grow neuron 0.2
            //chance to mutate funciton 0.5
            //chance to remove a neuron 0.5
            return NN_child;
        }

        //spit out a single random int from range
        public int RandomInt(int maxInclusive)
        {
            return rand.Next(0, maxInclusive + 1);
        }

        //save NN to file
        public void SaveNNFile(string blobName)
        {
            string blobDNA = StringFromNN();
            string path = $"C:\\Users\\Wild\\Unity Projects\\aiB\\Assets\\SavedDNAFiles\\{blobName}_BRAIN.txt";
            //Write some text to the test.txt file
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(blobDNA);
            writer.Close();
            StreamReader reader = new StreamReader(path);
            //Print the text from the file
            Debug.Log(reader.ReadToEnd());
            reader.Close();

        }
    }


    void Start()
    {



    }

    void Update()
    {


    }
}
