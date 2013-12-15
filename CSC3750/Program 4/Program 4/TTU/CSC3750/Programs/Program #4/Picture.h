#if !defined (PICTURE_H)
#define PICTURE_H

#include "InstanceObject.h"
#include "BasicObject.h"
#include "Pixel.h"
#include "Vector.h"

class Picture
{
   public:
      static void render(Pixel* pixel);
      static BasicObject* readObject(char* fileName);
      static InstanceObject* buildInstanceObject(char* fileName, BasicObject* obj);
	  static Vector *readCamera(double *, double *, double *, double *, double *, double *);
	  static void readFOV(double *, double *, double *);

   private:
};

#endif
