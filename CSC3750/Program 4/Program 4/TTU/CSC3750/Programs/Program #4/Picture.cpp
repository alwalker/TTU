#include "Matrix.H"
#include "AffineTransforms.h"
#include "Picture.h"
#include "Color.h"
#include "BasicObject.h"
#include "InstanceObject.h"
#include "Scene.h"
#include "FileIO.h"
#include "stdio.h"

void Picture::render(Pixel* pix)
{
	double eX, eY, eZ, aX, aY, aZ, fov, zMin, zMax;
	Vector *vUp;
	vUp = readCamera(&eX, &eY, &eZ, &aX, &aY, &aZ);
	readFOV(&zMax, &zMin, &fov);
	BasicObject* obj = readObject("sphere.txt");
	InstanceObject* io = buildInstanceObject("trs.txt", obj);
	Scene *sc;
	Matrix *windowTransform = AffineTransforms::getWindowTransform(pix->getWidth(), pix->getHeight());
	Matrix *aspectRatioTransform = AffineTransforms::getAspectTransform(pix->getWidth(), pix->getHeight(), fov);
	Matrix *normalTransform = AffineTransforms::getNormalTransform(zMin, zMax);
	Matrix *cameraTransform = AffineTransforms::getCameraTransform(eX, eY, eZ, aX, aY, aZ, vUp);
	Matrix *WNAC;

	/*printf("\nCamera: \n");
	cameraTransform->printMatrix();
	printf("\nNormal: \n");
	normalTransform->printMatrix();
	printf("\nAspect Ratio: \n");
	aspectRatioTransform->printMatrix();
	printf("\nWindow: \n");
	windowTransform->printMatrix();*/
	
	Matrix *temp = aspectRatioTransform->multiply(cameraTransform);
	Matrix *temp2 = normalTransform->multiply(temp);
	WNAC = windowTransform->multiply(temp2);

	
	//WNAC->printMatrix();

	delete temp;
	delete temp2;
	delete cameraTransform;
	delete normalTransform;
	delete windowTransform;
	delete aspectRatioTransform;
	
	sc = new Scene(WNAC);


	sc->addNode(io);

	sc->render(pix);

	delete sc;
	printf("\n\n");
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

Vector *Picture::readCamera(double *eX, double *eY, double *eZ, double *aX, double *aY, double *aZ)
{
	Vector *vUp;
	FileIO* inputFile = new FileIO("camera.txt", 1);

	//eye points
	string line = inputFile->readLine();
	line = inputFile->readLine();
	*eX = atof(line.c_str());
	line = inputFile->readLine();
	*eY = atof(line.c_str());
	line = inputFile->readLine();
	*eZ = atof(line.c_str());

	//at points
	line = inputFile->readLine();
	line = inputFile->readLine();
	*aX = atof(line.c_str());
	line = inputFile->readLine();
	*aY = atof(line.c_str());
	line = inputFile->readLine();
	*aZ = atof(line.c_str());

	//vUp
	double x, y, z;
	line = inputFile->readLine();
	line = inputFile->readLine();
	x = atof(line.c_str());
	line = inputFile->readLine();
	y = atof(line.c_str());
	line = inputFile->readLine();
	z = atof(line.c_str());

	vUp = new Vector(x, y, z);
	return vUp;
}

void Picture::readFOV(double *zMax, double *zMin, double *fov)
{
	FileIO *inputFile = new FileIO("fov.txt", 1);
	string line = inputFile->readLine();

	line = inputFile->readLine();
	*fov = atof(line.c_str());
	line = inputFile->readLine();

	line = inputFile->readLine();
	*zMax = atof(line.c_str());
	line = inputFile->readLine();

	line = inputFile->readLine();
	*zMin = atof(line.c_str());
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

   //standard TRS form
   io->buildTransform(scale);  //deletes the matrix when done
   io->buildTransform(rotX);  
   io->buildTransform(rotY);  
   io->buildTransform(rotZ); 
   io->buildTransform(translate);

   delete inputFile;
   return io;
}

