#include "Board.h"

/*
* Game Board with AXIS
*         0           1           2  
*               |           |           
*	 		    |           |
*  0    [0,0]   |   [0,1]   |   [0,2]  
*               |           |
*    ___________|___________|___________
*               |           |     
*               |           |           
*  1    [1,0]   |   [1,1]   |    [1,2]           
*	            |           |           
*    ___________|___________|___________
*               |           |     
*               |           |           
*  2    [2,0]   |   [2,1]   |    [2,2]       
*               |           |
*               |           |     
*/

void printGameBored(char board[BOARD_LENGTH][BOARD_LENGTH])
{
	const int lastRow = 2;
	int i = 0;
	printf("   1     2     3  \n");

	for (int row = 0; row < BOARD_LENGTH; row++) // print each row with index numbers to help players.
	{
		printf("      |     |     \n%d", row + 1);

		for (i = 0; i < BOARD_LENGTH - 1; i++)
		{
			printf("  %c  |", board[row][i]);
		}

		printf("  %c\n", board[row][i]);
		
		printf(row == lastRow ? "      |     |     \n" : " _____|_____|_____\n");
	}
}

bool isValidInput(int row, int col, char board[BOARD_LENGTH][BOARD_LENGTH])
{

	if (row >= 1 and row <= BOARD_LENGTH and 
		col >= 1 and col <= BOARD_LENGTH and
		board[row - 1][col - 1] == '-')
	{
		return true;
	}

	return false;
}

bool isWin(char player, char board[BOARD_LENGTH][BOARD_LENGTH])
{
	//rows
	for (int row = 0; row < BOARD_LENGTH; row++)
	{
		if (board[row][0] == player and
			board[row][1] == player and
			board[row][2] == player)
		{
			return true;
		}
	}

	//columns
	for (int col = 0; col < BOARD_LENGTH; col++)
	{
		if (board[0][col] == player and
			board[1][col] == player and
			board[2][col] == player)
		{
			return true;
		}
	}

	//first diaognal
	if (board[0][0] == player and
		board[1][1] == player and
		board[2][2] == player)
	{
		return true;
	}

	//second diaognal
	if (board[0][2] == player and
		board[1][1] == player and
		board[2][0] == player)
	{
		return true;
	}

	return false; // if we reach here the player didn't won :(;
}