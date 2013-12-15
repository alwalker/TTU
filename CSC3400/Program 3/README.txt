   The related source files are in the Toy Store, Toy Machine, and RMI Observer folders.  First
 compile the RMI Observer interface and the RMIObservable abstract class. Then the iToyStore and 
iToyMachine interaces and copy the resulting class files into appropriate folders.  Next compile the
ToyStoreDriver and ToyMachineServer classes (this will compile everything else), then use rmic to 
generate the stubs and copy them to any needed places as well.  Run the server (note this will not 
start up a registry, this must be done by hand), then run the client.

  The update method for RMIObserver does not work.  I was able to test around it and all the other 
funcionality seems to be working (adding and removing toys etc etc).  I was able to call the update 
method passing it a null, but I could not get it to work passing it a ToyMachine (which extends 
the RMIObservable class and is the expected argument).  I kept getting a IllegalArgumentException: 
argument type mismatch exception.  