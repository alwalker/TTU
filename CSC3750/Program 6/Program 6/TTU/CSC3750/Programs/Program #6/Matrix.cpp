#include "Matrix.h"
#include <math.h>
#include <iostream>
using namespace std;

Matrix::Matrix(int row, int column) : BasicMatrix(row, column)
{
}

Matrix::~Matrix()
{}

Matrix* Matrix::getIdentityMatrix(int size)
{
   Matrix* matrix = new Matrix(size, size);  //all zeroes
   for (int i = 0; i < size; i++)  //insert 1's on the diagonal
   {
      matrix->setElement(i, i, 1.0);
   }
   return matrix;
}

Matrix* Matrix::add(Matrix* otherMatrix)
{

   int rows = getNumRows();
   int cols = getNumColumns();

   if( getNumRows() != otherMatrix->getNumRows() || getNumColumns() != otherMatrix->getNumColumns())
   {
      return new Matrix(1,1);
   }

   Matrix* result = new Matrix(rows, cols);    

   for (int i = 1; i <= rows;i++)
   {
      for (int j = 1; j <= cols; j++)
      {
         result->setElement(i,j,getElement(i,j) + otherMatrix->getElement(i,j));
      }
   }

   return result;  
}

Matrix* Matrix::multiply(Matrix* otherMatrix)
{
   
   if (getNumColumns() != otherMatrix->getNumRows())
   {
      return new Matrix(1, 1);
   }

   Matrix* result = new Matrix(getNumRows(), otherMatrix->getNumColumns());

   int rows = result->getNumRows();
   int cols = result->getNumColumns();

   double x;

   //loop over all elements of resulting matrix
   for (int i = 1; i <= rows;i++)
   {
      for (int j = 1; j <= cols; j++)
      {
         x=0;
         //sum up multiplying matrices to obtain value placed in new matrix
         for (int k = 1; k <= getNumColumns(); k++)
         {
            x = x + getElement(i,k) * otherMatrix->getElement(k,j);
         }
         result->setElement(i,j,x);
      }
   }

   return result;
}

Matrix* Matrix::transpose()
{
   int rows = getNumRows();
   int cols = getNumColumns();
   Matrix* result = new Matrix(cols, rows);

   for (int i = 1; i <= rows; i++)
   {
      for (int j = 1; j <= cols; j++)
      {
         result->setElement(j, i, getElement(i,j));
      }
   }
    
   return result;
}

double** Matrix::toArray()
{
   Matrix* m = this;
   int rows = m->getNumRows();
   int cols = m->getNumColumns();

   double** array = new double*[rows];

   for (int x = 0; x < cols; x++)
   {
      double* temp = new double[cols];
      array[x] = temp;
   }

   for (int i = 1; i <= rows; i++)
   {
      for (int j = 1; j <= cols; j++)
      {
         array[i - 1][j - 1] = m->getElement(i, j);
      }
   }

   return array;
}

//assumes a square matrix
Matrix** Matrix::PLU()
{
   int rows = getNumRows();
   int cols = getNumColumns();

   Matrix* U = copy();
   Matrix* L = new Matrix(rows, cols);  
   for (int i = 1; i <= rows; i++)
   {
      L->setElement(i, i, 0); //don't want identity for L right now
   }
   Matrix* P = new Matrix(rows, cols); 

   for (int i = 1; i < rows; i++)
   {
      //find largest coefficient below the pivot element to avoid divide by zero
      double trial = fabs(getElement(i, i));
      int pivotRow1 = i;
      int pivotRow2 = pivotRow1;
      for (int j = i + 1; j <= rows; j++)
      {
         double temp = fabs(getElement(j, i));
         if (temp > trial) 
         {
            pivotRow2 = j;
            trial = temp;
         }
      }

      if (pivotRow1 < pivotRow2)
      {
         U->pivotRows(pivotRow1, pivotRow2);
         L->pivotRows(pivotRow1, pivotRow2);
         P->pivotRows(pivotRow1, pivotRow2);
      }
      L->setElement(i, i, 1.0);

      //multiply through by factor from the row above
      double denom = U->getElement(i, i);
      for (int j = i + 1; j <= rows; j++)
      {
         double numer = U->getElement(j, i);
         double factor = numer/denom;
         for (int k = i; k <= cols; k++)
         {
            double temp = U->getElement(i, k) * factor;
            double subtract = U->getElement(j, k) - temp;
            U->setElement(j, k, subtract);
         }
         L->setElement(j, i, factor);
      }
      L->setElement(rows, rows, 1.0);
   }

   Matrix** PLU = new Matrix*[3];
   PLU[0] = L;
   PLU[1] = U;
   PLU[2] = P;

   return PLU;
}

Matrix* Matrix::forwardSubstitution(Matrix* colVector)
{  
   int rows = colVector->getNumRows();
   Matrix* result = new Matrix(rows, 1);

   for (int i = 1; i <= rows; i++)
   {
      double rhs = colVector->getElement(i, 1);  
      double coeff = getElement(i, i);
      for (int j = 1; j < i; j++)
      {
         rhs = rhs - getElement(i, j)*result->getElement(j, 1);
      }
      result->setElement(i, 1, rhs/coeff);
   }

   return result;
}

Matrix* Matrix::backSubstitution(Matrix* colVector)
{
   int rows = colVector->getNumRows();
   Matrix* result = new Matrix(rows, 1);

   for (int i = rows; i >= 1 ; i--)
   {
      double rhs = colVector->getElement(i, 1); 
      double coeff = getElement(i, i);
      for (int j = rows; j > i; j--)
      {
         rhs = rhs - getElement(i, j)*result->getElement(j, 1);
      }
      result->setElement(i, 1, rhs/coeff);
   }

   return result;
}

//pivot rows row1 and row2 (swap them)
Matrix* Matrix::pivotRows(int row1, int row2)
{
   int cols = getNumColumns();
   for (int i = 1; i <= cols; i++)
   {
      double temp = getElement(row1, i);
      setElement(row1, i, getElement(row2, i));
      setElement(row2, i, temp);
   }
}

Matrix* Matrix::copy()
{
   int rows = getNumRows();
   int cols = getNumColumns();
   Matrix* mat = new Matrix(rows, cols);

   for (int i = 1; i <= rows; i++)
   {
      for (int j = 1; j <= cols; j++)
      {
         mat->setElement(i, j, getElement(i, j));
      }
   }

   return mat;
}

Matrix* Matrix::inverse()
{
   Matrix** PLUarray = PLU();
   Matrix* L = PLUarray[0];
   Matrix* U = PLUarray[1];
   Matrix* P = PLUarray[2];
   delete[] PLUarray;

   int rows = getNumRows();
   int cols = getNumColumns();

   Matrix* inverse = new Matrix(rows, cols);

   for (int i = 1; i <= cols; i++)
   {
      Matrix* colVector = new Matrix(rows, 1);
      colVector->setElement(i, 1, 1.0);  //basis vectors

      Matrix* temp = L->forwardSubstitution(colVector);
      delete colVector;
      colVector = temp;

      temp = U->backSubstitution(colVector);
      delete colVector;
      colVector = temp;

      for (int j = 1; j <= rows; j++)
      {
         inverse->setElement(j, i, colVector->getElement(j, 1));
      }
      delete colVector;
   }

   delete L;
   delete U;
   Matrix* temp = inverse->multiply(P);
   delete inverse;
   inverse = temp;
   delete P;

   return inverse;
}

