public class BetTaker
{
	private readonly decimal _minimumEdge;

	public BetTaker(decimal startingBankroll) : this(startingBankroll, 0)
	{
		Bankroll = startingBankroll;
	}

	public BetTaker(decimal startingBankroll, decimal minimumEdge)
	{
		Bankroll = startingBankroll;
		_minimumEdge = minimumEdge;
	}

	public decimal Bankroll { get; private set; }

	public (decimal HomeBet, decimal AwayBet) TryPlaceBet(BettingMatchup matchup)
	{
		var awayImpliedMarketOdds = GetImpliedOdds(matchup.AwayMarketMoneyLine);
		var homeImpliedMarketOdds = GetImpliedOdds(matchup.HomeMarketMoneyLine);

		var awayKelly = CalculateFullKelly(matchup.AwayOdds, awayImpliedMarketOdds);
		var homeKelly = CalculateFullKelly(matchup.HomeOdds, homeImpliedMarketOdds);

		awayKelly *= matchup.AwayKellyFactor;
		homeKelly *= matchup.HomeKellyFactor;

		awayKelly = Math.Min(Math.Max(0, awayKelly), .1m);
		homeKelly = Math.Min(Math.Max(0, homeKelly), .1m);

		var awayBet = awayKelly * Bankroll;
		var homeBet = homeKelly * Bankroll;

		if (matchup.HomeOdds - homeImpliedMarketOdds < _minimumEdge)
		{
			homeBet = 0;
		}

		if (matchup.AwayOdds - awayImpliedMarketOdds < _minimumEdge)
		{
			awayBet = 0;
		}

		Bankroll -= (awayBet + homeBet);

		return (homeBet, awayBet);
	}

	private decimal GetImpliedOdds(int marketMoneyLine)
	{
		decimal impliedMarketOdds = 0;

		if (marketMoneyLine == 0)
		{
			return 1;
		}

		if (marketMoneyLine < 0)
		{
			impliedMarketOdds = -(decimal)marketMoneyLine / (100m + -(decimal)marketMoneyLine);
		}
		else
		{
			impliedMarketOdds = 100m / (100m + (decimal)marketMoneyLine);
		}

		return impliedMarketOdds;
	}

	private decimal CalculateFullKelly(decimal odds, decimal impliedMarketOdds)
	{
		if (impliedMarketOdds == 1)
		{
			return 0;
		}

		return (odds - impliedMarketOdds) / (1 - impliedMarketOdds);
	}
}