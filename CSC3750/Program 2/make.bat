@echo off
del *.o
g++ -c BasicMatrix.cpp 
g++ -c Matrix.cpp 
g++ -c Color.cpp 
g++ -c Pixel.cpp 
g++ -c FileIO.cpp
g++ -c Vertex.cpp
g++ -c AffineTransforms.cpp
g++ -c Face.cpp
g++ -c DeleteObject.cpp
g++ -c BasicObject.cpp
g++ -c InstanceObject.cpp
g++ -c Scene.cpp
g++ -c Picture.cpp 
g++ -c Window.cpp 
g++ -o Main.exe Main.cpp Scene.o DeleteObject.o InstanceObject.o AffineTransforms.o Picture.o Window.o Pixel.o Matrix.o BasicMatrix.o Color.o FileIO.o Vertex.o Face.o BasicObject.o -lglut32 -lopengl32
Main
