using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToExcel;

namespace HockeyReferenceSkimmer
{
    class TableBuilder
    {
        public DataTable FilteredTable { get; set; }

        public TableBuilder()
        {
            FilteredTable = new DataTable();
            string[] cols = {"Player", "Team", "Points", "Differential", "Annual Salary"};
            foreach (string col in cols)
            {
                FilteredTable.Columns.Add(col);
            }
        }
        public void FilterData(DataTable skaters, DataTable roster, string team)
        {
            foreach (var skater in skaters.Select("Pos = 'D' AND GP > 59"))
            {
                var skaterProfile = roster.Select("Player = '" + skater["Player"].ToString() + "'").First();
                string[] skaterData = {skater["Player"].ToString(), team, skater["PTS"].ToString(), skater["+/-"].ToString(), skaterProfile["Salary"].ToString().Replace(",", "").Replace("$", "")};
                FilteredTable.Rows.Add(skaterData);
            }
        }

        public void PrintFilterData()
        {
            foreach (DataRow row in FilteredTable.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.WriteLine(item);
                }                
            }
        }
    }
}
