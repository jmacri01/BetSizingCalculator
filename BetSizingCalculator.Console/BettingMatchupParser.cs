using System.Globalization;
using System.Text.RegularExpressions;

public static class BettingMatchupParser
{
	public static List<BettingMatchup> Parse(string input)
	{
		var results = new List<BettingMatchup>();

		// Split by blank lines (one matchup per block)
		var blocks = Regex.Split(input.Trim(), @"\r?\n\r?\n");

		foreach (var block in blocks)
		{
			var lines = block.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			if (lines.Length < 3)
				continue;

			// ─────────────────────────────────────────────
			// Line 1: Teams, probabilities, money lines
			// Example:
			// Clippers: 0.213, Pistons: 0.786 (+369, -367)
			// ─────────────────────────────────────────────

			var headerRegex = new Regex(
				@"^(.*?):\s*([\d.]+),\s*(.*?):\s*([\d.]+)\s*\(([-+]?\d+),\s*([-+]?\d+)\)$");

			var headerMatch = headerRegex.Match(lines[0]);
			if (!headerMatch.Success)
				throw new FormatException($"Invalid matchup header: {lines[0]}");

			var awayName = headerMatch.Groups[1].Value.Trim();
			var awayOdds = decimal.Parse(headerMatch.Groups[2].Value, CultureInfo.InvariantCulture);

			var homeName = headerMatch.Groups[3].Value.Trim();
			var homeOdds = decimal.Parse(headerMatch.Groups[4].Value, CultureInfo.InvariantCulture);

			// ─────────────────────────────────────────────
			// Line 2 & 3: Kelly factors
			// ─────────────────────────────────────────────

			var kellyRegex = new Regex(@"Kelly factor:\s*([\d.]+)");

			var awayKellyMatch = kellyRegex.Match(lines[1]);
			var homeKellyMatch = kellyRegex.Match(lines[2]);

			if (!awayKellyMatch.Success || !homeKellyMatch.Success)
				throw new FormatException("Invalid Kelly factor line.");

			var awayKelly = decimal.Parse(awayKellyMatch.Groups[1].Value, CultureInfo.InvariantCulture);
			var homeKelly = decimal.Parse(homeKellyMatch.Groups[1].Value, CultureInfo.InvariantCulture);

			// ─────────────────────────────────────────────
			// Build object
			// ─────────────────────────────────────────────

			results.Add(new BettingMatchup
			{
				AwayName = awayName,
				HomeName = homeName,
				AwayOdds = awayOdds,
				HomeOdds = homeOdds,
				AwayKellyFactor = awayKelly,
				HomeKellyFactor = homeKelly
			});
		}

		return results;
	}
}