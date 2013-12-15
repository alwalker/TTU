#include "BasicObject.h"
#include <iostream>

using namespace std;

BasicObject::BasicObject()
{
   faces = new List<Face>();
   vertices = new List<Vertex>();
   sze = 0;
}

BasicObject::~BasicObject()
{
   //clean up all of the basic object heap-allocated memory
   ListIterator<Face>* faceIter = faces->iterator();
   while (faceIter->hasNext())
   {
      Face* face = faceIter->next();
      delete face;
   }

   ListIterator<Vertex>* vertexIter = vertices->iterator();
   while(vertexIter->hasNext())
   {
      Vertex* vertex = vertexIter->next();
      delete vertex;  
   }

   delete faceIter;
   delete faces;
   delete vertexIter;
   delete vertices;
}

void BasicObject::addVertex(Vertex* vertex)
{
   vertices->add(vertex);
}

void BasicObject::addFace(Face* face)
{
   faces->add(sze + 1, face);
   sze++;
}

Face* BasicObject::getFace(int index)
{
   return faces->get(index);
}

void BasicObject::computeSandT()
{
	double PI = 3.1415927;
	double y, row, phi, theta, xmax, ymax;
	double *xs = (double *)calloc(3, sizeof(double));
	double *zs = (double *)calloc(3, sizeof(double));
	double *sCoords = (double *)calloc(3, sizeof(double));
	double *tCoords = (double *)calloc(3, sizeof(double));
	Face *tempF;
	Vertex *tempV;
	ListIterator<Face> *fIter = faces->iterator();

	while(fIter->hasNext())
	{
		tempF = fIter->next();
		for(int i=1 ; i<=3 ; i++)
		{
			tempV = tempF->getVertex(i);
			xs[i-1] = tempV->getX();
			y = tempV->getY();
			zs[i-1] = tempV->getZ();

			row = sqrt(xs[i-1]*xs[i-1] + y*y + zs[i-1]*zs[i-1]);
			theta = acos(y/row);
			phi = atan(xs[i-1]/zs[i-1]);

			/*if(theta<0 || theta>PI)
			{
				cout<<"theta: "<<theta<<endl;
			}
			if(phi<0 || phi>2*PI)
			{
				cout<<" phi: "<<phi<<endl;
			}*/

			tCoords[i-1] = 1.0 - theta/PI;			
			sCoords[i-1] = phi/(2.0*PI);

			//Check quadrants
			if(zs[i-1] >= 0) //Quadrant II and III
			{
				//cout<<"Quadrant II or III"<<endl;
				sCoords[i-1] = sCoords[i-1] + .5;
			}
			else if(xs[i-1]<0 && zs[i-1]<0) //Quadrant IV
			{
				//cout<<"Quadrant IV"<<endl;
				sCoords[i-1] = sCoords[i-1] + 1.0;
			}
			
		}

		/*if (xs[0] > xmax)
		{
		  xmax = xs[1];
		}
		if (x1 < xmin)
		{
		  xmin = x1;
		}
		if (x2 > xmax)
		{
		  xmax = x2;
		}
		if (x2 < xmin)
		{
		  xmin = x2;
		}

		if (y1 > ymax)
		{
		  ymax = y1;
		}
		if (y1 < ymin)
		{
		  ymin = y1;
		}
		if (y2 > ymax)
		{
		  ymax = y2;
		}
		if (y2 < ymin)
		{
		  ymin = y2;
		}*/

		if(zs[0]<0 && zs[1]<0 && zs[2]<0)
		{
			if(xs[0]<0 && xs[1]>0)
			{
				sCoords[1] = sCoords[1] + 1;
			}
			else if(xs[0]<0 && xs[2]>0)
			{
				sCoords[2] = sCoords[2] + 1;
			}
			if(xs[1]<0 && xs[0]>0)
			{
				sCoords[0] = sCoords[0] + 1;
			}
			else if(xs[1]<0 && xs[2]>0)
			{
				sCoords[2] = sCoords[2] + 1;
			}
			if(xs[2]<0 && xs[1]>0)
			{
				sCoords[1] = sCoords[1] + 1;
			}
			else if(xs[2]<0 && xs[0]>0)
			{
				sCoords[0] = sCoords[0] + 1;
			}
		}
			
		

		tempF->setSCoords(sCoords[0], sCoords[1], sCoords[2]);
		tempF->setTCoords(tCoords[0], tCoords[1], tCoords[2]);
	}

	free(sCoords);
	free(tCoords);
	free(xs);
	free(zs);
	delete fIter;
}

void BasicObject::render(Pixel* px, Matrix* transform, Matrix *I, Color* diffuse, Matrix* zbuffer, Color *ambient, Light *pointLight, Vertex *eyePoint, double attenuation, double shininess, Texture *tx)
{
   ListIterator<Face>* faceIter = faces->iterator();

   while (faceIter->hasNext())
   {
      Face* face = faceIter->next();
      face->render(px, transform, I, diffuse, zbuffer, ambient, pointLight, eyePoint, attenuation, shininess, tx);
   }

   delete faceIter;
}

void BasicObject::printFaces()
{
	for(int i=1 ; i<=sze ; i++)
	{
		cout<<"Face #"<<i<<endl;
		getFace(i)->printFace();
	}
}
