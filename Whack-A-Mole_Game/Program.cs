using System;

namespace Whack_A_Mole_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteWelcomeMessage();

            bool continueProgram = true;
            string[] leaderboard = new string[] { "The Exterminator", "5000", "Exterminator's Assistant", "4500", "Backyard Defender", "4000",
            "Whack-A-Mole Pro", "3500", "Varmint Vanquisher", "3000", "Rodent Ranger", "2500", "Mole Masher", "2000", "Pest Pounder", "1500",
            "Weekend Warrior", "1000", "Novice", "500" };

            do
            {
                string playerOption = GetPlayerOption();

                if (playerOption == "1")
                {
                    leaderboard = PlayGame(leaderboard);
                }
                else if (playerOption == "2")
                {
                    ShowInstructions();
                }
                else if (playerOption == "3")
                {
                    ShowHighScores(leaderboard);
                }
                else
                {
                    continueProgram = false;
                }
            }
            while (continueProgram);

            DisplayExitMessage();
        }

        static void WriteWelcomeMessage()
        {
            Console.WriteLine("Hello, and welcome to WHACK-A-MOLE!\r\n");
        }

        static string GetPlayerOption()
        {
            bool validUserSelection = false;

            Console.WriteLine("Please select an option from the menu below. Type the number and press \"Enter\".\r\n");
            Console.WriteLine("------- GAME OPTIONS -------");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Instructions");
            Console.WriteLine("3. See High Scores");
            Console.WriteLine("4. Exit\r\n");

            string userSelection = Console.ReadLine();
            Console.WriteLine();

            do
            {
                if (userSelection != "1" && userSelection != "2" && userSelection != "3" && userSelection != "4")
                {
                    Console.WriteLine("Invalid entry. Please try again.");
                    userSelection = Console.ReadLine();
                    Console.WriteLine();
                }
                else
                {
                    validUserSelection = true;
                }
            }
            while (!validUserSelection);

            return userSelection;
        }

        static string[] PlayGame(string[] leaderboard)
        {
            Console.Clear();
            Console.WriteLine("------- NEW GAME -------\r\n");
            Console.WriteLine("Select game mode. Enter \"E\" for Easy or \"H\" for Hard:");
            string gameMode = Console.ReadLine();
            Console.WriteLine();

            while (gameMode != "e" && gameMode != "E" && gameMode != "h" && gameMode != "H")
            {
                Console.WriteLine("Invalid entry. Please select a game mode.");
                gameMode = Console.ReadLine();
                Console.WriteLine();
            }
            
            if (gameMode == "e" || gameMode == "E")
            {
                Console.Clear();
                leaderboard = RunGame("easy", leaderboard);
            }
            else if (gameMode == "h" || gameMode == "H")
            {
                Console.Clear();
                leaderboard = RunGame("hard", leaderboard);
            }

            return leaderboard;
        }

        static string[] RunGame(string difficulty, string[] leaderboard)
        {
            int round = 1;
            int lives = 3;
            int score = 0;
            string hole1;
            string hole2;
            string guess;
            string illustration = "";
            string[] holesArray = new string[2];
            bool correctGuess;
            bool newHighScore;
            string playerName = "";

            while (lives != 0)
            {
                holesArray = GenRandNums();
                hole1 = holesArray[0];
                hole2 = (difficulty == "easy") ? holesArray[1] : "0";

                Console.WriteLine($"----- ROUND: {round} -----\r\nSCORE: {score}     GARDEN TOOLS: {lives}\r\n");
                Console.WriteLine("Enter the hole (1, 2, or 3) that you think a mole will appear from:");
                Console.WriteLine();
                guess = Console.ReadLine();
                Console.WriteLine();

                while (guess != "1" && guess != "2" && guess != "3")
                {
                    Console.WriteLine("Invalid guess. Please enter 1, 2, or 3.");
                    guess = Console.ReadLine();
                    Console.WriteLine();
                }

                if (guess == hole1 || guess == hole2)
                {
                    correctGuess = true;
                    score += 100;
                    round++;
                }
                else
                {
                    correctGuess = false;
                    lives--;
                }

                if (correctGuess)
                {
                    illustration = IllustrateResult(guess, hole1, correctGuess, hole2);
                    Console.WriteLine(illustration);
                    Console.WriteLine();
                    Console.WriteLine($"Nice job! You whacked a mole! On to Round {round}!\r\n\r\n\r\n");
                }
                else if (correctGuess == false && lives != 0)
                {
                    illustration = IllustrateResult(guess, hole1, correctGuess, hole2);
                    Console.WriteLine(illustration);
                    Console.WriteLine();
                    Console.WriteLine("You missed and broke a garden tool. You'll have to replay this round.\r\n\r\n\r\n");
                }
                else if (score > 0)
                {
                    illustration = IllustrateResult(guess, hole1, correctGuess, hole2);
                    Console.WriteLine(illustration);
                    Console.WriteLine();
                    Console.WriteLine($"GAME OVER. But, you scored {score} points!\r\n\r\n\r\n");
                    UpdateHighScores(out newHighScore, leaderboard, "", score);

                    if (newHighScore)
                    {
                        Console.WriteLine("You got a new high score! Enter your name to be added to the high score leaderboard!");
                        playerName = Console.ReadLine();

                        while (playerName.Length > 25)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Invalid entry. Please enter a player name that is 25 characters or less.");
                            playerName = Console.ReadLine();
                        }

                        leaderboard = UpdateHighScores(out newHighScore, leaderboard, playerName, score);
                        Console.WriteLine();
                        Console.WriteLine("Success! You've been added to the high score leaderboard!\r\n\r\n");
                    }

                    Console.WriteLine("Press \"Enter\" to retun to the main menu.");
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    illustration = IllustrateResult(guess, hole1, correctGuess, hole2);
                    Console.WriteLine(illustration);
                    Console.WriteLine();
                    Console.WriteLine($"GAME OVER. Better luck next time!\r\n\r\n\r\n");
                    Console.WriteLine("Press \"Enter\" to retun to the main menu.");
                    Console.ReadLine();
                    Console.Clear();
                }
            }

            return leaderboard;
        }

        static string IllustrateResult(string guess, string hole1, bool correctGuess, string hole2 = "0")
        {
            string gap = "      ";
            string liveMoleLine1 = "  ^^  ";
            string liveMoleLine2 = " /**\\ ";
            string deadMoleLine1 = "  __  ";
            string deadMoleLine2 = " /xx\\ ";
            string missedLine1 = "  \\/  ";
            string missedLine2 = "  /\\  ";
            string line1Hole1 = "";
            string line1Hole2 = "";
            string line1Hole3 = "";
            string line2Hole1 = "";
            string line2Hole2 = "";
            string line2Hole3 = "";
            string line3 = "------      ------      ------";
            string line4 = "   1           2           3";
            string illustration = "";

            if (correctGuess)
            {
                if (guess == "1")
                {
                    line1Hole1 = deadMoleLine1;
                    line2Hole1 = deadMoleLine2;

                    if (guess == hole1)
                    {
                        if (hole2 == "2")
                        {
                            line1Hole2 = liveMoleLine1;
                            line2Hole2 = liveMoleLine2;
                            line1Hole3 = gap;
                            line2Hole3 = gap;
                        }
                        else if (hole2 == "3")
                        {
                            line1Hole2 = gap;
                            line2Hole2 = gap;                            
                            line1Hole3 = liveMoleLine1;
                            line2Hole3 = liveMoleLine2;
                        }
                        else
                        {
                            line1Hole2 = gap;
                            line2Hole2 = gap;
                            line1Hole3 = gap;
                            line2Hole3 = gap;
                        }
                    }
                    else if (guess == hole2)
                    {
                        if (hole1 == "2")
                        {
                            line1Hole2 = liveMoleLine1;
                            line2Hole2 = liveMoleLine2;
                            line1Hole3 = gap;
                            line2Hole3 = gap;
                        }
                        else if (hole1 == "3")
                        {
                            line1Hole2 = gap;
                            line2Hole2 = gap;
                            line1Hole3 = liveMoleLine1;
                            line2Hole3 = liveMoleLine2;
                        }
                    }
                }
                else if (guess == "2")
                {
                    line1Hole2 = deadMoleLine1;
                    line2Hole2 = deadMoleLine2;

                    if (guess == hole1)
                    {
                        if (hole2 == "1")
                        {
                            line1Hole1 = liveMoleLine1;
                            line2Hole1 = liveMoleLine2;
                            line1Hole3 = gap;
                            line2Hole3 = gap;
                        }
                        else if (hole2 == "3")
                        {
                            line1Hole1 = gap;
                            line2Hole1 = gap;
                            line1Hole3 = liveMoleLine1;
                            line2Hole3 = liveMoleLine2;
                        }
                        else
                        {
                            line1Hole1 = gap;
                            line2Hole1 = gap;
                            line1Hole3 = gap;
                            line2Hole3 = gap;

                        }
                    }
                    else if (guess == hole2)
                    {
                        if (hole1 == "1")
                        {
                            line1Hole1 = liveMoleLine1;
                            line2Hole1 = liveMoleLine2;
                            line1Hole3 = gap;
                            line2Hole3 = gap;

                        }
                        else if (hole1 == "3")
                        {
                            line1Hole1 = gap;
                            line2Hole1 = gap;
                            line1Hole3 = liveMoleLine1;
                            line2Hole3 = liveMoleLine2;
                        }
                    }
                }
                else if (guess == "3")
                {
                    line1Hole3 = deadMoleLine1;
                    line2Hole3 = deadMoleLine2;

                    if (guess == hole1)
                    {
                        if (hole2 == "1")
                        {
                            line1Hole1 = liveMoleLine1;
                            line2Hole1 = liveMoleLine2;
                            line1Hole2 = gap;
                            line2Hole2 = gap;
                        }
                        else if (hole2 == "2")
                        {
                            line1Hole1 = gap;
                            line2Hole1 = gap;
                            line1Hole2 = liveMoleLine1;
                            line2Hole2 = liveMoleLine2;
                        }
                        else
                        {
                            line1Hole1 = gap;
                            line2Hole1 = gap;
                            line1Hole2 = gap;
                            line2Hole2 = gap;
                        }
                    }
                    else if (guess == hole2)
                    {
                        if (hole1 == "1")
                        {
                            line1Hole1 = liveMoleLine1;
                            line2Hole1 = liveMoleLine2;
                            line1Hole2 = gap;
                            line2Hole2 = gap;
                        }
                        else if (hole1 == "2")
                        {
                            line1Hole1 = gap;
                            line2Hole1 = gap;
                            line1Hole2 = liveMoleLine1;
                            line2Hole2 = liveMoleLine2;
                        }
                    }
                }

            }
            //correctGuess is false
            else
            {
                if (guess == "1")
                {
                    line1Hole1 = missedLine1;
                    line2Hole1 = missedLine2;

                    if (hole1 == "2" || hole2 == "2")
                    {
                        line1Hole2 = liveMoleLine1;
                        line2Hole2 = liveMoleLine2;
                    }
                    else
                    {
                        line1Hole2 = gap;
                        line2Hole2 = gap;
                    }
                    
                    if (hole1 == "3" || hole2 == "3")
                    {
                        line1Hole3 = liveMoleLine1;
                        line2Hole3 = liveMoleLine2;
                    }
                    else
                    {
                        line1Hole3 = gap;
                        line2Hole3 = gap;
                    }
                }
                else if (guess == "2")
                {
                    line1Hole2 = missedLine1;
                    line2Hole2 = missedLine2;

                    if (hole1 == "1" || hole2 == "1")
                    {
                        line1Hole1 = liveMoleLine1;
                        line2Hole1 = liveMoleLine2;
                    }
                    else
                    {
                        line1Hole1 = gap;
                        line2Hole1 = gap;
                    }

                    if (hole1 == "3" || hole2 == "3")
                    {
                        line1Hole3 = liveMoleLine1;
                        line2Hole3 = liveMoleLine2;
                    }
                    else
                    {
                        line1Hole3 = gap;
                        line2Hole3 = gap;
                    }
                }
                else if (guess == "3")
                {
                    line1Hole3 = missedLine1;
                    line2Hole3 = missedLine2;

                    if (hole1 == "1" || hole2 == "1")
                    {
                        line1Hole1 = liveMoleLine1;
                        line2Hole1 = liveMoleLine2;
                    }
                    else
                    {
                        line1Hole1 = gap;
                        line2Hole1 = gap;
                    }

                    if (hole1 == "2" || hole2 == "2")
                    {
                        line1Hole2 = liveMoleLine1;
                        line2Hole2 = liveMoleLine2;
                    }
                    else
                    {
                        line1Hole2 = gap;
                        line2Hole2 = gap;
                    }
                }
            }

            illustration = line1Hole1 + gap + line1Hole2 + gap + line1Hole3 + "\r\n" +
                           line2Hole1 + gap + line2Hole2 + gap + line2Hole3 + "\r\n" +
                           line3 + "\r\n" + line4;

            return illustration;

        }

        //This generates two different random numbers from 1-3 and returns the values in an array
        static string[] GenRandNums()
        {
            Random random = new Random();
            string[] randomNumbers = new string[2];

            int randomNum1 = random.Next(1, 4);
            int randomNum2 = random.Next(1, 4);

            while (randomNum1 == randomNum2)
            {
                randomNum2 = random.Next(1, 4);
            }

            randomNumbers[0] = Convert.ToString(randomNum1);
            randomNumbers[1] = Convert.ToString(randomNum2);

            return randomNumbers;
        }

        static void ShowInstructions()
        {
            Console.Clear();
            Console.WriteLine("------- INSTRUCTIONS -------\r\n");
            Console.WriteLine("Oh no! Moles have invaded your backyard!\r\n");
            Console.WriteLine("  ^^  ");
            Console.WriteLine(" /**\\ ");
            Console.WriteLine("------\r\n");
            Console.WriteLine("They've made 3 holes, and are going to pop up out of them. If you're ready, you'll be able whack them!\r\n");
            Console.WriteLine("To defend your yard, you have three garden tools to use to whack the moles.");
            Console.WriteLine("Pick the hole you think a mole will pop out of by typing the number using the keyboard.\r\n");
            Console.WriteLine("------       ------       ------");
            Console.WriteLine("   1            2            3\r\n");
            Console.WriteLine("EASY MODE: Each round, two moles will appear from the holes.");
            Console.WriteLine("HARD MODE: Each round, only one mole will appear.\r\n");
            Console.WriteLine("When you guess correctly, the whacked mole will look like this:\r\n");
            Console.WriteLine("  __  ");
            Console.WriteLine(" /xx\\ ");
            Console.WriteLine("------\r\n");
            Console.WriteLine("For every mole you whack, you earn 100 points! But, if you guess incorrectly, you'll break one of your garden tools.\r\n");
            Console.WriteLine("Try to whack as many moles as you can before all your garden tools are broken!\r\n\r\n");
            Console.WriteLine("Press \"Enter\" to return to the main menu.");
            Console.ReadLine();
            Console.Clear();
        }
        
        static string[] UpdateHighScores(out bool newHighScore, string[] leaderboard, string playerName = "", int playerScore = 0)
        {
            string[] highScores = leaderboard;
            newHighScore = false;

            int[] highScoreInts = new int[highScores.Length / 2];
            
            for (int i = 0; i < highScoreInts.Length; i++)
            {
                highScoreInts[i] = Int32.Parse(highScores[(i * 2) + 1]);
            }

            if (playerScore > 0 && playerScore >= highScoreInts[highScoreInts.Length - 1])
            {
                int index = highScoreInts.Length;
                index--;

                while (playerScore >= highScoreInts[index])
                {
                    index--;
                    newHighScore = true;
                }

                index++;

                highScores[(index * 2) + 1] = Convert.ToString(playerScore);
                highScores[index * 2] = playerName;
            }
            return highScores;
        }
        
        static void ShowHighScores(string[] leaderboard)
        {
            Console.Clear();
            Console.WriteLine("------- HIGH SCORES -------\r\n");
            string[] highScores = UpdateHighScores(out bool newHighScore, leaderboard);
            int rankInt = 1;
            string rankString = "";
            int playerNameLength = 0;
            int playerNameSpaceLength = 25;
            int numberOfSpacesNeeded = 0;
            string nameSpacer = "";

            for (int i = 0; i < highScores.Length; i += 2)
            {
                rankString = Convert.ToString(rankInt);
                nameSpacer = "";
                playerNameLength = highScores[i].Length;
                numberOfSpacesNeeded = playerNameSpaceLength - playerNameLength;

                for (int j = 0; j < numberOfSpacesNeeded; j++)
                {
                    nameSpacer += " ";
                }

                if (rankInt < 10 && highScores[i + 1].Length == 3)
                {
                    Console.WriteLine($"{rankString}.  " + highScores[i] + nameSpacer + "    " + highScores[i + 1]);
                }
                else if (rankInt < 10 && highScores[i + 1].Length == 4)
                {
                    Console.WriteLine($"{rankString}.  " + highScores[i] + nameSpacer + "   " + highScores[i + 1]);
                }
                else if (rankInt < 10 && highScores[i + 1].Length == 5)
                {
                    Console.WriteLine($"{rankString}.  " + highScores[i] + nameSpacer + "  " + highScores[i + 1]);
                }
                else if (rankInt >= 10 && highScores[i + 1].Length == 3)
                {
                    Console.WriteLine($"{rankString}. " + highScores[i] + nameSpacer + "    " + highScores[i + 1]);
                }
                else if (rankInt >= 10 && highScores[i + 1].Length == 4)
                {
                    Console.WriteLine($"{rankString}. " + highScores[i] + nameSpacer + "   " + highScores[i + 1]);
                }
                else if (rankInt >= 10 && highScores[i + 1].Length == 5)
                {
                    Console.WriteLine($"{rankString}. " + highScores[i] + nameSpacer + "  " + highScores[i + 1]);
                }

                rankInt++;
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press \"Enter\" to return to the main menu.");
            Console.ReadLine();
            Console.Clear();
        }

        static void DisplayExitMessage()
        {
            Console.WriteLine("Thank you for playing! Goodbye!\r\n");
        }
    }
}
