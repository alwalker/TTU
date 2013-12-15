#if !defined (FACE_H)
#define FACE_H

#include "Pixel.h"
#include "Vertex.h"
#include "List.h"
#include "Vector.h"
#include "Light.h"
#include "Texture.h"

class Face
{
   private:
      List<Vertex>* vertices;
	  double *sCoords;
	  double *tCoords;
      int sze;

   public:
      Face();
      virtual ~Face();
      void addVertex(Vertex* vertex);
      Vertex* getVertex(int index);
      void render(Pixel* px, Matrix*, Matrix* transform, Color* diffuse, Matrix* zbuffer, Color*, Light*, Vertex*, double, double, Texture*);
      void drawTriangle(Vertex* first, Vertex* second, Vertex* third, Matrix* zbuffer, Pixel* px);
      void printFace();
	  static Vector *getUnNormal(Vertex *, Vertex *, Vertex *);
      static Vector* getNormal(Vertex* v1, Vertex* v2, Vertex* v3);
	  static Vector *getGouraudNormal(Vertex *);
	  Color *getInterpolatedColor(Vertex *, Vertex *, Light *, Color *, Color *, Vector *, double, double);
	  void renderWithTextures(Vertex *, Vertex *, Vertex *, Matrix *, Pixel *, Texture *);
	  Color *getTextureLights(Vertex *, Light *, Vector *);
	  void setSCoords(double, double, double);
	  void setTCoords(double, double, double);
	  void printCoords();
};

#endif
