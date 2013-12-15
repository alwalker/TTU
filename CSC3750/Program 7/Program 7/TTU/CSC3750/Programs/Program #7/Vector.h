#if !defined (VECTOR_H)
#define VECTOR_H

#include "Matrix.h"

class Vector
{
   private:
      Matrix* vector;

   public:
      Vector(double x, double y, double z);
      ~Vector();
      void setX(double x);
      void setY(double y);
      void setZ(double z);
      double getX();
      double getY();
      double getZ();
      Vector* cross(Vector* cross);
      double dot(Vector* dot);
      Vector* normalize();
      void printVector();
	  Vector *add(Vector *);
	  Vector *multiply(Matrix *);
	  Vector *scale(double);
};

#endif
