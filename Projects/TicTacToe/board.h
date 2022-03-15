#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>

#define MAX_TURNS 9
#define BOARD_LENGTH 3
#define and &&
#define or ||


/*
* Function print the game board to the screen.
* Input: borad - 2D char array contain the gane board.
* Output: None.
*/
void printGameBored(char board[BOARD_LENGTH][BOARD_LENGTH]);

/*
* Function check for valid input.
* valid input : col and row between 1 -3.
* Input: row - The row in the board.
*	     col - The col in the board.
* 
* Output: true for valid input, otherwise false.
*/
bool isValidInput(int row, int col, char board[BOARD_LENGTH][BOARD_LENGTH]);

/*
* Function check if the player won.
* Input: plaeyr - the player to check for win.
* Output: true if the player won otherwise false.
*/
bool isWin(char player, char board[BOARD_LENGTH][BOARD_LENGTH]);