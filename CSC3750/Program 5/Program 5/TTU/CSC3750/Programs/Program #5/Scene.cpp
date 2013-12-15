#include "Scene.h"

Scene::Scene()
{
   sceneTransform = Matrix::getIdentityMatrix(4);
   transformNodes = new List<TransformNode>();
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

void Scene::render(Pixel* px)
{
	//Create zbuffer
	Matrix *zBuffer = new Matrix(px->getHeight(), px->getWidth());
	for(int i=0 ; i<px->getHeight() ; i++)
	{
		for(int j=0 ; j<px->getWidth() ; j++)
		{
			zBuffer->setElement(i, j, -1);
		}
	}
   //loop over all the Instance instances in the Scene and render them
   ListIterator<TransformNode>* ioIter = transformNodes->iterator();
   while (ioIter->hasNext())
   {
      TransformNode* tn = ioIter->next();
      tn->render(px, sceneTransform, zBuffer);
   }
   delete ioIter;
}
