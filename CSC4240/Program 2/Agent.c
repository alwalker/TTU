/***********************************************************/
/*Andrew Walker           CSC4240                Program #2*/
/*Date Created: 10/24/07                                   */
/*Dates Modified:                                          */
/*File Name: Agent.c                                       */
/*Title: Wumplus World Agent                               */
/*Purpose: A intelligent agent for traversing the Wumpus   */
/*         World with the goal of getting as many points   */
/*         as possible.                                    */
/*Requirements: Permission to write and create files in the*/
/*              directory this program will be ran.        */
/*Notes: 1. Will return a move to the Wumpus World and     */
/*          accepts a position and list of perceptions.    */
/*       2. Will generate a file with extension .wump.s    */
/*          that contains the list of moves to a solution. */
/***********************************************************/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <time.h>
#include "Agent.h"

void goHome(int *);
void markMap(int *, char *);
void updateMap();

int size, numVisited, wumpCount;
char happy, vpref, hpref, reason, backed;
int *lastPos, *wumpiLoc;
char *lastMove;
char **agentMap;

void init(int inSize)
{
   int i, j;
   size = inSize;
   numVisited = 0;
   wumpCount = 0;
   lastMove = (char *)malloc(15*sizeof(char));
   lastPos = (int *)malloc(2*sizeof(int));
   lastPos[0] = size-1;
   lastPos[1] = 0;
   /*wumpiLoc will keep up with the column and row the known wumpi are in.  If the agent is in one of these*/
   /*it can use this to determine when and what diriction to shoot.*/
   wumpiLoc = (int *)malloc((2*ceil(size/10))*sizeof(int));
   for(i=0 ; i<(2*ceil(size/10)) ; i++)
      wumpiLoc[i] = -1;
   /*vpref and hpref will represent the way the agent is trying to regardless of what's in his way*/
   vpref = 'u';
   hpref = 'r';
   /*happy represents whether or not the agent is currently going in direction it truly wants to go*/
   happy = 1;
   /*the reason variable will state whether or not the agent changed directions because of a "bad"*/
   /*perception or because the end of the world was reached*/
   reason = 2; /*0=end of world, 1="bad" perception, 2=no change*/
   /*had to move back.  If even go back, if odd go opposite direction/*/
   backed = '0';
   
   /*agentMap will contain what it knows about the map using the following symbols in addition to*/
   /*the ones defined in the map generator: 
   0: Unknown
   A: Breeze,Stench,Moo
   B: Breeze
   C: Stench
   M: Moo
   D: Breeze,Stench
   E: Stench,Moo
   F: Breeze,Moo
   H: Safe*/
   agentMap = (char **)malloc(size*sizeof(char *));
   for (i=0 ; i<size ; i++)
      agentMap[i] = (char *)malloc(size*sizeof(char *));   
   for(i=0 ; i<size ; i++)
   {
      for(j=0 ; j<size ; j++)
         agentMap[i][j] = '0';
   }
}
   
char *getMove(char *percepts, int *pos, char square)
{
   /*if pit or wumpus near entrance quit*/
   if ((pos[0]==(size-1) && pos[1]==0) && (percepts[1]=='1' || percepts[2]=='1'))
      return "quit";
   
   srand(time(0));
      
   /*if glitter then grab*/
   if (percepts[0] == '1')
   {
      strcpy(lastMove, "grab");
      return lastMove;
   }
   
   /*add to current map*/
   if(agentMap[pos[0]][pos[1]] == '0')
   {
      printf("\nMarking Map");
      numVisited++;
      /*Label surrounding squares by percepts*/
      markMap(pos, percepts);
   }   
   agentMap[pos[0]][pos[1]] = square;   
   
   /*if all squares visited then leave*/
   if (numVisited == size*size)
      goHome(pos);   
   
   /*attemp to determine more about the map*/
   updateMap();
   printf("\nCurrent Row: %s", agentMap[pos[0]]);
   
   /*check for end of world*/
   if (pos[0] == 0)
      vpref = 'd';
   else if (pos[0] == size-1)
      vpref = 'u';
   if (pos[1] == 0)
      hpref = 'r';
   else if (pos[1] == size-1)
      hpref = 'l';
   
   /*if moved into a clear space clean backtracking
   if(percepts[1]=='0' || percepts[2]=='0' || percepts[3]=='0')
      backed = 0;*/
   
   /*if unhappy then move opposite of direction of way came back from*/
   if (happy == '0')
   {
      happy = '1';
      if(lastMove[0] == 'r' || lastMove[0] == 'l')
      {
         if(vpref=='u' && pos[0]<size-1)
         {
            strcpy(lastMove, "down");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else if(vpref=='d' && pos[0]>0)
         {
            strcpy(lastMove, "up");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else
         {
            if(vpref=='u' && pos[0]<size-1)
            {
               strcpy(lastMove, "up");
               lastPos[0] = pos[0];
               lastPos[1] = pos[1];
               return lastMove;
            }
            else if(vpref=='d' && pos[0]>0)
            {
               strcpy(lastMove, "down");
               lastPos[0] = pos[0];
               lastPos[1] = pos[1];
               return lastMove;
            }
         }
      }
      if(lastMove[0] == 'd' || lastMove[0] == 'u')
      {
         if(hpref=='l' && pos[1]<size-1)
         {
            strcpy(lastMove, "right");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else if(hpref=='r' && pos[1]>0)
         {
            strcpy(lastMove, "left");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else
         {
            if(hpref=='l' && pos[1]>0)
            {
               strcpy(lastMove, "left");
               lastPos[0] = pos[0];
               lastPos[1] = pos[1];
               return lastMove;
            }
            else if(hpref=='r' && pos[1]<size-1)
            {
               strcpy(lastMove, "right");
               lastPos[0] = pos[0];
               lastPos[1] = pos[1];
               return lastMove;
            }
         }
      }
   }
   /*check for backtracking.*/
   if (backed == '1')
   {
      printf("\nBacked!");
      backed = '0';
      if (lastMove[0]=='l' || lastMove[0]=='r')
      {
         if(vpref=='u' && (agentMap[pos[0]-1][pos[1]]=='X' || agentMap[pos[0]-1][pos[1]]=='H' || agentMap[pos[0]-1][pos[1]]=='S'))
         {
            strcpy(lastMove, "up");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else if (pos[0]<size-1 && (agentMap[pos[0]+1][pos[1]]=='X' || agentMap[pos[0]+1][pos[1]]=='H' || agentMap[pos[0]+1][pos[1]]=='S'))
         {
            strcpy(lastMove, "down");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
      }
      else if(lastMove[0]=='u' || lastMove[0]=='d')
      {
         if(hpref=='l' && (agentMap[pos[0]][pos[1]-1]=='X' || agentMap[pos[0]][pos[1]-1]=='H' || agentMap[pos[0]][pos[1]-1]=='S'))
         {
            strcpy(lastMove, "left");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else if (pos[1]<size-1 && (agentMap[pos[0]][pos[1]+1]=='X' || agentMap[pos[0]][pos[1]+1]=='H' || agentMap[pos[0]][pos[1]+1]=='S'))
         {
            strcpy(lastMove, "right");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
      }
      else /*go back...again*/
      {
         backed = '1';
         if (lastMove[0] == 'u')
         {
            strcpy(lastMove, "down");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else if(lastMove[0] == 'd')
         {
            strcpy(lastMove, "up");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else if (lastMove[0] == 'r')
         {
            strcpy(lastMove, "left");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else
         {
            strcpy(lastMove, "right");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
      }
   }         
   /*Check for wall.  If found take the preffered opposite direction, else backtrack.*/
   else if (percepts[4] == '1')
   {
      printf("\nWalled!");
      if (lastMove[0]=='l' || lastMove[0]=='r')
      {
         if (vpref=='u' && (agentMap[pos[0]-1][pos[1]]=='X' || agentMap[pos[0]-1][pos[1]]=='S' || agentMap[pos[0]-1][pos[1]]=='H'))
         {
            strcpy(lastMove, "up");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else if ((pos[0]<size-1) && (agentMap[pos[0]+1][pos[1]]=='X' || agentMap[pos[0]+1][pos[1]]=='S' || agentMap[pos[0]+1][pos[1]]=='H'))
         {            
            strcpy(lastMove, "down");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else /*go back*/
         {
            backed = '1';
            if (lastMove[0] == 'r')
            {
               strcpy(lastMove, "left");
               lastPos[0] = pos[0];
               lastPos[1] = pos[1];
               return lastMove;
            }
            else
            {
               strcpy(lastMove, "right");
               lastPos[0] = pos[0];
               lastPos[1] = pos[1];
               return lastMove;
            }
         }
      }
      if (lastMove[0]=='u' || lastMove[0]=='d')
      {
         if (hpref=='l' && (agentMap[pos[0]][pos[1]-1]=='X' || agentMap[pos[0]][pos[1]-1]=='S' || agentMap[pos[0]][pos[1]-1]=='H'))
         {
            strcpy(lastMove, "left");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else if ((pos[1]<size-1) && (agentMap[pos[0]][pos[1]+1]=='X' || agentMap[pos[0]][pos[1]+1]=='S' || agentMap[pos[0]][pos[1]+1]=='H'))
         {            
            strcpy(lastMove, "right");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else /*go back*/
         {
            backed = '1';
            if (lastMove[0] == 'u')
            {
               strcpy(lastMove, "down");
               lastPos[0] = pos[0];
               lastPos[1] = pos[1];
               return lastMove;
            }
            else
            {
               strcpy(lastMove, "up");
               lastPos[0] = pos[0];
               lastPos[1] = pos[1];
               return lastMove;
            }
         }
      }
   }
   /*If "bad" perception detected back up and change direction*/
   else if(percepts[1]=='1' || percepts[2]=='1' || percepts[3]=='1')
   {
      printf("\nI got a bad feeling about this");
      happy = '0';
      
      if (lastMove[0] == 'u')
      {
         strcpy(lastMove, "down");
         lastPos[0] = pos[0];
         lastPos[1] = pos[1];
         return lastMove;
      }
      else if(lastMove[0] == 'd')
      {
         strcpy(lastMove, "up");
         lastPos[0] = pos[0];
         lastPos[1] = pos[1];
         return lastMove;
      }
      else if (lastMove[0] == 'r')
      {
         strcpy(lastMove, "left");
         lastPos[0] = pos[0];
         lastPos[1] = pos[1];
         return lastMove;
      }
      else
      {
         strcpy(lastMove, "right");
         lastPos[0] = pos[0];
         lastPos[1] = pos[1];
         return lastMove;
      }
   }
   else
   {
      printf("\nNormal");
      backed = '0';
      /*If at the edge of world then move up or down*/
      if (pos[1] == size-1)
      {
         if (vpref == 'u')
         {
            strcpy(lastMove, "up");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else
         {
            strcpy(lastMove, "down");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
      }
      else
      {
         if (hpref == 'r')
         {
            strcpy(lastMove, "right");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
         else
         {
            strcpy(lastMove, "left");
            lastPos[0] = pos[0];
            lastPos[1] = pos[1];
            return lastMove;
         }
      }
   }
}
void goHome(int *pos)
{
}

void markMap(int *pos, char *percepts)
{
   /*if percepts blank then surrounding squares are safe*/
   if (percepts[1]=='0' && percepts[2]=='0' && percepts[3]=='0')
   {
      if (pos[0] > 0)
         agentMap[pos[0]-1][pos[1]] = 'H';
      if (pos[0] < size-1)
         agentMap[pos[0]+1][pos[1]] = 'H';
      if (pos[1] > 0)
         agentMap[pos[0]][pos[1]-1] = 'H';
      if (pos[1] < size-1)
         agentMap[pos[0]][pos[1]+1] = 'H';      
   }
   else if (percepts[1]=='1' && percepts[2]=='0' && percepts[3]=='0')
   {
      if (pos[0] > 0)
         if (agentMap[pos[0]-1][pos[1]]=='0') {agentMap[pos[0]-1][pos[1]] = 'B';}
      if (pos[0] < size-1)
         if (agentMap[pos[0]+1][pos[1]]=='0') {agentMap[pos[0]+1][pos[1]] = 'B';}
      if (pos[1] > 0)
         if (agentMap[pos[0]][pos[1]-1]=='0') {agentMap[pos[0]][pos[1]-1] = 'B';}
      if (pos[1] < size-1)
         if (agentMap[pos[0]+1][pos[1]]=='0') {agentMap[pos[0]][pos[1]+1] = 'B';}
   }
   else if (percepts[1]=='0' && percepts[2]=='1' && percepts[3]=='0')
   {
      if (pos[0] > 0)
         if (agentMap[pos[0]-1][pos[1]]=='0') {agentMap[pos[0]-1][pos[1]] = 'C';}
      if (pos[0] < size-1)
         if (agentMap[pos[0]+1][pos[1]]=='0') {agentMap[pos[0]+1][pos[1]] = 'C';}
      if (pos[1] > 0)
         if (agentMap[pos[0]][pos[1]-1]=='0') {agentMap[pos[0]][pos[1]-1] = 'C';}
      if (pos[1] < size-1)
         if (agentMap[pos[0]][pos[1]+1]=='0') {agentMap[pos[0]][pos[1]+1] = 'C';}
   }
   else if (percepts[1]=='0' && percepts[2]=='0' && percepts[3]=='1')
   {
      if (pos[0] > 0)
         if (agentMap[pos[0]-1][pos[1]]=='0') {agentMap[pos[0]-1][pos[1]] = 'M';}
      if (pos[0] < size-1)
         if (agentMap[pos[0]+1][pos[1]]=='0') {agentMap[pos[0]+1][pos[1]] = 'M';}
      if (pos[1] > 0)
         if (agentMap[pos[0]][pos[1]-1]=='0') {agentMap[pos[0]][pos[1]-1] = 'M';}
      if (pos[1] < size-1)
         if (agentMap[pos[0]][pos[1]+1]=='0') {agentMap[pos[0]][pos[1]+1] = 'M';}
   }
   else if (percepts[1]=='1' && percepts[2]=='1' && percepts[3]=='0')
   {
      if (pos[0] > 0)
         if (agentMap[pos[0]-1][pos[1]]=='0') {agentMap[pos[0]-1][pos[1]] = 'D';}
      if (pos[0] < size-1)
         if (agentMap[pos[0]+1][pos[1]]=='0') {agentMap[pos[0]+1][pos[1]] = 'D';}
      if (pos[1] > 0)
         if (agentMap[pos[0]][pos[1]-1]=='0') {agentMap[pos[0]][pos[1]-1] = 'D';}
      if (pos[1] < size-1)
         if (agentMap[pos[0]][pos[1]+1]=='0') {agentMap[pos[0]][pos[1]+1] = 'D';}
   }
   else if(percepts[1]=='0' && percepts[2]=='1' && percepts[3]=='1')
   {
      if (pos[0] > 0)
         if (agentMap[pos[0]-1][pos[1]]=='0') {agentMap[pos[0]-1][pos[1]] = 'E';}
      if (pos[0] < size-1)
         if (agentMap[pos[0]+1][pos[1]]=='0') {agentMap[pos[0]+1][pos[1]] = 'E';}
      if (pos[1] > 0)
         if (agentMap[pos[0]][pos[1]-1]=='0') {agentMap[pos[0]][pos[1]-1] = 'E';}
      if (pos[1] < size-1)
         if (agentMap[pos[0]][pos[1]+1]=='0') {agentMap[pos[0]][pos[1]+1] = 'E';}
   }
   else if(percepts[1]=='1' && percepts[2]=='0' && percepts[3]=='1')
   {
      if (pos[0] > 0)
         if (agentMap[pos[0]-1][pos[1]]=='0') {agentMap[pos[0]-1][pos[1]] = 'F';}
      if (pos[0] < size-1)
         if (agentMap[pos[0]+1][pos[1]]=='0') {agentMap[pos[0]+1][pos[1]] = 'F';}
      if (pos[1] > 0)
         if (agentMap[pos[0]][pos[1]-1]=='0') {agentMap[pos[0]][pos[1]-1] = 'F';}
      if (pos[1] < size-1)
         if (agentMap[pos[0]][pos[1]+1]=='0') {agentMap[pos[0]][pos[1]+1] = 'F';}
   }
   else if(percepts[1]=='1' && percepts[2]=='1' && percepts[3]=='1')
   {
      if (pos[0] > 0)
         if (agentMap[pos[0]-1][pos[1]]=='0') {agentMap[pos[0]-1][pos[1]] = 'A';}
      if (pos[0] < size-1)
         if (agentMap[pos[0]+1][pos[1]]=='0') {agentMap[pos[0]+1][pos[1]] = 'A';}
      if (pos[1] > 0)
         if (agentMap[pos[0]][pos[1]-1]=='0') {agentMap[pos[0]][pos[1]-1] = 'A';}
      if (pos[1] < size-1)
         if (agentMap[pos[0]][pos[1]+1]=='0') {agentMap[pos[0]][pos[1]+1] = 'A';}
   }
}

void updateMap()
{
   int i, j, k, breezeCount=0, stenchCount=0, mooCount=0;
   char *squares = (char *)malloc(4*sizeof(char));
   
   for (i=0 ; i<size ; i++)
   {
      for(j=0 ; j<size ; j++)
      {
         if (agentMap[i][j]=='A' || agentMap[i][j]=='B' || agentMap[i][j]=='C' || agentMap[i][j]=='M' || agentMap[i][j]=='D' || agentMap[i][j]=='E' || agentMap[i][j]=='F')
         {
            /*Try to determine what the square is.  This is only possible if only one of the directly*/
            /*adjacent squares contains a perception.*/
            /*First, get the four surrounding squares*/
            if (i > 0)
               squares[0] = agentMap[i-1][j];
            else
               squares[0] = 'X';               
            if (i < size-1)
               squares[1] = agentMap[i+1][j];
            else
               squares[1] = 'X';
            if (j > 0)
               squares[2] = agentMap[i][j-1];
            else
               squares[2] = 'X';
            if (j < size-1)
               squares[3] = agentMap[i][j+1];
            else
               squares[3] = 'X';
            
            /*Go through and count the perceptions.*/
            for (k=0 ; k<4 ; k++)
            {
               if (squares[k]=='B' || squares[k]=='D' || squares[k]=='A' || squares[k]=='F' || squares[k]=='0')
                  breezeCount++;
               if (squares[k]=='C' || squares[k]=='D' || squares[k]=='A' || squares[k]=='E' || squares[k]=='0')
                  stenchCount++;
               if (squares[k]=='M' || squares[k]=='E' || squares[k]=='A' || squares[k]=='F' || squares[k]=='0')
                  mooCount++;
            }
            /*If any of the counts are one then the squares that were not that precept are safe.*/
            if (breezeCount <= 1)
            {
               if ((squares[0]=='B' || squares[0]=='D' || squares[0]=='A' || squares[0]=='F') && (i>0))
                  agentMap[i-1][j] = 'P';
               if ((squares[1]=='B' || squares[1]=='D' || squares[1]=='A' || squares[1]=='F') && (i<size-1))
                  agentMap[i+1][j] = 'P';
               if ((squares[2]=='B' || squares[2]=='D' || squares[2]=='A' || squares[2]=='F') && (j>0))
                  agentMap[i][j-1] = 'P';
               if ((squares[3]=='B' || squares[3]=='D' || squares[3]=='A' || squares[3]=='F') && (j<size-1))
                  agentMap[i][j+1] = 'P';
            }
            if (stenchCount <= 1)
            {
               if ((squares[0]=='C' || squares[0]=='D' || squares[0]=='A' || squares[0]=='E') && (i>0))
               {
                  wumpiLoc[wumpCount+1] = i-1;
                  wumpiLoc[wumpCount+2] = j;
                  agentMap[i-1][j] = 'W';
               }
               if ((squares[1]=='C' || squares[1]=='D' || squares[1]=='A' || squares[1]=='E') && (i<size-1))
               {
                  wumpiLoc[wumpCount+1] = i+1;
                  wumpiLoc[wumpCount+2] = j;
                  agentMap[i+1][j] = 'W';
               }
               if ((squares[2]=='C' || squares[2]=='D' || squares[2]=='A' || squares[2]=='E') && (j>0))
               {
                  wumpiLoc[wumpCount+1] = i;
                  wumpiLoc[wumpCount+2] = j-1;
                  agentMap[i][j-1] = 'W';
               }
               if ((squares[3]=='C' || squares[3]=='D' || squares[3]=='A' || squares[3]=='E') && (j<size-1))
               {
                  wumpiLoc[wumpCount+1] = i;
                  wumpiLoc[wumpCount+2] = j+1;
                  agentMap[i][j+1] = 'W';
               }
            }
            if (mooCount <= 1)
            {
               if ((squares[0]=='M' || squares[0]=='E' || squares[0]=='A' || squares[0]=='F') && (i>0))
                  agentMap[i-1][j] = 'S';
               if ((squares[1]=='M' || squares[1]=='E' || squares[1]=='A' || squares[1]=='F') && (i<size-1))
                  agentMap[i+1][j] = 'S';
               if ((squares[2]=='M' || squares[2]=='E' || squares[2]=='A' || squares[2]=='F') && (j>0))
                  agentMap[i][j-1] = 'M';
               if ((squares[3]=='M' || squares[3]=='E' || squares[3]=='A' || squares[3]=='F') && (j<size-1))
                  agentMap[i][j+1] = 'P';
            }
         }
      }
   }
   
   free(squares);
}
