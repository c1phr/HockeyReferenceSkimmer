using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace HockeyReferenceSkimmer
{
    class TeamHtmlParser
    {

        public static HtmlDocument GetTeamTables(string teamCode, string year = "2015")
        {
            var url = "http://www.hockey-reference.com/teams/" + teamCode + "/" + year + ".html";
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.OptionWriteEmptyNodes = true;
            var request = HttpWebRequest.Create(url);
            Stream stream = request.GetResponse().GetResponseStream();
            doc.Load(stream);
            stream.Close();
            return doc;
        }

		public static DataTable GetTeamRoster(HtmlDocument doc)
        {
            //var headers = doc.DocumentNode.QuerySelectorAll("#roster th .tooltip .sort_default_asc");
            string[] headers = {"No.", "Player", "Pos", "Age", "Ht", "Wt", "S/C", "Exp", "Birth Date", "Summary", "Salary"};
            DataTable roster = new DataTable();
            foreach (string header in headers)
            {
                roster.Columns.Add(header);

            }
            foreach (var row in doc.DocumentNode.QuerySelectorAll("#roster tbody tr"))
            {				
                //var tdRow = row.ChildNodes.Select(td => td.InnerText).Except(new[] { "/n ", "\n  ", "\n", "\n   " }).ToArray();
				var tdRow = row.InnerText.Replace(" ", "").Split(Environment.NewLine.ToCharArray());
				var tdList = new List<string>(tdRow);

				for (int i=0; i < tdList.Count; i++)
				{
					// Clean up some extra strings that sometimes get tacked on the end
					if (string.IsNullOrEmpty(tdList[i]))
					{
						tdList.RemoveAt(i);
					}
				}
				roster.Rows.Add(tdList.ToArray());
            }

            return roster;
        }

        public static DataTable GetTeamSkaters(HtmlDocument doc)
        {
            //var headers = doc.DocumentNode.QuerySelectorAll("#skaters tr th");
            string[] headers =
            {
                "Rk", "Player", "Pos", "Age", "GP", "G", "A", "PTS", "+/-", "PIM", "GoalsEV", "GoalsPP",
                "GoalsSH", "GoalsGW", "AssistsEV", "AssistsPP", "AssistsSH", "S", "S%", "TOI", "ATOI", "OPS", "DPS",
                "PS"
            };
            DataTable skaters = new DataTable();
            foreach (string header in headers)
            {
                skaters.Columns.Add(header);
            }

            foreach (var row in doc.DocumentNode.QuerySelectorAll("#skaters tbody tr"))
            {        
                //var tdRow = row.ChildNodes.Select(td => td.InnerText).Except(new[] { "/n ", "\n  ", "\n", "\n   " }).ToArray();
				var tdRow = row.InnerText.Split(Environment.NewLine.ToCharArray());
				var tdList = new List<string>(tdRow);
				
				for (int i=0; i < tdList.Count; i++)
				{
					// Clean up some extra strings that sometimes get tacked on the end
					if (string.IsNullOrEmpty(tdList[i]))
					{
						tdList.RemoveAt(i);
					}
					else
					{
						tdList[i] = tdList[i].Trim();
					}
				}

				skaters.Rows.Add(tdList.ToArray());
            }

            return skaters;
        }
    }
}
