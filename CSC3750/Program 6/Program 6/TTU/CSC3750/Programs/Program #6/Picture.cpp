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

   //char* fileName = "sphere.txt";
   //BasicObject* sphere = readObject(fileName);
   ////delete[] fileName;  //mingw appears to delete this automatically

   //fileName = "trs.txt";
   //InstanceObject* sphereInstance = buildInstanceObject(fileName, sphere);
   ////delete[] fileName;

   //obtaining the window transform
   int widthPixels = px->getWidth();  //the dimensions of the panel on which the drawing will occur
   int heightPixels = px->getHeight();

   getShaderInfo(eye, ambient, light, &attenuation);
   Scene* scene = new Scene(light, ambient);
   scene->buildTransform(getCameraTransform("camera.txt"));
   scene->buildTransform(getPerspectiveTransform("fov.txt", widthPixels, heightPixels));
   scene->buildTransform(AffineTransforms::window(widthPixels, heightPixels));

	BasicObject* cube = readObject("cube.txt");
	BasicObject* cylinder = readObject("cylinder.txt");

	InstanceObject* upperArm = new InstanceObject(cube);
	upperArm->setDiffuseMaterial(new Color(1,0,0));
	upperArm->buildTransform(AffineTransforms::scale(.2, 1, .2));
	InstanceObject* lowerArm = new InstanceObject(cube);
	lowerArm->setDiffuseMaterial(new Color(0,1,0));
	lowerArm->buildTransform(AffineTransforms::scale(.2, 2, .2));
	InstanceObject* base = new InstanceObject(cylinder); //no instance transform
	base->setDiffuseMaterial(new Color(0,0,1));

	Matrix *temp;
	TransformNode *upperArmTransform, *lowerArmTransform, *baseTransform, *robotArm1, *robotArm2;

	//upper arm transform
	temp = AffineTransforms::translate(0, 1, 0);
	temp = AffineTransforms::rotateZ(90)->multiply(temp);
	temp = AffineTransforms::translate(0, -1, 0)->multiply(temp);
	temp = AffineTransforms::translate(0, 3, 0)->multiply(temp);
	upperArmTransform = new TransformNode();
	upperArmTransform->buildTransform(temp);

	//lower arm transform
	temp = AffineTransforms::translate(0, 2, 0);
	temp = AffineTransforms::rotateZ(-20)->multiply(temp);
	temp = AffineTransforms::translate(0, -2, 0)->multiply(temp);
	temp = AffineTransforms::translate(0, 3, 0)->multiply(temp);
	lowerArmTransform = new TransformNode();
	lowerArmTransform->buildTransform(temp);

	//base transform
	temp = AffineTransforms::rotateY(30);
	temp = AffineTransforms::translate(0, -2, 0)->multiply(temp);
	baseTransform = new TransformNode();
	baseTransform->buildTransform(temp);

	//robot arm #1
	temp = AffineTransforms::scale(.2, .2, .2);
	temp = AffineTransforms::translate(0, 0, -3)->multiply(temp);
	robotArm1 = new TransformNode();
	robotArm1->buildTransform(temp);

	//robot arm #2
	temp = AffineTransforms::scale(.2, .2, .2);
	temp = AffineTransforms::rotateX(45)->multiply(temp);
	temp = AffineTransforms::translate(-3, .5, 0)->multiply(temp);
	temp = AffineTransforms::scale(.2, .2, .2)->multiply(temp);
	robotArm2 = new TransformNode();
	robotArm2->buildTransform(temp);

	//build scene
	upperArmTransform->addChild(upperArm);
	lowerArmTransform->addChild(upperArmTransform);
	lowerArmTransform->addChild(lowerArm);
	baseTransform->addChild(lowerArmTransform);
	baseTransform->addChild(base);
	robotArm1->addChild(baseTransform);
	robotArm2->addChild(baseTransform);
	scene->addTransformNode(robotArm1);
	scene->addTransformNode(robotArm2);

	scene->render(px, eye, attenuation);
	delete scene;

   /*TransformNode* tn = new TransformNode();
   tn->addChild(sphereInstance);
   scene->addTransformNode(tn);

   scene->render(px, eye, attenuation);
   delete scene;*/
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
   BasicObject* obj = new BasicObject();
   FileIO* file = new FileIO(fileName, ' '); //assumed to be a read if a delimiter is specified

   List<Vertex> vertices;
   double x, y, z;
   string* token;
   int index1, index2, index3, temp; 

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
   delete file;
   return obj;
}

InstanceObject* Picture::buildInstanceObject(char* fileName, BasicObject* obj)
{
   InstanceObject* io = new InstanceObject(obj);
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
