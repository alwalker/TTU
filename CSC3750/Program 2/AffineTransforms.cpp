#include "AffineTransforms.h"
#include <Math.h>
#include <stdio.h>

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

Matrix* AffineTransforms::getWindowTransform(double x, double y)
{
	double a = y/x;
	Matrix* transform;

	Matrix* windowMatrix = new Matrix(4, 4);
	windowMatrix->setElement(1, 1, x/2);
	windowMatrix->setElement(2, 2, (-1 * y)/2);
	windowMatrix->setElement(3, 3, 1);
	windowMatrix->setElement(4, 4, 1);

	windowMatrix->setElement(1, 4, (x-1)/2);
	windowMatrix->setElement(2, 4, (y-1)/2);
	windowMatrix->setElement(3, 4, 1);

	Matrix* aspectRatioMatrix = new Matrix(4, 4);
	aspectRatioMatrix->setElement(1, 1, 1);
	aspectRatioMatrix->setElement(2, 2, 1/a); //2/2*a
	aspectRatioMatrix->setElement(3, 3, 1);
	aspectRatioMatrix->setElement(4, 4, 1);

	transform = windowMatrix->multiply(aspectRatioMatrix);
	delete windowMatrix;
	delete aspectRatioMatrix;

	return transform;
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
