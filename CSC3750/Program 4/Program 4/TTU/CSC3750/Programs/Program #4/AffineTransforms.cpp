#include "AffineTransforms.h"
#include "Vertex.h"
#include <Math.h>
#include <stdio.h>
#include <iostream>

#define PI 3.14159265

Matrix* AffineTransforms::scale(double x, double y, double z)
{
   Matrix* S = new Matrix(4, 4);

   S->setElement(1,1,x);
   S->setElement(1,2,0);
   S->setElement(1,3,0);
   S->setElement(1,4,0);

   S->setElement(2,1,0);
   S->setElement(2,2,y);
   S->setElement(2,3,0);
   S->setElement(2,4,0);

   S->setElement(3,1,0);
   S->setElement(3,2,0);
   S->setElement(3,3,z);
   S->setElement(3,4,0);

   S->setElement(4,1,0);
   S->setElement(4,2,0);
   S->setElement(4,3,0);
   S->setElement(4,4,1);

   return S;
}

Matrix* AffineTransforms::translate(double x, double y, double z)
{
	Matrix* temp = new Matrix(4, 4);

	temp->setElement(1, 1, 1);
	temp->setElement(2, 2, 1);
	temp->setElement(3, 3, 1);
	temp->setElement(4, 4, 1);

	temp->setElement(1, 4, x);
	temp->setElement(2, 4, y);
	temp->setElement(3, 4, z);

	return temp;
}

Matrix* AffineTransforms::rotateX(double degree)
{
	if(degree < 0)
	{
		degree += 360;
	}

	double radians = degree * (PI/180);
	Matrix* temp = new Matrix(4, 4);

	temp->setElement(1, 1, 1);
	temp->setElement(2, 2, cos(radians));
	temp->setElement(2, 3, -1 * sin(radians));
	temp->setElement(3, 2, sin(radians));
	temp->setElement(3, 3, cos(radians));
	temp->setElement(4, 4, 1);

	return temp;
}

Matrix* AffineTransforms::rotateY(double degree)
{
	if(degree < 0)
	{
		degree += 360;
	}

	double radians = degree * (PI/180);
	Matrix* temp = new Matrix(4, 4);

	temp->setElement(1, 1, cos(radians));
	temp->setElement(1, 3, sin(radians));
	temp->setElement(2, 2, 1);
	temp->setElement(3, 1, -1 * sin(radians));
	temp->setElement(3, 3, cos(radians));
	temp->setElement(4, 4, 1);
	
	return temp;
}

Matrix* AffineTransforms::rotateZ(double degree)
{
	if(degree < 0)
	{
		degree += 360;
	}

	double radians = degree * (PI/180);
	Matrix* temp = new Matrix(4, 4);

	temp->setElement(1, 1, cos(radians));
	temp->setElement(1, 2, -1 * sin(radians));
	temp->setElement(3, 3, 1);
	temp->setElement(2, 1, sin(radians));
	temp->setElement(2, 2, cos(radians));
	temp->setElement(4, 4, 1);

	return temp;
}

Matrix* AffineTransforms::getWindowTransform(double width, double height)
{
	Matrix* windowMatrix = new Matrix(4, 4);

	windowMatrix->setElement(1, 1, width/2);
	windowMatrix->setElement(2, 2, (-1 * height)/2);
	windowMatrix->setElement(3, 3, 1);
	windowMatrix->setElement(4, 4, 1);

	windowMatrix->setElement(1, 4, (width-1)/2);
	windowMatrix->setElement(2, 4, (height-1)/2);
	windowMatrix->setElement(3, 4, 1);

	return windowMatrix;
}

Matrix *AffineTransforms::getAspectTransform(double width, double height, double fov)
{
	double rFOV = (fov/2) * (PI/180);
	double a = height/width;
	double w = tan(rFOV);
	Matrix* aspectRatioMatrix = new Matrix(4, 4);

	aspectRatioMatrix->setElement(1, 1, 1/w);
	aspectRatioMatrix->setElement(2, 2, 1/w*a); 
	aspectRatioMatrix->setElement(3, 3, 1);
	aspectRatioMatrix->setElement(4, 4, 1);

	return aspectRatioMatrix;
}

Matrix *AffineTransforms::getNormalTransform(double zMin, double zMax)
{
	Matrix *normalTransform = new Matrix(4, 4);
	double a, b;

	a = (-1 *(zMax + zMin))/(zMax - zMin);
	b = (2 * zMax * zMin)/(zMax - zMin);

	//printf("\na: %f\tb: %f", a, b);

	normalTransform->setElement(1, 1, 1);
	normalTransform->setElement(2, 2, 1);
	normalTransform->setElement(3, 3, a);
	normalTransform->setElement(3, 4, b);
	normalTransform->setElement(4, 3, -1);

	return normalTransform;
}

Matrix *AffineTransforms::getCameraTransform(double eX, double eY, double eZ, double aX, double aY, double aZ, Vector *vUp)
{
	double numerator, denominator, nScaler, vUpScaler, rc14, rc24, rc34;
	Vector *n = Vector::PPSubtraction(aX, aY, aZ, eX, eY, eZ);
	Vector *nN = n->normalize();
	Vector *vUpN = vUp->normalize();
	Vector *v, *vN, *u, *uN, *scaledN, *scaledVUp;
	Matrix *cameraTransform = new Matrix(4, 4);
	
	delete n;
	delete vUp;

	denominator = vUpN->dotProduct(nN);
	numerator = -1 * denominator;
	denominator = denominator * denominator;
	denominator = 1 - denominator;
	denominator = sqrt(denominator);

	nScaler = numerator/denominator;
	vUpScaler = 1/denominator;

	scaledN = new Vector(nN->getX() * nScaler, nN->getY() * nScaler, nN->getZ() * nScaler);
	scaledVUp = new Vector(vUpN->getX() * vUpScaler, vUpN->getY() * vUpScaler, vUpN->getZ() * vUpScaler);

	delete vUpN;

	v = scaledN->add(scaledVUp);
	vN = v->normalize();

	delete scaledN;
	delete scaledVUp;
	delete v;

	u = vN->crossProduct(nN);
	uN = u->normalize();

	delete u;

	rc14 = -1*(uN->getX() * eX) - (uN->getY() * eY) - (uN->getZ() * eZ);
	rc24 = -1*(vN->getX() * eX) - (vN->getY() * eY) - (vN->getZ() * eZ);
	rc34 = -1*(nN->getX() * eX) - (nN->getY() * eY) - (nN->getZ() * eZ);

	cameraTransform->setElement(1, 1, uN->getX());
	cameraTransform->setElement(1, 2, uN->getY());
	cameraTransform->setElement(1, 3, uN->getZ());

	cameraTransform->setElement(2, 1, vN->getX());
	cameraTransform->setElement(2, 2, vN->getY());
	cameraTransform->setElement(2, 3, vN->getZ());

	cameraTransform->setElement(3, 1, nN->getX());
	cameraTransform->setElement(3, 2, nN->getY());
	cameraTransform->setElement(3, 3, nN->getZ());

	cameraTransform->setElement(1, 4, rc14);
	cameraTransform->setElement(2, 4, rc24);
	cameraTransform->setElement(3, 4, rc34);
	cameraTransform->setElement(4, 4, 1);

	delete vN;
	delete nN;
	delete uN;

	return cameraTransform;	
}
