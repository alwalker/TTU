/***********************************************************/
/*Andrew Walker           CSC4240                Program #2*/
/*Date Created: 10/16/07                                   */
/*Dates Modified: 10/17/07, 10/22/07                       */
/*File Name: Wumplus_Map_Generator.c                       */
/*Title: Wumplus World Map Generator                       */
/*Purpose: Creates a random text file that the Wumplus     */
/*         World program can use to generate a "World".    */
/*Requirements: Permission to write and create files in the*/
/*              directory this program will be ran.        */
/*Notes: 1. Will generate a file with extension .wump.m    */
/*       2. Takes two command line arguments               */
/*          a) An integer to denote how big to make the map*/
/*          b) A string to name the file                   */
/***********************************************************/

#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <ctype.h>
#include <time.h>
#include <math.h>

int main(int argc, char *argv[])
{
   int size, i, j;
   char *fileName;
   
   /*Check command line arguments for validity*/
   if (argc != 3)
   {
      printf("Incorrect number of command line arguments.\n");
      exit(0);
   }
   /*Test to see if first argument is a number*/
   for (i=0 ; i<strlen(argv[1]) ; i++)
   {
      if (isdigit(argv[1][i]) == 0)
      {
         printf("First argument isn't a number!\n");
         exit(0);
      }
   }
   size = atoi(argv[1]);
   fileName = strcat(argv[2], ".wump.m");
   
   /*Now create the 2d array that will represent the map.*/
   /*The map will be represented as a 2d array of characters:
   W: Wumpus, there will be n/10 Wumpi (round up)
   G: Gold, 5% chance a square contains gold
   S: Supmuw, there will be 2n/10 Supmuwes (round up)
   P: Pit, 20% chance a square will be a pit (from text book)
   L: Wall, 10% a square will be a wall
   E: Entrance, Will always be at location (n,1) (from text book)
   Y: Supmuw and Pit
   U: Gold and Wumpus
   Z: Supmuw and Wumpuss
   X: Normal square*/
   int square, temp;
   int wumpCount = (int)ceil(size/10);
   int supCount = (int)ceil((2*size)/10);
   srand(time(0));
   char **map = (char **)malloc(size*sizeof(char *));
   FILE *f;
   f = fopen(fileName, "w");
   
   for (i=0 ; i<size ; i++)
      map[i] = (char *)malloc(size*sizeof(char));
   
   /*loop through each square and generate a random number between one and one hundred to*/
   /*determine what will be in the square.*/
   for(i=0 ; i<size ; i++)
   {
      for(j=0 ; j<size ; j++)
      {
         square = rand()%101;
         if(j==0 && i==size-1) /*Skip entrace*/
         {
            printf("\nEntrance");
            map[i][j] = 'E';
         }                     
         else if(square <=5 && square >2) /*Gold*/
         {
            printf("\nSquare [%d,%d] contains gold", i, j);
            map[i][j] = 'G';
         }
         else if(square<=10 && square>5) /*Wall*/
         {
            printf("\nSquare [%d,%d] is a wall", i, j);
            map[i][j] = 'L';
         }
         else if(square<=20 && square>10) /*Pit*/
         {
            printf("\nSquare [%d,%d] is a pit", i, j);
            map[i][j] = 'P';
         }
         else
         {
            printf("\nSquare [%d,%d] is a normal square", i, j);
            map[i][j] = 'X';
         }
      }
   }
   
   /*Add the correct number of Wumpi and Supmuwes*/
   while(1)
   {
      /*Choose random map indices and check to see if they are far enough away from the cave*/
      i = rand()%size;
      j = rand()%size;
      /*Check to see if the index is far enough away from the entrance and that the square is normal*/
      if(i<floor((2.0/3)*size) && j>floor((1.0/3)*size))
      {
         if (map[i][j] == 'X')
         {
            temp = rand() % 2;
            if(wumpCount>0 && temp==0)
            {
               map[i][j] = 'W';
               wumpCount--;
               printf("\nAdded a Wumpus!");
            }
            else if(supCount>0 && temp==1)
            {
               map[i][j] = 'S';
               supCount--;
               printf("\nAdded a Supmuw");
            }
         }
         else if (map[i][j] == 'P')
         {
            if(supCount > 0)
            {
               map[i][j] = 'Y';
               supCount--;
               printf("\nAdded a Supmuw in a pit");
            }
         }
         else if (map[i][j] == 'G')
         {
            if(wumpCount > 0)
            {
               map[i][j] = 'U';
               wumpCount--;
               printf("\nAdded a Wumpus with the gold!");
            }
         }
         else if (map[i][j] == 'W')
         {
            if(wumpCount > 0)
            {
               map[i][j] = 'Z';
               wumpCount--;
               printf("\nAdded a Wumpus with the Supmuw!");
            }
         }
         else if (map[i][j] == 'S')
         {
            if(supCount > 0)
            {
               map[i][j] = 'Z';
               supCount--;
               printf("\nAdded a Supmuw with the Wumpus");
            }
         }
            
      }
      if (wumpCount==0 && supCount==0)
         break;
   }
   
   /*Loop through the map and change any Supmuwes to Wumpi*/
   for (i=0 ; i<size ; i++)
   {
      for(j=0 ; j<size ; j++)
      {
         if (map[i][j] == 'Y' || map[i][j] == 'S')
         {
            if (i > 0)               
            {
               if(map[i-1][j]=='W' || map[i-1][j]=='U' || map[i-1][j]=='Z')
               {               
                  map[i][j] = 'W';
                  printf("\nChanging the Supmuw @ [%d,%d] to a Wumpus U", i,j);
               }
            }
            if (i < size-1)
            {
               if(map[i+1][j]=='W' || map[i+1][j]=='U' || map[i+1][j]=='Z')
               {
                  map[i][j] = 'W';
                  printf("\nChanging the Supmuw @ [%d,%d] to a Wumpus D", i,j);
               }
            }
            if (j > 0)
            {
               if(map[i][j-1]=='W' || map[i][j-1]=='U' || map[i][j-1]=='Z')
               {
                  map[i][j] = 'W';
                  printf("\nChanging the Supmuw @ [%d,%d] to a Wumpus L", i,j);
               }
            }
            if (j < size-1)
            {
               if(map[i][j+1]=='W' || map[i][j+1]=='U' || map[i][j+1]=='Z')
               {
                  map[i][j] = 'W';
                  printf("\nChanging the Supmuw @ [%d,%d] to a Wumpus R", i,j);
               }
            }
         }
      }
   }
   
   /*Write map to the file*/
   fprintf(f, "%d\n", size);
   for(i=0 ; i<size ; i++)
      fprintf(f, "%s\n", map[i]);
   
   fclose(f);
   
   /*DONE*/
   return 1;
}
