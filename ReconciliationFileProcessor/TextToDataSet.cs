using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace ReconciliationFileProcessor
{
    public class TextToDataSet
    {
        public static DataSet Convert(StreamReader reader)
        {
            return Convert(reader, "MyTable", "\t");
        }

        private static DataSet Convert(StreamReader reader, string TableName, string delimiter)
        {
            //The DataSet to Return

            DataSet result = new DataSet();

            //Open the file in a stream reader.

            //StreamReader s = new StreamReader(File);
            StreamReader s = reader;

            //Split the first line into the columns       

            string[] columns = s.ReadLine().Split(delimiter.ToCharArray());

            //Add the new DataTable to the RecordSet

            result.Tables.Add(TableName);

            //Cycle the colums, adding those that don't exist yet 

            //and sequencing the one that do.

            foreach (string col in columns)
            {
                bool added = false;
                string next = "";
                int i = 0;
                while (!added)
                {
                    //Build the column name and remove any unwanted characters.

                    string columnname = col + next;
                    columnname = columnname.Replace("#", "");
                    columnname = columnname.Replace("'", "");
                    columnname = columnname.Replace("&", "");

                    //See if the column already exists

                    if (!result.Tables[TableName].Columns.Contains(columnname))
                    {
                        //if it doesn't then we add it here and mark it as added

                        result.Tables[TableName].Columns.Add(columnname.Trim());
                        added = true;
                    }
                    else
                    {
                        //if it did exist then we increment the sequencer and try again.

                        i++;
                        next = "_" + i.ToString();
                    }
                }
            }

            //Read the rest of the data in the file.        

            string AllData = s.ReadToEnd();

            //Split off each row at the Carriage Return/Line Feed

            //Default line ending in most windows exports.  

            //You may have to edit this to match your particular file.

            //This will work for Excel, Access, etc. default exports.

            string[] rows = AllData.Split("\r\n".ToCharArray());

            //Now add each row to the DataSet        

            foreach (string r in rows)
            {
                //Split the row at the delimiter.

                string[] items = r.Split(delimiter.ToCharArray());

                //Add the item

                result.Tables[TableName].Rows.Add(items);
            }

            //Return the imported data.        

            DataClearcing(result.Tables[0]);

            return result;
        }

        private static void DataClearcing(DataTable dt)
        {
            for (int row = dt.Rows.Count-1; row >= 0; row--)
            {
                object[] obj = dt.Rows[row].ItemArray;
                if (obj.Where(x => string.IsNullOrEmpty(x.ToString())).Count() > 0)
                {
                    dt.Rows[row].Delete();
                }
            }
        }
    }
}
