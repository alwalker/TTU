import java.rmi.*;
import java.rmi.server.*;

public class ToyMachine extends RMIObservable implements iToyMachine
{
	private int[] toys;

	public ToyMachine() throws RemoteException
	{
		super();
		toys = new int[4];
		
		for(int i=0 ; i<4 ; i++)
		{
			toys[i] = 0;
		}
	}

	public void addObserver(RMIObserver obs) throws RemoteException
	{
		System.out.println("in add observer");
		observers.add(obs);
	}

	public void clearChanged() throws RemoteException
	{
		changed = false;
	}
	
	public int countObservers() throws RemoteException
	{
		return observers.size();
	}

	public void deleteObserver(RMIObserver obs) throws RemoteException
	{
		iToyStore ts1 = (iToyStore)obs;
		iToyStore ts2;

		for(int i=0 ; i<observers.size() ; i++)
		{
			ts2 = (iToyStore)observers.get(i);

			if(ts1.getID() == ts2.getID())
			{
				observers.remove(observers.get(i));
			}
		}
	}

	public boolean hasChange() throws RemoteException
	{
		return changed;
	}

	public void notifyObservers() throws RemoteException
	{
		RMIObserver obs;
		System.out.println("in notify\nwe have " + observers.size() + " observers");
		for(int i=0 ; i<observers.size() ; i++)
		{
			obs = observers.get(i);
			obs.update(this);
		}		
	}

	public void notifyObservers(Object obj) throws RemoteException
	{
		RMIObserver obs;
		System.out.println("in notify\nwe have " + observers.size() + " observers");
		for(int i=0 ; i<observers.size() ; i++)
		{
			obs = observers.get(i);
			obs.update((RMIObservable)obj);
		}
		
	}

	public void setChanged() throws RemoteException
	{
		changed = true;
	}

	public synchronized int[] getToys() throws RemoteException
	{
		return toys;
	}

	public synchronized boolean addToy(int toyID) throws RemoteException
	{
		if(toyID>0 && toyID<=4)
		{
			toys[toyID-1]++;
			return true;
		}
		else
		{
			System.out.println("invalid toyID in add");
			return false;
		}
	}

	public synchronized boolean removeToy(int toyID) throws RemoteException
	{
		System.out.println("in remove");
		for(int i=0 ; i<4 ; i++)
		{
			System.out.println(i+1 + ": " + toys[i]);
		}

		if(toyID>0 && toyID<=4)
		{
			if(toys[toyID]<= 0)
			{
				System.out.println("out of toy: " + toyID);
				return false;
			}
			else
			{
				System.out.println("removed a: " + toyID);
				toys[toyID]--;
				return true;
			}
		}
		else
		{

			System.out.println("invalid toyID in remove");
			return false;
		}
	}
	
	public void addStore(String name) throws RemoteException
	{
		System.out.println("in add store");
		iToyStore ts = null;


		try
		{
			System.out.println("Looking up toy store: " + name);
			ts = (iToyStore)Naming.lookup(name);
			System.out.println("adding observer");
		}
		catch(Exception e)
		{
			System.out.println("it broke:\n" + e);
		}
		
		try
		{
			addObserver(ts);
		}
		catch(Exception ex)
		{
			System.out.println("error in addstore: " +ex);
		}
	}

}
