#if !defined (FACE_H)
#define FACE_H

#include "Pixel.h"
#include "Vertex.h"
#include "List.h"

class Face
{
   private:
      List<Vertex>* vertices;
      int sze;

   public:
      Face();
      virtual ~Face();
      void addVertex(Vertex* vertex);
      Vertex* getVertex(int index);
      void render(Pixel* px, Matrix* transform, Color*, Matrix*);
      void drawLine(Pixel* px, Vertex* v1, Vertex* v2);
	  void drawPolygon(Pixel*, Matrix*, Color*, Vertex*, Vertex*, Vertex*);
      void printFace();
};

#endif
