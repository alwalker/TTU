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
	  Vector *add(Vector *);
	  double dotProduct(Vector *);
	  Vector *crossProduct(Vector *);
	  Vector *normalize();
	  static Vector *PPSubtraction(double, double, double, double, double, double);
      void printVector();
};

#endif
