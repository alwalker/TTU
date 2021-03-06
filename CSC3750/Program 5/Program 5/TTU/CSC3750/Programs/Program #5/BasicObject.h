#if !defined (BASICOBJECT_H)
#define BASICOBJECT_H

#include "Face.h"
#include "Node.h"
#include "Color.h"

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
      void render(Pixel* px, Matrix* transform, Color*, Matrix*);
};

#endif
