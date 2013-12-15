#include "InstanceObject.h"
#include "BasicObject.h"
#include "Matrix.h"
#include <stdio.h>

InstanceObject::InstanceObject(BasicObject* inobj) : Node()
{
	/*Matrix* temp = new Matrix(4, 4);
	temp->setElement(1, 1, 1);
	temp->setElement(2, 2, 1);
	temp->setElement(3, 3, 1);
	temp->setElement(4, 4, 1);

	transform = temp;*/
	obj = inobj;

	obj->addRef();
}

InstanceObject::~InstanceObject()
{
	printf("\ndeleting in IO");
	obj->removeRef();
}

void InstanceObject::buildTransform(Matrix* mat)
{
	Matrix* temp = mat->multiply(transform);
	/*printf("\nIn matrix\n");
	mat->printMatrix();
	printf("\nCurrent Matrix\n");
	transform->printMatrix();
	printf("\nResult\n");
	temp->printMatrix();*/
	transform = temp;
}

void InstanceObject::render(Pixel *pix, Matrix* windowTransform)
{	
	obj->render(pix, windowTransform->multiply(transform));
}

void InstanceObject::printTransform()
{
	transform->printMatrix();
	printf("\n");
}

