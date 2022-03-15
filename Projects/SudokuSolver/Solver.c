#include "Solver.h"

bool solve(int board[][GRID_SIZE])
{
	for (size_t row = 0; row < GRID_SIZE; row++)
	{
		for (size_t col = 0; col < GRID_SIZE; col++)
		{
			if (board[row][col] == 0)
			{
				for (int number = 1; number <= GRID_SIZE; number++)
				{
					if (isValidNumber(board, row, col, number))
					{
						board[row][col] = number;

						if (solve(board))
						{
							return true;
						}
						else
						{
							board[row][col] = 0;
						}
					}
				}

				return false;
			}
		}
	}
	return true;
}