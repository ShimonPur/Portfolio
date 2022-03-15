#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>

#define and &&
#define GRID_SIZE 9


/// <summary>
/// Function print the board to screen.
/// </summary>
/// <param name="board">-The sudoku game board</param>
void printBoard(int board[][GRID_SIZE]);

/// <summary>
/// Function check if the number present in the given row.
/// </summary>
/// <param name="board">The sudoku game board</param>
/// <param name="row">-The row to check</param>
/// <param name="number">-The number to check</param>
/// <returns>true if the number is in the row, otherwise false</returns>
bool isNumberInRow(int board[][GRID_SIZE], int row, int number);

/// <summary>
/// Function check if the number present in the given column.
/// </summary>
/// <param name="board">-The sudoku game board</param>
/// <param name="column">-The column to check</param>
/// <param name="number">-The number to check</param>
/// <returns>true if the number is in the column, otherwise false</returns>
bool isNumberInColumn(int board[][GRID_SIZE], int column, int number);

/// <summary>
/// Function check if the number present in the given box.
/// </summary>
/// <param name="board">-The sudoku game board</param>
/// <param name="row">-The row of the box</param>
/// <param name="column">-The column of the box</param>
/// <param name="number">-The number to check</param>
/// <returns>true if the number is in the box, otherwise false</returns>
bool isNumberInBox(int board[][GRID_SIZE], int row, int column, int number);

/// <summary>
/// Function check if the number is valid.
/// </summary>
/// <param name="board">-The sudoku game board</param>
/// <param name="row">-The row of the number</param>
/// <param name="column">-The column of the number</param>
/// <param name="number">-The number to check</param>
/// <returns>true if the number is valid, otherwise fasle</returns>
bool isValidNumber(int board[][GRID_SIZE], int row, int column, int number);