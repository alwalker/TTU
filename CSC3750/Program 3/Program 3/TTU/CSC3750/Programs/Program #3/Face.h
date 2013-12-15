#if !defined (FACE_H)
#define FACE_H

#include "Vertex.h"
#include "Pixel.h"
#include "List.h"

class Face
{
   private:
	  List<Vertex> vertices; 
	  void renderDDA(Pixel*, double, double, double, double);
	  void renderBresenham(Pixel*, double, double, double, double);
   public:
     Face();
     Face(Vertex*, Vertex*, Vertex*);
     ~Face();
	void addVertex(Vertex* v);
	Vertex* getV1();
	Vertex* getV2();
	Vertex* getV3();
    void printFace();
	void render(Pixel*, Matrix*);
};

#endif

