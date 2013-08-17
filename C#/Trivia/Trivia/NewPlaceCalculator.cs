namespace Trivia
{
    public class NewPlaceCalculator 
    {
        public int CalculateNewPlace(int currentPlace, int roll, int numberOfPlaces)
        {
            return (currentPlace + roll) % numberOfPlaces;
        }
    }
}