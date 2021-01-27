/**
 * @author Andrew Harris
 * @date 21 Jan 2021
 * @version 1.1
 *
 * The original implementation of this was done in Java
 * and ported to C# as an introduction to the language.
 *
 */

using System;

namespace MagicSquares
{
    /**
     * 
     * The MagicSquares Class contains the methods containing
     * the game logic. A magic square is a 3x3 grid of numbers
     * between 1-9, where each row, column and diagonal sums to 15.
     * This is a fun way to implement bit masking in a practical
     * manner.
     * 
     */

    class MagicSquares
    {
        short choices;
        int[] choicesArray;
        string _name;

        // MagicSquares parameterless Constructor
        public MagicSquares()
        {
            this.choices = 0;
            choicesArray = new int[] { 2, 7, 6, 9, 5, 1, 4, 3, 8 };
        }

        // Parameterized Constructor
        public MagicSquares(string name)
        {
            this.choices = 0;
            choicesArray = new int[] { 2, 7, 6, 9, 5, 1, 4, 3, 8 };
            _name = name;
        }

        // Accessor for Name
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        /**
        * This method accepts a byte value from 1-9. Returns
        * TRUE if selected bit was changed and FALSE if
        * selection has already been chosen.
        * Throws ArgumentException if the choice is invalid
        *
        * @param selection the selected bit
        * @return wheter the selected bit is
        *         an acceptable choice
        */
        public bool Choose(byte selection)
        {
            if (selection > 9 || selection < 1)
            {
                throw new ArgumentException("Please select a number between " +
                        "1 and 9.");
            }
            else if (HasAlreadyChosen(selection))
            {
                return false;
            }
            else
            {
                short mask = CreateMask(selection - 1);
                choices = (short)(choices | mask);
                return true;
            }
        }

        /**
         * Helper method for the Choose method. Creates a bit mask
         * @param index of bit to mask
         * @return the mask
         */
        private static short CreateMask(int index)
        {
            //crete a mask where the bit of interest (index) is 1
            //assign decimal 1 / 0b0000_0001
            short mask = 1;
            //shift the mask over "index" times
            mask = (short)(mask << index);
            return mask;
        }

        /**
         * Returns true if the bit at index selection-1 is
         * set to the "on" position, or false if the bit
         * is in the "off" position
         *
         * @param selection the selected bit
         * @return state of selected bit
         */
        public bool HasAlreadyChosen(byte selection)
        {
            short mask = CreateMask(selection - 1);
            return (choices & mask) == mask;
        }

        /**
         *
         * @return an array representing the user selections
         */
        public short GetChoices()
        {
            return choices;
        }

        /**
        * Prints the current status of the
        * Magic Square
        */
        public void PrintChoices()
        {
            int choices = 1;
            foreach (int num in choicesArray)
            {
                //check if player has already chosen
                if (HasAlreadyChosen((byte)num))
                {
                    Console.Write(num + " ");
                }
                else
                {
                    Console.Write("_ ");
                }
                //print 3 across
                if (choices % 3 == 0)
                {
                    Console.WriteLine();
                }
                choices++;
            }
        }

    }

    /**
     * @author Andrew Harris
     * @date 21 Jan 2021
     * @version 1.1
     *
     * This Class contains the game play for the
     * MagicSquares game. It was ported to C# as 
     * an intro to the language.
     */
    class MagicSquaresGame
    {
        /**
         * DRIVER           DRIVER              DRIVER
         * 
         * This main method setups the game play and contains
         * the game loop where each player continues to take
         * turns until a player wins or the game is a DRAW
         * @param args not used
         */
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // print the instructions
            PrintDirections();
            //create Player 1 Object
            MagicSquares p1 = new MagicSquares(GetName(1));
            //create Player 2 Object
            MagicSquares p2 = new MagicSquares(GetName(2));

            int player1;
            int player2;

            while (true)
            {
                player1 = TakePlayerTurn(p1, p2);
                if (player1 == 1)
                {
                    Console.WriteLine("Player 1 wins!");
                    break;
                }
                if (player1 == 2)
                {
                    Console.WriteLine("The game is a draw!");
                    break;
                }
                player2 = TakePlayerTurn(p2, p1);
                if (player2 == 1)
                {
                    Console.WriteLine("Player 2 wins!");
                    break;
                }
                if (player2 == 2)
                {
                    Console.WriteLine("The game is a draw!");
                    break;
                }
            }

            /**
             * This TakePlayerTurn method will prompt for and
             * display the player's choice of numbers and then
             * determine if the game has been won,
             * is a draw, or the game should continue
             *
             * @param p1 -Player 1
             * @param p2 -Player 2
             * @return a number indicating the game state(CONTINUE, DRAW, WIN)
             */
            static int TakePlayerTurn(MagicSquares p1, MagicSquares p2)
            {
                //this choice has been validated in the GetSelection()
                byte choice = (byte)GetSelection(p1, p2);
                //assign the choice
                p1.Choose(choice);
                p1.PrintChoices();
                if (IsWin(p1))
                {
                    return 1;
                }
                else if (IsDraw(p1, p2))
                {
                    return 2;
                }
                return 0;
            }

            /**
             * This IsDraw method will determine if the game is a
             * draw due to all choices made and no one player had
             * a winning combination
             * @param p1 - player 1 choices
             * @param p2 -player 2 choices
             * @return true if all values have been picked, game is a DRAW, false otherwise
             */
            static bool IsDraw(MagicSquares p1, MagicSquares p2)
            {
                // 511 is the dec equiv. of the conditions for a draw
                // converted to binary
                return (p1.GetChoices() + p2.GetChoices() == 511);
            } 

            /**
             * This IsWin method determines if a player's magic
             * square choices result in a winning combination
             * @param p the player
             * @return true, if the player has won, false otherwise
             */
            static bool IsWin(MagicSquares p)
            {
                // these are the dec sums of the winning magic
                // square combos
                int[] winCond = new int[] { 98, 273, 140, 266, 84, 161, 146 };
                // loop through my victory conditions
                // and see if they picked a winning condition
                foreach (int num in winCond)
                {
                    // set a mask from my player choice
                    int mask = p.GetChoices() & num;
                    // if my mask equals a victory condition,
                    // for either this or all previous player
                    // choices
                    if (mask == num)
                    {
                        return true;
                    }
                }
                return false;
            }

            /**
             * This getSelection method asks the player for their number
             * selection from 1 - 9.  If the user does not enter a number in this
             * range, they are continually prompted until they do enter a number
             * between 1 and 9.  If the user has already entered a number that
             * has been chosen, they are continually prompted until they enter
             * a number that has not been previously selected.
             * @param p1 - Player 1
             * @param p2 - Player 2
             * @return the number chosen from 1-9
             *
             */
            static int GetSelection(MagicSquares p1, MagicSquares p2)
            {
                // Prompt user input
                Console.WriteLine(p1.Name + ", please enter a number: ");
                // store as a string
                string strChoice = Console.ReadLine();
                // cast to int
                int choice = Convert.ToInt32(strChoice);

                //check if the number is between 1 and 9
                //keep checking until valid
                if (choice < 1 || choice > 9)
                {
                    while (choice < 1 || choice > 9)
                    {
                        Console.WriteLine("Please enter a choice between 1 and 9...");
                        Console.WriteLine(p1.Name + ", please enter a number: ");
                        choice = Console.Read();
                    }
                }

                //check if it's been chosen already
                //keep checking until valid
                while (p1.HasAlreadyChosen((byte)choice) || p2.HasAlreadyChosen((byte)choice))
                {
                    Console.WriteLine("Please make a selection that hasn't already been chosen.");
                    Console.WriteLine("Please make a different selection: ");
                    choice = Console.Read();
                }
                return choice;
            }

            /**
             * This GetName method receives a player's number and prompts
             * that player for their name.
             * @param playerNum the player's number
             * @return the player's name
             */
            static string GetName(int playerNum)
            {
                Console.WriteLine("Please enter player name for player number " + playerNum);
                string name = Console.ReadLine();
                return name;
            }

            /**
             * This PrintDirections method prints out the game introduction
             */
            static void PrintDirections()
            {
                Console.WriteLine("Welcome to the game of Magic Squares");
                Console.WriteLine("***********************************");
                Console.WriteLine("Rules:");
                Console.WriteLine("2 players play the game.");
                Console.WriteLine("Each player takes turns picking a number from 1-9.");
                Console.WriteLine("No number can be chosen twice.");
                Console.WriteLine("The first player to have 3 numbers that sum to 15 wins!");
                Console.WriteLine("2 7 6");
                Console.WriteLine("9 5 1");
                Console.WriteLine("4 3 8");
                Console.WriteLine("***********************************");
                Console.WriteLine();
            }

        }
    }
}
