/***********************************************************/
/*Andrew Walker           CSC4240                Program #2*/
/*Date Created: 10/17/07                                   */
/*Dates Modified: 10/18/07, 10/19/07, 10/21/07, 10/22/07   */
/*                10/23/07, 10/24/07                       */
/*File Name: Wumplus_World.c                               */
/*Title: Wumplus World                                     */
/*Purpose: This is the world that the agent or a user will */
/*         interact with.  It will handle all perception   */
/*         and actions taken by either and will return the */
/*         score, new position, new perceptions, and any   */
/*         other message (i.e. death, obtained gold, etc.).*/
/*Requirements: Permisions needed to run the program and   */
/*              read the map file.                         */
/*Notes: Takes two command line arguments:                 */
/*       1) A string representing the file name for a map. */
/*          Unlike the map generator the .wump.m extension */
/*          must be given.                                 */
/*       2) Either a -a or -u to denote either an agent or */
/*          a user will be interacting with the world.     */
/***********************************************************/

#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <math.h>
#include <time.h>
#include "Agent.h"

char **map, *percepts;
int size, score, arrows;

void getPercepts(int *);
void execute(char *, int *);
int shoot(char *, int *);
int change(char *, int *);

int main(int argc, char *argv[])
{
   srand(time(0));
   /*Check command line arguments for validity*/
   char *option = (char *)malloc(3*sizeof(char));
   if (argc != 3)
   {
      printf("\nIncorrect number of command line arguments!");
      exit(0);
   }
   
   if (strcmp("-u", argv[1])!=0 && strcmp("-a", argv[1])!=0)
   {
      printf("\nIncorrect option!");
      exit(0);
   }
   else
      strcpy(option, argv[1]);
   
   /*Retrieve map from file*/
   FILE *f = fopen(argv[2], "r");
   if (f == NULL)
   {
      printf("\nInvalid file name!");
      exit(0);
   }
   
   /*create 2d array to represent map*/
   int i,j;
   char *temp, *sizeStr;
   sizeStr = (char *)malloc(3*sizeof(char));
   fscanf(f, "%s\n", sizeStr);   
   size = atoi(sizeStr);   
   temp = (char *)malloc((size + 1)*sizeof(char));
   map = (char **)malloc(size*sizeof(char *));
   for (i=0; i<size ; i++)
      map[i] = (char *)malloc(size*sizeof(char));
   
   /*read map into array*/
   for (i=0 ; i<size ; i++)
   {
      fscanf(f, "%s", temp);
      for (j=0 ; j<size ; j++)
         map[i][j] = temp[j];
   }
   free(temp);
   fclose(f);
   
   /*test
   printf("\nOpened a wumplus map of size: %d", size);
   for (i=0 ; i<size ; i++)
      printf("\nRow %d: %s", i, map[i]);*/
   
   /*Begin accepting input from either a user or an agent*/
   score = 0;
   percepts = (char *)calloc(7,sizeof(char)); /*The perceptions: Glitter, Breeze, Stench, Moo, Bump, Scream*/
   char *move = (char *)malloc(15*sizeof(char));
   int *pos = (int *)malloc(2*sizeof(int));
   FILE *g; /*file for solution*/
   pos[0] = size-1;
   pos[1] = 0;
   arrows = ceil(size/10);
   /*Since orignal version had one wumpus and one arrow, I have given the player as many arrows as wumpi.*/
   g = fopen("temp_solution.wump.s", "w");
      
   if (strcmp("-u", option) == 0)
   {
      system("clear");
      printf("\n\t\t\tWelcome to the Wumplus World!");
   }
   else
   {
      system("clear");
      printf("\n\t\t\tWelcome to the Wumplus World!\n\nStarting Agent...");
      init(size);
   }      
         
   int count = 0;
   while(1)
   {
      getPercepts(pos);
      if (strcmp("-u", option) == 0)
      {
         printf("\nPrecepts: %s\tPosition: [%d,%d]\tScore: %d", percepts, pos[0], pos[1], score);
         printf("\nPlease enter a move(left,right,up,down,shoot-dir, grab, quit): ");
         scanf("%s", move);
         getchar();
         percepts[4] = '0';
         percepts[5] = '0';
         if (strcmp("quit", move) == 0)
             exit(1);
         execute(move, pos);         
      }
      else
      {
         printf("\nBefore move: %s", move);
         printf("\nPrecepts: %s\tPosition: [%d,%d]\tScore: %d", percepts, pos[0], pos[1], score);
         strcpy(move, getMove(percepts, pos, map[pos[0]][pos[1]]));
         percepts[4] = '0';
         percepts[5] = '0';
         printf("\nAgent chose: %s", move);
         fputs(move, g);
         fputs("\n", g);
         if (strcmp("quit", move) == 0)
            exit(1);
         execute(move, pos);
         count++;
         if(count == 2*(size*size))
            exit(1);
         memset(move, 0, 15);
      }         
   }
    /*Done*/
   return 1;
}

void getPercepts(int *pos)
{
   int i, j;   
   
   /*blank first four precepts, keeping a bump or scream intact from call to execute function precepts*/
   for (i=0 ; i<=3 ; i++)
      percepts[i] = '0';
   
   /*Check for gold first*/
   if (map[pos[0]][pos[1]] == 'G')
      percepts[0] = '1';
   else
      percepts[0] = '0';
   
   /*Check the remaining perceptions*/
   if (pos[0] > 0)         
   {
      if (map[pos[0]-1][pos[1]] == 'Y' || map[pos[0]-1][pos[1]] == 'P')
         percepts[1] = '1';
      else if (percepts[1] != '1')
         percepts[1] = '0';
      if (map[pos[0]-1][pos[1]] == 'W' || map[pos[0]-1][pos[1]] == 'U' || map[pos[0]-1][pos[1]] == 'Z')
         percepts[2] = '1';
      else if (percepts[2] != '1')
         percepts[2] = '0';
      if (map[pos[0]-1][pos[1]] == 'S' || map[pos[0]-1][pos[1]] == 'Y')
         percepts[3] = '1';
      else if (percepts[3] != '1')
         percepts[3] = '0';
   }                  
   if (pos[0] < size-1)
   {
      if (map[pos[0]+1][pos[1]] == 'Y' || map[pos[0]+1][pos[1]] == 'P')
         percepts[1] = '1';
      else if (percepts[1] != '1')
         percepts[1] = '0';
      if (map[pos[0]+1][pos[1]] == 'W' || map[pos[0]+1][pos[1]] == 'U' || map[pos[0]+1][pos[1]] == 'Z')
         percepts[2] = '1';
      else if (percepts[2] != '1')
         percepts[2] = '0';
      if (map[pos[0]+1][pos[1]] == 'S' || map[pos[0]+1][pos[1]] == 'Y')
         percepts[3] = '1';
      else if (percepts[3] != '1')
         percepts[3] = '0';         
   }
   if (pos[1] > 0)
   {
      if (map[pos[0]][pos[1]-1] == 'Y' || map[pos[0]][pos[1]-1] == 'P')
         percepts[1] = '1';
      else if (percepts[1] != '1')
         percepts[1] = '0';
      if (map[pos[0]][pos[1]-1] == 'W' || map[pos[0]][pos[1]-1] == 'U' || map[pos[0]][pos[1]-1] == 'Z')
         percepts[2] = '1';
      else if (percepts[2] != '1')
         percepts[2] = '0';
      if (map[pos[0]][pos[1]-1] == 'S' || map[pos[0]][pos[1]-1] == 'Y')
         percepts[3] = '1';
      else if (percepts[3] != '1')
         percepts[3] = '0';
   }
   if (pos[1] < size-1)
   {
      if (map[pos[0]][pos[1]+1] == 'Y' || map[pos[0]][pos[1]+1] == 'P')
         percepts[1] = '1';
      else if (percepts[1] != '1')
         percepts[1] = '0';
      if (map[pos[0]][pos[1]+1] == 'W' || map[pos[0]][pos[1]+1] == 'U' || map[pos[0]][pos[1]+1] == 'Z')
         percepts[2] = '1';
      else if (percepts[2] != '1')
         percepts[2] = '0';
      if (map[pos[0]][pos[1]+1] == 'S' || map[pos[0]][pos[1]+1] == 'Y')
         percepts[3] = '1';
      else if (percepts[3] != '1')
         percepts[3] = '0';
   }      
}

void execute(char *move, int *pos)
{
   int killed = 0, bump = 0;
      
   if (strcmp("quit", move) == 0)
      exit(1);
   else if(strncmp(move, "shoot", 5) == 0)
   {
      printf("\nYou are trying to shoot an arrow");
      if (arrows > 0)
      {
         killed = shoot(move, pos);
         if (killed)
            percepts[5] = '1';
         else 
            printf("\nYou missed!");
      }
      else
      {
         printf("\nO snap! You are out of arrows!");
         return;
      }
   }
   else if (strcmp("grab", move) == 0)
   {
      if (map[pos[0]][pos[1]] == 'G')
      {
         score += 1000;
         printf("\nYou grabbed some gold!");
         map[pos[0]][pos[1]] = 'X';
      }
      else
         printf("\nNo gold for you!");
   }
   else if(strcmp(move, "up")==0 || strcmp(move, "down")==0 || strcmp(move, "left")==0 || strcmp(move, "right")==0)
   {
      score--;
      bump = change(move, pos);
      if (bump)
      {
         printf("\nYou hit a wall");
         percepts[4] = '1';
      }
   }
   else
      printf("\nInvalid Command");
}

int shoot(char *move, int *pos)
{
   int i, killed = 0;
   
   if (move[6] == 'u')
   {
      arrows--;
      score-=10;
      for (i=pos[0] ; i>=0 ; i--)
      {
         if (map[i][pos[1]] == 'L' || map[i][pos[1]] == 'S')
            break;
         if (map[i][pos[1]] == 'W')
         {
            killed = 1;
            map[i][pos[1]] = 'X';
            break;
         }
         if (map[i][pos[1]] == 'U')
         {
            killed = 1;
            map[i][pos[1]] = 'G';
            break;
         }
         if (map[i][pos[1]] == 'Z')
         {
            killed = 1;
            map[i][pos[1]] = 'S';
            break;
         }
      }
   }
   else if(move[6] == 'd')
   {
      arrows--;
      score-=10;
      for (i=pos[0] ; i<size ; i++)
      {
         if (map[i][pos[1]] == 'L' || map[i][pos[1]] == 'S')
            break;
         if (map[i][pos[1]] == 'W')
         {
            killed = 1;
            map[i][pos[1]] = 'X';
            break;
         }
         if (map[i][pos[1]] == 'U')
         {
            killed = 1;
            map[i][pos[1]] = 'G';
            break;
         }
         if (map[i][pos[1]] == 'Z')
         {
            killed = 1;
            map[i][pos[1]] = 'S';
            break;
         }
      }
   }
   else if (move[6] == 'l')
   {
      arrows--;
      score-=10;
      for (i=pos[1] ; i>=0 ; i--)
      {
         if (map[pos[0]][i] == 'L' || map[pos[0]][i] == 'S')
            break;
         if (map[pos[0]][i] == 'W')
         {
            killed = 1;
            map[pos[0]][i] = 'X';
            break;
         }
         if (map[pos[0]][i] == 'U')
         {
            killed = 1;
            map[pos[0]][i] = 'G';
            break;
         }
         if (map[pos[0]][i] == 'Z')
         {
            killed = 1;
            map[pos[0]][i] = 'S';
            break;
         }
      }
   }
   else if (move[6] == 'r')
   {
      arrows--;
      score-=10;
      for (i=pos[1] ; i<size ; i++)
      {
         if (map[pos[0]][i] == 'L' || map[pos[0]][i] == 'S')
            break;
         if (map[pos[0]][i] == 'W')
         {
            killed = 1;
            map[pos[0]][i] = 'X';
            break;
         }
         if (map[pos[0]][i] == 'U')
         {
            killed = 1;
            map[pos[0]][i] = 'G';
            break;
         }
         if (map[pos[0]][i] == 'Z')
         {
            killed = 1;
            map[pos[0]][i] = 'S';
            break;
         }
      }
   }
   else
      printf("\nInvalid direction");
   
   return killed;
}

int change(char *move, int *pos)
{
   if (move[0] == 'u')
   {
      if(pos[0] > 0)
      {
         if(map[pos[0]-1][pos[1]] == 'L')
            return 1;
         
         pos[0] = pos[0]-1;
         if(map[pos[0]][pos[1]]=='W' || map[pos[0]][pos[1]]=='Z' || map[pos[0]][pos[1]]=='U' || map[pos[0]][pos[1]]=='P')
         {
            printf("\nYou is dead! Final score: %d", score);
            exit(1);
         }
         else if(map[pos[0]][pos[1]] == 'S')
         {
            printf("\nThe supmuw gives you the #%d combo with cheese", (rand()%8)+1);
            score += 100;
         }
      }
      else
         return 1;
   }
   else if (move[0] == 'd')
   {
      if(pos[0] < size-1)
      {
         if(map[pos[0]+1][pos[1]] == 'L')
            return 1;
         
         pos[0] = pos[0]+1;
         if(map[pos[0]][pos[1]]=='W' || map[pos[0]][pos[1]]=='Z' || map[pos[0]][pos[1]]=='U' || map[pos[0]][pos[1]]=='P')
         {
            printf("\nYou is dead! Final score: %d", score);
            exit(1);
         }
         else if(map[pos[0]][pos[1]] == 'S')
         {
            printf("\nThe supmuw gives you the #%d combo with cheese", (rand()%8)+1);
            score += 100;
         }
      }
      else
         return 1;
   }
   else if (move[0] == 'l')
   {
      if(pos[1] > 0)
      {
         if(map[pos[0]][pos[1]-1] == 'L')
            return 1;
         
         pos[1] = pos[1]-1;
         if(map[pos[0]][pos[1]]=='W' || map[pos[0]][pos[1]]=='Z' || map[pos[0]][pos[1]]=='U' || map[pos[0]][pos[1]]=='P')
         {
            printf("\nYou is dead! Final Score: %d", score);
            exit(1);
         }
         else if(map[pos[0]][pos[1]] == 'S')
         {
            printf("\nThe supmuw gives you the #%d combo with cheese", (rand()%8)+1);
            score += 100;
         }
      }
      else
         return 1;
   }
   else if (move[0] == 'r')
   {
      if(pos[1] < size-1)
      {
         if(map[pos[0]][pos[1]+1] == 'L')
            return 1;
         
         pos[1] = pos[1]+1;
         if(map[pos[0]][pos[1]]=='W' || map[pos[0]][pos[1]]=='Z' || map[pos[0]][pos[1]]=='U' || map[pos[0]][pos[1]]=='P')
         {
            printf("\nYou is dead! Final Score: %d", score);
            exit(1);
         }
         else if(map[pos[0]][pos[1]] == 'S')
         {
            printf("\nThe supmuw gives you the #%d combo with cheese", (rand()%8)+1);
            score += 100;
         }
      }
      else
         return 1;
   }
   
   return 0;
}
