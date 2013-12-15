#include "AffineTransforms.h"
#include <Math.h>

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
