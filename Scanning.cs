using ScanSQL.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace ScanSQL
{
    public class Scanning : Login
    {
        internal LoginForm _loginView;
        internal MainForm _mainView;
        internal ConnectionService _connectionService;
        internal SettingsSQL _settings;
        private ProducteMode _mode;
        public HashSet<string> AvailableTables = new HashSet<string>();
        public HashSet<string> AllTables = new HashSet<string>();

        public Dictionary<string, int?> LastTables = new Dictionary<string, int?>();  // For this tables doesnt create triggers because there is conflict between  created triggers and bank product triggers
        public Dictionary<string, bool> TablesNeedeToCheck;
        private bool _firstTime;

        public string GetAllTablesSql { get; set; }

        public Scanning(LoginForm loginView,
                        ConnectionService connectionService,
                        MainForm mainView,
                        ProducteMode mode) : base(loginView, connectionService)
        {
            this._connectionService = connectionService;
            this._loginView = loginView;
            this._mainView = mainView;
            this._settings = new SettingsSQL();
            this._firstTime = true;
            this._mode = mode;
            if (this._mode == ProducteMode.Bank)
            {
                this.TablesNeedeToCheck = new Dictionary<string, bool>()
                {
                    {"BLACKLIST", true},
                    {"ACTRANS", true},
                    {"TEMPLATESMAPPING", true},
                    {"TEMPLATES", true},
                    {"DAHKCATCH", true},
                    {"DAHKFREEATTACH", true},
                    {"SYSDEF", true},
                    {"DCR", true},
                    {"CB_PROCERRORS", true},
                    {"CB_MESSAGES", true},
                    {"COM_PAYMENTS", true},
                    {"TIMESTAMP", true },
                    {"BLACKLISTCHANGEREQUESTS", false },
                    {"SWAPDOCS", true },
                    {"SWAPDOCSG", true },
                    {"UNITTESTLOG", false },
                    {"OLAPEXPORTLOG", false },
                    {"USERSET", false },
                };
            }
            else if (this._mode == ProducteMode.Enterprise)
            {
                this.TablesNeedeToCheck = new Dictionary<string, bool>()
                {
                    {"APICLIENTINFO", false},
                    {"DCR", false},
                    {"SESSIONINFO", false},
                    {"SYSDEF", false},
                    {"TEMPLATES", false},
                    {"APPRCONFIGBYUSER", false},
                    {"FAHI", true},
                    {"IBTRANSACTIONS", true},
                    {"MTREST", true}
                };
            }
        }

        internal void CreateLastTables()
        {
            foreach (var table in this.TablesNeedeToCheck)
            {
                if (table.Value == true && !this.LastTables.ContainsKey(table.Key))
                {
                    this.LastTables.Add(table.Key, 0);
                }
            }
        }

        internal void CheckTableExistsInDb()
        {
            foreach (var kvp in this.LastTables.ToList())
            {
                var table = kvp.Key;
                if (!this.AllTables.Contains(table))
                {
                    this.LastTables.Remove(table);
                }
            }
        }

        internal void GetAllTables()
        {
            using (var command = new SqlCommand(this._settings._getAllTablesDefault, this._connectionService.SqlConnection))
            {
                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var TableName = this.FormatTableNames(reader[0].ToString());
                            this.AllTables.Add(TableName);
                            if (!this.TablesNeedeToCheck.ContainsKey(TableName))
                            {
                                this.AvailableTables.Add(TableName);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Get All tables execution failed\nQUERY: {this.GetAllTablesSql}\n{e.Message}",
                                    "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);

                    this.AvailableTables = new HashSet<string>();
                    this.AllTables = new HashSet<string>();
                }
            }
        }

        internal void CreateTrigger()
        {
            string query;
            foreach (var table in this.AvailableTables)
            {
                //Check trigger exists in table, if not exists - Create
                //Create trigger for insert
                if (!this.CheckTriggersExist(this._settings._checkInsertTriggertExists, table))
                {
                    query = this.CreateTriggerHelper("insert", table);
                    this.ExecuteQuery(this._loginView.ConnectionString, query);
                }
                //Check trigger exists in table, if not exists - Create
                //Create trigger for update
                if (!this.CheckTriggersExist(this._settings._checkUpdateTriggertExists, table))
                {
                    query = this.CreateTriggerHelper("update", table);
                    this.ExecuteQuery(this._loginView.ConnectionString, query);
                }

                //Check trigger exists in table, if not exists - Create
                //Create trigger for delete
                if (!this.CheckTriggersExist(this._settings._checkDeleteTriggertExists, table))
                {
                    query = this.CreateTriggerHelper("delete", table);
                    this.ExecuteQuery(this._loginView.ConnectionString, query);
                }
            }
        }

        private string CreateTriggerHelper(string triggerType, string tableName)
        {
            string query;
            switch (triggerType.ToLower())
            {
                case "insert":
                    query = string.Format(this._settings._createInsertTrigger, tableName);
                    break;
                case "update":
                    query = string.Format(this._settings._createUpdateTrigger, tableName);
                    break;
                case "delete":
                    query = string.Format(this._settings._createDeleteTrigger, tableName);
                    break;
                default:
                    throw new ArgumentException("invalid trigger");
            }
            ;
            return query;
        }

        internal void RemoveTriggers()
        {
            string query;
            foreach (var table in this.AvailableTables)
            {
                //Remove insert trigger  
                query = this.RemoveTriggersHelper("insert", table);
                this.ExecuteQuery(this._loginView.ConnectionString, query);

                //Remove update trigger  
                query = this.RemoveTriggersHelper("update", table);
                this.ExecuteQuery(this._loginView.ConnectionString, query);

                //Remove delete trigger  
                query = this.RemoveTriggersHelper("delete", table);
                this.ExecuteQuery(this._loginView.ConnectionString, query);
            }
        }

        private string RemoveTriggersHelper(string triggerType, string tableName)
        {
            string query;
            switch (triggerType.ToLower())
            {
                case "insert":
                    query = string.Format(this._settings._removeInsertTrigger, tableName);
                    break;
                case "update":
                    query = string.Format(this._settings._removeUpdateTrigger, tableName);
                    break;
                case "delete":
                    query = string.Format(this._settings._removeDeleteTrigger, tableName);
                    break;
                default:
                    throw new ArgumentException("invalid trigger");
            }
            ;
            return query;
        }

        internal void CreateAuditLogTable()
        {
            this.ExecuteQuery(this._loginView.ConnectionString, this._settings._createAuditLogTable);
        }

        internal void TruncateAuditLogTable()
        {
            this.ExecuteQuery(this._loginView.ConnectionString, this._settings._truncateAuditLogTable);
        }

        internal void DropAuditLogTable()
        {
            this.ExecuteQuery(this._loginView.ConnectionString, this._settings._dropAuditLogTable);
        }

        private bool CheckTriggersExist(string query, string tableName)
        {
            var sqlQuery = string.Format(query, tableName);
            return this.CheckTableExist(sqlQuery);
        }

        internal bool CheckTableExist(string query)
        {
            try
            {
                using (var command = new SqlCommand(query, this._connectionService.SqlConnection))
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

        public int? ExecuteScalarQuery(string connectionString, string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    return command.ExecuteScalar() as int?;
                }
            }
        }

        private string FormatTableNames(string tableName)
        {
            var index = tableName.IndexOf('.') + 2;
            return tableName.Substring(index, tableName.Length - (index + 1));
        }


        public string CheckTablesWithoutTrigger()
        {
            var returnedValue = string.Empty;

            foreach (var kvp in this.LastTables.ToList())
            {
                var table = kvp.Key;
                var query = string.Format(this._settings._getCheckSumDefault, table);

                int? checksum = 0;
                try
                {
                    checksum = this.ExecuteScalarQuery(this._loginView.ConnectionString, query);
                }
                catch (SqlException e)
                {

                    this._mainView.Log(e.Message + " TABLE " + table);
                    this._mainView.Log("QUERY: " + query);
                    continue;
                }

                var printchanged = false;

                if (kvp.Value != checksum && checksum != null)
                {
                    printchanged = true;
                    this.LastTables[table] = checksum;
                }

                if (printchanged && !this._firstTime)
                {
                    returnedValue += $"CHANGE in TABLE: {table}\r\n";
                }
            }

            this._firstTime = false;

            return returnedValue;
        }

    }
}
