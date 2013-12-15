#if !defined (TRANSFORMNODE_H)
#define TRANSFORMNODE_H

#include "Node.h"
#include "List.h"
#include "Light.h"

class TransformNode : public Node
{
   private:
      List<Node>* children;
      int sze;

   public:
      TransformNode();
      virtual ~TransformNode();
      void addChild(Node* child);
      void buildTransform(Matrix* matrix);
      void render(Pixel* px, Matrix* later, Matrix* zbuffer, Color*, Light*, Vertex*, double);
      Node* getNode(int index);
};

#endif
