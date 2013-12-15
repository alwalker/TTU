#include "List.h"
#include "Pixel.h"
#include "Matrix.h"
#include "Scene.h"
#include <stdio.h>

Scene::Scene(Matrix* mat)
{
	windowMatrix = mat;
	Nodes = new List<Node>();
}

Scene::~Scene()
{
	printf("\ndeleting in scene - start");
	ListIterator<Node>* iter = Nodes->iterator();
	while(iter->hasNext())
	{
		iter->next()->removeRef();
	}

	delete iter;
	delete Nodes;
	delete windowMatrix;
	printf("\ndeleting in scene - end");
}

void Scene::addNode(Node* tNode)
{
	Nodes->add(tNode);
	tNode->addRef();
}

void Scene::render(Pixel* pix)
{
	ListIterator<Node>* iter = Nodes->iterator();

	while(iter->hasNext())
	{
		iter->next()->render(pix, windowMatrix);
	}

	delete iter;
}
