#if !defined (BASICOBJECT_H)
#define BASICOBJECT_H

#include <math.h>
#include "Face.h"
#include "Node.h"
#include "Light.h"
#include "Texture.h"

class BasicObject : public DeleteObject
{
   private:
      List<Face>* faces;
      List<Vertex>* vertices;
      int sze;

   public:
      BasicObject();
      virtual ~BasicObject();
      void addFace(Face* face);
      void addVertex(Vertex* vertex);
      Face* getFace(int index);
      void render(Pixel* px, Matrix*, Matrix* transform, Color* diffuse, Matrix* zbuffer, Color*, Light*, Vertex*, double, double, Texture *tx);
	  void printFaces();
	  void computeSandT();
};

#endif
