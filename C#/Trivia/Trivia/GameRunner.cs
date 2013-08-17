using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UglyTrivia;

namespace Trivia
{
    public class GameRunner
    {
        private const int MaximumRoll = 5;
        private const int MinimumRoll = 1;
        private const int WrongAnswer = 7;
        private const int PossibleAnswers = 9;

        private static bool notAWinner;

        public static void Main(String[] args)
        {
            TriviaBoardGame aTriviaBoardGame = new TriviaBoardGame();

            aTriviaBoardGame.AddPlayer("Chet");
            aTriviaBoardGame.AddPlayer("Pat");
            aTriviaBoardGame.AddPlayer("Sue");

            Random rand = args.Length != 0 ? new Random(args[0].GetHashCode()) : new Random();

            do
            {

                aTriviaBoardGame.MoveCurrentPlayerAndAskQuestionIfEndsOutsidePenaltyBox(rand.Next(MaximumRoll) + MinimumRoll);

                if (rand.Next(PossibleAnswers) == WrongAnswer)
                {
                    notAWinner = aTriviaBoardGame.EndTurnWithWrongAnswerReturnHasntWon();
                }
                else
                {
                    notAWinner = aTriviaBoardGame.EndTurnWithCorrectAnswerReturnHasntWon();
                }



            } while (notAWinner);

        }
    }

}

