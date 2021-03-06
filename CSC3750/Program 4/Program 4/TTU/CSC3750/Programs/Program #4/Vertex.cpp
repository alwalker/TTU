#include "Vertex.h"
#include <stdio.h>

Vertex::Vertex(double x, double y, double z)
{
   color = new Color(1, 1, 1);
   vertex = new Matrix(4, 1);

   vertex->setElement(1, 1, x);
   vertex->setElement(2, 1, y);
   vertex->setElement(3, 1, z);
   vertex->setElement(4, 1, 1.0);  //4th coordinate 1 for vertices
}

Vertex::~Vertex()
{
   delete vertex;
   delete color;
}

void Vertex::setX(double x)
{
   vertex->setElement(1, 1, x);
}

void Vertex::setY(double y)
{
   vertex->setElement(2, 1, y);
}

void Vertex::setZ(double z)
{
   vertex->setElement(3, 1, z);
}

void Vertex::setH(double h)
{
   vertex->setElement(4, 1, h);
}

double Vertex::getX()
{
   return vertex->getElement(1, 1);
}

double Vertex::getY()
{
   return vertex->getElement(2, 1);
}

double Vertex::getZ()
{
   return vertex->getElement(3, 1);
}

double Vertex::getH()
{
   return vertex->getElement(4, 1);
}

void Vertex::setRed(double r)
{
   color->setRed(r);
}

void Vertex::setGreen(double g)
{
   color->setGreen(g);
}

void Vertex::setBlue(double b)
{
   color->setBlue(b);
}

double Vertex::getRed()
{
   return color->getRed();
}

double Vertex::getGreen()
{
   return color->getGreen();
}

double Vertex::getBlue()
{
   return color->getBlue();
}

void Vertex::printVertex()
{
   vertex->printMatrix();
}

Vertex* Vertex::multiply(Matrix* otherMatrix)
{
	Matrix* temp = otherMatrix->multiply(vertex);
	Vertex* tempV = new Vertex(temp->getElement(1,1), temp->getElement(2,1), temp->getElement(3,1));
	tempV->setH(temp->getElement(4,1));

	delete temp;
	return tempV;
}

Vertex *Vertex::homogenize()
{
	//printf("\n4th: %f", vertex->getElement(4, 1));
	double x, y, z;
	Vertex *homogenized;

	x = getX()/vertex->getElement(4, 1);
	y = getY()/vertex->getElement(4, 1);
	z = getZ()/vertex->getElement(4, 1);

	homogenized = new Vertex(x, y, z);
	return homogenized;
}
