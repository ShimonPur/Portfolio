#include "board.h"
#include <stdbool.h>

char _ = ' '; // ignore variable

int main()
{
	bool win = false;
	char currentPlayer = 'X';
	unsigned int row = 0, col = 0;

	char board[BOARD_LENGTH][BOARD_LENGTH] = { {'-', '-', '-'},
											   {'-', '-', '-'},
											   {'-', '-', '-'} }; // The starting game Board.

	for (int i = 0; i < MAX_TURNS and !win; i++)
	{
		printGameBored(board);

		printf("%c turn\n", currentPlayer);

		do
		{
			printf("Pick [empty] location in the border [limits] (row column): ");

			if (scanf_s("%u %u", &row, &col) < 2)
			{
				_ = getchar(); // clean buffer
				printf("\n[Error while inputting]\n");
			}

		} while (!isValidInput(row, col, board));

		row -= 1; // fix size.
		col -= 1; // fix size.

		if (board[row][col] == '-')
		{
			board[row][col] = currentPlayer;
		}

		if (isWin(currentPlayer, board))
		{
			win = true;
		}

		currentPlayer = currentPlayer == 'X' ? 'O' : 'X';
		system("cls"); // clear the console when the game ends
	}

	printGameBored(board);

	if (win)
	{
		printf("%c WON!!", currentPlayer == 'X' ? 'O' : 'X');
	}
	else
	{
		printf("[DRAW]");
	}

	return 0;
}