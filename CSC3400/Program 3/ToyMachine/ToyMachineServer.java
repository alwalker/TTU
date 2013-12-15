import java.rmi.*;
import java.util.Random;

public class ToyMachineServer
{
	public static void main(String[] args)
	{
		int currentToy;
		Random rnd = new Random(1121);
		ToyMachine tm;

		try
		{
			tm = new ToyMachine();
			String registryURL = "ToyMachine";
			System.out.println("locating registry");
			System.out.println("rebinding");
			Naming.rebind(registryURL, tm);
			System.out.println("Server running");
		}
		catch(Exception ex)
		{
			System.out.println("It broke: " + ex);
			return;
		}

		while(true)
		{
			currentToy = rnd.nextInt(4) + 1;

			try
			{
				tm.addToy(currentToy);
				tm.notifyObservers();
			}
			catch(Exception e)
			{
				System.out.println("error in add: " + e);
			}

			try
			{
				Thread.sleep(2000);
			}
			catch(Exception ex)
			{
				System.out.println("error on sleep: " + ex);
			}
		}
					
	}
}
