using System.Collections.Generic;
using System.Linq;
namespace Assets.Scripts.MySql
{
    public class MySqlModel
    {
        private MySqlModel() { }
        private static MySqlModel m_MySqlModel;
        public static MySqlModel Instance => m_MySqlModel ?? (m_MySqlModel = new MySqlModel());
        public readonly MySqlAccess mySqlAccess = new MySqlAccess();
        public const string TableName = "tablebattery";

        public void CreateLocalDbIfNotExsit()
        {
            mySqlAccess.CreateLocalDbIfNotExsit();
        }

        public void CreateRemoteDbIfNotExsit()
        {
            mySqlAccess.CreateRemoteDbIfNotExsit();
        }

        public void CreateLocalTableIfNotExsit(object obj)
        {
            var colNames = obj.GetType().GetFields().Select(field => field.Name).ToList();
            colNames.Insert(0,"id");
            var colTypes = new List<string> {"int(11)"};
            for (var i = 1; i < colNames.Count; i++) colTypes.Add("varchar(45)");
            mySqlAccess.CreateLocalTableIfNotExsit(TableName, colNames.ToArray(), colTypes.ToArray());
        }

        public void CreateRemoteTableIfNotExsit(object obj)
        {
            var colNames = obj.GetType().GetFields().Select(field => field.Name).ToList();
            colNames.Insert(0, "id");
            var colTypes = new List<string> {"int(11)"};
            for (var i = 1; i < colNames.Count; i++) colTypes.Add("varchar(45)");
            mySqlAccess.CreateRemoteTableIfNotExsit(TableName, colNames.ToArray(), colTypes.ToArray());
        }

        public void LocalInsert(object obj)
        {
            var fieldsDic = obj.GetType().GetFields().ToDictionary(field => field.Name, field => field.GetValue(obj).ToString());
            var colNames = fieldsDic.Keys.ToArray();
            var values = fieldsDic.Values.ToArray();
            mySqlAccess.LocalInsert(TableName, colNames, values);
        }

        public void RemoteInsert(object obj)
        {
            var fieldsDic = obj.GetType().GetFields().ToDictionary(field => field.Name, field => field.GetValue(obj).ToString());
            var colNames = fieldsDic.Keys.ToArray();
            var values = fieldsDic.Values.ToArray();
            mySqlAccess.RemoteInsert(TableName, colNames, values);
        }
    }
}