import java.rmi.*;
import java.rmi.server.*;

public class TestImplementation extends UnicastRemoteObject implements TestInterface
{
	public TestImplementation() throws RemoteException
	{
		super();
	}

	public String sayHello(String msg) throws RemoteException
	{
		System.out.println("Hello, you said: " + msg);
		return "Waddup?!";
	}
}
