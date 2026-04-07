namespace App_Bets.Application.Dtos
{
    public class CalcularOddRequest
    {
        public List<double> Odds { get; set; } = new();
    }
}