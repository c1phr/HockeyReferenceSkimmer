using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
            foreach (var skater in skaters.Select("Pos = 'D'"))
            {
                if (Int32.Parse(skater["GP"].ToString()) > 59)
                {
                    var skaterProfile = roster.Select("Player = '" + skater["Player"].ToString().Replace(@"'", "") + "'").First();
                    string[] skaterData = { skater["Player"].ToString(), team, skater["PTS"].ToString(), skater["+/-"].ToString(), skaterProfile["Salary"].ToString().Replace(",", "").Replace("$", "") };
                    FilteredTable.Rows.Add(skaterData);
                }
				
            }
        }

		public DataTable GetFilteredData()
		{
			return FilteredTable;
		}

		public static void ExportCSV(DataTable FilteredData, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            // Grab the column names
			IEnumerable<string> cols = FilteredData.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            Console.WriteLine(string.Join(",", cols));
            sb.AppendLine(string.Join(",", cols));

            foreach (DataRow row in FilteredTable.Rows)
            {
                IEnumerable<string> data = row.ItemArray.Select(field => field.ToString());
                Console.WriteLine(string.Join(",", data));
                sb.AppendLine(string.Join(",", data));
            }

            File.WriteAllText(fileName, sb.ToString());
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
