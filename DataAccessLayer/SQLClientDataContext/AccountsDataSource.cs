using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AccountsDataSource
    {
        private static string? connectionString;

        public AccountsDataSource(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Account>> Get()
        {
            var list = new List<Account>();
            const string strQuery = @"SELECT * FROM [dbo].[Accounts] Order By AccountNumber";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand command = new SqlCommand(strQuery, conn);
                DataSet dataSet = new DataSet();
                try
                {
                    await conn.OpenAsync();
                    var dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(dataSet);
                    dataAdapter.Dispose();
                    await conn.CloseAsync();

                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dataSet.Tables[0].Rows)
                        {
                            var entity = new Account
                            {
                                Account_ID = string.IsNullOrEmpty(row["Account_ID"].ToString()) ? 0 : int.Parse(row["Account_ID"].ToString()),
                                AccountName = string.IsNullOrEmpty(row["AccountName"].ToString()) ? string.Empty : row["AccountName"].ToString(),
                                AccountType = string.IsNullOrEmpty(row["AccountType"].ToString()) ? 0 : int.Parse(row["AccountType"].ToString()),
                                AccountNumber = string.IsNullOrEmpty(row["AccountNumber"].ToString()) ? string.Empty : row["AccountNumber"].ToString(),
                                SavingsBalance = string.IsNullOrEmpty(row["SavingsBalance"].ToString()) ? 0.00 : double.Parse(row["SavingsBalance"].ToString()),
                                CheckingBalance = string.IsNullOrEmpty(row["CheckingBalance"].ToString()) ? 0.00 : double.Parse(row["CheckingBalance"].ToString()),
                                CreditBalance = string.IsNullOrEmpty(row["CreditBalance"].ToString()) ? 0.00 : double.Parse(row["CreditBalance"].ToString()),
                                IsActive = !string.IsNullOrEmpty(row["IsActive"].ToString()) && bool.Parse(row["IsActive"].ToString()),
                                CreatedBy = string.IsNullOrEmpty(row["CreatedBy"].ToString()) ? string.Empty : row["CreatedBy"].ToString().ToLower(),
                                CreatedDate = (Convert.IsDBNull(row["CreatedDate"].ToString())) ? DateTime.Now : (row["CreatedDate"].ToString() == "") ? DateTime.Now : Convert.ToDateTime(row["CreatedDate"].ToString()),
                                UpdatedDate = (Convert.IsDBNull(row["UpdatedDate"].ToString())) ? DateTime.Now : (row["UpdatedDate"].ToString() == "") ? DateTime.Now : Convert.ToDateTime(row["UpdatedDate"].ToString()),
                                UpdatedBy = string.IsNullOrEmpty(row["UpdatedBy"].ToString()) ? string.Empty : row["UpdatedBy"].ToString().Trim().ToLower(),
                            };
                            if (entity.CreatedDate != null)
                            {
                                entity.CreatedDateStr = entity.CreatedDate.ToString("yyyy/MM/dd HH:mm:ss");
                            }
                            if (entity.UpdatedDate != null)
                            {
                                entity.UpdatedDateStr = entity.UpdatedDate.ToString("yyyy/MM/dd HH:mm:ss");
                            }
                            list.Add(entity);
                        }
                    }
                    await command.DisposeAsync();
                    dataSet.Dispose();
                }
                catch (Exception ex)
                {
                    await command.DisposeAsync();
                    dataSet.Dispose();
                    throw ex;
                }
            }
            return list;
        }
    }
}
