#if !defined (SCENE_H)
#define SCENE_H

#include "Matrix.h"
#include "Pixel.h"
#include "TransformNode.h"

class Scene
{
   private:
      List<TransformNode>* transformNodes;
      Matrix* sceneTransform;

   public:
      Scene();
      virtual ~Scene();
      void addTransformNode(TransformNode* tn);
      void buildTransform(Matrix* mat);
      void render(Pixel* px);
};

#endif
