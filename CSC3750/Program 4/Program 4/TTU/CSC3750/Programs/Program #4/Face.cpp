#include "Face.h"
#include "Pixel.h"
#include "stdio.h"
#include "AffineTransforms.h"
#include "Color.h"
#include "Math.h"

Face::Face()
{
}

Face::Face(Vertex* v1, Vertex* v2, Vertex* v3)
{
	vertices.add(v1);
	vertices.add(v2);
	vertices.add(v3);
}

Face::~Face()
{
	
}

Vertex* Face::getV1()
{
	return vertices.get(1);
}

Vertex* Face::getV2()
{
	return vertices.get(2);
}

Vertex* Face::getV3()
{
	return vertices.get(3);
}

void Face::addVertex(Vertex *v)
{
	if(vertices.size() > 3)
	{
		return;
	}
	vertices.add(v);
}

void Face::printFace()
{
	ListIterator<Vertex>* iter = vertices.iterator();

	while(iter->hasNext())
	{
		iter->next()->printVertex();
	}
}

void Face::renderDDA(Pixel* pix, double x1, double x2, double y1, double y2)
{
	double x, y;
	double m = (y2 - y1)/(x2 - x1);
	Color clr = Color(255, 255, 255);

	if(fabs(m) < 1) //Loop over x
	{
		y = y1;

		for(int i=(int)(x1 + .5) ; i<=(int)(x2 + .5) ; i++)
		{
			pix->drawPixel(i, (int)(y + .5), clr);
			y += m;
		}
	}
	else //Loop over y
	{
		x = x1;
		m = (x2 - x1)/(y2 - y1);

		for(int i=(int)(y1 + .5) ; i<=(int)(y2 + .5) ; i++)
		{
			pix->drawPixel((int)(x + .5), i, clr);
			x += m;
		}
	}
}

/*
 * Bresenham algorithm implementation with improvements from lecture
 * 1. Integer operations
 * 2. Eleminate Divisions
 * 3. Compare to 0.
 */
void Face::renderBresenham(Pixel* pix, double x1, double x2, double y1, double y2)
{
	Color clr = Color(255, 255, 255);
	int xCurr = (int)(x1 + .5);
	int xEnd = (int)(x2 + .5);
	int yCurr = (int)(y1 + .5);
	int yEnd = (int)(y2 + .5);
	int dx = (int)(fabs(xCurr - xEnd) + .5); //No division here, change in x and y stored seperatly
	int dy = (int)(fabs(yCurr - yEnd) + .5); //No division here, change in x and y stored seperatly
	double fraction = .5;
	int inc1, inc2; //incrememnts for fractional part
	int stepx =  x2 > x1 ? 1:-1; 
	int stepy =  y2 > y1 ? 1:-1;

	pix->drawPixel(xCurr, yCurr, clr);

	if(dx > dy)//Loop over x
	{
		fraction = (2*dy) - dx; //Integer operations instead of floating point as in DDA
		inc1 = 2 * (dy - dx); //Integer operations instead of floating point as in DDA
		inc2 = 2 * dy; //Integer operations instead of floating point as in DDA

		for(int i=0 ; i<dx ; i++)
		{
			if(fraction >= 0) //Compare to 0
			{
				yCurr += stepy; //Step by one or negative one
				fraction += inc1;
			}
			else
			{
				fraction += inc2;
			}

			xCurr += stepx; //Step by one or negative one
			pix->drawPixel(xCurr, yCurr, clr);
		}
	}
	else //loop over y
	{
		fraction = (2*dx) - dy; //Integer operations instead of floating point as in DDA
		inc1 = 2*(dx - dy); //Integer operations instead of floating point as in DDA
		inc2 = 2*dx; //Integer operations instead of floating point as in DDA

		for(int i=0 ; i<dy ; i++)
		{
			if(fraction >= 0) //Compare to 0
			{
				xCurr += stepx; //Step by one or negative one
				fraction += inc1;
			}
			else
			{
				fraction += inc2;
			}
			
			yCurr += stepy; //Step by one or negative one
			pix->drawPixel(xCurr, yCurr, clr);
		}
	}
}
void Face::render(Pixel* pix, Matrix* transformMatrix)
{	
	ListIterator<Vertex>* iter = vertices.iterator();

	if(vertices.size() != 3)
	{
		printf("\nInvalid face!");
		return;
	}
		
	//Get your vertices in screen coordinates
	Vertex* tv1 = iter->next()->multiply(transformMatrix);
	Vertex* tv3 = iter->next()->multiply(transformMatrix);
	Vertex* tv2 = iter->next()->multiply(transformMatrix);

	//Homogenize vertices 
	Vertex *v1 = tv1->homogenize();
	Vertex *v2 = tv2->homogenize();
	Vertex *v3 = tv3->homogenize();

	//Create all your end points for each line
	double x11 = v1->getX();
	double x12 = v3->getX();
	double y11 = v1->getY();
	double y12 = v3->getY();

	double x21 = v2->getX();
	double x22 = v1->getX();
	double y21 = v2->getY();
	double y22 = v1->getY();

	double x31 = v3->getX();
	double x32 = v2->getX();
	double y31 = v3->getY();
	double y32 = v2->getY();

	//Don't need these any more
	delete v1;
	delete v2;
	delete v3;
	delete tv1;
	delete tv2;
	delete tv3;
	delete iter;

	//Draw your lines
	/*renderDDA(pix, x11, x12, y11, y12);
	renderDDA(pix, x21, x22, y21, y22);
	renderDDA(pix, x31, x32, y31, y32);*/

	renderBresenham(pix, x11, x12, y11, y12);
	renderBresenham(pix, x21, x22, y21, y22);
	renderBresenham(pix, x31, x32, y31, y32);
}


