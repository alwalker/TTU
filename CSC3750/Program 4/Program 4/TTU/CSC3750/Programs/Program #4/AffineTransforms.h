#if !defined (AFFINETRANSFORMS_H)
#define AFFINETRANSFORMS_H

#include "Matrix.h"
#include "Vector.h"

class AffineTransforms
{
   private:

   public:
      static Matrix* scale(double x, double y, double z);
	  static Matrix *getWindowTransform(double, double);
	  static Matrix *getAspectTransform(double, double, double);
	  static Matrix *getNormalTransform(double, double);
	  static Matrix *getCameraTransform(double, double, double, double, double, double, Vector *);
	  static Matrix* translate(double, double, double);
	  static Matrix* rotateX(double);
	  static Matrix* rotateY(double);
	  static Matrix* rotateZ(double);
};

#endif
