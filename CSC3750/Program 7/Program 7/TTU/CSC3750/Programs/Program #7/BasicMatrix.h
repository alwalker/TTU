#if !defined (BASICMATRIX_H)
#define BASICMATRIX_H

class BasicMatrix
{

   private:
      double** matrix;
      int rows;
      int columns;
      bool checkInput(int row, int column);

   public:
      BasicMatrix(int row, int column);
      ~BasicMatrix();
      int getNumRows();
      int getNumColumns();
      void setElement(int row, int column, double value);
      double getElement(int rows, int columns);
      void printMatrix();

};

#endif
