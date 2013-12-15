#include "InstanceObject.h"
#include "BasicObject.h"
#include "Matrix.h"
#include <stdio.h>

InstanceObject::InstanceObject(BasicObject* inobj)
{
	Matrix* temp = new Matrix(4, 4);
	temp->setElement(1, 1, 1);
	temp->setElement(2, 2, 1);
	temp->setElement(3, 3, 1);
	temp->setElement(4, 4, 1);

	transform = temp;
	obj = inobj;

	obj->addRef();
}

InstanceObject::~InstanceObject()
{
	delete transform;
	obj->removeRef();
}

void InstanceObject::buildTransform(Matrix* mat)
{
	Matrix* temp = mat->multiply(transform);
	transform = temp;
}

void InstanceObject::render(Pixel *pix, Matrix* windowTransform)
{
	Matrix *temp = windowTransform->multiply(transform);
	obj->render(pix, temp);
	delete temp;
}

void InstanceObject::printTransform()
{
	transform->printMatrix();
	printf("\n");
}

