#if !defined (SCENE_H)
#define SCENE_H

#include "Matrix.h"
#include "Pixel.h"
#include "TransformNode.h"
#include "Light.h"

class Scene
{
   private:
	   Light *pointLight;
	   Color *ambientLight;
      List<TransformNode>* transformNodes;
      Matrix* sceneTransform;

   public:
      Scene(Light*, Color*);
      virtual ~Scene();
      void addTransformNode(TransformNode* tn);
      void buildTransform(Matrix* mat);
      void render(Pixel* px, Vertex*, double);
};

#endif
