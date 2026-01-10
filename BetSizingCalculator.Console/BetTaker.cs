public class BetTaker
{
	public BetTaker(decimal startingBankroll)
	{
		Bankroll = startingBankroll;
	}

	public decimal Bankroll { get; private set; }

	public (decimal HomeBet, decimal AwayBet) TryPlaceBet(BettingMatchup matchup)
	{
		var awayKelly = CalculateFullKelly(matchup.AwayOdds, matchup.AwayMarketMoneyLine);
		var homeKelly = CalculateFullKelly(matchup.HomeOdds, matchup.HomeMarketMoneyLine);

		awayKelly *= matchup.AwayKellyFactor;
		homeKelly *= matchup.HomeKellyFactor;

		awayKelly = Math.Min(Math.Max(0, awayKelly), .1m);
		homeKelly = Math.Min(Math.Max(0, homeKelly), .1m);

		var awayBet = awayKelly * Bankroll;
		var homeBet = homeKelly * Bankroll;

		Bankroll -= (awayBet + homeBet);

		return (homeBet, awayBet);
	}

	private decimal CalculateFullKelly(decimal odds, int marketMoneyLine)
	{
		decimal impliedMarketOdds = 0;

		if (marketMoneyLine == 0)
		{
			return 0;
		}

		if (marketMoneyLine < 0)
		{
			impliedMarketOdds = -(decimal)marketMoneyLine / (100m + -(decimal)marketMoneyLine);
		}
		else
		{
			impliedMarketOdds = 100m / (100m + (decimal)marketMoneyLine);
		}

		return (odds - impliedMarketOdds) / (1 - impliedMarketOdds);
	}
}
