#include <string>
#include <vector>
#include <sstream>
#include <iostream>
#include <stdlib.h>
#include <cstring>
#include <sys/wait.h>
#define NUM_CMD 25

using namespace std;

vector<string> parseCmdLine(const string &line);
//char **splitCommand(const string &command);
void runCommand(const char *command);

int main()
{
	int count;
	int status; //Used for waitpid function
	int pIDs[NUM_CMD];
	int **pipes;
	string userCommand, userInput;
	vector<string> commandList;
	bool lastOne;
	bool firstOne;	

	while(1)
	{
		//Initialize all our variables
		count = 0;
		lastOne = false;
		firstOne = true;
		pipes = (int **)calloc(NUM_CMD, sizeof(int *));
		for(int i=0 ; i<NUM_CMD ; i++)
		{
			pipes[i] = (int *)calloc(2, sizeof(int *));
		}
		for(int i=0 ; i<NUM_CMD ; i++)
		{
			pIDs[i] = -1;
		}

		cout<<endl<<"Enter some commands: ";
		if(getline(cin, userInput))
		{
			commandList = parseCmdLine(userInput);
		}
		else
		{
			printf("\nError reading commands, program exiting.\n");
			return 0;
		}

		if(userInput.compare("exit") == 0 || userInput.compare("Exit") == 0)
		{
			return 0;
		}

		/*printf("\nStarting printout\nlist size: %d\n", commandList.size());
		for(i=0 ; i<commandList.size() ; i++)
		{
			cout<<commandList[i]<<endl;
		}
		printf("\nDone\n");
		return 0;*/

		while(!lastOne)
		{
			//cout<<endl<<"Top of loop";
			userCommand = commandList[count];
			
			if(count == (int)commandList.size() - 1)
			{
				//printf("\nlast one");
				lastOne = true;
			}

			if(!lastOne)
			{
				pipe(pipes[count]);
			}

			pIDs[count] = fork();

			//This is the parent process
			if(pIDs[count] != 0)
			{
				//cout<<endl<<"Parent process, lastone; "<<lastOne<<" firstone: "<<firstOne;
				if(!lastOne)
				{
					close(pipes[count][1]);
				}
				if(!firstOne)
				{
					close(pipes[count - 1][0]);
				}
			}
			else //Child process
			{
				//cout<<endl<<"Child process, lastone; "<<lastOne<<" firstone: "<<firstOne;
				if(firstOne and lastOne)
				{
					//printf("\nExecuting only command");
					runCommand(commandList[count].c_str());
				}
				else if(firstOne)
				{
					dup2(pipes[count][1], fileno(stdout));
					runCommand(commandList[count].c_str());
				}
				else if(lastOne)
				{
					dup2(pipes[count - 1][0], fileno(stdin));
					runCommand(commandList[count].c_str());
				}
				else
				{
					dup2(pipes[count - 1][0], fileno(stdin));
					dup2(pipes[count - 1][1], fileno(stdout));
					runCommand(commandList[count].c_str());
				}
			}
			//cout<<endl<<"End of loop, lastone: "<<lastOne<<" firstone: "<<firstOne;
			//fflush(NULL);
			firstOne = false;
			count++;
		}
		
		for(int i=0 ; i<25 ; i++)
		{
			if(pIDs[i] != -1)
			{
				//printf("\nWaiting on: %d", pIDs[i]);
				//fflush(NULL);
				if(waitpid(pIDs[i], &status, 0) == -1)
				{
					perror("Error waiting");
					exit(1);
				}
			}
		}
	}

	return 0;
}

//Changed this to return one string per command containing the command itself and all its arguments.
//Arguments will be parsed out when the command itself is called.
vector<string> parseCmdLine(const string &line) 
{
 	// Create a string stream that will read from "line"
	stringstream ss(line, stringstream::in);
	// We will put the token in "answer"
	vector<string> answer;
	vector<string> commands;
	int count = 0;

  	// While there are more tokens...
	while (!ss.eof()) 
	{
 		// Put a new string in our vector
		answer.push_back(string(""));
		// Put the next token into the new string
		ss >> answer[count];
		// count it
 		count++;
	}
	// return out vector of tokens

	int i;
	string temp;
	for(i=0 ; i<(int)answer.size() ; i++)
	{
		if(answer[i].compare("|") == 0)
		{
			commands.push_back(temp);
			temp = "";
		}
		else
		{
			temp += answer[i];
			temp += " ";
		}
	}
	commands.push_back(temp);

	return commands;
}

//Splits the command and argument list up then executes the command.
void runCommand(const char *command)
{
	//Build the double char pointer list of arguments needed for execvp	
	int count = 0;
	char *ph; //place holder for strtok function
	char **args = (char **)calloc(NUM_CMD, sizeof(char *));
	for(int i=0 ; i<NUM_CMD ; i++)
	{
		args[i] = (char *)calloc(256, sizeof(char));
	}
	
	ph = strtok((char *)command, " ");
	while(ph != NULL)
	{
		args[count] = ph;
		//printf("\nargs[%d]: *%s*", count, args[count]);
		ph = strtok(NULL, " ");
		count++;
	}
	args[count] = NULL;	
	
	//run the command
	//printf("\nexecuting");
	//fflush(NULL);
	execvp(args[0], args);
	perror("It broke!");
	exit(1);
}

