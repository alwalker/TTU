#include "Picture.h"
#include "Color.h"
#include "Matrix.h"
#include "FileIO.h"
#include "AffineTransforms.h"
#include "Scene.h"
#include "Vertex.h"
#include "Vector.h"

#include <math.h>
#include <string>
#include <iostream>
using namespace std;

void Picture::render(Pixel* px)
{
	Color *ambient = new Color(1, 1, 1);
	Vertex *eye = new Vertex(0,0,0);
	Light *light = new Light();
	double attenuation = 0;
//while(true)
//{

   char* fileName = "sphere.txt";
   //cout<<"calling read object"<<endl;
   BasicObject* sphere = readObject(fileName);
   sphere->computeSandT();
   //sphere->printFaces();
   //cout<<"called it bitches"<<endl;
   //delete[] fileName;  //mingw appears to delete this automatically

   fileName = "trs.txt";
   InstanceObject* sphereInstance = buildInstanceObject(fileName, sphere);
   //delete[] fileName;

   //obtaining the window transform
   int widthPixels = px->getWidth();  //the dimensions of the panel on which the drawing will occur
   int heightPixels = px->getHeight();

   getShaderInfo(eye, ambient, light, &attenuation);
   Scene* scene = new Scene(light, ambient);
   scene->buildTransform(getCameraTransform("camera.txt"));
   scene->buildTransform(getPerspectiveTransform("fov.txt", widthPixels, heightPixels));
   scene->buildTransform(AffineTransforms::window(widthPixels, heightPixels));


   TransformNode* tn = new TransformNode();
   tn->addChild(sphereInstance);
   scene->addTransformNode(tn);

   //for(;;)
   //{
   scene->render(px, eye, attenuation);
   //}
   delete scene;
//}
}

void Picture::getShaderInfo(Vertex *eye, Color *ambient, Light *light, double *attenuation)
{
	double x, y, z;
	Vertex *lightPOS;
	FileIO* file = new FileIO("camera.txt", 1);
	string line;

	line = file->readLine();
	line = file->readLine();
	eye->setX(atof(line.c_str()));
	line = file->readLine();
	eye->setY(atof(line.c_str()));
	line = file->readLine();
	eye->setZ(atof(line.c_str()));

	delete file;
	file = new FileIO("shade.txt", 1);

	line = file->readLine();
	line = file->readLine();
	x = atof(line.c_str());
	line = file->readLine();
	y = atof(line.c_str());
	line = file->readLine();
	z = atof(line.c_str());
	lightPOS = new Vertex(x, y, z);
	light->setLocation(lightPOS);

	line = file->readLine();
	line = file->readLine();
	ambient->setRed(atof(line.c_str()));
	line = file->readLine();
	ambient->setGreen(atof(line.c_str()));
	line = file->readLine();
	ambient->setBlue(atof(line.c_str()));

	line = file->readLine();
	line = file->readLine();
	*attenuation = atof(line.c_str());

	delete file;
}

BasicObject* Picture::readObject(char* fileName)
{
	int fCount = 1;
   BasicObject* obj = new BasicObject();
   FileIO* file = new FileIO(fileName, ' '); //assumed to be a read if a delimiter is specified

   List<Vertex> vertices;
   double x, y, z;
   string* token;
   int index1, index2, index3, temp; 
   double s1, s2, s3, t1, t2, t3;

   //parsing the complex index information for the triangular face
   //could include a vertex index, a vertex normal index, and texture coord information
   string* str1;
   string* str2;
   string* str3;  
   string str11;
   string str22;
   string str33;

   string* v = new string("v");
   string* f = new string("f");
   string *vt = new string("vt");

   while (!file->eof())
   {
      ListIterator<string>* iter = file->getTokens();
      token = iter->next();

      //all vertices are held in the vertices list so that the faces can be created
      if (*token == *v)  //vertices
      {
         delete token;

         token = iter->next();
         x = atof((*token).c_str());
         delete token;

         token = iter->next();
         y = atof((*token).c_str());
         delete token;

         token = iter->next();
         z = atof((*token).c_str());
         delete token;

         Vertex* vertex = new Vertex(x, y, z);
         vertices.add(vertex);
         obj->addVertex(vertex);
      }
	  else if(*token == *vt) //vertex texture coordinate
	  {
		Face *tempF = obj->getFace(fCount);
		//cout<<"in vt count: "<<fCount<<endl;

		delete token;
		token = iter->next();

		s1 = atof((*token).c_str());
		delete token;
		token = iter->next();

		t1 = atof((*token).c_str());
		delete token;
		token = iter->next();

		s2 = atof((*token).c_str());
		delete token;
		token = iter->next();

		t2 = atof((*token).c_str());
		delete token;
		token = iter->next();

		s3 = atof((*token).c_str());
		delete token;
		token = iter->next();

		t3 = atof((*token).c_str());
		delete token;

		/*cout<<"s1: "<<s1<<" s2: "<<s2<<" s3: "<<s3<<endl<<endl;
		cout<<"t1: "<<t1<<" t2: "<<t2<<" t3: "<<t3<<endl<<endl;*/
		tempF->setSCoords(s1, s2, s3);
		tempF->setTCoords(t1, t2, t3);

		fCount++;
	  }
      else if (*token == *f)  //face definition
      {
         delete token;

         //get the complex index information for the triangle face
         str1 = iter->next();
         str2 = iter->next();
         str3 = iter->next();

         //parse the index information for the vertex index
         temp = str1->find("/");
         str11 = str1->erase(temp, str1->size() - 1);

         temp = str2->find("/");
         str22 = str2->erase(temp, str2->size() - 1);

         temp = str3->find("/");
         str33 = str3->erase(temp, str3->size() - 1);
         char* cp;
         int len;

         cp = new char[10];
         len = str11.length();
         str11.copy(cp, len, 0);
         cp[len] = '\0';
         index1 = atoi(cp);
         delete[] cp;

         cp = new char[10];
         len = str22.length();
         str22.copy(cp, len, 0);
         cp[len] = '\0';
         index2 = atoi(cp);
         delete[] cp;

         cp = new char[10];
         len = str33.length();
         str33.copy(cp, len, 0);
         cp[len] = '\0';
         index3 = atoi(cp);
         delete[] cp;

         Face* face = new Face();

         Vertex* vertex1 = vertices.get(index1);
         Vertex* vertex2 = vertices.get(index2);
         Vertex* vertex3 = vertices.get(index3);

         face->addVertex(vertex1);
         face->addVertex(vertex2);
         face->addVertex(vertex3);

         obj->addFace(face);

         delete str1;
         delete str2;
         delete str3;
      }

      //still need to delete the tokens even if the line is completely disregarded
      else  
      {
         delete token;
         while(iter->hasNext())
         {
            token = iter->next();
            delete token;
         }
      }

      delete iter;
   }

   delete v;
   delete f;
   delete vt;
   delete file;
   return obj;
}

InstanceObject* Picture::buildInstanceObject(char* fileName, BasicObject* obj)
{
	Texture *texture;
	char *txFileName = (char *)calloc(512, sizeof(char));
	int length, width;   
   FileIO* inputFile = new FileIO(fileName, 1);  //for reading

   //scale transformation
   string line = inputFile->readLine();  //skip this line
   line = inputFile->readLine();
   double sx = atof(line.c_str());
   line = inputFile->readLine();
   double sy = atof(line.c_str());
   line = inputFile->readLine();
   double sz = atof(line.c_str());
   Matrix* scale = AffineTransforms::scale(sx, sy, sz);

   //rotation transformations
   line = inputFile->readLine();  //skip this line
   line = inputFile->readLine();
   double rx = atof(line.c_str());
   Matrix* rotX = AffineTransforms::rotateX(rx);
   line = inputFile->readLine();
   double ry = atof(line.c_str());
   Matrix* rotY = AffineTransforms::rotateY(ry);
   line = inputFile->readLine();
   double rz = atof(line.c_str());
   Matrix* rotZ = AffineTransforms::rotateZ(rz);

   //translation transformation
   line = inputFile->readLine();  //skip this line
   line = inputFile->readLine();
   double tx = atof(line.c_str());
   line = inputFile->readLine();
   double ty = atof(line.c_str());
   line = inputFile->readLine();
   double tz = atof(line.c_str());
   Matrix* translate = AffineTransforms::translate(tx, ty, tz);

   //material
   line = inputFile->readLine();  //skip this line
   line = inputFile->readLine();
   double mr = atof(line.c_str());
   line = inputFile->readLine();
   double mg = atof(line.c_str());
   line = inputFile->readLine();
   double mb = atof(line.c_str());
   Color* mat = new Color(mr, mg, mb);   

   //texture
   line = inputFile->readLine();
   line = inputFile->readLine();
   strcpy(txFileName, line.c_str());
   line = inputFile->readLine();
   width = atoi(line.c_str());
   line = inputFile->readLine();
   length = atoi(line.c_str());
   texture = new Texture(txFileName, width, length);
   InstanceObject* io = new InstanceObject(obj, texture);
   texture->setTextureColors(io->readTexture(texture));
   //texture->printTextureColors();
   io->setDiffuseMaterial(mat);

   //standard TRS form
   io->buildTransform(scale);  //deletes the matrix when done
   io->buildTransform(rotX);  
   io->buildTransform(rotY);  
   io->buildTransform(rotZ); 
   io->buildTransform(translate);

   //Get shininess
   FileIO* shaderFile = new FileIO("shade.txt", 1);
   line = shaderFile->readLine();
   line = shaderFile->readLine();
   line = shaderFile->readLine();
   line = shaderFile->readLine();
   line = shaderFile->readLine();
   line = shaderFile->readLine();
   line = shaderFile->readLine();
   line = shaderFile->readLine();
   line = shaderFile->readLine();
   line = shaderFile->readLine();
   line = shaderFile->readLine();
   line = shaderFile->readLine();

   io->setShininess(atof(line.c_str()));

   delete shaderFile;
   delete inputFile;
   return io;
}

Matrix* Picture::getCameraTransform(char* fileName)
{
   FileIO* inputFile = new FileIO(fileName, 1);  //for reading

   //eye point
   string line = inputFile->readLine();  //skip this line
   line = inputFile->readLine();
   double ex = atof(line.c_str());
   line = inputFile->readLine();
   double ey = atof(line.c_str());
   line = inputFile->readLine();
   double ez = atof(line.c_str());

   //at point
   line = inputFile->readLine();  //skip this line
   line = inputFile->readLine();
   double ax = atof(line.c_str());
   line = inputFile->readLine();
   double ay = atof(line.c_str());
   line = inputFile->readLine();
   double az = atof(line.c_str());

   //up vector
   line = inputFile->readLine();  //skip this line
   line = inputFile->readLine();
   double ux = atof(line.c_str());
   line = inputFile->readLine();
   double uy = atof(line.c_str());
   line = inputFile->readLine();
   double uz = atof(line.c_str());

   Vertex* eye = new Vertex(ex, ey, ez);
   Vertex* at = new Vertex(ax, ay, az);
   Vector* up = new Vector(ux, uy, uz);

   Matrix* camera = AffineTransforms::cameraTransform(eye, at, up);

   delete eye;
   delete at;
   delete up;
   delete inputFile;

   return camera;
}

Matrix* Picture::getPerspectiveTransform(char* fileName, int width, int height)
{
   FileIO* inputFile = new FileIO(fileName, 1);  //for reading

   //fov
   string line = inputFile->readLine();  //skip this line
   line = inputFile->readLine();
   double fov = atof(line.c_str());

   double angle = 3.1415927*fov/180.0;  //get the angle in radians
   double xmax = tan(angle/2);           //the width of the camera is determined by its fov
   double ymax = xmax*(height)/(width);  //the height of the camera is determined by the aspect ratio of the panel upon which the image will be rendered

   //zmax
   line = inputFile->readLine();  //skip this line
   line = inputFile->readLine();
   double zmax = atof(line.c_str());

   //zmin
   line = inputFile->readLine();  //skip this line
   line = inputFile->readLine();
   double zmin = atof(line.c_str());

   Matrix* pn = AffineTransforms::perspectiveNorm(xmax, ymax, zmax, zmin);

   delete inputFile;
   return pn;
}
