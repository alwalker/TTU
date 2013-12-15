import java.rmi.*;

public interface RMIObserver extends Remote
{
	void update(RMIObservable obs) throws RemoteException;
}
