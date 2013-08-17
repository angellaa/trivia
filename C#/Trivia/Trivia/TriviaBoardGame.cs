using System;
using System.Collections.Generic;
using System.Linq;
using Trivia;

namespace UglyTrivia
{
    public class TriviaBoardGame
    {
        private const int MaxNumberOfPlayers = 6;
        private const int WinningScore = 6;
        private const int NumberOfQuestionsPerCategory = 50;
        private const int MinimumPlayers = 2;
        private const int NumberOfPlaces = 12;
        private const string RockCategory = "Rock";

        List<string> playerNames = new List<string>();

        int[] boardPlaces = new int[MaxNumberOfPlayers];
        int[] playerScores = new int[MaxNumberOfPlayers];

        bool[] playerIsInPenaltyBox = new bool[MaxNumberOfPlayers];

        LinkedList<string> popQuestions = new LinkedList<string>();
        LinkedList<string> scienceQuestions = new LinkedList<string>();
        LinkedList<string> sportsQuestions = new LinkedList<string>();
        LinkedList<string> rockQuestions = new LinkedList<string>();

        private IOutputWriter outputWriter;

        int currentPlayer;
        bool isGettingOutOfPenaltyBox;

        public TriviaBoardGame() : this(new ConsoleOutputWriter())
        {
        }

        public TriviaBoardGame(IOutputWriter outputWriter)
        {

            if (outputWriter == null)
                throw new ArgumentNullException("outputWriter");

            this.outputWriter = outputWriter;

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

            if (string.IsNullOrWhiteSpace(playerName))
                throw new ArgumentNullException("playerName");

            playerNames.Add(playerName);

            outputWriter.WriteLine(playerName + " was added");
            outputWriter.WriteLine("They are player number " + playerNames.Count);
            return true;
        }

        public int GetNumberOfPlayers()
        {
            return playerNames.Count;
        }

        public void MoveCurrentPlayerAndAskQuestionIfEndsOutsidePenaltyBox(int roll)
        {
            if (roll <= 0)
                throw new ArgumentOutOfRangeException("roll", "Roll must be a positive number");

            outputWriter.WriteLine(playerNames[currentPlayer] + " is the current player");
            outputWriter.WriteLine("They have rolled a " + roll);

            if (playerIsInPenaltyBox[currentPlayer])
            {
                PlayInPenaltyBox(roll);
            }
            else
            {
                MovePlayerForward(roll);
            }
        }

        private void PlayInPenaltyBox(int roll)
        {
            bool isOddRoll = roll%2 != 0;

            if (isOddRoll)
            {
                isGettingOutOfPenaltyBox = true;

                outputWriter.WriteLine(playerNames[currentPlayer] + " is getting out of the penalty box");

                MovePlayerForward(roll);
            }
            else
            {
                outputWriter.WriteLine(playerNames[currentPlayer] + " is not getting out of the penalty box");
                isGettingOutOfPenaltyBox = false;
            }
        }

        private void MovePlayerForward(int roll)
        {
            var newPlaceCalculator = new NewPlaceCalculator();

            boardPlaces[currentPlayer] = newPlaceCalculator.CalculateNewPlace(boardPlaces[currentPlayer], roll,
                NumberOfPlaces);

            WriteLocation();
        }

        private void WriteLocation()
        {
            outputWriter.WriteLine(playerNames[currentPlayer]
                                   + "'s new location is "
                                   + boardPlaces[currentPlayer]);
            outputWriter.WriteLine("The category is " + CategoryForCurrentPlayersPlace());
            WriteQuestionToConsole();
        }

        private void WriteQuestionToConsole()
        {
            CheckPlayerIsOnPop();
            CheckPlayerIsOnScience();
            CheckPlayerIsOnSports();
            CheckPlayerIsOnRock();
        }

        private void CheckPlayerIsOnRock()
        {
            if (CategoryForCurrentPlayersPlace() == RockCategory)
            {
                outputWriter.WriteLine(rockQuestions.First());
                rockQuestions.RemoveFirst();
            }
        }

        private void CheckPlayerIsOnSports()
        {
            if (CategoryForCurrentPlayersPlace() == "Sports")
            {
                outputWriter.WriteLine(sportsQuestions.First());
                sportsQuestions.RemoveFirst();
            }
        }

        private void CheckPlayerIsOnScience()
        {
            if (CategoryForCurrentPlayersPlace() == "Science")
            {
                outputWriter.WriteLine(scienceQuestions.First());
                scienceQuestions.RemoveFirst();
            }
        }

        private void CheckPlayerIsOnPop()
        {
            if (CategoryForCurrentPlayersPlace() == "Pop")
            {
                outputWriter.WriteLine(popQuestions.First());
                popQuestions.RemoveFirst();
            }
        }


        private String CategoryForCurrentPlayersPlace()
        {
            int place = boardPlaces[currentPlayer];

            Dictionary<int, string> categories = new Dictionary<int, string>()
            {
                {0, "Pop"},
                {1, "Science"},
                {2, "Sports"},
                {3, "Rock"}
            };

            var module = ModulePlace(place);

            return categories[module];
        }

        private static int ModulePlace(int place)
        {
            return place % 4;
        }

        public bool EndTurnWithCorrectAnswerReturnHasntWon()
        {
            bool winner;
            if (playerIsInPenaltyBox[currentPlayer])
            {
                if (isGettingOutOfPenaltyBox)
                {
                    CurrentStatus();

                    winner = CurrentPlayerHasWon();
                    
                    ChangeCurrentPlayer();

                    return winner;
                }

                ChangeCurrentPlayer();
                return true;
            }

            CurrentStatus();

            winner = CurrentPlayerHasWon();
            ChangeCurrentPlayer();
            return winner;
        }

        private void CurrentStatus()
        {
            outputWriter.WriteLine("Answer was correct!!!!");
            playerScores[currentPlayer]++;
            outputWriter.WriteLine(playerNames[currentPlayer]
                                   + " now has "
                                   + playerScores[currentPlayer]
                                   + " Gold Coins.");
        }

        private void ChangeCurrentPlayer()
        {
            currentPlayer++;
            if (currentPlayer == playerNames.Count) currentPlayer = 0;
        }

        public bool EndTurnWithWrongAnswerReturnHasntWon()
        {
            outputWriter.WriteLine("Question was incorrectly answered");
            outputWriter.WriteLine(playerNames[currentPlayer] + " was sent to the penalty box");
            playerIsInPenaltyBox[currentPlayer] = true;

            ChangeCurrentPlayer();
            return true;
        }


        private bool CurrentPlayerHasWon()
        {
            return !(playerScores[currentPlayer] == WinningScore);
        }
    }

}
