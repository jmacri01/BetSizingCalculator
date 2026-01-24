using System.IO;
using System.Text;

public class Program
{
	public static void Main()
	{
		System.Console.WriteLine("Projected Odds:");
		/* 
			Expected input format:
			Los Angeles Clippers: 0.213, Detroit Pistons: 0.786 (+369, -367)
			Los Angeles Clippers Kelly factor: 0.0
			Detroit Pistons Kelly factor: 0.10

			Miami Heat: 0.759, Indiana Pacers: 0.240 (-315, +317)
			Miami Heat Kelly factor: 0.10
			Indiana Pacers Kelly factor: 0.0
		*/

		//CTRL + Z to end
		var input = Console.In.ReadToEnd();

		var bettingMatchups = BettingMatchupParser.Parse(input);

		System.Console.WriteLine("Bankroll:");
		var bankroll = decimal.Parse(System.Console.ReadLine());

		System.Console.WriteLine("Minimum Edge (Number between 0 and 1):");
		var edgeFilter = decimal.Parse(System.Console.ReadLine());

		var betTaker = new BetTaker(bankroll, edgeFilter);

		foreach (var betMatchup in bettingMatchups)
		{
			System.Console.WriteLine($"{betMatchup.AwayName} @ {betMatchup.HomeName}");

			if(betMatchup.AwayKellyFactor == 0)
			{
				System.Console.ForegroundColor = ConsoleColor.Yellow;
				System.Console.WriteLine($"{betMatchup.AwayName} has 0 Kelly Factor.");
				System.Console.ForegroundColor = ConsoleColor.White;
				betMatchup.AwayMarketMoneyLine = 0;
			}
			else
			{
				System.Console.WriteLine($"{betMatchup.AwayName} Money Line:");
				betMatchup.AwayMarketMoneyLine = int.Parse(System.Console.ReadLine());
			}

			if (betMatchup.HomeKellyFactor == 0)
			{
				System.Console.ForegroundColor = ConsoleColor.Yellow;
				System.Console.WriteLine($"{betMatchup.HomeName} has 0 Kelly Factor.");
				System.Console.ForegroundColor = ConsoleColor.White;
				betMatchup.HomeMarketMoneyLine = 0;
			}
			else
			{
				System.Console.WriteLine($"{betMatchup.HomeName} Money Line:");
				betMatchup.HomeMarketMoneyLine = int.Parse(System.Console.ReadLine());
			}

			var bets = betTaker.TryPlaceBet(betMatchup);
			System.Console.WriteLine("");
			System.Console.ForegroundColor = ConsoleColor.Green;
			System.Console.WriteLine($"{betMatchup.AwayName} Bet: {bets.AwayBet:C}, {betMatchup.HomeName} Bet: {bets.HomeBet:C}");
			System.Console.ForegroundColor = ConsoleColor.White;
			System.Console.WriteLine("");
			System.Console.WriteLine($"Bankroll: {betTaker.Bankroll:C}");
			System.Console.WriteLine("");
		}
	}
}
