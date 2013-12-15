#include "BasicMatrix.h"
#include <iostream>
using namespace std;

BasicMatrix::BasicMatrix(int row, int column)
{

   if (row < 1)
   {
      row = 1;
   }

   if (column < 1)
   {
      column = 1;
   }
 
   rows = row;
   columns = column;

   matrix = new double*[rows];  //array of pointers

   //initialize the matrix to contain all zeroes (might not be square in general
   for (int x = 0; x < rows; x++)
   {
      double* temp = new double[columns];  //array of doubles
      matrix[x] = temp;
      for (int y = 0; y < columns; y++)
      {
         matrix[x][y] = 0;
         if (rows == columns && x == y)
         {
            matrix[x][y] = 1;
         }
      }
   }
}

BasicMatrix::~BasicMatrix()
{
   int rows = getNumRows();
   for (int x = 0; x < rows; x++)
   {
      delete[] matrix[x];
   }
   delete[] matrix;
}

void BasicMatrix::printMatrix()
{

   for (int x = 1; x <= rows; x++)
   {
      for (int y = 1; y <= columns; y++)
      {
         cout << getElement(x,y) << "   ";
      }
      cout << endl;
   }

}

bool BasicMatrix::checkInput(int row, int column)
{
    if (row > rows || row < 1 || column > columns || column < 1)
    {
       return 1;  //false
    }
    return 0;
}

int BasicMatrix::getNumRows()
{
   return rows;
}

int BasicMatrix::getNumColumns()
{
   return columns;
}

void BasicMatrix::setElement(int row, int column, double value)
{
   bool b = checkInput(row, column);
   if (b == 0)
   {
      matrix[row - 1][column - 1] = value;
   }
}

double BasicMatrix::getElement(int row, int column)
{
   bool b = checkInput(row, column);
   if (b == 0)
   {
      return matrix[row - 1][column - 1];
   }
}
