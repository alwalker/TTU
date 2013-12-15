#if !defined (VERTEX_H)
#define VERTEX_H

#include "Color.h"
#include "Vector.h"
#include "List.h"

class Face;
class Vertex
{
   private:
	   List<Face> *faces;
      Matrix* vertex;
      Color* color;
     
   public:
      Vertex(double x, double y, double z);
      virtual ~Vertex();
      void setX(double x);
      void setY(double y);
      void setZ(double z);
      void setH(double h);
      double getX();
      double getY();
      double getZ();
      double getH();
      void setRed(double x);
      void setGreen(double y);
      void setBlue(double z);
      double getRed();
      double getGreen();
      double getBlue();
      Vertex* multiply(Matrix* matrix);
      Vector* subtract(Vertex* vertex);
      Vertex* homogenize();
	  void addFace(Face *);
	  List<Face> *getFaces();
      void printVertex();
};

#endif
