import java.rmi.*;

public class TestRMIObservable extends RMIObservable
{
	public TestRMIObservable() throws RemoteException
	{
		super();
	}

	public void addObserver(RMIObserver obs) throws RemoteException
	{
		System.out.println("adding");
		observers.add(obs);
	}

	public void deleteObserver(RMIObserver obs) throws RemoteException
	{
		System.out.println("removing");
		observers.remove(obs);
	}

	public void clearChanged() throws RemoteException
	{
		changed = false;
	}

	public int countObservers() throws RemoteException
	{
		System.out.println("counting");
		return observers.size();
	}

	public boolean hasChange() throws RemoteException
	{
		System.out.println("changed?");
		return changed;
	}

	public void notifyObservers() throws RemoteException
	{
		System.out.println("notifying");
	}

	public void notifyObservers(Object obj) throws RemoteException
	{
	}

	public void setChanged() throws RemoteException
	{
		System.out.println("setting changed");
		changed = true;
	}
	
}
