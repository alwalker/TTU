#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h>
#include <stdio.h>
#include <string.h>
#include <pthread.h>
#include <stdlib.h>
#include <unistd.h>

#define MAX_STR 1024
#define MAX_PEOPLE 25

void handleConnection(int); /*Handles a client connection*/
void *handleClient(void *); /*Handles the clients requests*/
char **parseCommand(char *); /*parses a request from the client*/
char *addPerson(char **); /*adds a person to the address book*/
char *getInfo(char *, char *); /*gets the requested info for a given SSN*/
char *getSSNs(char *); /*get all SSNs for a given last name*/

typedef struct Person
{
  char *firstName;
  char *lastName;
  char *SSN;
  char *street;
  char *houseNo;
  char *city;
  char *state;
  char *zip;
}Person;

pthread_mutex_t mut;
int numPeople;
Person *people;

int main(int argc, char **argv)
{
  numPeople = 0;
  people = (Person *)calloc(MAX_PEOPLE, sizeof(Person));
  int sockfd, newsockfd, clilen, portNumber;
  int on = 1; /*for setsockopt function*/
  /*check for valid arguments*/
  if(argc<2)
  {
    printf("\nPlease specify a port number! Program exiting.");
    return 1;
  }
  else
  {
    portNumber = atoi(argv[1]);
  }
  
  struct sockaddr_in clientAddress, serverAddress;
  
  sockfd = socket(AF_INET, SOCK_STREAM, 0); /*create socket for listening*/
  if(setsockopt(sockfd, SOL_SOCKET, SO_REUSEADDR, &on, sizeof(on)) < 0) /*lets us reuse socket if still open*/
  {
    perror("Error setting socket: ");
    return 1;
  }
  
  if(sockfd == 0)
  {
    perror("Error creating socket: ");
    return 1;
  }
  
  memset((void *)&serverAddress, 0, sizeof(serverAddress)); /*zero out server address struct*/
  
  serverAddress.sin_family = AF_INET;
  serverAddress.sin_addr.s_addr = htonl(INADDR_ANY);
  serverAddress.sin_port = htons(portNumber);
  
  /*bind socket for listening*/
  if(bind(sockfd, (struct sockaddr *)&serverAddress, sizeof(serverAddress)) < 0)
  {
    perror("Error binding listening socket: ");
    return 1;
  }
  
  listen(sockfd, 5); /*listen for connections to the server*/
  while(1)
  {
    clilen = sizeof(clientAddress);
    newsockfd = accept(sockfd, (struct sockaddr *)&clientAddress, (socklen_t *)&clilen); /*get socket descriptor for client*/
    if(newsockfd < 0)
    {
      perror("Error getting connection from client: ");
      return 1;
    }
    
    handleConnection(newsockfd);
  }
}

/*Handles the client connection.  Creates a thread and has it listen for client's requests.*/
/*Input: the socket descriptor*/
/*Output: None, this function will loop until the client closes*/
void handleConnection(int sockfd)
{
  pthread_attr_t attributes;
  pthread_t clientThread;
  pthread_attr_init(&attributes);
  pthread_attr_setdetachstate(&attributes, PTHREAD_CREATE_DETACHED);
  
  pthread_create(&clientThread, &attributes, handleClient, (void *)&sockfd);
}

/*Handles the clients requests.*/
/*Input: The socket descriptor*/
/*Output: none*/
void *handleClient(void *args)
{
  void *ret = (void *)calloc(1, sizeof(void));
  int i;
  int size = 0;
  int sockfd = *(int *)(args);
  char *response;
  char *message = (char *)calloc(MAX_STR + 1, sizeof(char));
  char **commands;
  
  /*Loop until client is done.*/
  while(1)
  {
    memset(message, '\0', MAX_STR + 1);
    size = recv(sockfd, message, MAX_STR + 1, 0); /*Add one so we can append '\0'*/
    if(size < 0)
    {
      perror("Error reading socket");
      free(message);
      return ret;
    }
    
    printf("\nGot: %s", message);
    fflush(NULL);
    commands = parseCommand(message);
    
    if(commands == NULL)
    {
      /*tell user to try again*/
    }
    else if(strcmp(commands[0], "EXIT") == 0)
    {
      printf("\nClient: %d exiting", sockfd);
      free(message);
      close(sockfd);
      return ret;
    }
    else if(strcmp(commands[0], "ADD_PERSON") == 0)
    {
      perror("chose add person");
      response = addPerson(commands);
    }
    else if(strcmp(commands[0], "GET_SSNS") == 0)
    {
      response = getSSNs(commands[1]);
    }
    else
    {
      perror("chose else");
      response = getInfo(commands[0], commands[1]);
    }      
    
    /*free the command list*/
    for(i=0 ; i<MAX_STR ; i++)
    {
      free(commands[i]);
    }
    free(commands);
    
    printf("sending message: %s", response);
    fflush(NULL);
    send(sockfd, response, strlen(response), 0);
    perror("sent message");
    free(response);
  }
}

/*Parses a command from the client.*/
/*Input: the raw message from the client*/
/*Output: A char** where the first string is the command and the succeeding onese are the argumets.*/
char **parseCommand(char *message)
{
  int i, j = 0;
  int count = 0;
  char *temp = (char *)calloc(MAX_STR, sizeof(char));
  char **command = (char **)calloc(MAX_STR, sizeof(char *));
  for(i=0 ; i<MAX_STR ; i++)
  {
    command[i] = (char *)calloc(MAX_STR, sizeof(char));
  }
  
  for(i=0 ; i<strlen(message) ; i++)
  {
    if(message[i] == ':')
    {
      temp[j] = '\0';
      strcpy(command[count], temp);
      memset((void *)temp, '\0', sizeof(temp));
      count++;
      j = 0;
    }
    else
    {
      temp[j] = message[i];
      j++;
    }
  }
  
  /*Get last argument*/
  temp[j] = '\0';
  strcpy(command[count], temp);
  free(temp);
  
  for(i=0 ; i<= count ; i++)
  {
    printf("\nCommand: %s", command[i]);
    fflush(NULL);
  }
  
  return command;
}

/*Adds a person to the address book*/
/*Input: All the information for the user.*/
/*Output: The response to be sent back to the client*/
char *addPerson(char **per)
{
  char *response = (char *)calloc(MAX_STR, sizeof(char));
  
  if(numPeople >= MAX_PEOPLE)
  {
    strcpy(response, "0:FULL");
    return response;
  }
  
  int i, j = 0;
  int placed = 0;
  
  perror("starting addperson");
  
  /*Make new person*/
  Person newPerson;
  newPerson.firstName = (char *)calloc(MAX_STR, sizeof(char));
  newPerson.lastName = (char *)calloc(MAX_STR, sizeof(char));
  newPerson.SSN = (char *)calloc(MAX_STR, sizeof(char));
  newPerson.street = (char *)calloc(MAX_STR, sizeof(char));
  newPerson.houseNo = (char *)calloc(MAX_STR, sizeof(char));
  newPerson.city = (char *)calloc(MAX_STR, sizeof(char));
  newPerson.state = (char *)calloc(MAX_STR, sizeof(char));
  newPerson.zip = (char *)calloc(MAX_STR, sizeof(char));
  
  perror("starting copies");
  
  strcpy(newPerson.firstName, per[1]);
  strcpy(newPerson.lastName, per[2]);
  strcpy(newPerson.SSN, per[3]);
  strcpy(newPerson.street, per[4]);
  strcpy(newPerson.houseNo, per[5]);
  strcpy(newPerson.city, per[6]);
  strcpy(newPerson.state, per[7]);
  strcpy(newPerson.zip, per[8]);
  
  perror("finished copies");
//   printf("\nnewPerson after copy");
//   printf("\nfirst name: %s", newPerson.firstName);
//   printf("\nlast name: %s", newPerson.lastName);
//   printf("\nSSN: %s", newPerson.SSN);
//   printf("\nstreet: %s", newPerson.street);
//   printf("\nhouse number: %s", newPerson.houseNo);
//   printf("\ncity: %s", newPerson.city);
//   printf("\nstate: %s", newPerson.state);
//   printf("\nzip: %s", newPerson.zip);
  
  /*Add person to list*/
  pthread_mutex_lock(&mut);
  if(numPeople > 0)
  {
    perror("more than no people");
    Person *newList = (Person *)calloc(MAX_PEOPLE, sizeof(Person));
    for(i=0 ; i<numPeople ; i++)
    {
      if(strcmp(people[i].SSN, newPerson.SSN) == 0)
      {
	//can't have duplicates
	strcpy(response, "0:DUPLICATE");
	return response;
      }
      else if(!placed && strcmp(people[i].SSN, newPerson.SSN) > 0)
      {
	newList[j] = newPerson;
	j++;
	newList[j] = people[i];
	placed = 1;
      }
      else
      {
	newList[j] = people[i];
      }
      j++;
    }
    
    if(!placed)
    {
      newList[i] = newPerson;
    }
    
    perror("mem stuf");
    numPeople++;
    free(people);
    people = newList;
  }
  else
  {
    perror("no people");
    people[0] = newPerson;
    numPeople++;
  }
  
  /*Send response*/
  response[0] = '1';
  response[1] = '\0';
  
  /*print people list*/
  for(i=0 ; i<numPeople ; i++)
  {
    printf("\nperson #%d:", i);
    printf("\nfirst name: %s", people[i].firstName);
    printf("\nlast name: %s", people[i].lastName);
    printf("\nSSN: %s", people[i].SSN);
    printf("\nstreet: %s", people[i].street);
    printf("\nhouse number: %s", people[i].houseNo);
    printf("\ncity: %s", people[i].city);
    printf("\nstate: %s", people[i].state);
    printf("\nzip: %s", people[i].zip);
    printf("\n");
  }
  
  pthread_mutex_unlock(&mut);
  return response;
}

/*Gets the specified info for the given ssn*/
/*Input: A char * representing what information to get (name, street, house no, etc etc)*/
/*	 The SSN of the person to look up.*/
/*Output: The response to be sent back to the client*/
char *getInfo(char *info, char *ssn)
{ 
  int min = 0;
  int max = numPeople;
  int mid;
  int found = 0;
  char *response = (char *)calloc(MAX_STR, sizeof(char));
  response[0] = '1';
  response[1] = ':';
  response[2] = '\0';
  
  /*Find the SSN first. Binary search is O(logn)*/
  pthread_mutex_lock(&mut);
  while(min <= max)
  {
    mid = (min + max)/2;
    if(strcmp(people[mid].SSN, ssn) == 0)
    {
      found = 1;
      break;
    }
    else if(strcmp(ssn, people[mid].SSN) < 0)
    {
      max = mid -1;
    }
    else
    {
      min = mid + 1;
    }
  }
  pthread_mutex_unlock(&mut);
  
  if(!found)
  {
    strcpy(response, "0:NOTFOUND");
    return response;
  }
  
  /*return requested info*/
  pthread_mutex_lock(&mut);
  if(strcmp(info, "GET_NAME") == 0)
  {
    strcat(response, people[mid].firstName);
    strcat(response, ",");
    strcat(response, people[mid].lastName);
  }
  else if(strcmp(info, "GET_STREET") == 0)
  {
    strcat(response, people[mid].street);
  }
  else if(strcmp(info, "GET_HOUSENO") == 0)
  {
    strcat(response, people[mid].houseNo);
  }
  else if(strcmp(info, "GET_CITY") == 0)
  {
    strcat(response, people[mid].city);
  }
  else if(strcmp(info, "GET_STATE") == 0)
  {
    strcat(response, people[mid].state);
  }
  else if(strcmp(info, "GET_ZIP") == 0)
  {
    strcat(response, people[mid].zip);
  }
  pthread_mutex_unlock(&mut);
  
  return response;
}

/*Gets all Social Security numbers for a given last name.*/
/*Input: A char * containing the last name*/
/*Output: The response to be sent back to the client*/
char *getSSNs(char *lastName)
{
  char *SSNs = (char *)calloc(MAX_STR, sizeof(char));
  if(numPeople == 0)
  {
    strcpy(SSNs, "0:EMPTY");
    return SSNs;
  }
  
  int i = 0;  
  SSNs[0] = '1';
  SSNs[1] = ':';
  SSNs[2] = '\0';
  
  pthread_mutex_lock(&mut);
  for(i=0 ; i<numPeople ; i++)
  {
    if(strcmp(lastName, people[i].lastName) == 0)
    {
      strcat(SSNs, people[i].SSN);
      strcat(SSNs, ":");
    }
  }
  pthread_mutex_unlock(&mut);
  
  return SSNs;
}