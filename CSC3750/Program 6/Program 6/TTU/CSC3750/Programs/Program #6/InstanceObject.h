#if !defined (INSTANCEOBJECT_H)
#define INSTANCEOBJECT_H

#include "Pixel.h"
#include "Matrix.h"
#include "BasicObject.h"
#include "Node.h"

class InstanceObject : public Node
{
   private:
	   double shininess;
      BasicObject* obj;
      Color* diffuse;

   public:
      InstanceObject(BasicObject* bo);
      virtual ~InstanceObject();
      void setDiffuseMaterial(Color* mat);
	  void setShininess(double);
      void buildTransform(Matrix* matrix);
      void render(Pixel* px, Matrix* transform, Matrix* zbuffer, Color*, Light*, Vertex*, double);
};

#endif
