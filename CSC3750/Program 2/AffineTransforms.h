#if !defined (AFFINETRANSFORMS_H)
#define AFFINETRANSFORMS_H

#include "Matrix.h"

class AffineTransforms
{
   private:

   public:
      static Matrix* scale(double x, double y, double z);
	  static Matrix* getWindowTransform(double x, double y);
	  static Matrix* translate(double, double, double);
	  static Matrix* rotateX(double);
	  static Matrix* rotateY(double);
	  static Matrix* rotateZ(double);
};

#endif
