import java.rmi.*;
import java.rmi.registry.*;

public class TestClient
{
	public static void main(String[] args)
	{
		try
		{
			String registryURL = "test";
			Registry reg = LocateRegistry.getRegistry("127.0.0.1");
			System.out.println("Looking up");
			TestInterface h = (TestInterface)reg.lookup(registryURL);
			System.out.println("calling remote method");
			System.out.println(h.sayHello("allo mate"));
		}
		catch(Exception e)
		{
			System.out.println("it broke:\n" + e);
		}		
	}
}
