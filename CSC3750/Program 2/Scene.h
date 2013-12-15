#if !defined (SCENE_H)
#define SCENE_H

#include "List.h"
#include "Pixel.h"
#include "Matrix.h"
#include "InstanceObject.h"

class Scene
{
	private:
		List<InstanceObject>* ios;
		Matrix* windowMatrix;

	public:
		Scene(Matrix*);
		~Scene();
		void render(Pixel*);
		void addIO(InstanceObject* io);
};

#endif


