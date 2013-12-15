#if !defined (INSTANCEOBJECT_H)
#define INSTANCEOBJECT_H

#include "Pixel.h"
#include "Matrix.h"
#include "BasicObject.h"
#include "Node.h"
#include "Texture.h"

class InstanceObject : public Node
{
   private:
	   double shininess;
      BasicObject* obj;
      Color* diffuse;
	  Texture *tx;

   public:
      InstanceObject(BasicObject* bo, Texture *tx);
      virtual ~InstanceObject();
      void setDiffuseMaterial(Color* mat);
	  void setShininess(double);
      void buildTransform(Matrix* matrix);
      void render(Pixel* px, Matrix* transform, Matrix* zbuffer, Color*, Light*, Vertex*, double);
	  unsigned char* readTexture(Texture *);
};

#endif
