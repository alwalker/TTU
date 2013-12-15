cls
@echo off
del *.o
echo Compiling
g++ -c BasicMatrix.cpp 
g++ -c Matrix.cpp
g++ -c Vector.cpp 
g++ -c Color.cpp 
g++ -c Pixel.cpp 
g++ -c FileIO.cpp
g++ -c Vertex.cpp
g++ -c Light.cpp
g++ -c AffineTransforms.cpp
g++ -c Face.cpp
g++ -c DeleteObject.cpp
g++ -c Node.cpp
g++ -c BasicObject.cpp
g++ -c InstanceObject.cpp
g++ -c Picture.cpp 
g++ -c Window.cpp
g++ -c TransformNode.cpp
g++ -c Scene.cpp 
g++ -o Main.exe Main.cpp Light.o Vector.o Scene.o Node.o TransformNode.o DeleteObject.o InstanceObject.o AffineTransforms.o Picture.o Window.o Pixel.o Matrix.o BasicMatrix.o Color.o FileIO.o Vertex.o Face.o BasicObject.o -lglut32 -lopengl32
echo Running
Main
