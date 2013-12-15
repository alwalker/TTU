import java.rmi.*;

public interface iToyMachine extends Remote
{
	public boolean addToy(int toyID) throws RemoteException;
	public boolean removeToy(int toyID) throws RemoteException;
	public void addStore(String name) throws RemoteException;
	public int[] getToys() throws RemoteException;
}
