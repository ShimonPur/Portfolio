#include "Board.h"

void printBoard(int board[][GRID_SIZE])
{
	for (size_t i = 0; i < GRID_SIZE; i++)
	{
		if (i % 3 == 0 and i != 0)
			printf("-----------------\n");
		for (size_t j = 0; j < GRID_SIZE; j++)
		{
			if ((j + 1) % 3 == 0 and j != 0 and j != GRID_SIZE - 1)
				printf("%d|", board[i][j]);
			else
				printf("%d ", board[i][j]);
		}

		printf("\n");
	}
}

bool isNumberInRow(int board[][GRID_SIZE], int row, int number)
{
	for (size_t i = 0; i < GRID_SIZE; i++)
	{
		if (board[row][i] == number)
		{
			return true;
		}
	}

	return false;
}

bool isNumberInColumn(int board[][GRID_SIZE], int column, int number)
{
	for (size_t i = 0; i < GRID_SIZE; i++)
	{
		if (board[i][column] == number)
		{
			return true;
		}
	}

	return false;
}

bool isNumberInBox(int board[][GRID_SIZE], int row, int column, int number)
{
	int localBoxRow = row - row % 3;
	int localBoxColumn = column - column % 3;

	for (size_t i = localBoxRow; i < localBoxRow + 3; i++)
	{
		for (size_t j = localBoxColumn; j < localBoxColumn + 3; j++)
		{
			if (board[i][j] == number)
			{
				return true;
			}
		}
	}

	return false;
}

bool isValidNumber(int board[][GRID_SIZE], int row, int column, int number)
{
	return (!isNumberInRow(board, row, number) and
		    !isNumberInColumn(board, column, number) and
		    !isNumberInBox(board, row, column, number));
}
