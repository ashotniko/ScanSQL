using ScanSQL.Enums;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScanSQL
{
    public class OutputLog : Scanning
    {
        public List<string> _outputList = new List<string>();
        public Stack<List<string>> _firstStuck = new Stack<List<string>>();
        public Stack<List<string>> _secondStuck = new Stack<List<string>>();
        private string _tableNameFormatted;
        private string _auditActionFormatted;
        private string _oldValuesFormatted;
        private string _newValuesFormatted;
        private string _pageNumber;
        private int _index = 0;
        public OutputLog(LoginForm loginView,
                         ConnectionService connectionService,
                         MainForm mainView,
                         ProducteMode mode) : base(loginView, connectionService, mainView, mode)
        {

        }

        public void CreateTriggerForTables()
        {
            this._mainView.Log("________________________________________________________________________________________________________");
            this._mainView.Log("Working process started. Please wait until the process is complete.");
            this._mainView.Log("________________________________________________________________________________________________________");
            this._mainView.Log("Preparing tables...");
            this.CreateLastTables();
            this.GetAllTables();
            this.CheckTableExistsInDb();
            this._mainView.Log("Creating the AuditLog table...");
            this.CreateAuditLogTable();
            this._mainView.Log("Loading... This may take some time.");
            this.CreateTrigger();
            this.CheckTablesWithoutTrigger();
            this._mainView.Log("Process completed.");
            this._mainView.Log("You can now start.");
            this._mainView.Log("________________________________________________________________________________________________________");
        }

        public void RemoveCreatedTriggers()
        {
            this._mainView.Log("________________________________________________________________________________________________________");
            this._mainView.Log("Working process started. Please wait until the process is complete.");
            this._mainView.Log("________________________________________________________________________________________________________");
            this._mainView.Log("Preparing tables...");
            this.GetAllTables();
            this.CheckTableExistsInDb();
            this._mainView.Log("Removing the AuditLog table...");
            this.DropAuditLogTable();
            this._mainView.Log("Loading... This may take some time.");
            this.RemoveTriggers();
            this._mainView.Log("Process completed.");
            this._mainView.Log("________________________________________________________________________________________________________");
        }

        public void ScanAuditLog()
        {
            this.RefresStucks();

            if (this.CheckTableExist(this._settings._checkAuditLogsExits))
            {
                this.GetValueFromSql(this._settings._selectForAuditLogs);
                this._outputList = new List<string>();
            }
            else
            {
                MessageBox.Show($"AuditLog table doesn't exists, \r\n " +
                                $"please click on 'Start' button \r\n " +
                                $"to create AuditLog tabel", "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

        }

        private void GetValueFromSql(string query)
        {
            try
            {
                using (var command = new SqlCommand(query, this._connectionService.SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        this._index++;
                        this._pageNumber = this.StringFormat("Page", this._index.ToString());
                        this._mainView.Log("________________________________________________________________________________________________________________________________________________");
                        this._mainView.Log(this._pageNumber);
                        this._mainView.Log("________________________________________________________________________________________________________________________________________________");
                        this._mainView.Log("Scanning tables where triggers work.");

                        //Add output to list to track it
                        this._outputList.Add(this._pageNumber);
                        this._outputList.Add("________________________________________________________________________________________________________________________________________________");
                        this._outputList.Add("Scanning tables where triggers work.");

                        while (reader.Read())
                        {
                            this._tableNameFormatted = this.StringFormat("TableName", reader.GetValue(0).ToString());
                            this._auditActionFormatted = this.StringFormat("AuditAction", reader.GetValue(1).ToString());
                            this._oldValuesFormatted = this.StringFormat("OldValues", reader.GetValue(2).ToString());
                            this._newValuesFormatted = this.StringFormat("NewValues", reader.GetValue(3).ToString());

                            //Output the information
                            this._mainView.Log("________________________________________________________________________________________________________________________________________________");
                            this._mainView.Log(this._tableNameFormatted);
                            this._mainView.Log(this._auditActionFormatted);
                            this._mainView.Log(this._oldValuesFormatted);
                            this._mainView.Log(this._newValuesFormatted);
                            this._mainView.Log("________________________________________________________________________________________________________________________________________________");

                            //Add output to list to track it
                            this._outputList.Add("________________________________________________________________________________________________________________________________________________");
                            this._outputList.Add(this._tableNameFormatted);
                            this._outputList.Add(this._auditActionFormatted);
                            this._outputList.Add(this._oldValuesFormatted);
                            this._outputList.Add(this._newValuesFormatted);
                            this._outputList.Add("________________________________________________________________________________________________________________________________________________");
                        }
                    }
                }
                this._mainView.Log("________________________________________________________________________________________________________________________________________________");
                this._mainView.Log("SCANNING TABLES WHERE TRIGGERS DOESN'T WORK. PLEASE WAIT UNTIL YOU SEE FINISH.");
                var changedTables = this.CheckTablesWithoutTrigger();
                this._mainView.Log(changedTables);
                this._mainView.Log("FINISH FINISH FINISH");

                this._outputList.Add("________________________________________________________________________________________________________________________________________________");
                this._outputList.Add("SCANNING TABLES WHERE TRIGGERS DOESN'T WORK. PLEASE WAIT UNTIL YOU SEE FINISH.");
                this._outputList.Add(changedTables);

                //Add list to stuck
                this._firstStuck.Push(this._outputList);
                //After Logging output, empty AuditLog table
                this.TruncateAuditLogTable();
            }
            catch (SqlException ex)
            {
                // Handle exception
                this._mainView.Log($"SQL Error: {ex.Message}");
            }

        }

        private string StringFormat(string rowName, string value)
        {
            var returnString =
                @"{0} - {1}";
            return string.Format(returnString, rowName, value);
        }

        public void StepBack()
        {
            this._secondStuck.Push(this._firstStuck.Pop());
            var stepBackOutput = this._firstStuck.Peek();

            foreach (var listItem in stepBackOutput)
            {
                this._mainView.Log(listItem);
            }
        }

        public void StepForward()
        {
            this._firstStuck.Push(this._secondStuck.Pop());
            var stepForwardOutput = this._firstStuck.Peek();

            foreach (var listItem in stepForwardOutput)
            {
                this._mainView.Log(listItem);
            }
        }

        internal void RefresStucks()
        {
            var length = this._secondStuck.Count;
            for (var i = 0; i <= length - 1; i++)
            {
                this._firstStuck.Push(this._secondStuck.Pop());
            }
        }

        internal void ClearOutputHistory()
        {
            this._firstStuck = new Stack<List<string>>();
            this._secondStuck = new Stack<List<string>>();
            this._index = 0;
            //this.ResetTables();
        }
    }
}
