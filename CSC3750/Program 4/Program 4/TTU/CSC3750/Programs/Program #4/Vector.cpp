#include "Vector.h"
#include <Math.h>
#include <stdio.h>

Vector::Vector(double x, double y, double z)
{
   vector = new Matrix(4, 1);
   vector->setElement(1, 1, x);
   vector->setElement(2, 1, y);
   vector->setElement(3, 1, z);
   vector->setElement(4, 1, 0.0);  //4th coordinate 0 for vectors
}

Vector::~Vector()
{
   delete vector;
}

void Vector::setX(double x)
{
   vector->setElement(1, 1, x);
}

void Vector::setY(double y)
{
   vector->setElement(2, 1, y);
}

void Vector::setZ(double z)
{
   vector->setElement(3, 1, z);
}

double Vector::getX()
{
   return vector->getElement(1, 1);
}

double Vector::getY()
{
   return vector->getElement(2, 1);
}

double Vector::getZ()
{
   return vector->getElement(3, 1);
}

void Vector::printVector()
{
   vector->printMatrix();
}

double Vector::dotProduct(Vector *otherVector)
{
	double dotProduct = getX() * otherVector->getX();
	dotProduct = dotProduct + (getY() * otherVector->getY());
	dotProduct = dotProduct + (getZ() * otherVector->getZ());

	return dotProduct;
}

Vector *Vector::crossProduct(Vector *otherVector)
{
	double x, y, z;
	Vector *crossProduct;
	
	x = (getY()*otherVector->getZ()) - (getZ()*otherVector->getY());
	y = (getZ()*otherVector->getX()) - (getX()*otherVector->getZ());
	z = (getX()*otherVector->getY()) - (getY()*otherVector->getX());

	crossProduct = new Vector(x, y, z);

	return crossProduct;
}

Vector *Vector::normalize()
{
	double length;
	Vector *normalized;

	length = getX() * getX();
	length = length + (getY() * getY());
	length = length + (getZ() * getZ());
	length = sqrt(length);
	
	normalized = new Vector(getX()/length, getY()/length, getZ()/length);

	return normalized;
}

Vector *Vector::PPSubtraction(double x1, double y1, double z1, double x2, double y2, double z2)
{
	Vector *v;
	double x, y, z;

	x = x2 - x1;
	y = y2 - y1;
	z = z2 - z1;

	v = new Vector(x, y, z);

	return v;
}

Vector *Vector::add(Vector *otherVector)
{
	Vector *newVector;
	double x, y, z;

	x = getX() + otherVector->getX();
	y = getY() + otherVector->getY();
	z = getZ() + otherVector->getZ();

	newVector = new Vector(x, y, z);

	return newVector;
}
