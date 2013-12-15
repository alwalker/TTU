#include "Scene.h"

Scene::Scene(Light *light, Color *color)
{
	pointLight = light;
   sceneTransform = Matrix::getIdentityMatrix(4);
   transformNodes = new List<TransformNode>();
   ambientLight = color;
}

Scene::~Scene()
{
   //clean up all of the basic object heap-allocated memory
   ListIterator<TransformNode>* ioIter = transformNodes->iterator();
   while (ioIter->hasNext())
   {
      TransformNode* tn = ioIter->next();
      tn->removeRef();
   }

   delete ioIter;
   delete transformNodes;
   delete sceneTransform;
}

void Scene::buildTransform(Matrix* mat)
{
   Matrix* temp = mat->multiply(sceneTransform);
   delete sceneTransform;
   delete mat;
   sceneTransform = temp;
}

void Scene::addTransformNode(TransformNode* tn)
{
   transformNodes->add(tn);
   tn->addRef();
}

void Scene::render(Pixel* px, Vertex *eyePoint, double attenuation)
{
   //create the zbuffer matrix
   int width = px->getWidth();
   int height = px->getHeight();
   Matrix* zbuffer = new Matrix(height, width);    //rows come first

   //populate zbuffer with -1
   for (int i = 1; i <= height; i++)
   {
      for (int j = 1; j <= width; j++)
      {
         zbuffer->setElement(i, j, -1.0);
      }
   }
   
   //loop over all the Instance instances in the Scene and render them
   ListIterator<TransformNode>* ioIter = transformNodes->iterator();
   while (ioIter->hasNext())
   {
      TransformNode* tn = ioIter->next();
      tn->render(px, sceneTransform, zbuffer, ambientLight, pointLight, eyePoint, attenuation);
   }

   delete ioIter;
   delete zbuffer;
}
