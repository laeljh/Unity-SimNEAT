using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Test1 : MonoBehaviour
{
    public class Neuron
    {
        int neuronNumber = new int();
        int neuronFunction = new int(); //function number
        List<int> forwardsTo = new List<int>();
        float calculatedOutput = 0.0f;
        float receivedInputs = 0.0f;

        public void SetNumber(int number)
        {
            neuronNumber = number;
        }

        public int GetNumber()
        {
            return neuronNumber;
        }

        public void SetFunction(int functionCode)
        {
            neuronFunction = functionCode;
        }

        public int GetFunctionCode()
        {
            return neuronFunction;
        }

        public void GetInput(float value)
        {
            receivedInputs = receivedInputs + value;
        }

        public float Fire()
        {
            //NEED TO CHANGE FIRE FUNCTION TO INCORPORATE THE ACTIVATION
            calculatedOutput = receivedInputs;
            receivedInputs = 0;
            return calculatedOutput;
        }

        
    }

    //defines NN class 
    public class NeuralNet
    {
        private List<int> NeuronNumbers = new List<int>();
        private List<List<int>> Paths = new List<List<int>>();
        private Dictionary<int, Neuron> actualNeurons = new Dictionary<int, Neuron>();
        //BELOW NEEDS TO BE RETOUGHT
        //neural net operates on list of neuron numbers and list of connection numbers, 
        //but the execution is done by using a dictionary of actual neuron objects




        //setter for neurons list
        public void SetNeurons(List<int> NeuronsList)
        {
            Debug.Log("Loading neurons list from input");
            foreach(int n in NeuronsList)
            {
                actualNeurons.Add(n, new Neuron());
            }
            Debug.Log("Created neurons and saved their numbers in a list");
            NeuronNumbers = NeuronsList;
        }
        //setter for neurons paths
        public void SetPaths(List<List<int>> PathsListOfListOfInt)
        {
            Debug.Log("Loading paths list from input");
            Paths = PathsListOfListOfInt;
        }

        public List<int> GetNeuronNumbers()
        {
            return NeuronNumbers;
        }

        public List<List<int>> GetPaths()
        {
            return Paths;
        }

        public void ReadNNet()
        {
            string Separator = "========================";
            Debug.Log("Reading the net...");
            Debug.Log(Separator);
            if (NeuronNumbers.Count > 0)
            {
                foreach (int neuron in NeuronNumbers)
                {

                    Debug.Log($"Checking neuron {neuron}");
                    foreach (List<int> path in Paths)
                    {
                        if (path[0] == neuron)
                        {
                            Debug.Log($">>>>>>>> It connects to : {path[1]}");
                        }
                    }
                    Debug.Log(Separator);
                }
            }
            else
            {
                Debug.Log("NN IS EMPTY...");
                Debug.Log(Separator);
            }
            
        }
        private bool CheckIfPathsHaveConnection(List<int> connection)
        {
            bool hasElement = false;
            foreach(List<int> path in Paths)
            {
                if(path[0] == connection[0] && path[1] == connection[1])
                {
                    hasElement = true;
                    break;
                }
                else
                {
                    hasElement = false;
                }
            }
            return hasElement;
        }
        public void AddConnection(int sourceNeuron, int destinationNeuron)
        {
            List<int> connection = new List<int> { sourceNeuron,destinationNeuron};
            //if the neuron list even has these neurons to be connected
            if (NeuronNumbers.Contains(connection[0]) && NeuronNumbers.Contains(connection[1]))
            {
                if (!CheckIfPathsHaveConnection(connection)) //and the connection doesn't yet exist
                {
                    Paths.Add(connection);
                    Debug.Log($"Added a connection: {connection[0]} to {connection[1]}");
                }
                else
                {
                    Debug.Log($"Connection {connection[0]} to {connection[1]} already existed...");
                }
            }
            else
            {
                Debug.Log("Trying to connect nonexisint neurons... connection failed.");
            }

        }

        public void RemoveConnection(int sourceNeuron, int destinationNeuron)
        {
            List<int> connection = new List<int>{ sourceNeuron, destinationNeuron };
            //if connection[0] in paths has more than 1 connection and connection[1] in paths has more than one connection remove
            //check if it exists
            //check the forward connectiosn from 0
            //check the incoming connections from 1
            //if they all have more than one connection, can remove this one without making the neuron useless
            if (CheckIfPathsHaveConnection(connection))
            {
                Debug.Log($"Connection {connection[0]} to {connection[1]} ready for removal found...");
                //set counters to check if there are more than 1 connections for each of the neuron (so no last connection could be removed for any neuron)
                bool notOnlyInput = false;
                bool notOnlyOutput = false;
                bool safeToRemove = false;  
                //for every connected pair in paths do this
                foreach (List<int> connected in Paths)
                {
                    if (connected != connection) //checking only other connections than the one meant for removal, if there are other connections for each of these neurons
                    {
                        if (connected[0] == connection[0]) //it found another connection where connection[0] sends it's data somwhere 
                        {
                            notOnlyInput = true;
                        }
                        if (connected[1] == connection[1]) //it found another connection where connection[1] receives it's data from somwhere 
                        {
                            notOnlyOutput = true;
                        }
                        if (notOnlyInput && notOnlyOutput) //if it's verified that these neurons have other connections, can safely remove the connection and break the loop
                        {
                            safeToRemove = true;
                            break;
                        }
                    }
                }
                if (safeToRemove)
                {
                    Paths.RemoveAll(path => path[0] == connection[0] && path[1] == connection[1]);
                    //Paths.Remove(connection); //removes the connection
                    Debug.Log($"Connection {connection[0]} to {connection[1]} was safely removed...");
                }
                else
                {
                    Debug.Log($"Connection {connection[0]} to {connection[1]} is the only connection between {connection[0]} and {connection[1]}, therefore it cannot be removed");
                }
            }
            else
            {
                Debug.Log($"Connection {connection[0]} to {connection[1]} not found, can't remove!");
            }

        }

        public void AddNeuron(int neuronNumber, int betweenSource, int betweenDestination, bool ForcedAdd = false)
        {
            
            Debug.Log("Neuron insertion initiated...");
            if (!NeuronNumbers.Contains(neuronNumber)) //if the neuron doesn't exist yet
            {
                List<int> addOnPath = new List<int> { betweenSource, betweenDestination };
                Debug.Log($"Adding neuron {neuronNumber}, it doesn't exist yet...");
                //add neuron on existing path, unless forced add is enabled, then if the connection doesn't exist, add it anyway as long as there are appropriate neurons to connect to
                bool ForceIt = ForcedAdd && NeuronNumbers.Contains(addOnPath[0]) && NeuronNumbers.Contains(addOnPath[1]);
                if (CheckIfPathsHaveConnection(addOnPath) || ForceIt)
                {
                    Debug.Log($"Inserting it on a path between {addOnPath[0]} and {addOnPath[1]}");
                    NeuronNumbers.Add(neuronNumber);
                    actualNeurons.Add(neuronNumber, new Neuron()); //adding an actual number except the adition to the list
                    Debug.Log("Neuron added.");
                    Debug.Log($"Creating connections: {addOnPath[0]} to {neuronNumber}, and {neuronNumber} to {addOnPath[1]}");
                    AddConnection(addOnPath[0], neuronNumber);
                    AddConnection(neuronNumber, addOnPath[1]);
                    Debug.Log("Removing old connection...");
                    RemoveConnection(addOnPath[0],addOnPath[1]);
                    Debug.Log("Adding a neuron complete.");
                }
            }
            else
            {
                Debug.Log($"Can't add, neuron {neuronNumber} already existed.");
            }


        }

       // public void RemoveNeuron(int neuronNumber, )

        //structure - 
        /*
         there are thre datasets
        1) list of neuron numbers
        2) list of connections between neuron numbers
        3) dictionary of neuron objects, where key is the number and value is the actual neuron

         there are notable situations in the structure changes:
        1) adding or removing a connection affects only the connection list
        2) adding or removing a neuron is checked by the neuron number list (faster) but onec the decisions are made, the same changes are done to the dictionary

         any data analysis operation on neurons can be done using mainly the reference in 

         */
    }



    //initialization
    NeuralNet TestNet = new NeuralNet();
    //preparing variables of an example neural net
    public List<int> Neurons = new List<int>{0, 1, 2, 3, 4, 5};
    public List<List<int>> Paths = new List<List<int>>
    {
        new List<int>{0,1 },
        new List<int>{1,2 },
        new List<int>{1,3 },
        new List<int>{2,3 },
        new List<int>{2,4 },
        new List<int>{3,4 },
        new List<int>{4,5 },
    };




    // Start is called before the first frame update
    void Start()
    {
        //TestNet.ReadNNet();

        TestNet.SetNeurons(Neurons);
        TestNet.SetPaths(Paths);

        //TestNet.ReadNNet();
        int currentSize = TestNet.GetNeuronNumbers().Count - 1;
        for (int i = 0; i< currentSize; i++)
        {
            TestNet.AddConnection(i, i);
            TestNet.AddNeuron(TestNet.GetNeuronNumbers().Count, i, i);
        }

        TestNet.ReadNNet();
        

        /*
        TestNet.ReadNNet();
        TestNet.AddConnection(1, 1);
        TestNet.ReadNNet();
        TestNet.RemoveConnection(1, 1);
        TestNet.ReadNNet();
        for (int i = 0; i < TestNet.GetNeurons().Count - 1; i++)
        {
            TestNet.RemoveConnection(i, i);
        }
        TestNet.ReadNNet(); */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
