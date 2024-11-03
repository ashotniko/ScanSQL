namespace ScanSQL
{
    public class Login
    {
        private LoginForm _loginView;
        private ConnectionService _connectionService;

        public Login(LoginForm view, ConnectionService connectionService)
        {
            _loginView = view;
            _connectionService = connectionService;
        }

        public string CreateConnectionString(string nameDB, string serverName, string userName, string password)
        {
            return $"Data Source={nameDB};Initial Catalog={serverName};User ID={userName};Password={password};";
        }

        public bool TryConnectToDatabase(string connectionString)
        {
            return _connectionService.TryConnect(connectionString);
        }
    }
}
