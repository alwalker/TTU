#include "InstanceObject.h"

InstanceObject::InstanceObject(BasicObject* bo)
{
   obj = bo;
   obj->addRef();
   diffuse = new Color(1.0, 1.0, 1.0);
}

InstanceObject::~InstanceObject()
{
   obj->removeRef();  //several ios can use the same bo, so delete is tricky
   delete diffuse;
}

void InstanceObject::setDiffuseMaterial(Color* mat)
{
   Color* temp = diffuse;
   diffuse = mat;
   delete temp;
}

void InstanceObject::setShininess(double shiny)
{
	shininess = shiny;
}

void InstanceObject::buildTransform(Matrix* matrix)
{
   //multiple calls to this method will achieve the proper concatenation (later matrices are on the left)
   Matrix* temp = matrix->multiply(transform);
   delete transform;
   delete matrix;
   transform = temp;
}

void InstanceObject::render(Pixel* px, Matrix* laterTransform, Matrix* zbuffer, Color *ambient, Light *pointLight, Vertex *eyePoint, double attenuation)
{
	//Change light point to world coordinates
	//Vertex *temp;
	//temp = pointLight->getLocation()->multiply(transform);
	////delete pointLight->getLocation();
	//pointLight->setLocation(temp);
	//perror("rawr");
	//pointLight->getLocation()->printVertex();

   Matrix* updatedMatrix = laterTransform->multiply(transform);
   obj->render(px, laterTransform, transform, diffuse, zbuffer, ambient, pointLight, eyePoint, attenuation, shininess);
   delete updatedMatrix;
}
