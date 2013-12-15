#if !defined (PICTURE_H)
#define PICTURE_H

#include "InstanceObject.h"
#include "BasicObject.h"
#include "Pixel.h"

class Picture
{
   public:
      static void render(Pixel* pixel);
      static BasicObject* readObject(char* fileName);
      static InstanceObject* buildInstanceObject(char* fileName, BasicObject* obj);

   private:
};

#endif
