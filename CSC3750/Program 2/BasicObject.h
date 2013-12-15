#if !defined (BASICOBJECT_H)
#define BASICOBJECT_H

#include "Vertex.h"
#include "Face.h"
#include "List.h"
#include "Pixel.h"
#include "DeleteObject.h"

class BasicObject : public DeleteObject
{
   private:
	List<Vertex> vertices;
      List<Face> faces;
     
   public:
      BasicObject();
      virtual ~BasicObject();
      void addFace(Face* fc);
      void printFaces();
      void render(Pixel*, Matrix*);
      void addVertex(Vertex* v);
};

#endif


