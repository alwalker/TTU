import java.rmi.*;
import java.rmi.registry.*;

public class TestServer
{
	public static void main(String[] args)
	{
		try
		{
			TestImplementation test = new TestImplementation();
			//String registryURL = "rmi://127.0.0.1:1099/test";
			String registryURL = "test";
			System.out.println("locating registry");
			//Registry reg = LocateRegistry.getRegistry("127.0.0.1");
			System.out.println("rebinding");
			Naming.rebind(registryURL, test);
			System.out.println("Server running");
		}
		catch(Exception ex)
		{
			System.out.println("It broke: " + ex);
			return;
		}		
	}
}
