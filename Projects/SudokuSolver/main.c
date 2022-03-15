#include "Solver.h"

int main()
{
	int board[GRID_SIZE][GRID_SIZE] = {  //Test sudoku board
			{7, 0, 2, 0, 5, 0, 6, 0, 0}, // 1
			{0, 0, 0, 0, 0, 3, 0, 0, 0}, // 2
			{1, 0, 0, 0, 0, 9, 5, 0, 0}, // 3
			{8, 0, 0, 0, 0, 0, 0, 9, 0}, // 4
			{0, 4, 3, 0, 0, 0, 7, 5, 0}, // 5
			{0, 9, 0, 0, 0, 0, 0, 0, 8}, // 6
			{0, 0, 9, 7, 0, 0, 0, 0, 5}, // 7
			{0, 0, 0, 2, 0, 0, 0, 0, 0}, // 8
			{0, 0, 7, 0, 4, 0, 2, 0, 3}	 // 9
	};

	printf("[     BEFORE:   ]\n");
	printBoard(board);

	if (solve(board))
	{
		printf("\n[    SUCSESS!   ]\n");
		printBoard(board);
	}
	else
	{
		printf("[     ERROR!    ]\n");
	}

	return 0;
}