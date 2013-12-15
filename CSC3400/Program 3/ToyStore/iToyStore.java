import java.rmi.*;

public interface iToyStore extends RMIObserver
{
	public int getID() throws RemoteException;
}
