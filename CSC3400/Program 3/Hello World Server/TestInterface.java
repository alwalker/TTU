import java.rmi.*;

public interface TestInterface extends Remote
{
	public String sayHello(String msg) throws java.rmi.RemoteException;
}
