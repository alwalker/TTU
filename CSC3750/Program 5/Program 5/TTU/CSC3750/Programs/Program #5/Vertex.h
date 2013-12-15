#if !defined (VERTEX_H)
#define VERTEX_H

#include "Color.h"
#include "Vector.h"
#include "List.h"

class Vertex
{
   private:
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
      void printVertex();
};

#endif
