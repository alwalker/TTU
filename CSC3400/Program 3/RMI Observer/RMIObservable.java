import java.rmi.*;
import java.rmi.server.*;
import java.util.List;
import java.util.ArrayList;

public abstract class RMIObservable extends UnicastRemoteObject
{
    Boolean changed;
    List<RMIObserver> observers;

    public RMIObservable() throws RemoteException
    {
        super();
        changed = false;
        observers = new ArrayList<RMIObserver>();
    }

    abstract void addObserver(RMIObserver obs) throws RemoteException;
    abstract void clearChanged() throws RemoteException;
    abstract int countObservers() throws RemoteException;
    abstract void deleteObserver(RMIObserver obs) throws RemoteException;
    abstract boolean hasChange() throws RemoteException;
    abstract void notifyObservers() throws RemoteException;
    abstract void notifyObservers(Object obj) throws RemoteException;
    abstract void setChanged() throws RemoteException;
}
