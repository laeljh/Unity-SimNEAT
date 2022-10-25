using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FunctionComplexityEvaluator : MonoBehaviour
{
    /*
     * THIS SCRITP ATTACHED TO A GAME OBJECT WILL RUN EVERY FUNCTION IN UPDATE
     * AND CREATE A LSIT OF OUTPUTS INLCUDING THE NAME OF THE FUNCTION AND THE TIME IT TOOK FOR ALL ITERATIONS
     * THIS IS AN EVALUATION SCRIPT AND NOT MEANT TO BE USED DIRECTLY BY THE NETWORK IN ANY WAY, ONLY FOR DEV AND ANALYSIS 
     */
    public int testingFunction = 0;
    public int numberOfIterations = 10;
    private int maxFunctions = 13;
    private bool run = true;
    public int iterated = 0;
    public float timed;
    public List<string> results = new List<string>();
    public List<string> resultsPerSec = new List<string>();
    //takes an input and a number of function to perform the calculation
    public float RunFunction(int number, float input)
    {

        float x = input;
        float output = 0;

        switch (number) //gets the function number and runs the sum of input trhoug to get the output
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
                output = Mathf.Sqrt(x);
                break;
            case 10:
                output = Mathf.Pow(x, (1 / 3));
                break;
            case 11:
                output = -x;
                break;
            case 12:
                output = 1 / x;
                break;
            default:
                output = x;
                Debug.Log("Given non existing function index for the neuron, falling back to using pass function");
                break;




        }
        return output;
    }
    //returns name of the function
    public string GetFunctionName(int number)
    {
        string activationFunctionName = "";
        switch (number) //gets the function number and runs the sum of input trhoug to get the output
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
                Debug.Log("Given non existing function index for the neuron, falling back to using pass function");
                break;




        }
        return activationFunctionName;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   

    //the evaluation needs to happen in update
    //need to evalute function here
    void Update()
    {
        if (run)
        {
            if (iterated == 0 && testingFunction < maxFunctions)
            {
                Debug.Log($"{testingFunction}. Initializing evaluation of {numberOfIterations} iterations for {GetFunctionName(testingFunction)}...");

            }


            if (iterated < numberOfIterations)
            {
                RunFunction(testingFunction, (iterated/100));
                timed += Time.deltaTime;
                iterated++;
            }
            else
            {

                if (testingFunction < maxFunctions)
                {
                    Debug.Log($"Test complete, took total of {timed}s");
                    string result = GetFunctionName(testingFunction) + ": " + timed.ToString() + "s"; 
                    results.Add(result);
                    result = GetFunctionName(testingFunction) + ": " + ((iterated / timed)/1000).ToString() + "it/ms";
                    resultsPerSec.Add(result);
                    testingFunction++;
                    if (testingFunction < maxFunctions)
                    {
                        iterated = 0;
                        timed = 0;
                    }
                    else run = false;


                }
            }
        }


        
    }
}
