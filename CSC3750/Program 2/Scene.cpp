#include "List.h"
#include "Pixel.h"
#include "Matrix.h"
#include "InstanceObject.h"
#include "Scene.h"
#include <stdio.h>

Scene::Scene(Matrix* mat)
{
	windowMatrix = mat;
	ios = new List<InstanceObject>();
}

Scene::~Scene()
{
	ListIterator<InstanceObject>* iter = ios->iterator();
	while(iter->hasNext())
	{
		delete iter->next();
	}

	delete iter;
	printf("\nDeleting\n");
}

void Scene::addIO(InstanceObject* io)
{
	ios->add(io);
}

void Scene::render(Pixel* pix)
{
	ListIterator<InstanceObject>* iter = ios->iterator();

	while(iter->hasNext())
	{
		iter->next()->render(pix, windowMatrix);
	}

	delete iter;
}
