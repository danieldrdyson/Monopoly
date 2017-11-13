// Daniel Dyson
// Date Created: 7/17/17
// Last Modified: 11/12/17, 8:59 PM
// This is a recreation of the board game, Monopoly.
// Note: in the RegularTurn() method, I added another condition to the do...while loop to end a player's turn if they are not in the game
// anymore (if they rolled doubles and went bankrupt, then they should stop playing regardless if they rolled doubles or not)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Collections;
using static System.Random;
using System.IO;

namespace MonopolyGame
{
    class Monopoly
    {
        static ArrayList availableProperties;
        static ArrayList propertiesByGroup;
        static bool[] mortgagedProperties;
        static string[] PROPERTY_NAMES;
        static int[] PROPERTY_PRICES;
        static int[,] PROPERTY_RENTS;
        static string[] GROUP_NAMES;
        static int[] numberOfHousesOnEachBoardSpace;
        static Queue<int> communityChest;
        static Queue<int> chance;
        static ArrayList boardSpaces;
        static int[] rents;
        static int playerCount;
        static int[] playersMoney;
        static int[] playersTotalWorth;
        static ArrayList[] playersProperties;
        static Queue<int> playerIndicesStillInGame;
        static int[] playersDiceRollValue;
        static int[] playersPosition;
        static int[] playersNumOfGetOutOfJailFreeCards;
        static bool[] playersInJail;
        static int[] playersNumOfHouses;
        static int[] playersNumOfHotels;
        static int[] playersNumOfRailroads;
        static int[] playersNumOfUtilities;
        static int[] playersNumOfAttemptsToGetOutOfJail;
        static bool[] playersStillInGame;
        static string[] playersName;
        static Random rand;
        static int MAX_NUM_OF_BOARD_SPACES = 40;
        static int currentPlayerTurn;
        static int diceValue;

        static void Main(string[] args)
        {
            PrepareMonopoly();
        }

        public static void PrepareMonopoly()
        {
            Clear();
            WriteLine("Welcome to the game of Monopoly!");
            bool isValid = false;
            do
            {
                playerCount = RetrieveInput("Error - enter the number of players that will be playing (2 or more)", "How many players will be playing?");
                if (playerCount < 2)
                {
                    WriteLine("Error - enter the number of players that will be playing (2 or more)");
                }
                else
                {
                    isValid = true;
                }
            } while (isValid == false);
            playersMoney = new int[playerCount];
            playersTotalWorth = new int[playerCount];
            playersProperties = new ArrayList[playerCount];
            playerIndicesStillInGame = new Queue<int>();
            playersDiceRollValue = new int[playerCount];
            playersPosition = new int[playerCount];
            playersNumOfGetOutOfJailFreeCards = new int[playerCount];
            playersInJail = new bool[playerCount];
            playersNumOfHouses = new int[playerCount];
            playersNumOfHotels = new int[playerCount];
            playersNumOfRailroads = new int[playerCount];
            playersNumOfUtilities = new int[playerCount];
            playersNumOfAttemptsToGetOutOfJail = new int[playerCount];
            playersStillInGame = new bool[playerCount];
            playersName = new string[playerCount];
            rand = new Random();
            availableProperties = new ArrayList();
            propertiesByGroup = new ArrayList();
            mortgagedProperties = new bool[40];
            PROPERTY_NAMES = new string[40];
            PROPERTY_PRICES = new int[40];
            PROPERTY_RENTS = new int[40, 6];//[location,number of houses]
            GROUP_NAMES = new string[20];
            numberOfHousesOnEachBoardSpace = new int[40];//[location]
            communityChest = new Queue<int>();
            chance = new Queue<int>();
            boardSpaces = new ArrayList();
            rents = new int[40];
            for (int count = 0; count < 40; ++count)
            {
                numberOfHousesOnEachBoardSpace[count] = 0;
                mortgagedProperties[count] = false;
            }
            for (int count = 0; count < playerCount; ++count)
            {
                WriteLine($"Player {count + 1}, please enter your name");
                playersName[count] = ReadLine();
                playersMoney[count] = 1500;
                playersTotalWorth[count] = 1500;
                playersProperties[count] = new ArrayList();
                playersPosition[count] = 0;
                playersNumOfGetOutOfJailFreeCards[count] = 0;
                playersInJail[count] = false;
                playersNumOfHouses[count] = 0;
                playersNumOfHotels[count] = 0;
                playersNumOfRailroads[count] = 0;
                playersNumOfUtilities[count] = 0;
                playersNumOfAttemptsToGetOutOfJail[count] = 0;
                playersStillInGame[count] = true;
            }
            ShuffleCommunityChest();
            ShuffleChance();
            availableProperties.Add(1);
            availableProperties.Add(3);
            ArrayList colorGroup = new ArrayList();
            colorGroup.Add(1);
            colorGroup.Add(3);
            propertiesByGroup.Add(colorGroup.Clone());
            colorGroup.Clear();
            GROUP_NAMES[0] = "Purple Group";
            PROPERTY_NAMES[1] = "Mediterranean Avenue";
            PROPERTY_PRICES[1] = 60;
            PROPERTY_RENTS[1, 0] = 2;
            PROPERTY_RENTS[1, 1] = 10;
            PROPERTY_RENTS[1, 2] = 30;
            PROPERTY_RENTS[1, 3] = 90;
            PROPERTY_RENTS[1, 4] = 160;
            PROPERTY_RENTS[1, 5] = 250;
            PROPERTY_NAMES[3] = "Baltic Avenue";
            PROPERTY_PRICES[3] = 60;
            PROPERTY_RENTS[3, 0] = 4;
            PROPERTY_RENTS[3, 1] = 20;
            PROPERTY_RENTS[3, 2] = 60;
            PROPERTY_RENTS[3, 3] = 180;
            PROPERTY_RENTS[3, 4] = 320;
            PROPERTY_RENTS[3, 5] = 450;
            availableProperties.Add(5);
            availableProperties.Add(6);
            availableProperties.Add(8);
            availableProperties.Add(9);
            colorGroup.Add(6);
            colorGroup.Add(8);
            colorGroup.Add(9);
            propertiesByGroup.Add(colorGroup.Clone());
            colorGroup.Clear();
            GROUP_NAMES[1] = "Light-Blue Group";
            PROPERTY_NAMES[6] = "Oriental Avenue";
            PROPERTY_PRICES[6] = 100;
            PROPERTY_RENTS[6, 0] = 6;
            PROPERTY_RENTS[6, 1] = 30;
            PROPERTY_RENTS[6, 2] = 90;
            PROPERTY_RENTS[6, 3] = 270;
            PROPERTY_RENTS[6, 4] = 400;
            PROPERTY_RENTS[6, 5] = 550;
            PROPERTY_NAMES[8] = "Vermont Avenue";
            PROPERTY_PRICES[8] = 100;
            PROPERTY_RENTS[8, 0] = 6;
            PROPERTY_RENTS[8, 1] = 30;
            PROPERTY_RENTS[8, 2] = 90;
            PROPERTY_RENTS[8, 3] = 270;
            PROPERTY_RENTS[8, 4] = 400;
            PROPERTY_RENTS[8, 5] = 550;
            PROPERTY_NAMES[9] = "Connecticut Avenue";
            PROPERTY_PRICES[9] = 120;
            PROPERTY_RENTS[9, 0] = 8;
            PROPERTY_RENTS[9, 1] = 40;
            PROPERTY_RENTS[9, 2] = 100;
            PROPERTY_RENTS[9, 3] = 300;
            PROPERTY_RENTS[9, 4] = 450;
            PROPERTY_RENTS[9, 5] = 600;
            availableProperties.Add(11);
            availableProperties.Add(12);//Electric company
            availableProperties.Add(13);
            availableProperties.Add(14);
            colorGroup.Add(11);
            colorGroup.Add(13);
            colorGroup.Add(14);
            propertiesByGroup.Add(colorGroup.Clone());
            colorGroup.Clear();
            GROUP_NAMES[2] = "Pink Group";
            PROPERTY_NAMES[11] = "St. Charles Place";
            PROPERTY_PRICES[11] = 140;
            PROPERTY_RENTS[11, 0] = 10;
            PROPERTY_RENTS[11, 1] = 50;
            PROPERTY_RENTS[11, 2] = 150;
            PROPERTY_RENTS[11, 3] = 450;
            PROPERTY_RENTS[11, 4] = 625;
            PROPERTY_RENTS[11, 5] = 750;
            PROPERTY_NAMES[13] = "States Avenue";
            PROPERTY_PRICES[13] = 140;
            PROPERTY_RENTS[13, 0] = 10;
            PROPERTY_RENTS[13, 1] = 50;
            PROPERTY_RENTS[13, 2] = 150;
            PROPERTY_RENTS[13, 3] = 450;
            PROPERTY_RENTS[13, 4] = 625;
            PROPERTY_RENTS[13, 5] = 750;
            PROPERTY_NAMES[14] = "Virginia Avenue";
            PROPERTY_PRICES[14] = 160;
            PROPERTY_RENTS[14, 0] = 12;
            PROPERTY_RENTS[14, 1] = 60;
            PROPERTY_RENTS[14, 2] = 180;
            PROPERTY_RENTS[14, 3] = 500;
            PROPERTY_RENTS[14, 4] = 700;
            PROPERTY_RENTS[14, 5] = 900;
            availableProperties.Add(15);
            availableProperties.Add(16);
            availableProperties.Add(18);
            availableProperties.Add(19);
            colorGroup.Add(16);
            colorGroup.Add(18);
            colorGroup.Add(19);
            propertiesByGroup.Add(colorGroup.Clone());
            colorGroup.Clear();
            GROUP_NAMES[3] = "Gold Group";
            PROPERTY_NAMES[16] = "St. James Place";
            PROPERTY_PRICES[16] = 180;
            PROPERTY_RENTS[16, 0] = 14;
            PROPERTY_RENTS[16, 1] = 70;
            PROPERTY_RENTS[16, 2] = 200;
            PROPERTY_RENTS[16, 3] = 550;
            PROPERTY_RENTS[16, 4] = 750;
            PROPERTY_RENTS[16, 5] = 950;
            PROPERTY_NAMES[18] = "Tennessee Avenue";
            PROPERTY_PRICES[18] = 180;
            PROPERTY_RENTS[18, 0] = 14;
            PROPERTY_RENTS[18, 1] = 70;
            PROPERTY_RENTS[18, 2] = 200;
            PROPERTY_RENTS[18, 3] = 550;
            PROPERTY_RENTS[18, 4] = 750;
            PROPERTY_RENTS[18, 5] = 950;
            PROPERTY_NAMES[19] = "New York Avenue";
            PROPERTY_PRICES[19] = 200;
            PROPERTY_RENTS[19, 0] = 16;
            PROPERTY_RENTS[19, 1] = 80;
            PROPERTY_RENTS[19, 2] = 220;
            PROPERTY_RENTS[19, 3] = 600;
            PROPERTY_RENTS[19, 4] = 800;
            PROPERTY_RENTS[19, 5] = 1000;
            availableProperties.Add(21);
            availableProperties.Add(23);
            availableProperties.Add(24);
            colorGroup.Add(21);
            colorGroup.Add(23);
            colorGroup.Add(24);
            propertiesByGroup.Add(colorGroup.Clone());
            colorGroup.Clear();
            GROUP_NAMES[4] = "Red Group";
            PROPERTY_NAMES[21] = "Kentucky Avenue";
            PROPERTY_PRICES[21] = 220;
            PROPERTY_RENTS[21, 0] = 18;
            PROPERTY_RENTS[21, 1] = 90;
            PROPERTY_RENTS[21, 2] = 250;
            PROPERTY_RENTS[21, 3] = 700;
            PROPERTY_RENTS[21, 4] = 875;
            PROPERTY_RENTS[21, 5] = 1050;
            PROPERTY_NAMES[23] = "Indiana Avenue";
            PROPERTY_PRICES[23] = 220;
            PROPERTY_RENTS[23, 0] = 18;
            PROPERTY_RENTS[23, 1] = 90;
            PROPERTY_RENTS[23, 2] = 250;
            PROPERTY_RENTS[23, 3] = 700;
            PROPERTY_RENTS[23, 4] = 875;
            PROPERTY_RENTS[23, 5] = 1050;
            PROPERTY_NAMES[24] = "Illinois Avenue";
            PROPERTY_PRICES[24] = 240;
            PROPERTY_RENTS[24, 0] = 20;
            PROPERTY_RENTS[24, 1] = 100;
            PROPERTY_RENTS[24, 2] = 300;
            PROPERTY_RENTS[24, 3] = 750;
            PROPERTY_RENTS[24, 4] = 925;
            PROPERTY_RENTS[24, 5] = 1100;
            availableProperties.Add(25);
            availableProperties.Add(26);
            availableProperties.Add(27);
            availableProperties.Add(28);//Water company
            availableProperties.Add(29);
            colorGroup.Add(26);
            colorGroup.Add(27);
            colorGroup.Add(29);
            propertiesByGroup.Add(colorGroup.Clone());
            colorGroup.Clear();
            GROUP_NAMES[5] = "Yellow Group";
            PROPERTY_NAMES[26] = "Atlantic Avenue";
            PROPERTY_PRICES[26] = 260;
            PROPERTY_RENTS[26, 0] = 22;
            PROPERTY_RENTS[26, 1] = 110;
            PROPERTY_RENTS[26, 2] = 330;
            PROPERTY_RENTS[26, 3] = 800;
            PROPERTY_RENTS[26, 4] = 975;
            PROPERTY_RENTS[26, 5] = 1150;
            PROPERTY_NAMES[27] = "Ventnor Avenue";
            PROPERTY_PRICES[27] = 260;
            PROPERTY_RENTS[27, 0] = 22;
            PROPERTY_RENTS[27, 1] = 110;
            PROPERTY_RENTS[27, 2] = 330;
            PROPERTY_RENTS[27, 3] = 800;
            PROPERTY_RENTS[27, 4] = 975;
            PROPERTY_RENTS[27, 5] = 1150;
            PROPERTY_NAMES[29] = "Marvin Gardens";
            PROPERTY_PRICES[29] = 280;
            PROPERTY_RENTS[29, 0] = 24;
            PROPERTY_RENTS[29, 1] = 120;
            PROPERTY_RENTS[29, 2] = 360;
            PROPERTY_RENTS[29, 3] = 850;
            PROPERTY_RENTS[29, 4] = 1025;
            PROPERTY_RENTS[29, 5] = 1200;
            colorGroup.Add(12);
            colorGroup.Add(28);
            propertiesByGroup.Add(colorGroup.Clone());
            colorGroup.Clear();
            GROUP_NAMES[6] = "Utilities";
            PROPERTY_NAMES[12] = "Electric Company";
            PROPERTY_PRICES[12] = 150;
            PROPERTY_NAMES[28] = "Water Company";
            PROPERTY_PRICES[28] = 150;
            availableProperties.Add(31);
            availableProperties.Add(32);
            availableProperties.Add(34);
            colorGroup.Add(31);
            colorGroup.Add(32);
            colorGroup.Add(34);
            propertiesByGroup.Add(colorGroup.Clone());
            colorGroup.Clear();
            GROUP_NAMES[7] = "Green Group";
            PROPERTY_NAMES[31] = "Pacific Avenue";
            PROPERTY_PRICES[31] = 300;
            PROPERTY_RENTS[31, 0] = 26;
            PROPERTY_RENTS[31, 1] = 130;
            PROPERTY_RENTS[31, 2] = 390;
            PROPERTY_RENTS[31, 3] = 900;
            PROPERTY_RENTS[31, 4] = 1100;
            PROPERTY_RENTS[31, 5] = 1275;
            PROPERTY_NAMES[32] = "North Carolina Avenue";
            PROPERTY_PRICES[32] = 300;
            PROPERTY_RENTS[32, 0] = 26;
            PROPERTY_RENTS[32, 1] = 130;
            PROPERTY_RENTS[32, 2] = 390;
            PROPERTY_RENTS[32, 3] = 900;
            PROPERTY_RENTS[32, 4] = 1100;
            PROPERTY_RENTS[32, 5] = 1275;
            PROPERTY_NAMES[34] = "Pennsylvania Avenue";
            PROPERTY_PRICES[34] = 320;
            PROPERTY_RENTS[34, 0] = 28;
            PROPERTY_RENTS[34, 1] = 150;
            PROPERTY_RENTS[34, 2] = 450;
            PROPERTY_RENTS[34, 3] = 1000;
            PROPERTY_RENTS[34, 4] = 1200;
            PROPERTY_RENTS[34, 5] = 1400;
            availableProperties.Add(35);
            colorGroup.Add(5);
            colorGroup.Add(15);
            colorGroup.Add(25);
            colorGroup.Add(35);
            propertiesByGroup.Add(colorGroup.Clone());
            colorGroup.Clear();
            GROUP_NAMES[8] = "Railroads";
            PROPERTY_NAMES[5] = "Reading Railroad";
            PROPERTY_PRICES[5] = 200;
            PROPERTY_NAMES[15] = "Pennsylvania Railroad";
            PROPERTY_PRICES[15] = 200;
            PROPERTY_NAMES[25] = "B. & O. Railroad";
            PROPERTY_PRICES[25] = 200;
            PROPERTY_NAMES[35] = "Short Line";
            PROPERTY_PRICES[35] = 200;
            availableProperties.Add(37);
            availableProperties.Add(39);
            colorGroup.Add(37);
            colorGroup.Add(39);
            propertiesByGroup.Add(colorGroup.Clone());
            colorGroup.Clear();
            GROUP_NAMES[9] = "Blue Group";
            PROPERTY_NAMES[37] = "Park Place";
            PROPERTY_PRICES[37] = 350;
            PROPERTY_RENTS[37, 0] = 35;
            PROPERTY_RENTS[37, 1] = 175;
            PROPERTY_RENTS[37, 2] = 500;
            PROPERTY_RENTS[37, 3] = 1100;
            PROPERTY_RENTS[37, 4] = 1300;
            PROPERTY_RENTS[37, 5] = 1500;
            PROPERTY_NAMES[39] = "Boardwalk";
            PROPERTY_PRICES[39] = 400;
            PROPERTY_RENTS[39, 0] = 50;
            PROPERTY_RENTS[39, 1] = 200;
            PROPERTY_RENTS[39, 2] = 600;
            PROPERTY_RENTS[39, 3] = 1400;
            PROPERTY_RENTS[39, 4] = 1700;
            PROPERTY_RENTS[39, 5] = 2000;
            PlayMonopoly();
        }

        public static void PlayMonopoly()
        {
            ChooseStartingPlayer();
            do
            {
                PlayerTurn(playerIndicesStillInGame.Dequeue());
            } while (playerIndicesStillInGame.Count > 1);
            int playerWinner = playerIndicesStillInGame.Dequeue();
            WriteLine($"{playersName[playerWinner]} wins!!!");
            ReadKey();
            WriteToLeaderboards(playerWinner);
            DisplayLeaderboards();
        }

        public static void ShuffleCommunityChest()
        {
            ArrayList identifiers = new ArrayList();
            int cardCount = 16;
            for (int identifier = 0; identifier < cardCount; ++identifier)
            {
                identifiers.Add(identifier);
            }
            for (int count = 0; count < 100; ++count)
            {
                for (int index = 0; index < cardCount; ++index)
                {
                    int randIndex = rand.Next(0, cardCount);
                    object temp = identifiers[index];
                    identifiers[index] = identifiers[randIndex];
                    identifiers[randIndex] = temp;
                }
            }
            for (int count = 0; count < cardCount; ++count)
            {
                communityChest.Enqueue(Convert.ToInt32(identifiers[count]));
            }
        }

        public static void ShuffleChance()
        {
            ArrayList identifiers = new ArrayList();
            int cardCount = 16;
            for (int identifier = 0; identifier < cardCount; ++identifier)
            {
                identifiers.Add(identifier);
            }
            for (int count = 0; count < 100; ++count)
            {
                for (int index = 0; index < cardCount; ++index)
                {
                    int randIndex = rand.Next(0, cardCount);
                    object temp = identifiers[index];
                    identifiers[index] = identifiers[randIndex];
                    identifiers[randIndex] = temp;
                }
            }
            for (int count = 0; count < cardCount; ++count)
            {
                chance.Enqueue(Convert.ToInt32(identifiers[count]));
            }
        }

        public static void ChooseStartingPlayer()
        {
            WriteLine("A player will be chosen to go first");
            for (int count = 0; count < playerCount; ++count)
            {
                playersDiceRollValue[count] = RollDice();
                WriteLine($"{playersName[count]} rolled {playersDiceRollValue[count]}");
            }
            int greatestVal = playersDiceRollValue.Max();
            ArrayList playerIndexWithGreatestVal = new ArrayList();
            for (int count = 0; count < playerCount; ++count)
            {
                if (playersDiceRollValue[count] == greatestVal)
                {
                    playerIndexWithGreatestVal.Add(count);
                }
            }
            if (playerIndexWithGreatestVal.Count > 1)
            {
                bool startingPlayerChosen = false;
                while (startingPlayerChosen == false)
                {
                    WriteLine("Two or more players tied in their rolls");
                    ReadKey();
                    int[] playerVals = new int[playerIndexWithGreatestVal.Count];
                    int count = 0;
                    foreach (int index in playerIndexWithGreatestVal)
                    {
                        playersDiceRollValue[index] = RollDice();
                        playerVals[count] = playersDiceRollValue[index];
                        ++count;
                        WriteLine($"{playersName[index]} rolled {playersDiceRollValue[index]}");
                    }
                    greatestVal = playerVals.Max();
                    for (count = playerIndexWithGreatestVal.Count - 1; count >= 0; --count)
                    {
                        if ((playersDiceRollValue[Convert.ToInt32(playerIndexWithGreatestVal[count])]) != greatestVal)
                        {
                            playerIndexWithGreatestVal.RemoveAt(count);
                        }
                    }
                    if (playerIndexWithGreatestVal.Count == 1)
                    {
                        startingPlayerChosen = true;
                        WriteLine($"{playersName[(int)(playerIndexWithGreatestVal[0])]} will start the game");
                    }
                }
            }
            else
            {
                WriteLine($"{playersName[(int)(playerIndexWithGreatestVal[0])]} will start the game");
            }
            int indexCount = Convert.ToInt32(playerIndexWithGreatestVal[0]);
            for (int count = 0; count < playerCount; ++count)
            {
                playerIndicesStillInGame.Enqueue(indexCount);
                ++indexCount;
                if (indexCount == playerCount)
                {
                    indexCount = 0;
                }
            }
            ReadKey();
        }

        public static int RollDice()
        {
            int die1, die2;
            int total;
            die1 = rand.Next(1, 7);
            die2 = rand.Next(1, 7);
            total = die1 + die2;
            return total;
        }

        public static int RollDice(out bool rollDoubles)
        {
            int die1, die2;
            int total;
            rollDoubles = false;
            die1 = rand.Next(1, 7);
            die2 = rand.Next(1, 7);
            total = die1 + die2;
            if (die1 == die2)
            {
                rollDoubles = true;
            }
            return total;
        }

        public static void PlayerTurn(int player)
        {
            currentPlayerTurn = player;
            string header = $"\n{playersName[player]}";
            Clear();
            WriteLine(header);
            if (playersInJail[player])
            {
                JailTurn(player);
            }
            else
            {
                RegularTurn(player);
            }
            if (playersStillInGame[player])
            {
                playerIndicesStillInGame.Enqueue(player);
            }
            else
            {
                WriteLine($"{playersName[player]} went bankrupt!");
                ReadKey();
            }
        }

        public static void PreTurn(int player)
        {
            bool canBuyHouses = false;
            bool hasMortgagedProperties;
            int choice;
            bool playerWillBeginTurn = false;
            do
            {
                WriteLine();
                WriteLine($"Your money: {playersMoney[player]:c0}");
                WriteLine($"Your total worth: {playersTotalWorth[player]:c0}");
                WriteLine("Your properties:");
                WriteLine(ReturnPlayerProperties(player));
                canBuyHouses = IdentifyIfPlayerCanBuyHouses(player);
                ArrayList mortgagedProps;
                hasMortgagedProperties = false;
                ReturnPlayerMortgagedProperties(player, out mortgagedProps);
                if (mortgagedProps.Count > 0)
                {
                    hasMortgagedProperties = true;
                }
                //0:Begin turn, 1:Buy houses, 2: Unmortgage, 3:Make Trade
                ArrayList availableCommands = new ArrayList();
                availableCommands.Add(0);
                if (canBuyHouses)
                {
                    availableCommands.Add(1);
                }
                if (hasMortgagedProperties)
                {
                    availableCommands.Add(2);
                }
                int index = 0;
                foreach (ArrayList opponent in playersProperties)
                {
                    if (index != player)
                    {
                        if (opponent.Count > 0)
                        {
                            if (availableCommands.Contains(3) == false)
                            {
                                availableCommands.Add(3);
                            }
                        }
                    }
                    ++index;
                }
                if (playersNumOfGetOutOfJailFreeCards[player] > 0)
                {
                    availableCommands.Add(4);
                }
                index = 0;
                foreach (ArrayList opponent in playersProperties)
                {
                    if (index != player)
                    {
                        if (opponent.Count > 0)
                        {
                            if (availableCommands.Contains(5) == false)
                            {
                                availableCommands.Add(5);
                            }
                        }
                    }
                    ++index;
                }
                if (availableCommands.Count > 1)
                {
                    string prompt = $"{playersName[player]}, what would you like to do?:\n0:Begin your turn";
                    string error = "Error - choice not recognized";
                    bool isValid = false;
                    if (availableCommands.Contains(1))
                    {
                        prompt += "\n1:Buy houses";
                    }
                    if (availableCommands.Contains(2))
                    {
                        prompt += "\n2:Unmortgage properties";
                    }
                    if (availableCommands.Contains(3))
                    {
                        prompt += "\n3:Make a Trade";
                    }
                    if (availableCommands.Contains(4))
                    {
                        prompt += "\n4:Sell a Get Out of Jail Free card";
                    }
                    if (availableCommands.Contains(5))
                    {
                        prompt += "\n5:View other players' properties";
                    }
                    do
                    {
                        choice = RetrieveInput(error, prompt);
                        foreach (int command in availableCommands)
                        {
                            if (command == choice)
                            {
                                isValid = true;
                            }
                        }
                        if (isValid == false)
                        {
                            WriteLine(error);
                            ReadKey();
                        }

                    } while (isValid == false);
                    if (choice == 0)
                    {
                        playerWillBeginTurn = true;
                    }
                    else if (choice == 1)
                    {
                        BuyHouses(player);
                    }
                    else if (choice == 2)
                    {
                        UnmortgageProperty(player);
                    }
                    else if (choice == 3)
                    {
                        prompt = $"{playersName[player]}, which player would you like to make a trade with?";
                        error = "Error - choose the player you would like to make a trade with";
                        for (int count = 0; count < playerCount; ++count)
                        {
                            if (count != player)
                            {
                                prompt += $"\n{count}:{playersName[count]}";
                            }
                        }
                        isValid = false;
                        if (playerCount > 2)
                        {
                            do
                            {

                                choice = RetrieveInput(error, prompt);
                                for (int count = 0; count < playerCount; ++count)
                                {
                                    if (count != player)
                                    {
                                        if (choice == count)
                                        {
                                            isValid = true;
                                        }
                                    }
                                }
                                if (isValid == false)
                                {
                                    WriteLine(error);
                                    ReadKey();
                                }
                            } while (isValid == false);
                        }
                        else
                        {
                            for (int count = 0; count < playerCount; ++count)
                            {
                                if (count != player)
                                {
                                    choice = count;
                                }
                            }
                        }
                        MakeTradeWithPlayer(player, choice);
                    }
                    else if (choice == 4)
                    {
                        --playersNumOfGetOutOfJailFreeCards[player];
                        playersMoney[player] += 50;
                        WriteLine("You have sold your Get Out of Jail Free Card");
                        ReadKey();
                    }
                    else if (choice == 5)
                    {
                        for (int count = 0; count < playerCount; ++count)
                        {
                            if (count != player)
                            {
                                WriteLine($"{playersName[count]}");
                                WriteLine(ReturnPlayerProperties(count));
                            }
                        }
                        ReadKey();
                    }
                }
                else
                {
                    playerWillBeginTurn = true;
                }
            } while (playerWillBeginTurn == false);
        }

        public static void JailTurn(int player)
        {
            PreTurn(player);
            string[] choices = { "0:Use Get Out of Jail Free Card", $"1:Try rolling doubles ({playersNumOfAttemptsToGetOutOfJail[player]} attempts left)", "2:Pay $50" };
            ArrayList availableChoices = new ArrayList();
            if (playersNumOfGetOutOfJailFreeCards[player] > 0)
            {
                availableChoices.Add(0);
            }
            if (playersNumOfAttemptsToGetOutOfJail[player] > 0)
            {
                availableChoices.Add(1);
            }
            availableChoices.Add(2);
            string prompt = $"\n{playersName[player]}, you are currently in jail. You must choose one of the following options:\n";
            foreach (int option in availableChoices)
            {
                prompt += $"{choices[option]}\n";
            }
            bool rollDoubles = false;
            bool usedFreeCardOrPaidMoney = false;
            string messageUsedLastAttempt = "You used your last attempt. You must pay $50, or use a Get Out of Jail Free Card.";
            string messageRolledDoubles = "You rolled doubles! You got out of jail for free";
            string messageChoseFreeCard = "You have chosen to use a Get Out of Jail Free card";
            string messageChoseToRollDoubles = "You have chosen to roll doubles";
            string messageChoseToPay = "You have chosen to pay $50";
            if (availableChoices.Count == 1)
            {
                WriteLine("You have no other choice but to pay $50 to get out of jail");
                if (playersMoney[player] < 50)
                {
                    SellAndMortgageProperty(player, 50);
                }
                playersMoney[player] -= 50;
                playersTotalWorth[player] -= 50;
                usedFreeCardOrPaidMoney = true;
            }
            else
            {
                string error = "Error - choose one of the following options";
                int choice;
                bool isValid = false;
                do
                {
                    choice = RetrieveInput(error, prompt);
                    foreach (int option in availableChoices)
                    {
                        if (option == choice)
                        {
                            isValid = true;
                        }
                    }
                    if (isValid == false)
                    {
                        WriteLine(error);
                        ReadKey();
                    }
                } while (isValid == false);
                isValid = false;
                if (choice == 0)
                {//use free card
                    WriteLine(messageChoseFreeCard);
                    --playersNumOfGetOutOfJailFreeCards[player];
                    if (communityChest.Count > chance.Count)
                    {
                        chance.Enqueue(7);
                    }
                    else
                    {
                        communityChest.Enqueue(4);
                    }
                    playersInJail[player] = false;
                    playersTotalWorth[player] -= 50;
                    usedFreeCardOrPaidMoney = true;
                }
                else if (choice == 1)
                {//roll doubles
                    WriteLine(messageChoseToRollDoubles);
                    --playersNumOfAttemptsToGetOutOfJail[player];
                    RollDice(out rollDoubles);
                    if (rollDoubles)
                    {
                        WriteLine(messageRolledDoubles);
                        playersInJail[player] = false;
                    }
                    else
                    {
                        WriteLine("You did not roll doubles.");
                        if (playersNumOfAttemptsToGetOutOfJail[player] == 0)
                        {
                            WriteLine(messageUsedLastAttempt);
                            if (playersNumOfGetOutOfJailFreeCards[player] > 0)
                            {
                                prompt = "What would you like to do?\n0:Use a Get Out of Jail Free Card\n1:Pay $50";
                                error = "Error - choose one of the following options";
                                isValid = false;
                                do
                                {
                                    choice = RetrieveInput(error, prompt);
                                    if (choice == 0 || choice == 1)
                                    {
                                        isValid = true;
                                    }
                                    else
                                    {
                                        WriteLine(error);
                                        ReadKey();
                                    }
                                } while (isValid == false);
                                if (choice == 0)
                                {
                                    --playersNumOfGetOutOfJailFreeCards[player];
                                    if (communityChest.Count > chance.Count)
                                    {
                                        chance.Enqueue(7);
                                    }
                                    else
                                    {
                                        communityChest.Enqueue(4);
                                    }
                                    playersTotalWorth[player] -= 50;
                                }
                                else
                                {
                                    if (playersMoney[player] < 50)
                                    {
                                        SellAndMortgageProperty(player, 50);
                                    }
                                    playersMoney[player] -= 50;
                                    playersTotalWorth[player] -= 50;
                                }
                            }
                            else
                            {
                                if (playersMoney[player] < 50)
                                {
                                    SellAndMortgageProperty(player, 50);
                                }
                                playersMoney[player] -= 50;
                                playersTotalWorth[player] -= 50;
                            }
                            playersInJail[player] = false;
                        }
                    }
                }
                else
                {//pay $50
                    WriteLine(messageChoseToPay);
                    if (playersMoney[player] < 50)
                    {
                        SellAndMortgageProperty(player, 50);
                    }
                    playersMoney[player] -= 50;
                    playersTotalWorth[player] -= 50;
                    usedFreeCardOrPaidMoney = true;
                    playersInJail[player] = false;
                }
            }
            if (playersInJail[player] == false)
            {
                WriteLine("You are now out of jail.");
                ReadKey();
                if (usedFreeCardOrPaidMoney)
                {
                    RegularTurn(player);
                }
                else if (rollDoubles)
                {
                    RegularTurnOnce(player);
                }
            }
            else
            {
                ReadKey();
            }
        }

        public static void RegularTurn(int player)
        {
            int distanceToTravel;
            bool rollDoubles = false;
            int numOfDoubles = 0;
            bool playerTurnEnded = false;
            do
            {
                PreTurn(player);
                WriteLine($"\n{playersName[player]} will now roll");
                distanceToTravel = RollDice(out rollDoubles);
                diceValue = distanceToTravel;
                WriteLine($"{playersName[player]} rolled {distanceToTravel}");
                if (rollDoubles)
                {
                    ++numOfDoubles;
                    WriteLine("The roll was doubles");
                }
                if (numOfDoubles == 3)
                {
                    playersPosition[player] = 10;
                    playerTurnEnded = true;
                    playersInJail[player] = true;
                    WriteLine($"{playersName[player]} rolled three consecutive doubles - Go to Jail");
                }
                else
                {
                    WriteLine("The player moves that distance");
                    playersPosition[player] += distanceToTravel;
                    if (playersPosition[player] >= MAX_NUM_OF_BOARD_SPACES)
                    {
                        playersPosition[player] -= MAX_NUM_OF_BOARD_SPACES;
                        playersMoney[player] += 200;
                        playersTotalWorth[player] += 200;
                        WriteLine("You passed GO - collect $200");

                    }
                    ActivateBoardSpace(playersPosition[player]);
                }
                ReadKey();
                if (playersInJail[player])
                {
                    playerTurnEnded = true;
                }
                if (playersTotalWorth[player] == 0)
                {
                    playerTurnEnded = true;
                    playersStillInGame[player] = false;
                }
            } while (playerTurnEnded == false && rollDoubles == true && playersStillInGame[player] == true);
        }

        public static void RegularTurnOnce(int player)
        {
            int distanceToTravel;
            WriteLine($"{playersName[player]} will now roll");
            distanceToTravel = RollDice();
            diceValue = distanceToTravel;
            WriteLine($"{playersName[player]} rolled {distanceToTravel}");
            WriteLine("The player moves that distance");
            playersPosition[player] += distanceToTravel;
            if (playersPosition[player] >= MAX_NUM_OF_BOARD_SPACES)
            {
                playersPosition[player] -= MAX_NUM_OF_BOARD_SPACES;
                playersMoney[player] += 200;
                playersTotalWorth[player] += 200;
                WriteLine("You passed GO - collect $200");
            }
            ActivateBoardSpace(playersPosition[player]);
            ReadKey();
        }

        public static void ActivateBoardSpace(int location)
        {
            int cost = 0;
            switch (location)
            {
                case 0://GO Space
                    WriteLine("You landed on GO");
                    break;
                case 1:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 2://Draw community chest card
                    WriteLine("You landed on Community Chest");
                    IdentifyCommunityChestCard(communityChest.Dequeue());
                    break;
                case 3:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 4:
                    cost = 200;
                    WriteLine("You landed on Income tax - Pay $200");
                    if (playersMoney[currentPlayerTurn] < cost)
                    {
                        SellAndMortgageProperty(currentPlayerTurn, cost);
                    }
                    playersMoney[currentPlayerTurn] -= cost;
                    playersTotalWorth[currentPlayerTurn] -= cost;
                    break;
                case 5:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 6:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 7:
                    WriteLine("You landed on Chance");
                    IdentifyChanceCard(chance.Dequeue());
                    break;
                case 8:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 9:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 10://Jail
                    WriteLine("You landed on Jail - Just visiting ");
                    break;
                case 11://St. Charles Place
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 12://Electric Company
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 13:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 14:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 15:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 16:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 17:
                    WriteLine("You landed on Community Chest");
                    IdentifyCommunityChestCard(communityChest.Dequeue());
                    break;
                case 18:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 19:
                    WriteLine($"You landed on {PROPERTY_NAMES[19]}");
                    ActivatePropertySpace(location);
                    break;
                case 20://Free parking
                    WriteLine("You landed on Free parking ");
                    break;
                case 21:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 22:
                    WriteLine("You landed on Chance");
                    IdentifyChanceCard(chance.Dequeue());
                    break;
                case 23:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 24://Illinois Ave
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 25:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 26:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 27:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 28://Water Company
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 29:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 30://Go to jail
                    WriteLine("You landed on \"Go to Jail\" - you must go to Jail");
                    playersPosition[currentPlayerTurn] = 10;
                    playersInJail[currentPlayerTurn] = true;
                    playersNumOfAttemptsToGetOutOfJail[currentPlayerTurn] = 3;
                    break;
                case 31:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 32:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 33:
                    WriteLine("You landed on Community Chest");
                    IdentifyCommunityChestCard(communityChest.Dequeue());
                    break;
                case 34:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 35:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 36:
                    WriteLine("You landed on Chance");
                    IdentifyChanceCard(chance.Dequeue());
                    break;
                case 37:
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
                case 38:
                    cost = 75;
                    WriteLine("Luxury tax - Pay $75");
                    if (playersMoney[currentPlayerTurn] < cost)
                    {
                        SellAndMortgageProperty(currentPlayerTurn, cost);
                    }
                    playersMoney[currentPlayerTurn] -= cost;
                    playersTotalWorth[currentPlayerTurn] -= cost;
                    break;
                case 39://Boardwalk
                    WriteLine($"You landed on {PROPERTY_NAMES[location]}");
                    ActivatePropertySpace(location);
                    break;
            }
        }

        public static void ActivatePropertySpace(int location, bool chanceOrCommunityChestCard = false)
        {
            bool isOwned = !availableProperties.Contains(location);
            bool allColoredPropertiesOwned = true;
            int groupIdentifier = 0;
            int playerWinner = -1;
            if (isOwned)
            {
                if (playersProperties[currentPlayerTurn].Contains(location) == false)//If the current player does not own this property...
                {
                    WriteLine($"Another player owns {PROPERTY_NAMES[location]}");
                    groupIdentifier = IdentifyPropertyGroup(location);
                    playerWinner = IdentifyPropertyOwner(location);
                    allColoredPropertiesOwned = IdentifyIfAllPropertiesOfGroupAreOwned(playerWinner, groupIdentifier);
                    int rent = 0;
                    if (location == 5 || location == 15 || location == 25 || location == 35)
                    {
                        rent = 25;
                        for (int count = 0; count < playersNumOfRailroads[playerWinner] - 1; ++count)
                        {
                            rent *= 2;
                        }
                        if (chanceOrCommunityChestCard)
                        {
                            rent *= 2;
                        }
                    }
                    else if (location == 12 || location == 28)
                    {
                        if (chanceOrCommunityChestCard)
                        {
                            WriteLine("The nearest utility is already owned. You must roll the dice, and pay the owner ten times the amount shown");
                            ReadKey();
                            rent = RollDice();
                            WriteLine($"You rolled {rent}. You must pay the owner {rent * 10:c0}");
                            rent *= 10;
                            ReadKey();
                        }
                        if (playersNumOfUtilities[playerWinner] == 1)
                        {
                            rent = diceValue * 4;
                        }
                        else if (playersNumOfUtilities[playerWinner] == 2)
                        {
                            rent = diceValue * 10;
                        }
                    }
                    else
                    {
                        int numOfHouses = numberOfHousesOnEachBoardSpace[location];
                        rent = PROPERTY_RENTS[location, numOfHouses];
                        if (allColoredPropertiesOwned && numOfHouses == 0)
                        {
                            rent *= 2;
                        }
                    }
                    if (rent > playersMoney[currentPlayerTurn])
                    {
                        SellAndMortgageProperty(currentPlayerTurn, rent);
                    }
                    int amountPaid;
                    if (playersMoney[currentPlayerTurn] <= rent)
                    {
                        playersMoney[playerWinner] += playersMoney[currentPlayerTurn];
                        playersTotalWorth[playerWinner] += playersMoney[currentPlayerTurn];
                        amountPaid = playersMoney[currentPlayerTurn];
                        playersMoney[currentPlayerTurn] = 0;
                    }
                    else
                    {
                        playersMoney[currentPlayerTurn] -= rent;
                        playersTotalWorth[currentPlayerTurn] -= rent;
                        playersMoney[playerWinner] += rent;
                        playersTotalWorth[playerWinner] += rent;
                        amountPaid = rent;
                    }
                    WriteLine($"{playersName[currentPlayerTurn]} pays {playersName[playerWinner]} {amountPaid:c0}");
                }
                else
                {
                    WriteLine($"You already own {PROPERTY_NAMES[location]}");
                }
            }
            else
            {
                WriteLine($"\n{PROPERTY_NAMES[location]} is not owned");
                if (playersTotalWorth[currentPlayerTurn] >= PROPERTY_PRICES[location])
                {
                    string prompt = $"Would you like to buy {PROPERTY_NAMES[location]}?\nYour money: {playersMoney[currentPlayerTurn]:c0}" +
                    $"\nYour total worth: {playersTotalWorth[currentPlayerTurn]:c0}\nProperty Price: {PROPERTY_PRICES[location]:c0}" +
                    "\n0:Yes, I would like to buy this property\n1:No, I do not want this property";
                    string error = "Error - choose whether or not you would like to buy this property";
                    int choice;
                    bool isValid = false;
                    do
                    {
                        choice = RetrieveInput(error, prompt);
                        if (choice == 0 || choice == 1)
                        {
                            isValid = true;
                        }
                        else
                        {
                            WriteLine(error);
                            ReadKey();
                        }
                    } while (isValid == false);
                    if (choice == 0)
                    {
                        BuyProperty(location);
                        availableProperties.Remove(location);
                        playersProperties[currentPlayerTurn].Add(location);
                        WriteLine($"You have successfully bought {PROPERTY_NAMES[location]}, {playersName[currentPlayerTurn]}");
                    }
                    else
                    {
                        WriteLine($"You have chosen not to buy {PROPERTY_NAMES[location]}. The property will now go up for auction");
                        ReadKey();
                        AuctionProperty(location, currentPlayerTurn);
                    }
                }
                else
                {
                    WriteLine("You do not have enough money or properties to buy this property. It will now go to auction");
                    ReadKey();
                    AuctionProperty(location, currentPlayerTurn);
                }
            }
        }

        public static void IdentifyCommunityChestCard(int identifier)
        {
            int cost = 0;
            switch (identifier)
            {
                case 0:
                    playersPosition[currentPlayerTurn] = 0;
                    playersMoney[currentPlayerTurn] += 200;
                    playersTotalWorth[currentPlayerTurn] += 200;
                    WriteLine("Advance to Go (Collect $200) ");
                    break;
                case 1:
                    playersMoney[currentPlayerTurn] += 200;
                    playersTotalWorth[currentPlayerTurn] += 200;
                    WriteLine("Bank error in your favor – Collect $200 ");
                    break;
                case 2:
                    cost = 50;
                    WriteLine("Doctor's fees {fee} – Pay $50 ");
                    if (playersMoney[currentPlayerTurn] < cost)
                    {
                        SellAndMortgageProperty(currentPlayerTurn, cost);
                    }
                    playersMoney[currentPlayerTurn] -= cost;
                    playersTotalWorth[currentPlayerTurn] -= cost;
                    break;
                case 3:
                    playersMoney[currentPlayerTurn] += 45;
                    playersTotalWorth[currentPlayerTurn] += 45;
                    WriteLine("From sale of stock you get $45 ");
                    break;
                case 4:
                    ++playersNumOfGetOutOfJailFreeCards[currentPlayerTurn];
                    WriteLine("Get Out of Jail Free {Get out of Jail, Free} – This card may be kept until needed or sold ");
                    break;
                case 5:
                    playersPosition[currentPlayerTurn] = 10;
                    playersInJail[currentPlayerTurn] = true;
                    playersNumOfAttemptsToGetOutOfJail[currentPlayerTurn] = 3;
                    WriteLine("Go to Jail – Go directly to jail – Do not pass Go – Do not collect $200 ");
                    break;
                case 6:
                    WriteLine("Grand Opera Night Opening – Collect $50 from every player for opening night seats ");
                    cost = 50;
                    for (int count = 0; count < playerCount; ++count)
                    {
                        if (count != currentPlayerTurn)
                        {
                            if (cost > playersMoney[count])
                            {
                                SellAndMortgageProperty(currentPlayerTurn, cost);
                            }
                            playersMoney[count] -= 50;
                            playersTotalWorth[count] -= 50;
                            playersMoney[currentPlayerTurn] += 50;
                            playersTotalWorth[currentPlayerTurn] += 50;
                        }
                    }
                    break;
                case 7:
                    playersMoney[currentPlayerTurn] += 100;
                    playersTotalWorth[currentPlayerTurn] += 100;
                    WriteLine("Xmas Fund matures - Collect $100 ");
                    break;
                case 8:
                    playersMoney[currentPlayerTurn] += 20;
                    playersTotalWorth[currentPlayerTurn] += 20;
                    WriteLine("Income tax refund – Collect $20 ");
                    break;
                case 9:
                    playersMoney[currentPlayerTurn] += 100;
                    playersTotalWorth[currentPlayerTurn] += 100;
                    WriteLine("Life insurance matures – Collect $100 ");
                    break;
                case 10:
                    WriteLine("Pay hospital $100 ");
                    cost = 100;
                    if (playersMoney[currentPlayerTurn] < cost)
                    {
                        SellAndMortgageProperty(currentPlayerTurn, cost);
                    }
                    playersMoney[currentPlayerTurn] -= cost;
                    playersTotalWorth[currentPlayerTurn] -= cost;
                    break;
                case 11:
                    WriteLine("Pay school tax of $150 ");
                    cost = 150;
                    if (playersMoney[currentPlayerTurn] < cost)
                    {
                        SellAndMortgageProperty(currentPlayerTurn, cost);
                    }
                    playersMoney[currentPlayerTurn] -= cost;
                    playersTotalWorth[currentPlayerTurn] -= cost;
                    break;
                case 12:
                    playersMoney[currentPlayerTurn] += 25;
                    playersTotalWorth[currentPlayerTurn] += 25;
                    WriteLine("Receive for services $25 ");
                    break;
                case 13:
                    cost = (40 * playersNumOfHouses[currentPlayerTurn]) + (115 * playersNumOfHotels[currentPlayerTurn]);
                    WriteLine("You are assessed for street repairs – $40 per house – $115 per hotel ");
                    if (playersMoney[currentPlayerTurn] < cost)
                    {
                        SellAndMortgageProperty(currentPlayerTurn, cost);
                    }
                    playersMoney[currentPlayerTurn] -= cost;
                    playersTotalWorth[currentPlayerTurn] -= cost;
                    break;
                case 14:
                    playersMoney[currentPlayerTurn] += 10;
                    playersTotalWorth[currentPlayerTurn] += 10;
                    WriteLine("You have won second prize in a beauty contest – Collect $10 ");
                    break;
                case 15:
                    playersMoney[currentPlayerTurn] += 100;
                    playersTotalWorth[currentPlayerTurn] += 100;
                    WriteLine("You inherit $100 ");
                    break;
            }
            if (identifier != 4)
            {
                communityChest.Enqueue(identifier);
            }
        }

        public static void IdentifyChanceCard(int identifier)
        {
            int initialPosition = playersPosition[currentPlayerTurn];
            int cost = 0;
            switch (identifier)
            {
                case 0:
                    playersPosition[currentPlayerTurn] = 0;
                    playersMoney[currentPlayerTurn] += 200;
                    playersTotalWorth[currentPlayerTurn] += 200;
                    WriteLine("Advance to Go (Collect $200) ");
                    break;
                case 1:
                    playersPosition[currentPlayerTurn] = 24;
                    if (initialPosition > 24)
                    {
                        playersMoney[currentPlayerTurn] += 200;
                        playersTotalWorth[currentPlayerTurn] += 200;
                    }
                    WriteLine("Advance to Illinois Ave - If you pass Go, collect $200 ");
                    ActivatePropertySpace(24);
                    break;
                case 2:
                    playersPosition[currentPlayerTurn] = 11;
                    if (initialPosition > 11)
                    {
                        playersMoney[currentPlayerTurn] += 200;
                        playersTotalWorth[currentPlayerTurn] += 200;
                    }
                    WriteLine("Advance to St. Charles Place – If you pass Go, collect $200 ");
                    ActivatePropertySpace(11);
                    break;
                case 3:
                    WriteLine("Advance token to nearest Utility. If unowned, you may buy it from the Bank. \n" +
                        "If owned, throw dice and pay owner a total ten times the amount thrown.");
                    while (playersPosition[currentPlayerTurn] != 12 && playersPosition[currentPlayerTurn] != 28)
                    {
                        ++playersPosition[currentPlayerTurn];
                        if (playersPosition[currentPlayerTurn] >= MAX_NUM_OF_BOARD_SPACES)
                        {
                            playersPosition[currentPlayerTurn] -= MAX_NUM_OF_BOARD_SPACES;
                            playersMoney[currentPlayerTurn] += 200;
                            playersTotalWorth[currentPlayerTurn] += 200;
                        }
                    }
                    ActivatePropertySpace(playersPosition[currentPlayerTurn]);
                    break;
                case 4:
                case 5:
                    WriteLine("Advance token to the nearest Railroad and pay owner twice the rental to which he/she {he} is otherwise entitled. \n" +
                        "If Railroad is unowned, you may buy it from the Bank. ");
                    while (playersPosition[currentPlayerTurn] != 5 && playersPosition[currentPlayerTurn] != 15 && playersPosition[currentPlayerTurn] != 25 && playersPosition[currentPlayerTurn] != 35)
                    {
                        ++playersPosition[currentPlayerTurn];
                        if (playersPosition[currentPlayerTurn] >= MAX_NUM_OF_BOARD_SPACES)
                        {
                            playersPosition[currentPlayerTurn] -= MAX_NUM_OF_BOARD_SPACES;
                            playersMoney[currentPlayerTurn] += 200;
                            playersTotalWorth[currentPlayerTurn] += 200;
                        }
                    }
                    ActivatePropertySpace(playersPosition[currentPlayerTurn]);
                    break;
                case 6:
                    playersMoney[currentPlayerTurn] += 50;
                    playersTotalWorth[currentPlayerTurn] += 50;
                    WriteLine("Bank pays you dividend of $50 ");
                    break;
                case 7:
                    ++playersNumOfGetOutOfJailFreeCards[currentPlayerTurn];
                    WriteLine("Get out of Jail Free – This card may be kept until needed, or traded/sold ");
                    break;
                case 8:
                    playersPosition[currentPlayerTurn] -= 3;
                    if (playersPosition[currentPlayerTurn] < 0)
                    {
                        playersPosition[currentPlayerTurn] += MAX_NUM_OF_BOARD_SPACES;
                    }
                    WriteLine("Go Back 3 Spaces ");
                    ActivateBoardSpace(playersPosition[currentPlayerTurn]);
                    break;
                case 9:
                    playersPosition[currentPlayerTurn] = 10;
                    playersInJail[currentPlayerTurn] = true;
                    playersNumOfAttemptsToGetOutOfJail[currentPlayerTurn] = 3;
                    WriteLine("Go to Jail – Go directly to Jail – Do not pass Go, do not collect $200 ");
                    break;
                case 10:
                    cost = (25 * playersNumOfHouses[currentPlayerTurn]) + (100 * playersNumOfHotels[currentPlayerTurn]);
                    WriteLine("Make general repairs on all your property – For each house pay $25 – For each hotel $100 ");
                    if (playersMoney[currentPlayerTurn] < cost)
                    {
                        SellAndMortgageProperty(currentPlayerTurn, cost);
                    }
                    playersMoney[currentPlayerTurn] -= cost;
                    playersTotalWorth[currentPlayerTurn] -= cost;
                    break;
                case 11:
                    cost = 15;
                    if (playersMoney[currentPlayerTurn] < cost)
                    {
                        SellAndMortgageProperty(currentPlayerTurn, cost);
                    }
                    playersMoney[currentPlayerTurn] -= cost;
                    playersTotalWorth[currentPlayerTurn] -= cost;
                    WriteLine("Pay poor tax of $15 ");
                    break;
                case 12:
                    while (playersPosition[currentPlayerTurn] != 5)
                    {
                        ++playersPosition[currentPlayerTurn];
                        if (playersPosition[currentPlayerTurn] >= MAX_NUM_OF_BOARD_SPACES)
                        {
                            playersPosition[currentPlayerTurn] -= MAX_NUM_OF_BOARD_SPACES;
                            playersMoney[currentPlayerTurn] += 200;
                            playersTotalWorth[currentPlayerTurn] += 200;
                        }
                    }
                    WriteLine("Take a ride on the Reading – If you pass Go, collect $200 ");
                    ActivatePropertySpace(playersPosition[currentPlayerTurn]);
                    break;
                case 13:
                    playersPosition[currentPlayerTurn] = 39;
                    WriteLine("Take a walk on the Boardwalk – Advance token to Boardwalk ");
                    ActivatePropertySpace(39);
                    break;
                case 14:
                    WriteLine("You have been elected Chairman of the Board – Pay each player $50 ");
                    cost = 50;
                    for (int count = 0; count < playerCount; ++count)
                    {
                        if (playersMoney[currentPlayerTurn] < cost)
                        {
                            if (count != currentPlayerTurn)
                            {
                                SellAndMortgageProperty(count, cost);
                            }
                        }
                        playersMoney[currentPlayerTurn] -= cost;
                        playersTotalWorth[currentPlayerTurn] -= cost;
                        playersMoney[count] += 50;
                        playersTotalWorth[count] += 50;
                    }
                    break;
                case 15:
                    playersMoney[currentPlayerTurn] += 150;
                    playersTotalWorth[currentPlayerTurn] += 150;
                    WriteLine("Your building {and} loan matures – Collect $150 ");
                    break;
            }
            if (identifier != 7)
            {
                chance.Enqueue(identifier);
            }
        }

        public static void BuyProperty(int location)
        {
            int price = PROPERTY_PRICES[location];
            if (price > playersMoney[currentPlayerTurn])
            {
                SellAndMortgageProperty(currentPlayerTurn, price);
            }
            playersMoney[currentPlayerTurn] -= price;
            if (location == 5 || location == 15 || location == 25 || location == 35)
            {
                ++playersNumOfRailroads[currentPlayerTurn];
            }
            if (location == 12 || location == 28)
            {
                ++playersNumOfUtilities[currentPlayerTurn];
            }
        }

        public static void AuctionProperty(int location, int player)
        {
            ++player;
            if (player == playerCount)
            {
                player = 0;
            }
            int bid = PROPERTY_PRICES[location];
            int choice;
            int highestBidder = -1;
            bool isValid = false;
            string prompt, error;
            bool contested = true;
            bool bidPlaced = false;
            ArrayList bidders = new ArrayList();
            while (contested)
            {
                int countBefore = bidders.Count;
                for (int iteration = 0; iteration < playerCount; ++iteration)
                {
                    if (player != highestBidder)
                    {
                        if (playersTotalWorth[player] >= bid)
                        {
                            prompt = $"{playersName[player]}, would you like to bid {bid:c0} for {PROPERTY_NAMES[location]}? ";
                            if (countBefore == 1)
                            {
                                prompt += "\n(This is your last chance!)";
                            }
                            prompt += $"\nYour money: {playersMoney[player]:c0}\nYour total worth: {playersTotalWorth[player]:c0}\n0:Yes\n1:No";
                            error = $"Error - choose whether to bid {bid:c0} for {PROPERTY_NAMES[location]}.";
                            do
                            {
                                choice = RetrieveInput(error, prompt);
                                if (choice == 0 || choice == 1)
                                {
                                    isValid = true;
                                }
                                else
                                {
                                    WriteLine(error);
                                    ReadKey();
                                }
                            } while (isValid == false);
                            if (choice == 0)
                            {
                                WriteLine("You have chosen to bid");
                                highestBidder = player;
                                if (bidders.Contains(player) == false)
                                {
                                    bidders.Add(player);
                                }
                                bidPlaced = true;
                                bid += 10;
                                ReadKey();
                            }
                            else
                            {
                                WriteLine("You have chosen not to bid");
                                bidders.Remove(player);
                                ReadKey();
                            }
                        }
                        else
                        {
                            WriteLine($"{playersName[player]} does not have enough money to continue bidding");
                        }
                    }
                    ++player;
                    if (player == playerCount)
                    {
                        player = 0;
                    }
                }
                int countAfter = bidders.Count;
                if (bidders.Count == 1 && countBefore == countAfter)
                {
                    WriteLine($"{playersName[highestBidder]} is the only bidder left, and therefore receives the property");
                    contested = false;
                    player = (int)bidders[0];
                }
                else if (bidders.Count == 0 && countBefore == countAfter)
                {
                    WriteLine($"No player wants to purchase {PROPERTY_NAMES[location]}. This property will remain unowned");
                    contested = false;
                }
            }
            if (bidPlaced)
            {
                bid -= 10;
                if (bid > playersMoney[player])
                {
                    SellAndMortgageProperty(player, bid);
                }
                playersMoney[player] -= bid;
                playersTotalWorth[player] -= (bid - PROPERTY_PRICES[location]);
                playersProperties[player].Add(location);
                availableProperties.Remove(location);
            }
        }

        public static void SellAndMortgageProperty(int player, int requiredMoneyNeeded, int[] exceptions = null)
        {
            do
            {
                if (playersProperties[player].Count > 0)
                {
                    WriteLine();
                    WriteLine($"Your money: {playersMoney[player]:c0}");
                    WriteLine($"Money required: {requiredMoneyNeeded:c0}");
                    ArrayList props;
                    string prompt = ReturnPlayerProperties(player, out props, exceptions) + "\nWhat would you like to do?:";
                    string error = "Error - choose what to sell from the given commands";
                    ArrayList availableCommands = new ArrayList();
                    string[] commands = { "\n0:Mortgage a property", "\n1:Sell a property", "\n2:Sell a house", "\n3:Sell a Get Out of Jail Free card" };
                    bool canMortgage = false, canSellProperty = false, canSellHouse = false;
                    foreach (int property in props)
                    {
                        if (numberOfHousesOnEachBoardSpace[property] > 0)
                        {
                            canSellHouse = true;
                        }
                        else
                        {
                            canSellProperty = true;
                            if (mortgagedProperties[property] == false)
                            {
                                canMortgage = true;
                            }
                        }
                    }
                    if (canMortgage)
                    {
                        availableCommands.Add(0);
                        prompt += commands[0];
                    }
                    if (canSellProperty)
                    {
                        availableCommands.Add(1);
                        prompt += commands[1];
                    }
                    if (canSellHouse)
                    {
                        availableCommands.Add(2);
                        prompt += commands[2];
                    }
                    if (playersNumOfGetOutOfJailFreeCards[player] > 0)
                    {
                        availableCommands.Add(3);
                        prompt += commands[3];
                    }
                    int choice;
                    ArrayList unmortgagedAndHouselessProps;
                    ReturnPlayerUnmortgagedAndHouselessProperties(player, out unmortgagedAndHouselessProps);
                    ArrayList mortgagedPropsAndUnmortgagedHouselessProps;
                    ReturnPlayerMortgagedPropertiesAndUnmortgagedPropertiesWithoutHouses(player, out mortgagedPropsAndUnmortgagedHouselessProps);
                    bool isValid = false;
                    do
                    {
                        choice = RetrieveInput(error, prompt);
                        foreach (int command in availableCommands)
                        {
                            if (command == choice)
                            {
                                isValid = true;
                            }
                        }
                        if (isValid == false)
                        {
                            WriteLine(error);
                            ReadKey();
                        }
                    } while (isValid == false);
                    isValid = false;
                    if (choice == 0)//If the player wants to mortgage a property...
                    {
                        WriteLine("You chose to mortgage a property.");
                        prompt = ReturnPlayerUnmortgagedAndHouselessProperties(player, out unmortgagedAndHouselessProps);
                        prompt += "\nWhich property would you like to mortgage?";
                        error = "Error - choose a property you would like to mortgage";
                        do
                        {
                            choice = RetrieveInput(error, prompt);
                            if (unmortgagedAndHouselessProps.Contains(choice))
                            {
                                isValid = true;
                            }
                            else
                            {
                                WriteLine(error);
                                ReadKey();
                            }
                        } while (isValid == false);
                        mortgagedProperties[choice] = true;
                        playersMoney[player] += PROPERTY_PRICES[choice] / 2;
                        WriteLine($"{PROPERTY_NAMES[choice]} is now mortgaged");
                        ReadKey();
                    }
                    else if (choice == 1)
                    {
                        WriteLine("You chose to sell a property.");
                        prompt = ReturnPlayerMortgagedPropertiesAndUnmortgagedPropertiesWithoutHouses(player, out mortgagedPropsAndUnmortgagedHouselessProps, exceptions);
                        prompt += "\nWhich property would you like to sell?";
                        error = "Error - choose a property you would like to sell";
                        if (mortgagedPropsAndUnmortgagedHouselessProps.Count == 1)
                        {
                            choice = (int)mortgagedPropsAndUnmortgagedHouselessProps[0];
                        }
                        else
                        {
                            do
                            {
                                choice = RetrieveInput(error, prompt);
                                foreach (int availableChoice in mortgagedPropsAndUnmortgagedHouselessProps)
                                {
                                    if (availableChoice == choice)
                                    {
                                        isValid = true;
                                    }
                                }
                                if (isValid == false)
                                {
                                    WriteLine(error);
                                }
                            } while (isValid == false);
                        }
                        if (mortgagedProperties[choice])
                        {
                            playersMoney[player] += PROPERTY_PRICES[choice] / 2;
                            mortgagedProperties[choice] = false;
                        }
                        else
                        {
                            playersMoney[player] += PROPERTY_PRICES[choice];
                        }
                        playersProperties[player].Remove(choice);
                        availableProperties.Add(choice);
                        if (choice == 5 || choice == 15 || choice == 25 || choice == 35)
                        {
                            --playersNumOfRailroads[currentPlayerTurn];
                        }
                        if (choice == 12 || choice == 28)
                        {
                            --playersNumOfUtilities[currentPlayerTurn];
                        }
                        WriteLine($"{PROPERTY_NAMES[choice]} is now sold");
                        ReadKey();
                    }
                    else if (choice == 2)
                    {
                        SellHouses(player);
                    }
                    else
                    {
                        --playersNumOfGetOutOfJailFreeCards[player];
                        playersMoney[player] += 50;
                        WriteLine("You have sold your Get Out of Jail Free card");
                        ReadKey();
                    }
                }
            } while (playersMoney[player] < requiredMoneyNeeded && playersProperties[player].Count > 0);
            if (playersMoney[player] <= requiredMoneyNeeded && playersMoney[player] == playersTotalWorth[player])
            {
                playersStillInGame[player] = false;
            }
        }

        public static void UnmortgageProperty(int player)
        {
            ArrayList mortgagedProps;
            string prompt = ReturnPlayerMortgagedProperties(player, out mortgagedProps);
            prompt += $"\n{playersName[player]}, what property would you like to unmortgage?";
            string error = "Error - choose the property you would like to unmortgage";
            int choice;
            int mortgagePrice = 0;
            bool isValid = false;
            if (mortgagedProps.Count > 1)
            {
                do
                {
                    choice = RetrieveInput(error, prompt);
                    if (mortgagedProps.Contains(choice))
                    {
                        mortgagePrice = PROPERTY_PRICES[choice] / 2;
                        if (playersMoney[player] < mortgagePrice && (playersTotalWorth[player] - mortgagePrice) >= 0)
                        {
                            isValid = true;
                        }
                        else if (playersMoney[player] > mortgagePrice)
                        {
                            isValid = true;
                        }
                    }
                } while (isValid == false);
            }
            else
            {
                choice = (int)mortgagedProps[0];
            }
            if (playersMoney[player] < mortgagePrice && (playersTotalWorth[player] - mortgagePrice) >= 0)
            {
                SellAndMortgageProperty(player, PROPERTY_PRICES[choice] / 2);
            }
            playersMoney[player] -= PROPERTY_PRICES[choice] / 2;
            mortgagedProperties[choice] = false;
            WriteLine($"You have unmortgaged {PROPERTY_NAMES[choice]}");
            ReadKey();
        }

        public static void BuyHouses(int player)
        {
            bool isValid = false;
            WriteLine("You chose to buy houses.");
            int groupIdentifier;
            int location = 0;
            ArrayList groupsOwned_Identifiers = new ArrayList();
            int cost;
            for (int count = 0; count < propertiesByGroup.Count; ++count)
            {
                if (count != 6 && count != 8)//Excluding utilities and railroads
                {
                    if (IdentifyIfAllPropertiesOfGroupAreOwned(player, count))
                    {
                        groupsOwned_Identifiers.Add(count);
                    }
                }
            }
            if (groupsOwned_Identifiers.Count > 1)
            {
                do
                {
                    string prompt = "Which group would you like to build houses on? ";
                    foreach (int identifier in groupsOwned_Identifiers)
                    {
                        prompt += $"\n{identifier}:{GROUP_NAMES[identifier]}";
                    }
                    string error = "Error - enter a number representing a color group you own. \nIf all properties in a color group have 5 houses each, you may not build anymore on this color group";
                    groupIdentifier = RetrieveInput(error, prompt);

                    if (groupsOwned_Identifiers.Contains(groupIdentifier))
                    {
                        isValid = true;
                    }
                    else
                    {
                        WriteLine(error);
                        ReadKey();
                    }
                }
                while (isValid == false);
            }
            else
            {
                groupIdentifier = (int)groupsOwned_Identifiers[0];
            }
            if (groupIdentifier == 0 || groupIdentifier == 1)
            {
                cost = 50;
            }
            else if (groupIdentifier == 2 || groupIdentifier == 3)
            {
                cost = 100;
            }
            else if (groupIdentifier == 4 || groupIdentifier == 5)
            {
                cost = 150;
            }
            else if (groupIdentifier == 7 || groupIdentifier == 9)
            {
                cost = 200;
            }
            else
            {
                cost = 0;
            }
            isValid = false;
            int numOfProperties = ((ArrayList)propertiesByGroup[groupIdentifier]).Count;
            int[] propertiesInGroup_LocationIdentifiers = new int[numOfProperties];
            int[] houseCounts = new int[numOfProperties];
            int totalNumOfHouses = 0;
            for (int count = 0; count < numOfProperties; ++count)
            {
                propertiesInGroup_LocationIdentifiers[count] = (int)((ArrayList)propertiesByGroup[groupIdentifier])[count];
                houseCounts[count] = numberOfHousesOnEachBoardSpace[propertiesInGroup_LocationIdentifiers[count]];
                totalNumOfHouses += houseCounts[count];
            }
            int min = houseCounts.Min();
            if (min == 5)
            {
                WriteLine("You cannot build anymore houses for this group");
            }
            else
            {
                ArrayList availableChoices = new ArrayList();
                int index = 0;
                foreach (int houses in houseCounts)
                {
                    if (houses == min)
                    {
                        availableChoices.Add(((ArrayList)propertiesByGroup[groupIdentifier])[index]);
                    }
                    ++index;
                }
                if (availableChoices.Count == 1)
                {
                    location = (int)availableChoices[0];
                }
                else
                {
                    isValid = false;
                    do
                    {
                        string prompt = "Which property would you like to build a house on? \n";
                        for (int count = 0; count < numOfProperties; ++count)
                        {
                            if (houseCounts[count] == min)
                            {
                                prompt += $"{propertiesInGroup_LocationIdentifiers[count]}:{PROPERTY_NAMES[propertiesInGroup_LocationIdentifiers[count]]}(Houses = {numberOfHousesOnEachBoardSpace[propertiesInGroup_LocationIdentifiers[count]]})\n";
                            }
                        }
                        string error = "Error - enter a location to build a house on (you may not have more than 5 houses built on one property)";
                        location = RetrieveInput(error, prompt);
                        if (availableChoices.Contains(location))
                        {
                            isValid = true;
                        }
                        if (numberOfHousesOnEachBoardSpace[location] == 5)
                        {
                            isValid = false;
                        }
                        if (isValid == false)
                        {
                            WriteLine(error);
                        }
                    } while (isValid == false);
                }
                int groupValue = 0;
                foreach (int property in (ArrayList)propertiesByGroup[groupIdentifier])
                {
                    groupValue += PROPERTY_PRICES[property];
                    groupValue += numberOfHousesOnEachBoardSpace[property] * (cost / 2);
                }
                if (playersTotalWorth[player] - groupValue >= cost)
                {
                    if (cost > playersMoney[player])
                    {
                        int[] exceptions = new int[propertiesInGroup_LocationIdentifiers.Length];
                        index = 0;
                        foreach (int locationIdentifier in propertiesInGroup_LocationIdentifiers)
                        {
                            exceptions[index] = locationIdentifier;
                            ++index;
                        }
                        SellAndMortgageProperty(player, cost, exceptions);
                    }
                    playersMoney[player] -= cost;
                    playersTotalWorth[player] -= cost / 2;
                    ++numberOfHousesOnEachBoardSpace[location];
                    WriteLine($"You have built a house on {PROPERTY_NAMES[location]}");
                    ReadKey();
                }
                else
                {
                    WriteLine($"You do not have enough money to buy a house on {PROPERTY_NAMES[location]}");
                    ReadKey();
                }
            }
        }

        public static void SellHouses(int player)
        {
            string prompt, error;
            bool isValid = false;
            int chosenGroup;
            int location;
            int choice;
            int housePrice = 0;
            WriteLine("You chose to sell houses.");
            ArrayList groupsOwned_Identifiers = new ArrayList();
            for (int count = 0; count < propertiesByGroup.Count; ++count)
            {
                if (count != 6 && count != 8)//Excluding utilities and railroads
                {
                    if (IdentifyIfAllPropertiesOfGroupAreOwned(player, count))
                    {
                        groupsOwned_Identifiers.Add(count);
                    }
                }
            }
            if (groupsOwned_Identifiers.Count > 1)
            {
                prompt = "Which color group would you like to sell houses from?";
                foreach (int group in groupsOwned_Identifiers)
                {
                    if (group == 0 || group == 1)
                        housePrice = 25;
                    else if (group == 2 || group == 3)
                        housePrice = 50;
                    else if (group == 4 || group == 5)
                        housePrice = 75;
                    else if (group == 7 || group == 9)
                        housePrice = 100;
                    prompt += $"\n{group}:{GROUP_NAMES[group]} (Price = {housePrice:c0})";
                }
                error = "Error - choose a color group you would like to sell houses from";
                do
                {
                    choice = RetrieveInput(error, prompt);
                    foreach (int group in groupsOwned_Identifiers)
                    {
                        if (choice == group)
                        {
                            isValid = true;
                        }
                    }
                    if (isValid == false)
                    {
                        WriteLine(error);
                        ReadKey();
                    }
                } while (isValid == false);
                chosenGroup = choice;
            }
            else
            {
                chosenGroup = (int)groupsOwned_Identifiers[0];
                if (chosenGroup == 0 || chosenGroup == 1)
                    housePrice = 25;
                else if (chosenGroup == 2 || chosenGroup == 3)
                    housePrice = 50;
                else if (chosenGroup == 4 || chosenGroup == 5)
                    housePrice = 75;
                else if (chosenGroup == 7 || chosenGroup == 9)
                    housePrice = 100;
            }
            prompt = $"\nPrice of each house = {housePrice:c0}\nWhich property would you like to sell houses from?";
            ArrayList availableChoices = new ArrayList();
            int[] houseCounts = new int[((ArrayList)propertiesByGroup[chosenGroup]).Count];
            for (int count = 0; count < houseCounts.Length; ++count)
            {
                houseCounts[count] = numberOfHousesOnEachBoardSpace[(int)((ArrayList)propertiesByGroup[chosenGroup])[count]];
            }
            int maxHouses = houseCounts.Max();
            foreach (int property in (ArrayList)propertiesByGroup[chosenGroup])
            {
                if (numberOfHousesOnEachBoardSpace[property] == maxHouses)
                {
                    prompt += $"\n{property}:{PROPERTY_NAMES[property]} (Houses = {numberOfHousesOnEachBoardSpace[property]}";
                    availableChoices.Add(property);
                }
            }
            error = "Error - choose a property to sell houses from";
            do
            {
                location = RetrieveInput(error, prompt);
                if (availableChoices.Contains(location))
                {
                    if (numberOfHousesOnEachBoardSpace[location] > 0)
                    {
                        isValid = true;
                    }
                    else
                    {
                        WriteLine(error);
                        ReadKey();
                    }
                }
                else
                {
                    WriteLine(error);
                    ReadKey();
                }
            } while (isValid == false);
            --numberOfHousesOnEachBoardSpace[location];
            playersMoney[player] += housePrice;
            WriteLine($"You have sold a house on {PROPERTY_NAMES[location]}");
            ReadKey();
        }

        public static int IdentifyPropertyGroup(int location)//Finds the color or utility group this property belongs to
        {
            int groupIdentifier = -1;
            for (int count = 0; count < propertiesByGroup.Count; ++count)
            {
                for (int count2 = 0; count2 < ((ArrayList)propertiesByGroup[count]).Count; ++count2)
                {
                    if (Convert.ToInt32(((ArrayList)propertiesByGroup[count])[count2]) == location)
                    {
                        groupIdentifier = count;
                    }
                }
            }
            if (groupIdentifier < 0)
            {
                WriteLine("Error - could not identify property group");
                ReadKey();
            }
            return groupIdentifier;
        }

        public static int IdentifyPropertyOwner(int location)//Finds the player that owns the property
        {
            int playerOwner = -1;
            for (int count = 0; count < playerCount; ++count)
            {
                if (count != currentPlayerTurn)
                {
                    if (playersProperties[count].Contains(location))
                    {
                        playerOwner = count;
                    }
                }
            }
            if (playerOwner < 0)
            {
                WriteLine("Error - could not identify property owner");
                ReadKey();
            }
            return playerOwner;
        }

        public static bool IdentifyIfAllPropertiesOfGroupAreOwned(int playerOwner, int groupIdentifier)//Determines if they own all the properties of a particular color group
        {
            bool allColoredPropertiesOwned = true;
            if (playersProperties[playerOwner].Count > 0)
            {
                for (int count = 0; count < ((ArrayList)propertiesByGroup[groupIdentifier]).Count; ++count)
                {
                    if (!playersProperties[playerOwner].Contains(((ArrayList)propertiesByGroup[groupIdentifier])[count]))
                    {
                        allColoredPropertiesOwned = false;
                    }
                }
            }
            else
            {
                allColoredPropertiesOwned = false;
            }
            return allColoredPropertiesOwned;
        }

        public static bool IdentifyIfPlayerCanBuyHouses(int player)
        {
            bool canBuyHouses = false;
            ArrayList colorGroups = new ArrayList();
            for (int groupIdentifier = 0; groupIdentifier < propertiesByGroup.Count; ++groupIdentifier)
            {
                if (groupIdentifier != 6 && groupIdentifier != 8)//Excluding utilities and railroads
                {
                    if (IdentifyIfAllPropertiesOfGroupAreOwned(player, groupIdentifier))
                    {
                        colorGroups.Add(groupIdentifier);
                    }
                }
            }
            int totalHouseCount;
            for (int index = colorGroups.Count - 1; index >= 0; --index)
            {
                totalHouseCount = 0;
                int groupIdentifier = (int)colorGroups[index];
                int numberOfPropertiesInGroup = ((ArrayList)propertiesByGroup[groupIdentifier]).Count;
                for (int propertyCount = 0; propertyCount < numberOfPropertiesInGroup; ++propertyCount)
                {
                    int location = (int)((ArrayList)propertiesByGroup[groupIdentifier])[propertyCount];
                    totalHouseCount += numberOfHousesOnEachBoardSpace[location];
                }
                if (totalHouseCount == 5 * numberOfPropertiesInGroup)
                {
                    colorGroups.RemoveAt(index);
                }
            }
            for (int index = colorGroups.Count - 1; index >= 0; --index)
            {
                int groupIdentifier = (int)colorGroups[index];
                int cost = 0;
                if (groupIdentifier == 0 || groupIdentifier == 1)
                {
                    cost = 50;
                }
                else if (groupIdentifier == 2 || groupIdentifier == 3)
                {
                    cost = 100;
                }
                else if (groupIdentifier == 4 || groupIdentifier == 5)
                {
                    cost = 150;
                }
                else if (groupIdentifier == 7 || groupIdentifier == 9)
                {
                    cost = 200;
                }
                int baseGroupValue = 0;
                foreach (int group in colorGroups)
                {
                    foreach (int location in (ArrayList)propertiesByGroup[group])
                    {
                        baseGroupValue += PROPERTY_PRICES[location];
                    }
                }
                if ((playersTotalWorth[player] - cost - baseGroupValue) < 0)
                {
                    colorGroups.RemoveAt(index);
                }
            }
            if (colorGroups.Count > 0)
            {
                canBuyHouses = true;
            }
            return canBuyHouses;
        }

        public static bool IdentifyIfPlayerHasHouses(int player)
        {
            bool hasHouses = false;
            foreach (int property in playersProperties[player])
            {
                if (numberOfHousesOnEachBoardSpace[property] > 0)
                {
                    hasHouses = true;
                }
            }
            return hasHouses;
        }

        public static int RetrieveInput(string errorMessage, string prompt = "")
        {
            int choice;
            bool isValid = false;
            do
            {
                WriteLine(prompt);
                Write(">> ");
                if (int.TryParse(ReadLine(), out choice) == false)
                {
                    WriteLine(errorMessage);
                    ReadKey();
                }
                else
                {
                    isValid = true;
                }
            }
            while (isValid == false);
            return choice;
        }

        public static void DisplayPlayerProperties(int player)
        {
            playersProperties[player].Sort();
            WriteLine("Your properties:");
            foreach (int location in playersProperties[player])
            {
                WriteLine($"{location}:{PROPERTY_NAMES[location]} (Houses = {numberOfHousesOnEachBoardSpace[location]}. Mortgage = {PROPERTY_PRICES[location] / 2})");
            }
        }

        public static string ReturnPlayerProperties(int player, int[] exceptions = null)
        {
            playersProperties[player].Sort();
            string properties = "";
            foreach (int location in playersProperties[player])
            {
                bool isException = false;
                if (exceptions != null)
                {
                    for (int count = 0; count < exceptions.Length; ++count)
                    {
                        if (location == exceptions[count])
                        {
                            isException = true;
                        }
                    }
                }
                if (isException == false)
                {
                    if (mortgagedProperties[location])
                    {
                        properties += $"{location}:{PROPERTY_NAMES[location]} (Mortgage = {PROPERTY_PRICES[location] / 2:c0})\n";
                    }
                    else
                    {
                        properties += $"{location}:{PROPERTY_NAMES[location]} (Houses = {numberOfHousesOnEachBoardSpace[location]}. Price = {PROPERTY_PRICES[location]:c0})\n";
                    }
                }
            }
            return properties;
        }

        public static string ReturnPlayerProperties(int player, out ArrayList props, int[] exceptions = null)
        {
            playersProperties[player].Sort();
            string properties = "";
            props = new ArrayList();
            foreach (int location in playersProperties[player])
            {
                bool isException = false;
                if (exceptions != null)
                {
                    for (int count = 0; count < exceptions.Length; ++count)
                    {
                        if (location == exceptions[count])
                        {
                            isException = true;
                        }
                    }
                }
                if (isException == false)
                {
                    props.Add(location);
                    if (mortgagedProperties[location])
                    {
                        properties += $"{location}:{PROPERTY_NAMES[location]} (Mortgage = {PROPERTY_PRICES[location] / 2:c0})\n";
                    }
                    else
                    {
                        properties += $"{location}:{PROPERTY_NAMES[location]} (Houses = {numberOfHousesOnEachBoardSpace[location]}. Price = {PROPERTY_PRICES[location]:c0})\n";
                    }
                }
            }
            return properties;
        }

        public static string ReturnPlayerUnmortgagedProperties(int player)
        {
            playersProperties[player].Sort();
            string properties = "Your unmortgaged properties:\n";
            foreach (int location in playersProperties[player])
            {
                if (mortgagedProperties[location] == false)
                {
                    properties += $"{location}:{PROPERTY_NAMES[location]} (Houses = {numberOfHousesOnEachBoardSpace[location]}. Mortgage = {PROPERTY_PRICES[location] / 2})\n";
                }
            }
            return properties;
        }

        public static string ReturnPlayerMortgagedProperties(int player)
        {
            playersProperties[player].Sort();
            string properties = "Your mortgaged properties:\n";
            foreach (int location in playersProperties[player])
            {
                if (mortgagedProperties[location])
                {
                    properties += $"{location}:{PROPERTY_NAMES[location]} (Mortgage = {PROPERTY_PRICES[location] / 2})\n";
                }
            }
            return properties;
        }

        public static string ReturnPlayerMortgagedProperties(int player, out ArrayList mortgagedProps)
        {
            playersProperties[player].Sort();
            string properties = "Mortgaged properties:\n";
            mortgagedProps = new ArrayList();
            foreach (int location in playersProperties[player])
            {
                if (mortgagedProperties[location])
                {
                    properties += $"{location}:{PROPERTY_NAMES[location]} (Mortgage = {PROPERTY_PRICES[location] / 2})\n";
                    mortgagedProps.Add(location);
                }
            }
            return properties;
        }

        public static string ReturnPlayerPropertiesWithHouses(int player)
        {
            playersProperties[player].Sort();
            string properties = "Properties with houses:\n";
            foreach (int location in playersProperties[player])
            {
                if (numberOfHousesOnEachBoardSpace[location] > 0)
                {
                    int housePrice = 0;
                    if (location >= 1 && location <= 9)
                    {
                        housePrice = 25;
                    }
                    else if (location >= 11 && location <= 19)
                    {
                        housePrice = 50;
                    }
                    else if (location >= 21 && location <= 29)
                    {
                        housePrice = 75;
                    }
                    else if (location >= 31 && location <= 39)
                    {
                        housePrice = 100;
                    }
                    properties += $"{location}:{PROPERTY_NAMES[location]} (Number of houses = {numberOfHousesOnEachBoardSpace[location]}. House price = {housePrice:c0})\n";
                }
            }
            return properties;
        }

        public static string ReturnPlayerPropertiesWithHouses(int player, out ArrayList propsWithHouses)
        {
            string properties = "";
            propsWithHouses = new ArrayList();
            foreach (int location in playersProperties[player])
            {
                if (numberOfHousesOnEachBoardSpace[location] > 0)
                {
                    int housePrice = 0;
                    if (location >= 1 && location <= 9)
                    {
                        housePrice = 25;
                    }
                    else if (location >= 11 && location <= 19)
                    {
                        housePrice = 50;
                    }
                    else if (location >= 21 && location <= 29)
                    {
                        housePrice = 75;
                    }
                    else if (location >= 31 && location <= 39)
                    {
                        housePrice = 100;
                    }
                    properties += $"{location}:{PROPERTY_NAMES[location]} (Number of houses = {numberOfHousesOnEachBoardSpace[location]}. House price = {housePrice:c0})\n";
                    propsWithHouses.Add(location);
                }
            }
            return properties;
        }

        public static string ReturnPlayerMortgagedPropertiesAndUnmortgagedPropertiesWithoutHouses(int player, out ArrayList props, int[] exceptions = null)
        {
            playersProperties[player].Sort();
            props = new ArrayList();
            string properties = "Your mortgaged properties, and unmortgaged properties without houses:\n";
            foreach (int location in playersProperties[player])
            {
                if (exceptions != null)
                {
                    if (exceptions.Contains(location) == false)
                    {
                        if (numberOfHousesOnEachBoardSpace[location] == 0)
                        {
                            props.Add(location);
                            if (mortgagedProperties[location])
                            {
                                properties += $"{location}:{PROPERTY_NAMES[location]} (Mortgage = {PROPERTY_PRICES[location] / 2:c0})\n";
                            }
                            else
                            {
                                properties += $"{location}:{PROPERTY_NAMES[location]} (Price = {PROPERTY_PRICES[location]:c0})\n";
                            }
                        }
                    }
                }
                else
                {
                    if (numberOfHousesOnEachBoardSpace[location] == 0)
                    {
                        props.Add(location);
                        if (mortgagedProperties[location])
                        {
                            properties += $"{location}:{PROPERTY_NAMES[location]} (Mortgage = {PROPERTY_PRICES[location] / 2:c0})\n";
                        }
                        else
                        {
                            properties += $"{location}:{PROPERTY_NAMES[location]} (Price = {PROPERTY_PRICES[location]:c0})\n";
                        }
                    }
                }
            }
            return properties;
        }

        public static string ReturnPlayerUnmortgagedAndHouselessProperties(int player, out ArrayList props)
        {
            playersProperties[player].Sort();
            props = new ArrayList();
            string properties = "Unmortgaged properties without houses:\n";
            foreach (int location in playersProperties[player])
            {
                if (numberOfHousesOnEachBoardSpace[location] == 0)
                {
                    if (mortgagedProperties[location] == false)
                    {
                        properties += $"{location}:{PROPERTY_NAMES[location]} (Mortgage = {PROPERTY_PRICES[location] / 2:c0})\n";
                        props.Add(location);
                    }
                }
            }
            return properties;
        }

        public static void MakeTradeWithPlayer(int initialInterrogator, int initialInterrogated)
        {
            ArrayList interrogator_PropertyRequest = RequestProperty(initialInterrogator, initialInterrogated);
            int interrogator_MoneyRequest = RequestMoney(initialInterrogator, initialInterrogated);
            ArrayList interrogated_PropertyRequest = RequestProperty(initialInterrogated, initialInterrogator);
            int interrogated_MoneyRequest = RequestMoney(initialInterrogated, initialInterrogator);
            CompleteTrade(initialInterrogator, initialInterrogated,
                interrogator_PropertyRequest, interrogator_MoneyRequest,
                interrogated_PropertyRequest, interrogated_MoneyRequest);
        }

        public static ArrayList RequestProperty(int interrogator, int interrogated)
        {
            string prompt;
            string error;
            ArrayList propertiesToChooseFrom;
            ArrayList groupsOwned_Identifiers = new ArrayList();
            ArrayList availableCommands = new ArrayList();
            string[] commands = { "\n0:Nothing (there are no properties I would like)", "\n1:A property", "\n2:A color group" };
            int choice;
            ArrayList interrogator_PropertyRequest = new ArrayList();
            bool isValid = false;
            for (int count = 0; count < propertiesByGroup.Count; ++count)
            {
                if (count != 6 && count != 8)//Excluding utilities and railroads
                {
                    if (IdentifyIfAllPropertiesOfGroupAreOwned(interrogated, count))
                    {
                        groupsOwned_Identifiers.Add(count);
                    }
                }
            }
            availableCommands.Add(0);
            if (playersProperties[interrogated].Count > 0)
            {
                availableCommands.Add(1);
            }
            if (groupsOwned_Identifiers.Count > 0)
            {
                availableCommands.Add(2);
            }
            prompt = $"{playersName[interrogator]}, what would you like from {playersName[interrogated]}?";
            foreach (int com in availableCommands)
            {
                prompt += commands[com];
            }
            error = $"Error - choose what you want from one of the given commands";
            do
            {
                choice = RetrieveInput(error, prompt);
                foreach (int com in availableCommands)
                {
                    if (com == choice)
                    {
                        isValid = true;
                    }
                }
                if (isValid == false)
                {
                    WriteLine(error);
                    ReadKey();
                }
            } while (isValid == false);
            if (choice == 1)
            {
                prompt = $"{playersName[interrogator]}, what property do you want from {playersName[interrogated]}? (Enter 0 if you do not want a property)\n";
                error = $"Error - choose what property you would like from {playersName[interrogated]}";
                prompt += ReturnPlayerProperties(interrogated, out propertiesToChooseFrom);
                do
                {
                    choice = RetrieveInput(error, prompt);
                    if (choice == 0)
                    {
                        isValid = true;
                    }
                    else
                    {
                        if (propertiesToChooseFrom.Contains(choice))
                        {
                            isValid = true;
                        }
                        else
                        {
                            WriteLine(error);
                            ReadKey();
                        }
                    }
                } while (isValid == false);
                interrogator_PropertyRequest.Add(choice);
            }
            else if (choice == 2)
            {
                prompt = $"{playersName[interrogator]}, what color group do you want from {playersName[interrogated]}? (Enter -1 if you do not want a color group)\n";
                foreach (int group in groupsOwned_Identifiers)
                {
                    prompt += $"\n{group}:{GROUP_NAMES[group]}";
                }
                error = $"Error - choose what color group you would like from {playersName[interrogated]}";
                do
                {
                    choice = RetrieveInput(error, prompt);
                    if (choice == -1)
                    {
                        isValid = true;
                    }
                    else
                    {
                        if (groupsOwned_Identifiers.Contains(choice))
                        {
                            isValid = true;
                        }
                        else
                        {
                            WriteLine(error);
                            ReadKey();
                        }
                    }
                } while (isValid == false);
                foreach (int property in (ArrayList)propertiesByGroup[choice])
                {
                    interrogator_PropertyRequest.Add(property);
                }
            }
            return interrogator_PropertyRequest;
        }

        public static int RequestMoney(int interrogator, int interrogated)
        {
            string prompt = $"{playersName[interrogator]}, how much money do you want from {playersName[interrogated]} in addition to this trade?" +
                $"\n{playersName[interrogated]}'s Money: {playersMoney[interrogated]:c0}";
            string error = $"Error - enter how much money you would like from {playersName[interrogated]}";
            int choice;
            bool isValid = false;
            do
            {
                choice = RetrieveInput(error, prompt);
                if (choice >= 0 && choice <= playersMoney[interrogated])
                {
                    isValid = true;
                }
                else
                {
                    WriteLine(error);
                    ReadKey();
                }
            } while (isValid == false);
            return choice;
        }

        public static void CompleteTrade(int initialInterrogator, int initialInterrogated,
            ArrayList interrogator_PropertyRequest, int interrogator_MoneyRequest,
            ArrayList interrogated_PropertyRequest, int interrogated_MoneyRequest)
        {
            string prompt = $"{playersName[initialInterrogator]}, do you agree with these conditions?\n0:Yes\n1:No";
            string error = "Error - choose whether or not you agree with these conditions";
            int choice;
            bool isValid = false;
            do
            {
                choice = RetrieveInput(error, prompt);
                if (choice == 0 || choice == 1)
                {
                    isValid = true;
                }
                else
                {
                    WriteLine(error);
                    ReadKey();
                }
            } while (isValid == false);
            if (choice == 0)
            {
                isValid = false;
                prompt = $"{playersName[initialInterrogated]}, do you agree with these conditions?\n0:Yes\n1:No";
                error = "Error - choose whether or not you agree with these conditions";
                do
                {
                    choice = RetrieveInput(error, prompt);
                    if (choice == 0 || choice == 1)
                    {
                        isValid = true;
                    }
                    else
                    {
                        WriteLine(error);
                        ReadKey();
                    }
                } while (isValid == false);
                if (choice == 0)
                {
                    WriteLine($"{playersName[initialInterrogator]} and {playersName[initialInterrogated]} have agreed on a trade");
                    if (interrogator_PropertyRequest.Count > 0)
                    {
                        playersProperties[initialInterrogator].Add(interrogator_PropertyRequest[0]);
                        playersProperties[initialInterrogated].Remove(interrogator_PropertyRequest[0]);
                        if (mortgagedProperties[(int)interrogator_PropertyRequest[0]])
                        {
                            playersTotalWorth[initialInterrogator] += PROPERTY_PRICES[(int)interrogator_PropertyRequest[0]] / 2;
                            playersTotalWorth[initialInterrogated] -= PROPERTY_PRICES[(int)interrogator_PropertyRequest[0]] / 2;
                        }
                        else
                        {
                            playersTotalWorth[initialInterrogator] += PROPERTY_PRICES[(int)interrogator_PropertyRequest[0]];
                            playersTotalWorth[initialInterrogated] -= PROPERTY_PRICES[(int)interrogator_PropertyRequest[0]];
                        }
                        foreach (int propertyRequested in interrogator_PropertyRequest)
                        {
                            WriteLine($"{playersName[initialInterrogator]} has received {PROPERTY_NAMES[propertyRequested]}");
                        }
                    }
                    if (interrogator_MoneyRequest > 0)
                    {
                        playersMoney[initialInterrogator] += interrogator_MoneyRequest;
                        playersMoney[initialInterrogated] -= interrogator_MoneyRequest;
                        playersTotalWorth[initialInterrogator] += interrogator_MoneyRequest;
                        playersTotalWorth[initialInterrogated] -= interrogator_MoneyRequest;
                        WriteLine($"{playersName[initialInterrogator]} has received {interrogator_MoneyRequest:c0}");
                    }
                    if (interrogated_PropertyRequest.Count > 0)
                    {
                        playersProperties[initialInterrogated].Add(interrogated_PropertyRequest[0]);
                        playersProperties[initialInterrogator].Remove(interrogated_PropertyRequest[0]);
                        if (mortgagedProperties[(int)interrogated_PropertyRequest[0]])
                        {
                            playersTotalWorth[initialInterrogated] += PROPERTY_PRICES[(int)interrogated_PropertyRequest[0]] / 2;
                            playersTotalWorth[initialInterrogator] -= PROPERTY_PRICES[(int)interrogated_PropertyRequest[0]] / 2;
                        }
                        else
                        {
                            playersTotalWorth[initialInterrogated] += PROPERTY_PRICES[(int)interrogated_PropertyRequest[0]];
                            playersTotalWorth[initialInterrogator] -= PROPERTY_PRICES[(int)interrogated_PropertyRequest[0]];
                        }
                        foreach (int propertyRequested in interrogated_PropertyRequest)
                        {
                            WriteLine($"{playersName[initialInterrogated]} has received {PROPERTY_NAMES[propertyRequested]}");
                        }
                    }
                    if (interrogated_MoneyRequest > 0)
                    {
                        playersMoney[initialInterrogated] += interrogated_MoneyRequest;
                        playersMoney[initialInterrogator] -= interrogated_MoneyRequest;
                        playersTotalWorth[initialInterrogated] += interrogated_MoneyRequest;
                        playersTotalWorth[initialInterrogator] -= interrogated_MoneyRequest;
                        WriteLine($"{playersName[initialInterrogated]} has received {interrogated_MoneyRequest:c0}");
                    }
                    ReadKey();
                }
                else
                {
                    WriteLine($"{playersName[initialInterrogated]} does not agree with this trade. The deal is off");
                    ReadKey();
                }
            }
            else
            {
                WriteLine($"{playersName[initialInterrogator]} does not agree with this trade. The deal is off");
                ReadKey();
            }
            WriteLine();
        }

        public static void WriteToLeaderboards(int player)
        {
            ArrayList leaderboardRecords = new ArrayList();
            string newRecord = $"{playersName[player]}\t\t{playersTotalWorth[player]}";
            string path = "Monopoly Leaderboards.txt";
            try
            {
                if (File.Exists(path))
                {
                    StreamReader read = new StreamReader(path);
                    while (!read.EndOfStream)
                    {
                        leaderboardRecords.Add(read.ReadLine());
                    }
                    read.Close();
                    string score;
                    bool canBreak = false;
                    bool lowestScore = true;
                    //For all the lines
                    for (int records = 0; records < leaderboardRecords.Count; ++records)
                    {
                        score = null;
                        string line = leaderboardRecords[records].ToString();
                        char[] lineChars = line.ToCharArray();
                        //For the last characters of this line
                        //Start the variable c after the length of the name
                        for (int c = 8; c < lineChars.Length; ++c)
                        {
                            if (char.IsNumber(lineChars[c]))
                            {
                                score += lineChars[c].ToString();
                            }
                        }
                        if (int.Parse(score) == playersTotalWorth[player] || int.Parse(score) < playersTotalWorth[player])
                        {
                            //add this score to the place before this record
                            leaderboardRecords.Insert(records, newRecord);
                            lowestScore = false;
                            canBreak = true;
                        }
                        if (canBreak)
                            break;
                    }
                    if (lowestScore)
                    {
                        leaderboardRecords.Add(newRecord);
                    }
                    StreamWriter write = new StreamWriter(path);
                    for (int recordsAgain = 0; recordsAgain < leaderboardRecords.Count; ++recordsAgain)
                    {
                        write.WriteLine(leaderboardRecords[recordsAgain]);
                    }
                    write.Close();
                }
                else
                {
                    StreamWriter write = new StreamWriter(path);
                    write.WriteLine(newRecord);
                    write.Close();
                }
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
            }
        }

        public static void DisplayLeaderboards()
        {
            ArrayList leaderboardRecords = new ArrayList();
            string path = "Monopoly Leaderboards.txt";
            try
            {
                if (File.Exists(path))
                {
                    StreamReader read = new StreamReader(path);
                    while (!read.EndOfStream)
                    {
                        WriteLine(read.ReadLine());
                    }
                    read.Close();
                }
            }
            catch (FileNotFoundException)
            {
                WriteLine("There are no leaderboard stats for Monopoly yet!");
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
            }
            ReadKey();
        }
    }
}
