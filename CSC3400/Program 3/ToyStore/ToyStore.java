import java.rmi.*;
import java.rmi.server.*;
import java.util.List;
import java.util.ArrayList;
import java.util.Scanner;
import java.io.*;

public class ToyStore extends UnicastRemoteObject implements iToyStore, Serializable
{
	int id;
	int[] toys;

	public ToyStore() throws RemoteException
	{
		id = 0;
	}

	public int getID()
	{
		return id;
	}

	public void update(RMIObservable o)
	{
		System.out.println("in update");
		iToyMachine tm = (iToyMachine)o;

		if(o == null)
		{
			System.out.println("error on update");
			return;
		}

		try
		{
			toys = tm.getToys();
		}
		catch(Exception ex)
		{
			System.out.println("error in update: " + ex);
		}

		for(int i=0 ; i<4 ; i++)
		{
			System.out.println((i+1) + ": " + toys[i]);
		}
	}	
}
