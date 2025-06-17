using ScanSQL.Enums;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanSQL
{
    public partial class MainForm : Form
    {

        Scanning _scan;
        OutputLog _output;
        ConnectionService _connectionService;
        internal bool scanButtonDIsabled = true;

        public MainForm(LoginForm loginForm, ConnectionService connectionService)
        {
            _connectionService = connectionService;
            _scan = new Scanning(loginForm, _connectionService, this, (ProducteMode)loginForm.cmbProductMode.SelectedItem);
            _output = new OutputLog(loginForm, _connectionService, this, (ProducteMode)loginForm.cmbProductMode.SelectedItem);
            InitializeComponent();
        }

        private void Start_Click(object sender, System.EventArgs e)
        {
            Output.Clear();
            _output.CreateTriggerForTables();
        }

        private void Output_TextChanged(object sender, System.EventArgs e)
        {

        }

        private async void Scan_Click(object sender, System.EventArgs e)
        {
            Output.Clear();
            await Task.Delay(1000);
            _output.ScanAuditLog();
        }


        private async void Reset_Click(object sender, System.EventArgs e)
        {
            Output.Clear();
            await Task.Delay(1000);
            _output.RemoveCreatedTriggers();
        }

        public void Log(string text)
        {
            if (InvokeRequired)
            {
                MethodInvoker invoker = () => Output.AppendText(text + "\r\n\r\n");
                Invoke(invoker);
            }

            else
            {
                Output.AppendText(text + "\r\n\r\n");
            }
        }

        private void VScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Display a confirmation dialog before closing
            var result = MessageBox.Show("Are you sure you want to close the application?",
                                         "Confirm Exit",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);


            // If the user selects 'Yes', 
            if (result == DialogResult.Yes)
            {
                _connectionService.CloseConnection();
            }
            // If the user selects 'No', cancel the close action
            else
            {
                e.Cancel = true;  // Prevents the form from closing
            }
        }

        private void PreviousButton_Click(object sender, System.EventArgs e)
        {
            if (_output._firstStuck.Count > 1)
            {
                Output.Clear();
                _output.StepBack();
            }
        }

        private void NextButton_Click(object sender, System.EventArgs e)
        {
            if (_output._secondStuck.Count > 1 || (_output._firstStuck.Count >= 1 && _output._secondStuck.Count >= 1))
            {
                Output.Clear();
                _output.StepForward();
            }
        }

        private void EmptyAuditLog_Click(object sender, System.EventArgs e)
        {
            _scan.TruncateAuditLogTable();
        }

        private void ClearOutputHistory_Click(object sender, System.EventArgs e)
        {
            Output.Clear();
            _output.ClearOutputHistory();
        }
    }
}
