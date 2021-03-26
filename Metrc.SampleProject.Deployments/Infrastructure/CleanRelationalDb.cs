using System;
using System.Data;
using System.Data.SqlClient;

namespace Metrc.SampleProject.Deployments.Infrastructure
{
    public sealed class CleanRelationalDb
    {
        private const String DropAllObjects = @"
DECLARE @name NVARCHAR(128),
        @constraint NVARCHAR(254),
        @SQL NVARCHAR(254);

/* Drop all non-system stored procs */
SET @name = (SELECT TOP (1) [name] FROM sysobjects WHERE [type] = 'P' AND category = 0 ORDER BY [name]);
WHILE @name is not null
BEGIN
    SET @SQL = 'DROP PROCEDURE [dbo].[' + RTRIM(@name) +']';
    EXEC sp_executesql @SQL;
    PRINT 'Dropped Procedure: ' + @name;
    SET @name = (SELECT TOP (1) [name] FROM sysobjects WHERE [type] = 'P' AND category = 0 AND [name] > @name ORDER BY [name]);
END

/* Drop all views */
SET @name = (SELECT TOP (1) [name] FROM sysobjects WHERE [type] = 'V' AND category = 0 ORDER BY [name]);
WHILE @name IS NOT NULL
BEGIN
    SET @SQL = 'DROP VIEW [dbo].[' + RTRIM(@name) +']';
    EXEC sp_executesql @SQL;
    PRINT 'Dropped View: ' + @name;
    SET @name = (SELECT TOP (1) [name] FROM sysobjects WHERE [type] = 'V' AND category = 0 AND [name] > @name ORDER BY [name]);
END

/* Drop all functions */
SET @name = (SELECT TOP (1) [name] FROM sysobjects WHERE [type] IN (N'FN', N'IF', N'TF', N'FS', N'FT') AND category = 0 ORDER BY [name]);
WHILE @name IS NOT NULL
BEGIN
    SET @SQL = 'DROP FUNCTION [dbo].[' + RTRIM(@name) +']';
    EXEC sp_executesql @SQL;
    PRINT 'Dropped Function: ' + @name;
    SET @name = (SELECT TOP (1) [name] FROM sysobjects WHERE [type] IN (N'FN', N'IF', N'TF', N'FS', N'FT') AND category = 0 AND [name] > @name ORDER BY [name]);
END

/* Drop all Foreign Key constraints */
SET @name = (SELECT TOP (1) TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_CATALOG = DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' ORDER BY TABLE_NAME);
WHILE @name is not null
BEGIN
    SET @constraint = (SELECT TOP (1) CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_CATALOG = DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME);
    WHILE @constraint IS NOT NULL
    BEGIN
        SET @SQL = 'ALTER TABLE [dbo].[' + RTRIM(@name) +'] DROP CONSTRAINT ' + RTRIM(@constraint);
        EXEC sp_executesql @SQL;
        PRINT 'Dropped FK Constraint: ' + @constraint + ' on ' + @name;
        SET @constraint = (SELECT TOP (1) CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_CATALOG = DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND CONSTRAINT_NAME <> @constraint AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME);
    END
    SET @name = (SELECT TOP (1) TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_CATALOG = DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' ORDER BY TABLE_NAME);
END

/* Drop all tables */
SET @name = (SELECT TOP (1) [name] FROM sysobjects WHERE [type] = 'U' AND category = 0 ORDER BY [name]);
WHILE @name IS NOT NULL
BEGIN
    SET @SQL = 'DROP TABLE [dbo].[' + RTRIM(@name) +']';
    EXEC sp_executesql @SQL;
    PRINT 'Dropped Table: ' + @name;
    SET @name = (SELECT TOP (1) [name] FROM sysobjects WHERE [type] = 'U' AND category = 0 AND [name] > @name ORDER BY [name]);
END";

        public CleanRelationalDb(String connectionString)
        {
            _ConnectionString = connectionString;
        }

        public void Execute()
        {
            using (var connection = new SqlConnection(_ConnectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = DropAllObjects;
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        private readonly String _ConnectionString;
    }
}
