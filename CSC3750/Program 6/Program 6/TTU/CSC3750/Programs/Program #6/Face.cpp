#include "Face.h"
#include <math.h>
#include <iostream>
#include <stdio.h>
using namespace std;

Face::Face()
{
   vertices = new List<Vertex>();
   sze = 0;
}

Face::~Face()
{
   delete vertices;
}

void Face::addVertex(Vertex* vertex)
{
   if (sze < 3)
   {
      vertices->add(sze + 1, vertex);
      sze++;
	  vertex->addFace(this);
   }
}

Vertex* Face::getVertex(int index)
{
   return vertices->get(index);
}

Vector* Face::getNormal(Vertex* v1, Vertex* v2, Vertex* v3)
{
   Vector* v2minusv1 = v2->subtract(v1);
   Vector* v3minusv1 = v3->subtract(v1);
   Vector* temp = v2minusv1->cross(v3minusv1);
   Vector* n = temp->normalize();
   delete v2minusv1;
   delete v3minusv1;
   delete temp;
   return n;
}

Vector* Face::getUnNormal(Vertex* v1, Vertex* v2, Vertex* v3)
{
   Vector *v2minusv1 = v2->subtract(v1);
   Vector *v3minusv1 = v3->subtract(v1);
   Vector *n = v2minusv1->cross(v3minusv1);

   delete v2minusv1;
   delete v3minusv1;
   return n;
}

Vector *Face::getGouraudNormal(Vertex *v)
{
	int count = 0;
	Vector *n, *temp1, *temp2;
	Face *face;
	List<Face> *faces = v->getFaces();
	ListIterator<Face> *faceIter = faces->iterator();
	n = new Vector(0, 0, 0);

	while(faceIter->hasNext())
	{
		count++;
		face = faceIter->next();
		temp1 = getUnNormal(face->getVertex(1), face->getVertex(2), face->getVertex(3));
		temp2 = n->add(temp1);
		delete temp1;
		delete n;
		n = temp2;
	}

	temp1 = n->scale(1.0/count);
	delete n;
	n = temp1;

	delete faceIter;	
	return n;
}

void Face::render(Pixel* px, Matrix *WNAC, Matrix *I, Color* diffuse, Matrix* zbuffer, Color *ambient, Light *pointLight, Vertex *eyePoint, double attenuation, double shininess)
{	
	//cout<<"WNAC"<<endl;
	//WNAC->printMatrix();
	//cout<<"I"<<endl;
	//I->printMatrix();
	//cout<<"Color"<<endl;
	////diffuse->printColor();
	//cout<<"Light color"<<endl;
	////light->getColor()->printColor();
	//cout<<"Light pos"<<endl;
	//pointLight->getLocation()->printVertex();
	//cout<<"attenuation: "<<attenuation<<"\tshininess:"<<shininess;

	Color *v1Color, *v2Color, *v3Color;
	Vector *tempV;
	Vertex *temp, *Iv1, *Iv2, *Iv3;
	Vertex* v1 = getVertex(1);
	Vertex* v2 = getVertex(2);
	Vertex* v3 = getVertex(3);

	//put vertices into world coordinates 
	Iv1 = v1->multiply(I);
	Iv2 = v2->multiply(I);
	Iv3 = v3->multiply(I);

   	//Finish transformation
	Matrix *temp2 = WNAC->multiply(I);
	temp = v1->multiply(temp2);
	v1 = temp;
	temp = v2->multiply(temp2);
	v2 = temp;
	temp = v3->multiply(temp2);  
	v3 = temp;
	delete temp2;

	temp = v1->homogenize();
	delete v1;
	v1 = temp;
	temp = v2->homogenize();
	delete v2;
	v2 = temp;
	temp = v3->homogenize();
	delete v3;
	v3 = temp;

	//Normal for back face culling
	Vector* n = getNormal(v1, v2, v3);

	//Get proper transform matrix for vectors
	Matrix *VI = I->inverse();
	temp2 = VI->transpose();
	delete VI;
	VI = temp2;

	//Normal for interpolated shading algorithm
	Vector *in = getUnNormal(getVertex(1), getVertex(2), getVertex(3));
	tempV = in->multiply(VI);
	delete in;
	in = tempV;
	tempV = in->normalize();
	delete in;
	in = tempV;

	//Normals for Gouraud shading
	Vector *in1 = getGouraudNormal(getVertex(1));
	tempV = in1->multiply(VI);
	delete in1;
	in1 = tempV;
	tempV = in1->normalize();
	delete in1;
	in1 = tempV;

	Vector *in2 = getGouraudNormal(getVertex(2));
	tempV = in2->multiply(VI);
	delete in2;
	in2 = tempV;
	tempV = in2->normalize();
	delete in2;
	in2 = tempV;

	Vector *in3 = getGouraudNormal(getVertex(3));
	tempV = in3->multiply(VI);
	delete in3;
	in3 = tempV;
	tempV = in3->normalize();
	delete in3;
	in3 = tempV;

	//back face cullinge
	if (n->getZ() < 0)  //discard positive normals due to windowing transform
	{
		//Get the new vertex colors from shading algorithm
		v1Color = getInterpolatedColor(Iv1, eyePoint, pointLight, diffuse, ambient, in1, shininess, attenuation);
		v2Color = getInterpolatedColor(Iv2, eyePoint, pointLight, diffuse, ambient, in2, shininess, attenuation);
		v3Color = getInterpolatedColor(Iv3, eyePoint, pointLight, diffuse, ambient, in3, shininess, attenuation);

		//set the diffuse color from the instance object material
		v1->setRed(v1Color->getRed());
		v2->setRed(v2Color->getRed());
		v3->setRed(v3Color->getRed());

		v1->setGreen(v1Color->getGreen());
		v2->setGreen(v2Color->getGreen());
		v3->setGreen(v3Color->getGreen());

		v1->setBlue(v1Color->getBlue());
		v2->setBlue(v2Color->getBlue());
		v3->setBlue(v3Color->getBlue());


		//check color interpolation
		/*v1->setRed(1);
		v2->setRed(0);
		v3->setRed(0);

		v1->setGreen(0);
		v2->setGreen(1);
		v3->setGreen(0);

		v1->setBlue(0);
		v2->setBlue(0);
		v3->setBlue(1);*/


		drawTriangle(v1, v2, v3, zbuffer, px);
		delete v1Color;
		delete v2Color;
		delete v3Color;
   }

   delete v1;
   delete v2;
   delete v3;
   delete n;
   delete Iv1;
   delete Iv2;
   delete Iv3;
   delete in;
   delete in1;
   delete in2;
   delete in3;
   delete VI;
}

Color* Face::getInterpolatedColor(Vertex *v, Vertex *e, Light *light, Color *diffuse, Color *ambient, Vector *n, double alpha, double attenuation)
{
	/*cout<<"in method"<<endl;
	cout<<"v:"<<endl;
	v->printVertex();
	cout<<"eye point: "<<endl;
	e->printVertex();
	cout<<"normal: "<<endl;
	n->printVector();
	cout<<"alpha: "<<alpha<<"\tattenuation: "<<attenuation<<endl;*/

	double xd, yd, zd, xs, ys, zs, d, lDotn, rDotv, denominator;
	Vector *l, *r, *minusL, *nScale, *vn, *temp;
	Color *result;

	//Compute L
	l = light->getLocation()->subtract(v);
	//Compute d
	d = sqrt(l->dot(l));
	temp = l->normalize();
	delete l;
	l = temp;  

	//Compute v
	vn = e->subtract(v);
	temp = vn->normalize();
	delete vn;
	vn = temp;

	//Compute r
	minusL = l->scale(-1);
	nScale = n->scale(2 * l->dot(n));
	r = minusL->add(nScale);

	lDotn = l->dot(n);
	if(lDotn < 0)
	{
	   lDotn = 0;
	}

	rDotv = r->dot(vn);
	if(rDotv < 0)
	{
		rDotv = 0;
	}
	rDotv = pow(rDotv, alpha);

	denominator = d * attenuation;
	//cout<<"d: "<<d<<endl<<"attenuation: "<<attenuation<<endl<<"denominator: "<<denominator<<endl;

	//Compute diffuse term	
	xd = diffuse->getRed()*light->getColor()->getRed()*lDotn;
	xd = xd/denominator;
	yd = diffuse->getGreen()*light->getColor()->getGreen()*lDotn;
	yd = yd/denominator;
	zd = diffuse->getBlue()*light->getColor()->getBlue()*lDotn;
	zd = zd/denominator;

	//Compute specular term
	xs = light->getColor()->getRed()*rDotv;
	xs = xs/denominator;
	ys = light->getColor()->getGreen()*rDotv;
	ys = ys/denominator;
	zs = light->getColor()->getBlue()*rDotv;
	zs = zs/denominator;

	delete l;
	delete r;
	delete minusL;
	delete nScale;
	delete vn;

	result = new Color(xd+xs+ambient->getRed(), yd+ys+ambient->getGreen(), zd+zs+ambient->getBlue());
	return result;
}

void Face::drawTriangle(Vertex* first, Vertex* second, Vertex* third, Matrix* zbuffer, Pixel* px)
{
   double x0, x1, x2, y0, y1, y2, z0, z1, z2;
   x0 = first->getX();   //point a
   y0 = first->getY();  
   z0 = first->getZ();
   x1 = second->getX();  //point b
   y1 = second->getY();
   z1 = second->getZ();
   x2 = third->getX();   //point c
   y2 = third->getY();
   z2 = third->getZ();

   double r0, g0, b0, r1, g1, b1, r2, g2, b2;

   r0 = first->getRed();
   g0 = first->getGreen();
   b0 = first->getBlue();
   r1 = second->getRed();
   g1 = second->getGreen();
   b1 = second->getBlue();
   r2 = third->getRed();
   g2 = third->getGreen();
   b2 = third->getBlue();

   //find the bounding rectangle
   double xmax = x0;
   double xmin = x0;
   double ymax = y0;
   double ymin = y0;

   if (x1 > xmax)
   {
      xmax = x1;
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
   }

   //the following clipping based on an idea from Roger Shelton
   //I need the clipping planes in screen coords and I can get them from the z buffer
   double maxX = zbuffer->getNumColumns() - 1 + 0.5;  //use these values to clip in x and y
   double maxY = zbuffer->getNumRows() - 1 + 0.5;

   double minX = -0.5;
   double minY = -0.5;

   //check to see if the face is completely clipped out
   if (xmax < minX)  
   {
      return;
   }
   if (xmin > maxX)
   {
      return;
   }
   if (ymax < minY)
   {
      return;
   }
   if (ymin > maxY)
   {
      return;
   }

   //set clipping
   if (xmin < minX)
   {
      xmin = minX;
   }
   if (xmax > maxX)
   {
      xmax = maxX;
   }
   if (ymin < minY)
   {
      ymin = minY;
   }
   if (ymax > maxY)
   {
      ymax = maxY;
   }

   double bx  = x1 - x0;
   double by  = y1 - y0;
   double cx  = x2 - x0;
   double cy  = y2 - y0;

   double dr1 = r1 - r0;
   double dr2 = r2 - r0;
   double dg1 = g1 - g0;
   double dg2 = g2 - g0;
   double db1 = b1 - b0;
   double db2 = b2 - b0;

   double dz1 = z1 - z0;
   double dz2 = z2 - z0;

   double denom = 1.0/((double)(cy*bx - by*cx));

   double bx_d = bx*denom;
   double by_d = by*denom;
   double cx_d = cx*denom;
   double cy_d = cy*denom;

   double beta_incr_x  =  cy_d;
   double gamma_incr_x = -by_d;

   double beta_incr_y  = -cx_d;
   double gamma_incr_y =  bx_d;

   double x_start = ((int) xmin - x0);
   double y_start = ((int) ymin - y0);

   double beta_start  = cy_d*x_start - cx_d*y_start;  
   double gamma_start = bx_d*y_start - by_d*x_start;   

   double r_start  = r0 + beta_start*dr1 + gamma_start*dr2;
   double g_start  = g0 + beta_start*dg1 + gamma_start*dg2;
   double b_start  = b0 + beta_start*db1 + gamma_start*db2;
   double z_start  = z0 + beta_start*dz1 + gamma_start*dz2;

   double r_incr_x = beta_incr_x*dr1 + gamma_incr_x*dr2;
   double g_incr_x = beta_incr_x*dg1 + gamma_incr_x*dg2;
   double b_incr_x = beta_incr_x*db1 + gamma_incr_x*db2;
   double z_incr_x = beta_incr_x*dz1 + gamma_incr_x*dz2;

   double r_incr_y = beta_incr_y*dr1 + gamma_incr_y*dr2;
   double g_incr_y = beta_incr_y*dg1 + gamma_incr_y*dg2;
   double b_incr_y = beta_incr_y*db1 + gamma_incr_y*db2;
   double z_incr_y = beta_incr_y*dz1 + gamma_incr_y*dz2;

   //loop over the bounding rectangle for the triangle only
   for (int x = (int) xmin; x <= (int) xmax; x++) 
   {             
      //reset beta and gamma for the next loop over y
      double beta  = beta_start;
      double gamma = gamma_start;

      double r = r_start;
      double g = g_start;
      double b = b_start;
      double z = z_start;

      for (int y = (int) ymin; y <= (int) ymax; y++)
      {
         //inside the triangle?
         if (beta <= 1 && beta >= 0 && gamma <= 1 && gamma >= 0 && beta + gamma <= 1 )  
         {
            if (z > zbuffer->getElement(y + 1, x + 1) && z < 1)
            {
               zbuffer->setElement(y + 1, x + 1, z);
               Color color(r, g, b);
               px->drawPixel(x, y, color);
            }
         }

         beta  += beta_incr_y;
         gamma += gamma_incr_y;

         r += r_incr_y;
         g += g_incr_y;
         b += b_incr_y;

         z += z_incr_y;
      }

      //incremental changes (efficient)
      beta_start  += beta_incr_x;
      gamma_start += gamma_incr_x;

      r_start += r_incr_x;
      g_start += g_incr_x;
      b_start += b_incr_x;

      z_start += z_incr_x;
   }
}

void Face::printFace()
{
   for (int i = 1; i <= sze; i++)
   {
      Vertex* vertex = getVertex(i);
      vertex->printVertex();
   }
}
