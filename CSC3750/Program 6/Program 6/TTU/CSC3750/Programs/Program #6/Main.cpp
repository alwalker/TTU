#include "Window.h"

int main(int argc, char** argv)
{
   Window window(argc, argv, 800, 600, 450, 250, "Rendering Algorithms");
   window.show();
   return 0;
}
