using System;
using System.Collections.Generic;
using System.Linq;

namespace UglyTrivia
{
    public class TriviaBoardGame
    {
        private const int MaxNumberOfPlayers = 6;
        private const int WinningScore = 6;
        private const int NumberOfQuestionsPerCategory = 50;
        private const int MinimumPlayers = 2;
        private const int NumberOfPlaces = 12;

        List<string> playerNames = new List<string>();

        int[] boardPlaces = new int[MaxNumberOfPlayers];
        int[] playerScores = new int[MaxNumberOfPlayers];

        bool[] playerIsInPenaltyBox = new bool[MaxNumberOfPlayers];

        LinkedList<string> popQuestions = new LinkedList<string>();
        LinkedList<string> scienceQuestions = new LinkedList<string>();
        LinkedList<string> sportsQuestions = new LinkedList<string>();
        LinkedList<string> rockQuestions = new LinkedList<string>();

        int currentPlayer;
        bool isGettingOutOfPenaltyBox;

        public TriviaBoardGame()
        {
            for (int i = 0; i < NumberOfQuestionsPerCategory; i++)
            {
                popQuestions.AddLast("Pop Question " + i);
                scienceQuestions.AddLast(("Science Question " + i));
                sportsQuestions.AddLast(("Sports Question " + i));
                rockQuestions.AddLast(CreateRockQuestion(i));
            }
        }

        public String CreateRockQuestion(int index)
        {
            return "Rock Question " + index;
        }

        public bool CanStartGame()
        {
            return (GetNumberOfPlayers() >= MinimumPlayers);
        }

        public bool AddPlayer(String playerName)
        {
            playerNames.Add(playerName);

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + playerNames.Count);
            return true;
        }

        public int GetNumberOfPlayers()
        {
            return playerNames.Count;
        }

        public void MoveCurrentPlayerAndAskQuestionIfEndsOutsidePenaltyBox(int roll)
        {
            Console.WriteLine(playerNames[currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (playerIsInPenaltyBox[currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(playerNames[currentPlayer] + " is getting out of the penalty box");

                    boardPlaces[currentPlayer] = boardPlaces[currentPlayer] + roll;
                    if (boardPlaces[currentPlayer] > NumberOfPlaces - 1) boardPlaces[currentPlayer] = boardPlaces[currentPlayer] - NumberOfPlaces;

                    Console.WriteLine(playerNames[currentPlayer]
                            + "'s new location is "
                            + boardPlaces[currentPlayer]);
                    Console.WriteLine("The category is " + CategoryForCurrentPlayersPlace());
                    WriteQuestionToConsole();
                }
                else
                {
                    Console.WriteLine(playerNames[currentPlayer] + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                boardPlaces[currentPlayer] = boardPlaces[currentPlayer] + roll;
                if (boardPlaces[currentPlayer] > NumberOfPlaces - 1) boardPlaces[currentPlayer] = boardPlaces[currentPlayer] - NumberOfPlaces;

                Console.WriteLine(playerNames[currentPlayer]
                        + "'s new location is "
                        + boardPlaces[currentPlayer]);
                Console.WriteLine("The category is " + CategoryForCurrentPlayersPlace());
                WriteQuestionToConsole();
            }
        }

        private void WriteQuestionToConsole()
        {
            if (CategoryForCurrentPlayersPlace() == "Pop")
            {
                Console.WriteLine(popQuestions.First());
                popQuestions.RemoveFirst();
            }
            if (CategoryForCurrentPlayersPlace() == "Science")
            {
                Console.WriteLine(scienceQuestions.First());
                scienceQuestions.RemoveFirst();
            }
            if (CategoryForCurrentPlayersPlace() == "Sports")
            {
                Console.WriteLine(sportsQuestions.First());
                sportsQuestions.RemoveFirst();
            }
            if (CategoryForCurrentPlayersPlace() == "Rock")
            {
                Console.WriteLine(rockQuestions.First());
                rockQuestions.RemoveFirst();
            }
        }


        private String CategoryForCurrentPlayersPlace()
        {
            if (boardPlaces[currentPlayer] == 0) return "Pop";
            if (boardPlaces[currentPlayer] == 4) return "Pop";
            if (boardPlaces[currentPlayer] == 8) return "Pop";
            if (boardPlaces[currentPlayer] == 1) return "Science";
            if (boardPlaces[currentPlayer] == 5) return "Science";
            if (boardPlaces[currentPlayer] == 9) return "Science";
            if (boardPlaces[currentPlayer] == 2) return "Sports";
            if (boardPlaces[currentPlayer] == 6) return "Sports";
            if (boardPlaces[currentPlayer] == 10) return "Sports";
            return "Rock";
        }

        public bool EndTurnWithCorrectAnswerReturnHasntWon()
        {
            bool winner;
            if (playerIsInPenaltyBox[currentPlayer])
            {
                if (isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    playerScores[currentPlayer]++;
                    Console.WriteLine(playerNames[currentPlayer]
                            + " now has "
                            + playerScores[currentPlayer]
                            + " Gold Coins.");

                    winner = CurrentPlayerHasWon();
                    
                    currentPlayer++;
                    if (currentPlayer == playerNames.Count) currentPlayer = 0;

                    return winner;
                }
            
                currentPlayer++;
                if (currentPlayer == playerNames.Count) currentPlayer = 0;
                return true;
            }

            Console.WriteLine("Answer was corrent!!!!");
            playerScores[currentPlayer]++;
            Console.WriteLine(playerNames[currentPlayer]
                              + " now has "
                              + playerScores[currentPlayer]
                              + " Gold Coins.");

            winner = CurrentPlayerHasWon();
            currentPlayer++;
            if (currentPlayer == playerNames.Count) currentPlayer = 0;

            return winner;
        }

        public bool EndTurnWithWrongAnswerReturnHasntWon()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(playerNames[currentPlayer] + " was sent to the penalty box");
            playerIsInPenaltyBox[currentPlayer] = true;

            currentPlayer++;
            if (currentPlayer == playerNames.Count) currentPlayer = 0;
            return true;
        }


        private bool CurrentPlayerHasWon()
        {
            return !(playerScores[currentPlayer] == WinningScore);
        }
    }

}
