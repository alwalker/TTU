#if !defined (INSTANCEOBJECT_H)
#define INSTANCEOBJECT_H

#include "Pixel.h"
#include "Matrix.h"
#include "BasicObject.h"
#include "Node.h"
#include "Color.h"

class InstanceObject : public Node
{
   private:
	   Color *material;
      BasicObject* obj;

   public:
      InstanceObject(BasicObject* bo, Color*);
      virtual ~InstanceObject();
      void buildTransform(Matrix* matrix);
      void render(Pixel* px, Matrix* transform, Matrix*);
};

#endif
