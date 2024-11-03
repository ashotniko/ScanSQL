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
        string _tableNameFormatted;
        string _auditActionFormatted;
        string _oldValuesFormatted;
        string _newValuesFormatted;
        string _pageNumber;
        private int _index = 0;
        public OutputLog(LoginForm loginView,
                         ConnectionService connectionService,
                         MainForm mainView) : base(loginView, connectionService, mainView)
        {

        }

        public void CreateTriggerForTables()
        {
            _mainView.Log("________________________________________________________________________________________________________");
            _mainView.Log("Working process started. Please wait until the process is complete.");
            _mainView.Log("________________________________________________________________________________________________________");
            _mainView.Log("Preparing tables...");
            GetAllTables();
            _mainView.Log("Creating the AuditLog table...");
            CreateAuditLogTable();
            _mainView.Log("Loading... This may take some time.");
            CreateTrigger();
            _mainView.Log("Process completed.");
            _mainView.Log("You can now start.");
            _mainView.Log("________________________________________________________________________________________________________");
        }

        public void RemoveCreatedTriggers()
        {
            _mainView.Log("________________________________________________________________________________________________________");
            _mainView.Log("Working process started. Please wait until the process is complete.");
            _mainView.Log("________________________________________________________________________________________________________");
            _mainView.Log("Preparing tables...");
            GetAllTables();
            _mainView.Log("Removing the AuditLog table...");
            DropAuditLogTable();
            _mainView.Log("Loading... This may take some time.");
            RemoveTriggers();
            _mainView.Log("Process completed.");
            _mainView.Log("________________________________________________________________________________________________________");
        }

        public void ScanAuditLog()
        {
            RefresStucks();

            if (CheckTableExist(_settings._checkAuditLogsExits))
            {
                GetValueFromSql(_settings._selectForAuditLogs);
                _outputList = new List<string>();
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
                using (var command = new SqlCommand(query, _connectionService.SqlConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            _index++;
                            _pageNumber = StringFormat("Page", _index.ToString());
                            _mainView.Log("________________________________________________________________________________________________________________________________________________");
                            _mainView.Log(_pageNumber);
                            _mainView.Log("________________________________________________________________________________________________________________________________________________");

                            //Add output to list to track it
                            _outputList.Add(_pageNumber);
                            _outputList.Add("________________________________________________________________________________________________________________________________________________");
                        }
                        else
                        {
                            _mainView.Log("________________________________________________________________________________________________________________________________________________");
                            _mainView.Log("AuditLog table is empty. \r\n" +
                                          "Create, update or delete data to see the output");
                            _mainView.Log("________________________________________________________________________________________________________________________________________________");
                            return;
                        }

                        while (reader.Read())
                        {
                            _tableNameFormatted = StringFormat("TableName", reader.GetValue(0).ToString());
                            _auditActionFormatted = StringFormat("AuditAction", reader.GetValue(1).ToString());
                            _oldValuesFormatted = StringFormat("OldValues", reader.GetValue(2).ToString());
                            _newValuesFormatted = StringFormat("NewValues", reader.GetValue(3).ToString());

                            //Output the information
                            _mainView.Log("________________________________________________________________________________________________________________________________________________");
                            _mainView.Log(_tableNameFormatted);
                            _mainView.Log(_auditActionFormatted);
                            _mainView.Log(_oldValuesFormatted);
                            _mainView.Log(_newValuesFormatted);
                            _mainView.Log("________________________________________________________________________________________________________________________________________________");

                            //Add output to list to track it
                            _outputList.Add("________________________________________________________________________________________________________________________________________________");
                            _outputList.Add(_tableNameFormatted);
                            _outputList.Add(_auditActionFormatted);
                            _outputList.Add(_oldValuesFormatted);
                            _outputList.Add(_newValuesFormatted);
                            _outputList.Add("________________________________________________________________________________________________________________________________________________");
                        }

                        //Add list to stuck
                        _firstStuck.Push(_outputList);
                    }
                }
                //After Logging output, empty AuditLog table
                TruncateAuditLogTable();
            }
            catch (SqlException ex)
            {
                // Handle exception
                _mainView.Log($"SQL Error: {ex.Message}");
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
            _secondStuck.Push(_firstStuck.Pop());
            var stepBackOutput = _firstStuck.Peek();

            foreach (var listItem in stepBackOutput)
            {
                _mainView.Log(listItem);
            }
        }

        public void StepForward()
        {
            _firstStuck.Push(_secondStuck.Pop());
            var stepForwardOutput = _firstStuck.Peek();

            foreach (var listItem in stepForwardOutput)
            {
                _mainView.Log(listItem);
            }
        }

        internal void RefresStucks()
        {
            var length = _secondStuck.Count;
            for (var i = 0; i <= length - 1; i++)
            {
                _firstStuck.Push(_secondStuck.Pop());
            }
        }

        internal void ClearOutputHistory()
        {
            _firstStuck = new Stack<List<string>>();
            _secondStuck = new Stack<List<string>>();
            _index = 0;
        }
    }
}
