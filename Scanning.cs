using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScanSQL
{
    public class Scanning : Login
    {
        internal LoginForm _loginView;
        internal MainForm _mainView;
        internal ConnectionService _connectionService;
        internal SettingsSQL _settings;
        public HashSet<string> availableTables = new HashSet<string>(163);

        private const string _getAllTablesSqlCfgKey = "GetAllTablesSql";
        public string GetAllTablesSql { get; set; }

        public Scanning(LoginForm loginView,
                        ConnectionService connectionService,
                        MainForm mainView) : base(loginView, connectionService)
        {
            _connectionService = connectionService;
            _loginView = loginView;
            _mainView = mainView;
            _settings = new SettingsSQL();
        }

        internal void GetAllTables()
        {
            using (var command = new SqlCommand(_settings._getAllTablesDefault, _connectionService.SqlConnection))
            {
                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var TableName = FormatTableNames(reader[0].ToString());
                            availableTables.Add(TableName);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Get All tables execution failed\nQUERY: {GetAllTablesSql}\n{e.Message}",
                                    "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);

                    availableTables = new HashSet<string>(163);
                }
            }
        }

        internal void CreateTrigger()
        {
            string query;
            foreach (var table in availableTables)
            {
                //Check trigger exists in table, if not exists - Create
                //Create trigger for insert
                if (!CheckTriggersExist(_settings._checkInsertTriggertExists, table))
                {
                    query = CreateTriggerHelper("insert", table);
                    ExecuteQuery(_loginView.ConnectionString, query);
                }
                //Check trigger exists in table, if not exists - Create
                //Create trigger for update
                if (!CheckTriggersExist(_settings._checkUpdateTriggertExists, table))
                {
                    query = CreateTriggerHelper("update", table);
                    ExecuteQuery(_loginView.ConnectionString, query);
                }

                //Check trigger exists in table, if not exists - Create
                //Create trigger for delete
                if (!CheckTriggersExist(_settings._checkDeleteTriggertExists, table))
                {
                    query = CreateTriggerHelper("delete", table);
                    ExecuteQuery(_loginView.ConnectionString, query);
                }
            }
        }

        private string CreateTriggerHelper(string triggerType, string tableName)
        {
            string query;
            switch (triggerType.ToLower())
            {
                case "insert":
                    query = string.Format(_settings._createInsertTrigger, tableName);
                    break;
                case "update":
                    query = string.Format(_settings._createUpdateTrigger, tableName);
                    break;
                case "delete":
                    query = string.Format(_settings._createDeleteTrigger, tableName);
                    break;
                default:
                    throw new ArgumentException("invalid trigger");
            };
            return query;
        }

        internal void RemoveTriggers()
        {
            string query;
            foreach (var table in availableTables)
            {
                //Remove insert trigger  
                query = RemoveTriggersHelper("insert", table);
                ExecuteQuery(_loginView.ConnectionString, query);

                //Remove update trigger  
                query = RemoveTriggersHelper("update", table);
                ExecuteQuery(_loginView.ConnectionString, query);

                //Remove delete trigger  
                query = RemoveTriggersHelper("delete", table);
                ExecuteQuery(_loginView.ConnectionString, query);
            }
        }

        private string RemoveTriggersHelper(string triggerType, string tableName)
        {
            string query;
            switch (triggerType.ToLower())
            {
                case "insert":
                    query = string.Format(_settings._removeInsertTrigger, tableName);
                    break;
                case "update":
                    query = string.Format(_settings._removeUpdateTrigger, tableName);
                    break;
                case "delete":
                    query = string.Format(_settings._removeDeleteTrigger, tableName);
                    break;
                default:
                    throw new ArgumentException("invalid trigger");
            };
            return query;
        }

        internal void CreateAuditLogTable()
        {
            ExecuteQuery(_loginView.ConnectionString, _settings._createAuditLogTable);
        }

        internal void TruncateAuditLogTable()
        {
            ExecuteQuery(_loginView.ConnectionString, _settings._truncateAuditLogTable);
        }

        internal void DropAuditLogTable()
        {
            ExecuteQuery(_loginView.ConnectionString, _settings._dropAuditLogTable);
        }

        private bool CheckTriggersExist(string query, string tableName)
        {
            var sqlQuery = string.Format(query, tableName);
            return CheckTableExist(sqlQuery);
        }

        internal bool CheckTableExist(string query)
        {
            try
            {
                using (var command = new SqlCommand(query, _connectionService.SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }

            catch (SqlException ex)
            {
                // Handle the SQL exception, and optionally restart the program
                MessageBox.Show("SQL error occurred: " + ex.Message + "\nClose the application and login again ", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        public void ExecuteQuery(string connectionString, string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private string FormatTableNames(string tableName)
        {
            var index = tableName.IndexOf('.') + 2;
            return tableName.Substring(index, tableName.Length - (index + 1));
        }
    }
}
