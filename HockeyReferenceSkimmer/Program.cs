using System;
using HtmlAgilityPack;

namespace HockeyReferenceSkimmer
{
	class MainClass
	{
	    public static string[] Teams
	    {
	        get { return new[] {"ANA", "ARI", "BOS", "BUF", "CAR", "CBJ", "CGY", "CHI", "COL", "DAL", "DET", "EDM", "FLA", "LAK", "MIN", "MTL", "NJD", "NSH", "NYI", "NYR", "OTT", "PHI", "PIT", "SJS", "STL", "TBL", "TOR", "VAN", "WPG", "WSH"}; }
	    }
		public static void Main (string[] args)
		{
            var processor = new TableBuilder();
		    foreach (var team in Teams)
		    {
				Console.WriteLine ("Processing: " + team);
                HtmlDocument TeamTable = TeamHtmlParser.GetTeamTables(team);
                var roster = TeamHtmlParser.GetTeamRoster(TeamTable);
                var skaters = TeamHtmlParser.GetTeamSkaters(TeamTable);                
                processor.FilterData(skaters, roster, team);
		    }
		    
            processor.ExportCSV("ProjectData.csv");
		}

	}
}
