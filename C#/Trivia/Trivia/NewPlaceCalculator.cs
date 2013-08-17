namespace Trivia
{
    public class NewPlaceCalculator 
    {
        public int CalculateNewPlace(int currentPlace, int roll, int numberOfPlaces)
        {
            int result = (currentPlace + roll)%numberOfPlaces;
            return result;
        }
    }
}