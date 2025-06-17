namespace ScanSQL
{
    public class SettingsSQL
    {
        #region SQL commands
        public string _getAllTablesDefault =
            @"SELECT '[' + sys.schemas.name + '].[' + sys.tables.name + ']' AS TABLEFULLNAME 
            FROM sys.tables 
            JOIN sys.schemas ON sys.tables.schema_id = sys.schemas.schema_id 
            and sys.schemas.name = 'dbo' and sys.tables.name not in ('dtproperties','AuditLog')
            ORDER BY sys.tables.name";

        public string _checkAuditLogsExits =
            @"SELECT 1 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_SCHEMA = 'dbo' 
                AND TABLE_NAME = 'AuditLog'
            ";

        public string _createInsertTrigger =
            @"
            CREATE TRIGGER trg_{0}_Insert
            ON dbo.{0}
            AFTER INSERT
            AS
            BEGIN
                INSERT INTO AuditLog(TableName, AuditAction, AuditTime, UserName, NewValues)
                SELECT '{0}', 'INSERT', GETDATE(), SUSER_NAME(), (SELECT * FROM inserted FOR JSON AUTO, ROOT('Rows'))
            END;";

        public string _createUpdateTrigger =
            @"
            CREATE TRIGGER trg_{0}_Update
            ON dbo.{0}
            AFTER UPDATE
            AS
            BEGIN
                INSERT INTO AuditLog(TableName, AuditAction, AuditTime, UserName, OldValues, NewValues)
                SELECT '{0}', 'UPDATE', GETDATE(), SUSER_NAME(), (SELECT * FROM deleted FOR JSON AUTO, ROOT('Rows')), (SELECT * FROM inserted FOR JSON AUTO, ROOT('Rows'))
            END;";

        public string _createDeleteTrigger =
            @"
            CREATE TRIGGER trg_{0}_Delete
            ON dbo.{0}
            AFTER DELETE
            AS
            BEGIN
                INSERT INTO AuditLog(TableName, AuditAction, AuditTime, UserName, OldValues)
                SELECT '{0}', 'DELETE', GETDATE(), SUSER_NAME(), (SELECT * FROM deleted FOR JSON AUTO, ROOT('Rows'))
            END;";

        public string _removeInsertTrigger =
            @"
            IF EXISTS (SELECT * 
                       FROM sys.triggers 
                       WHERE name = 'trg_{0}_Insert' 
                       AND parent_id = OBJECT_ID('dbo.{0}'))
            BEGIN
                DROP TRIGGER dbo.trg_{0}_Insert;
            END
            ";

        public string _removeUpdateTrigger =
            @"
            IF EXISTS (SELECT * 
                       FROM sys.triggers 
                       WHERE name = 'trg_{0}_Update' 
                       AND parent_id = OBJECT_ID('dbo.{0}'))
            BEGIN
                DROP TRIGGER dbo.trg_{0}_Update;
            END
            ";

        public string _removeDeleteTrigger =
            @"
            IF EXISTS (SELECT * 
                       FROM sys.triggers 
                       WHERE name = 'trg_{0}_Delete' 
                       AND parent_id = OBJECT_ID('dbo.{0}'))
            BEGIN
                DROP TRIGGER dbo.trg_{0}_Delete;
            END
            ";

        public string _createAuditLogTable =
            @"
            IF NOT EXISTS (SELECT * 
                           FROM INFORMATION_SCHEMA.TABLES 
                           WHERE TABLE_SCHEMA = 'dbo' 
                           AND TABLE_NAME = 'AuditLog')
            BEGIN
                CREATE TABLE dbo.AuditLog (
                    TableName NVARCHAR(128),
                    AuditAction NVARCHAR(50),
                    AuditTime DATETIME,
                    UserName NVARCHAR(128),
                    OldValues NVARCHAR(MAX),
                    NewValues NVARCHAR(MAX)
                );
            END;";

        public string _checkInsertTriggertExists =
            @"
            SELECT 1 
            FROM sys.triggers 
            WHERE name = 'trg_{0}_Insert'
            ";

        public string _checkUpdateTriggertExists =
           @"
            SELECT 1 
            FROM sys.triggers 
            WHERE name = 'trg_{0}_Update'
            ";

        public string _checkDeleteTriggertExists =
           @"
            SELECT 1 
            FROM sys.triggers 
            WHERE name = 'trg_{0}_Delete'
            ";

        public string _truncateAuditLogTable =
            @"
            IF EXISTS(SELECT * 
                      FROM INFORMATION_SCHEMA.TABLES 
                      WHERE TABLE_SCHEMA = 'dbo' 
                      AND TABLE_NAME = 'AuditLog')
		              BEGIN

		              IF EXISTS(SELECT* FROM AuditLog)
			              		BEGIN
			              			TRUNCATE TABLE AuditLog
			              		END
		              END
            ";

        public string _dropAuditLogTable =
            @"
            IF EXISTS(SELECT * 
                      FROM INFORMATION_SCHEMA.TABLES 
                      WHERE TABLE_SCHEMA = 'dbo' 
                      AND TABLE_NAME = 'AuditLog')
            	BEGIN
            		DROP TABLE AuditLog
            	END
            ";

        public string _selectForAuditLogs =
            @"
                SELECT TableName, AuditAction, OldValues, NewValues
                FROM AuditLog
                WHERE NOT (OldValues IS NULL AND NewValues IS NULL)
                ORDER BY TableName";

        public string _getCheckSumDefault = @"SELECT CHECKSUM_AGG(BINARY_CHECKSUM(*)) FROM  {0} WITH (NOLOCK)";

        #endregion
    }
}
