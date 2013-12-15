#include "TransformNode.h"
#include "ListIterator.h"
#include <stdio.h>

TransformNode::TransformNode(Matrix* mat)
{
	transform = mat;
	nodes = new List<Node>();
}

TransformNode::~TransformNode()
{
	Node* temp;	

	ListIterator<Node>* iter = nodes->iterator();
	printf("\nDeleting transform node - start");
	while(iter->hasNext())
	{		
		temp = iter->next();
		printf("\nin loop temp: %p", temp);
		if(temp != NULL)
		{
			delete temp;
			temp = NULL;
		}
	}

	delete iter;
	delete nodes;

	printf("\nDeleting transform node - end");
}

void TransformNode::render(Pixel* pix, Matrix* mat)
{
	ListIterator<Node>* iter = nodes->iterator();

	while(iter->hasNext())
	{
		iter->next()->render(pix, mat->multiply(transform));
	}

	delete iter;
}

void TransformNode::addNode(Node* node)
{
	nodes->add(node);
}
