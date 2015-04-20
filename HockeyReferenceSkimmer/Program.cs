using System;
using HtmlAgilityPack;
using System.Data;
using System.Linq;

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
            var processor2014 = new TableBuilder();
			var processor2015 = new TableBuilder();
		    foreach (var team in Teams)
		    {
				Console.WriteLine ("Processing 2014: " + team);
				HtmlDocument TeamTable2014 = TeamHtmlParser.GetTeamTables (team, "2014");
                var roster2014 = TeamHtmlParser.GetTeamRoster(TeamTable2014);
                var skaters2014 = TeamHtmlParser.GetTeamSkaters(TeamTable2014);                
                processor2014.FilterData(skaters2014, roster2014, team);

				Console.WriteLine ("Processing 2015: " + team);
				HtmlDocument TeamTable2015 = TeamHtmlParser.GetTeamTables (team, "2015");
				var roster2015 = TeamHtmlParser.GetTeamRoster (TeamTable2015);
				var skaters2015 = TeamHtmlParser.GetTeamSkaters (TeamTable2015);
				processor2015.FilterData (skaters2015, roster2015, team);
		    }
			DataTable Data2014 = processor2014.GetFilteredData ();
			DataTable Data2015 = processor2015.GetFilteredData ();
			DataTable DataMerged = new DataTable ();
			foreach (DataColumn col in Data2015.Columns)
			{
				DataMerged.Columns.Add (col);
			}
			foreach (DataRow row in Data2015)
			{
				
			}
		    
			TableBuilder.ExportCSV(DataMerged, "ProjectData.csv");
		}

	}
}
