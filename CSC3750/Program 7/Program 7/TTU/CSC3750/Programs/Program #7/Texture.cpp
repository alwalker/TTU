#include "Texture.h"
#include <stdio.h>

Texture::Texture(char* fileName, int w, int h)
{
   textureFile = fileName;
   width = w;
   height = h;   
}

Texture::~Texture()
{
   delete[] textureFile;
   delete[] textureColors;
}

char* Texture::getFileName()
{
   return textureFile;
}

int Texture::getWidth()
{
   return width;
}

int Texture::getHeight()
{
   return height;
}

void Texture::setTextureColors(unsigned char *colors)
{
	textureColors = colors;
}

unsigned char *Texture::getTextureColors()
{
	return textureColors;
}

void Texture::printTextureColors()
{
	int width = getWidth();
   int height = getHeight();
   int numColors = width*height*3;

   for(int i=0 ; i<numColors ; i++)
   {
	   printf("%d", textureColors[i]);
   }
}
