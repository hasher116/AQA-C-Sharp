using System.Data;
using System.Reflection;

namespace PowerBank_AQA_ApiTesting.Hooks
{
    public static class Converter
    {
        public static List<T> ConvertDataTableToObject<T>(DataTable dataTable)
            where T : class, new()
        {
            List<T> list = new List<T>();

            foreach (DataRow row in dataTable.Rows)
            {
                T obj = new T();

                foreach (DataColumn col in dataTable.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(col.ColumnName);
                    if (prop != null && row[col] != DBNull.Value)
                    {
                        prop.SetValue(obj, Convert.ChangeType(row[col], prop.PropertyType), null);
                    }
                }

                list.Add(obj);
            }

            return list;
        }

        public static T ConvertDataRowToObject<T>(DataRow dataRow)
            where T : class, new()
        {
            T obj = new T();
            foreach (DataColumn col in dataRow.Table.Columns)
            {
                PropertyInfo prop = obj.GetType().GetProperty(col.ColumnName);
                if (prop != null && dataRow[col] != DBNull.Value)
                {
                    prop.SetValue(obj, Convert.ChangeType(dataRow[col], prop.PropertyType), null);
                }
            }

            return obj;
        }

        public static DataTable ConvertClassToDataTable<T>(List<T> objects)
        {
            DataTable dt = new DataTable();

            foreach (var prop in typeof(T).GetProperties())
            {
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in objects)
            {
                DataRow dataRow = dt.NewRow();
                foreach (var prop in typeof(T).GetProperties())
                {
                    dataRow[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                dt.Rows.Add(dataRow);
            }

            return dt;
        }

        public static DataRow ConvertClassToDataRow<T>(T obj)
        {
            DataTable dt = new DataTable();

            foreach (var prop in typeof(T).GetProperties())
            {
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            DataRow dataRow = dt.NewRow();
            foreach (var prop in typeof(T).GetProperties())
            {
                dataRow[prop.Name] = prop.GetValue(obj) ?? DBNull.Value;
            }

            dt.Rows.Add(dataRow);

            return dt.Rows[0];
        }
    }
}
