import java.util.Scanner;
import java.rmi.*;
import java.rmi.server.*;

public class ToyStoreDriver
{
	public static int printMenu()
	{
		Scanner sc = new Scanner(System.in);
		int choice;

		System.out.println("\n\n\n\n\n\n");
		System.out.println("Menu");
		System.out.println("1. Buy Toy");
		System.out.println("2. See Inventory");
		System.out.println("3. Exit");
		System.out.print("Choose an option: ");

		choice = sc.nextInt();
		System.out.println("\nYou chose: " + choice);
		return choice;
	}

	/*public static void printInventory()
	{
		for(int i=0 ; i<4 ; i++)
		{
			System.out.println((String)(i+1) + ": " + toys[i]);
		}
	}*/

	public static void buyToy(iToyMachine tm)
	{
		int toyID;
		Scanner sc = new Scanner(System.in);

		System.out.print("Enter toy id: ");
		toyID = sc.nextInt();
		System.out.println();

		try
		{
			tm.removeToy(toyID);
		}
		catch(Exception ex)
		{
			System.out.println("error remove: " + ex);
		}
	}

	public static void main(String[] args)
	{
		
		int choice;
		ToyStore ts = null;
		try
		{
			ts = new ToyStore();
		}
		catch(Exception ex)
		{
			System.out.println("error on creating ts");
		}

		iToyMachine tm2 = null;

		try
		{
			System.out.println("Serving toy store");
			Naming.rebind("ToyStore1", ts);
			String registryURL = "ToyMachine";
			System.out.println("Looking up");
			tm2 = (iToyMachine)Naming.lookup(registryURL);
			System.out.println("adding observer");
			tm2.addStore("ToyStore1");
		}
		catch(Exception e)
		{
			System.out.println("it broke:\n" + e);
		}

		while(true)
		{
			choice = printMenu();

			if(choice == 3)
			{
				System.exit(0);
			}
			else if(choice == 2)
			{
				//printInventory();
			}
			else
			{
				buyToy(tm2);
			}
		}
					
	}
}
