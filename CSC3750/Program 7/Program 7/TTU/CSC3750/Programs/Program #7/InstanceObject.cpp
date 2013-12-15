#include "InstanceObject.h"
#include <fstream>

using namespace std;

InstanceObject::InstanceObject(BasicObject* bo, Texture *txIn)
{
   obj = bo;
   obj->addRef();
   tx = txIn;
   diffuse = new Color(1.0, 1.0, 1.0);
}

InstanceObject::~InstanceObject()
{
   obj->removeRef();  //several ios can use the same bo, so delete is tricky
   delete diffuse;
   delete tx;
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
   Matrix* updatedMatrix = laterTransform->multiply(transform);
   //obj->printFaces();
   obj->render(px, laterTransform, transform, diffuse, zbuffer, ambient, pointLight, eyePoint, attenuation, shininess, tx);
   delete updatedMatrix;
}

unsigned char* InstanceObject::readTexture(Texture* tex)
{
   int width = tex->getWidth();
   int height = tex->getHeight();
   int numColors = width*height*3;
   char* textureFile = tex->getFileName();

   unsigned char* textureColors = new unsigned char[numColors];  

   ifstream infile(textureFile, ios::binary);   
   infile.read((char*)textureColors, numColors);
   infile.close();

   /*for(int i=0 ; i<800*400*3 ; i++)
   {
	   printf("%d", textureColors[i]);
   }*/

   return textureColors;
}
