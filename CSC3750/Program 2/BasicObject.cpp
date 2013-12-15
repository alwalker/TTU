#include "BasicObject.h"
#include "Pixel.h"
#include "stdio.h"

BasicObject::BasicObject()
{
}

BasicObject::~BasicObject()
{
	ListIterator<Face>* iter2 = faces.iterator();
	while(iter2->hasNext())
	{
		delete iter2->next();
	}

	ListIterator<Vertex>* iter = vertices.iterator();
	while(iter->hasNext())
	{
		delete iter->next();
	}	

	delete iter;
	delete iter2;
	printf("\nDeleting\n");
}

void BasicObject::addFace(Face* fc)
{
	faces.add(fc);
}

void BasicObject::addVertex(Vertex* v)
{
	vertices.add(v);
}

void BasicObject::render(Pixel* pix, Matrix* transformMatrix)
{
	int count = 1;
	ListIterator<Face>* iter = faces.iterator();

	while(iter->hasNext())
	{
		//printf("\nPrinting face #%d", count);
		count++;
		iter->next()->render(pix, transformMatrix);
	}

	delete iter;
}

void BasicObject::printFaces()
{
	int count = 0;
	ListIterator<Face>* iter = faces.iterator();

	while(iter->hasNext())
	{
		count++;
		printf("face %d:\n", count);
		iter->next()->printFace();
	}

	delete iter;
}
