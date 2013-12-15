#if !defined (MATRIX_H)
#define MATRIX_H

#include "BasicMatrix.h"

class Matrix : public BasicMatrix
{
   private:
      Matrix* pivotRows(int i, int j);

   public:
      Matrix(int row, int column);
      ~Matrix();
      static Matrix* getIdentityMatrix(int size);
      Matrix* add(Matrix* otherMatrix);
      Matrix* multiply(Matrix* otherMatrix);
      Matrix* transpose();
      Matrix** PLU();
      Matrix* copy();
      Matrix* forwardSubstitution(Matrix* col);
      Matrix* backSubstitution(Matrix* col);
      Matrix* inverse();
      double** toArray();

};

#endif
