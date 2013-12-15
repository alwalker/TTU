#if !defined (TRANSFORMNODE_H)
#define TRANSFORMNODE_H

#include "BasicObject.h"
#include "Matrix.h"
#include "List.h"
#include "Node.h"

class TransformNode : public Node
{
private:
	List<Node>* nodes;
	
public:
	TransformNode(Matrix*);
	~TransformNode();
	void render(Pixel*, Matrix*);
	void addNode(Node*);
};

#endif
