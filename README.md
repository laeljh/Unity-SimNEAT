# Unity-SimNEAT
A C# neural net model (made for Unity, but compatible with any C# environment) inspired by Googles WANN and NEAT. 
This network connects to any agent, by specifying all the inputs and connects to the outputs. 
It is universal, but I created it with intention to use it my biome simulations with sexual/asexual reproduction (and it works!)
However this is a highly flexible model, not far from state of the art (it was state of the art at the time of making) capable 
of learning any inputs/outputs models and environments. 
THE ONLY MODEL AT THE TIME OF CREATION CAPABLE OF EVOLVING BACKLOOPED NEURON PATHS FOR FORMING MEMORIES AND PROCESSING CHAIN DATA.

You need to plug it to an agent. The neural net will start with a few neurons preconnected and as the simulation runs, each new iteration, based on mechanics of natural selection has chance to add or remove neurons with different functions to try to grow a brain solving for particular environment. 

In my tests I gave this brain to bacteria models capable of movign and some sensory inputs of the environment and in 40k iterations strains capable of searching for food emerged. 
I then put this strain as my base and started the sim again. Few more methods of comotion evolved (pulse motion, regular motion, spiraling etc.) and other small optimizations occured.

The network is optimized in such way that it promotes for the smallest size of networks, it tries to trim quite a lot. That's why it's important to start with relatively large population or have survival conditions relatively high at the start.

If you want to use this in your project, please let me know it's there. I would love to make a mension of your project in a portfolio I'm building. 
I am also happy to help you understand it better (altought I made sure to leave some comments in the file) and possibly (if your project excites me) I can help you make any adjustments and optimalization for better fitting for hte purpose. 

If you think this is SimthingNEAT then hit me up. I'm looking for Junior dev jobs. 
