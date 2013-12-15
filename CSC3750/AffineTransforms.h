#if !defined (AFFINETRANSFORMS_H)
#define AFFINETRANSFORMS_H

#include "Matrix.h"

class AffineTransforms
{
   private:

   public:
      static Matrix* scale(double x, double y, double z);
	  static Matrix* getWindowTransform(double x, double y);
};

#endif
