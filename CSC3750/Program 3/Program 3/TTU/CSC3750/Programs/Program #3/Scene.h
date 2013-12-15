#if !defined (SCENE_H)
#define SCENE_H

#include "List.h"
#include "Pixel.h"
#include "Matrix.h"
#include "TransformNode.h"

class Scene
{
	private:
		List<Node>* Nodes;
		Matrix* windowMatrix;

	public:
		Scene(Matrix*);
		~Scene();
		void render(Pixel*);
		void addNode(Node* tNode);
};

#endif


