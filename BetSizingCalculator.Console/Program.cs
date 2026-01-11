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

		var betTaker = new BetTaker(bankroll);

		foreach (var betMatchup in bettingMatchups.Where(x => x.AwayKellyFactor > 0 || x.HomeKellyFactor > 0))
		{
			System.Console.WriteLine($"{betMatchup.AwayName} @ {betMatchup.HomeName}");

			System.Console.WriteLine($"{betMatchup.AwayName} Money Line:");
			var awayMoneyLine = int.Parse(System.Console.ReadLine());
			System.Console.WriteLine($"{betMatchup.HomeName} Money Line:");
			var homeMoneyLine = int.Parse(System.Console.ReadLine());

			betMatchup.AwayMarketMoneyLine = awayMoneyLine;
			betMatchup.HomeMarketMoneyLine = homeMoneyLine;

			var bets = betTaker.TryPlaceBet(betMatchup);
			System.Console.WriteLine("");
			System.Console.WriteLine($"{betMatchup.AwayName} Bet: {bets.AwayBet:C}, {betMatchup.HomeName} Bet: {bets.HomeBet:C}");
			System.Console.WriteLine("");
			System.Console.WriteLine($"Bankroll: {betTaker.Bankroll:C}");
			System.Console.WriteLine("");
		}
	}
}
