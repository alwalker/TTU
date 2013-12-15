#include "BasicObject.h"
#include <stdio.h>

BasicObject::BasicObject()
{
   faces = new List<Face>();
   vertices = new List<Vertex>();
   sze = 0;
}

BasicObject::~BasicObject()
{
   //clean up all of the basic object heap-allocated memory
   ListIterator<Face>* faceIter = faces->iterator();
   while (faceIter->hasNext())
   {
      Face* face = faceIter->next();
      delete face;
   }

   ListIterator<Vertex>* vertexIter = vertices->iterator();
   while(vertexIter->hasNext())
   {
      Vertex* vertex = vertexIter->next();
      delete vertex;  
   }

   delete faceIter;
   delete faces;
   delete vertexIter;
   delete vertices;
}

void BasicObject::addVertex(Vertex* vertex)
{
   vertices->add(vertex);
}

void BasicObject::addFace(Face* face)
{
   faces->add(sze + 1, face);
   sze++;
}

Face* BasicObject::getFace(int index)
{
   return faces->get(index);
}

void BasicObject::render(Pixel* px, Matrix* transform, Color *color, Matrix *zBuffer)
{
	int count = 0;
   ListIterator<Face>* faceIter = faces->iterator();

   while (faceIter->hasNext())
   {
	   /*printf("\nface %d:\n", count+1);
	   if(count == 5)
	   {
		   return;
	   }*/
      Face* face = faceIter->next();
      face->render(px, transform, color, zBuffer);
	  count++;
   }

   delete faceIter;
}
