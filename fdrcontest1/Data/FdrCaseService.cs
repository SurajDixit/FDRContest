using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Data.SqlClient;
using System.Data;

namespace fdrcontest1.Data
{
    public class FdrCaseService
    {

      //  private DataTable dataTable = new DataTable();

        private string ConnString = "Server=tcp:fdrcontest.database.windows.net,1433;Initial Catalog=FDRContestDB;Persist Security Info=False;User ID=surajadmin;Password='Passw0rd!';MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public Task<FdrCase[]> GetEngineerDataAsync(string email)
        {
            DataTable dataTable = new DataTable();
            String commandText = "select * from dbo.fdrcontesttable where email='" + email +"'";
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                command.Connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dataTable);
            }

            return Task.FromResult(dataTable.AsEnumerable().Select(item => new FdrCase
            {
                Engineer = item["engineer"].ToString(),
                CaseNumber = item["casenumber"].ToString(),
                Email = item["email"].ToString(),
                Description = item["description"].ToString(),
                IsFDR = item["isFDR"].ToString(),
                DateTime = item["submittedon"].ToString()
            }).ToArray());
        }

        public Task<FdrCase[]> GetAllEngineerDataAsync()
        {
            DataTable dataTable = new DataTable();
            String commandText = "select * from dbo.fdrcontesttable";
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                command.Connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dataTable);
            }

            return Task.FromResult(dataTable.AsEnumerable().Select(item => new FdrCase
            {
                Engineer = item["engineer"].ToString(),
                CaseNumber = item["casenumber"].ToString(),
                Email = item["email"].ToString(),
                Description = item["description"].ToString(),
                IsFDR = item["isFDR"].ToString(),
                DateTime = item["submittedon"].ToString()
            }).ToArray());
        }

        public Task<FdrCount[]> GetCountOfApprovedCasesPerEngineer()
        {
            int count = 1;
            DataTable dataTable = new DataTable();
            String commandText = "SELECT TOP 5 engineer, COUNT(*) as count from dbo.fdrcontesttable where isFDR='Approved' GROUP BY engineer ORDER BY [count] DESC";
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                command.Connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dataTable);
            }

            return Task.FromResult(dataTable.AsEnumerable().Select(item => new FdrCount
            {
                Position = count++,
                Engineer = item["engineer"].ToString(),
                CaseCount = item["count"].ToString()
            }).ToArray());
        }

        public Task<FdrCount[]> GetCountOfTotalCasesPerEngineer()
        {
            int count = 1;
            DataTable dataTable = new DataTable();
            String commandText = "SELECT  engineer, COUNT(*) as count from dbo.fdrcontesttable GROUP BY engineer ORDER BY [count] DESC";
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                command.Connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dataTable);
            }

            return Task.FromResult(dataTable.AsEnumerable().Select(item => new FdrCount
            {
                Position = count++,
                Engineer = item["engineer"].ToString(),
                CaseCount = item["count"].ToString()
            }).ToArray());
        }


    }
}
