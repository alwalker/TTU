#include "Node.h"
#include <stdio.h>

Node::Node() : DeleteObject()
{
   transform = Matrix::getIdentityMatrix(4);
}

Node::~Node()
{
	printf("\nDeleting in node");
	delete transform;
}
