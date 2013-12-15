#if !defined (PICTURE_H)
#define PICTURE_H

#include "Pixel.h"
#include "BasicObject.h"
#include "InstanceObject.h"

class Picture
{
   public:
      static void render(Pixel* px);
      static BasicObject* readObject(char* fileName);
      static InstanceObject* buildInstanceObject(char* fileName, BasicObject* obj);
      static Matrix* getCameraTransform(char* fileName);
      static Matrix* getPerspectiveTransform(char* fileName, int width, int height);
	  static void getShaderInfo(Vertex*, Color*, Light*, double*);

   private:
};

#endif
