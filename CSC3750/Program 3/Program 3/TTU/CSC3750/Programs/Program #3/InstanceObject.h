#if !defined (INSTANCEOBJECT_H)
#define INSTANCEOBJECT_H

#include "BasicObject.h"
#include "Matrix.h"
#include "Node.h"

class InstanceObject : public Node
{
private:
	BasicObject* obj;
	//Matrix* transform;

public:
	InstanceObject(BasicObject*);
	~InstanceObject();
	void buildTransform(Matrix*);
	void render(Pixel*, Matrix*);
	void printTransform();
};

#endif
