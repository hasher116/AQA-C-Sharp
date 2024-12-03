using PowerBank_AQA_DbTestingCore.Infrastructure;
using System.Data;
using System.Text;

namespace PowerBank_AQA_DbTestingCore.DbExtensions
{
    public static class Extensions
    {
        public static (string str, bool isMoreMaxRows) ConvertToString(this DataTable dataTable)
        {
            if (dataTable == null)
            {
                return (null, false);
            }

            var isMoreMaxRows = dataTable.Rows.Count > Constants.MAX_ROWS;
            var rowCount = isMoreMaxRows ? Constants.MAX_ROWS : dataTable.Rows.Count;
            var columnWidths = dataTable.GetColumnSize();

            var str = new StringBuilder();

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var text = dataTable.Columns[i].ColumnName.Shorten();
                str.Append("|" + PadCenter(text, columnWidths[i] + 2));
            }

            str.Append($"|{Environment.NewLine}{new string('=', str.Length)}{Environment.NewLine}");

            var currentRow = 1;

            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    var text = row[i].ToString().Shorten();
                    str.Append("|" + PadCenter(text, columnWidths[i] + 2));
                }

                str.Append($"|{Environment.NewLine}");

                if (currentRow < rowCount)
                {
                    currentRow++;
                }

                else
                {
                    break;
                }
            }

            return (str.ToString(), isMoreMaxRows);
        }

        public static string ConvertToString(this DataRow dataRow)
        {
            var str = new StringBuilder();
            var columnWidths = GetColumnSize(dataRow.Table.Columns, dataRow);

            for (int i = 0; i < dataRow.Table.Columns.Count; i++)
            {
                var text = dataRow.Table.Columns[i].ColumnName.Shorten();
                str.Append("|" + PadCenter(text, columnWidths[i] + 2));
            }

            str.Append($"|{Environment.NewLine}{new string('=', str.Length - 2)}{Environment.NewLine}");

            for (int i = 0; i < dataRow.Table.Columns.Count; i++)
            {
                var text = dataRow[i].ToString().Shorten();
                str.Append("|" + PadCenter(text, columnWidths[i] + 2));
            }

            str.Append($"|{Environment.NewLine}");

            return str.ToString();
        }

        private static int[] GetColumnSize(this DataTable dataTable)
        {
            var columnWidths = new int[dataTable.Columns.Count];

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var length = dataTable.Columns[i].ColumnName.Length;
                if (columnWidths[i] < length)
                {
                    columnWidths[i] = length;
                }
            }

            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    var length = row[i].ToString()!.Length;
                    if (columnWidths[i] < length)
                    {
                        columnWidths[i] = length;
                    }
                }
            }

            return columnWidths;
        }

        private static int[] GetColumnSize(DataColumnCollection columns, DataRow dataRow)
        {
            var columnWidths = new int[columns.Count];

            for (int i = 0; i < columns.Count; i++)
            {
                var length = columns[i].ColumnName.Length;
                if (columnWidths[i] < length)
                {
                    columnWidths[i] = length;
                }
            }

            for (int i = 0; i < columns.Count; i++)
            {
                var length = dataRow[i].ToString()!.Length;
                if (columnWidths[i] < length)
                {
                    columnWidths[i] = length;
                }
            }

            return columnWidths;
        }

        private static string Shorten(this string str)
        {
            return
                str.Length > Constants.MAX_LENGTH ?
                str[..^3] + "..." : str;
        }

        private static string PadCenter(string text, int maxLength)
        {
            var diff = maxLength - text.Length;
            var result = new string(' ', diff / 2) + text + new string(' ', (int)((diff / 2) + 0.5));
            if (result.Length < maxLength)
            {
                result = result + new string(' ', maxLength - result.Length);
            }
            return result;
        }
    }
}
