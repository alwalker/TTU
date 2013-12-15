#include "InstanceObject.h"

InstanceObject::InstanceObject(BasicObject* bo, Color *clr)
{
	material = clr;
   obj = bo;
   obj->addRef();
}

InstanceObject::~InstanceObject()
{
   obj->removeRef();
}

void InstanceObject::buildTransform(Matrix* matrix)
{
   //multiple calls to this method will achieve the proper concatenation (later matrices are on the left)
   Matrix* temp = matrix->multiply(transform);
   delete transform;
   delete matrix;
   transform = temp;
}

void InstanceObject::render(Pixel* px, Matrix* laterTransform, Matrix *zBuffer)
{
   Matrix* updatedMatrix = laterTransform->multiply(transform);
   obj->render(px, updatedMatrix, material, zBuffer);
   delete updatedMatrix;
}
