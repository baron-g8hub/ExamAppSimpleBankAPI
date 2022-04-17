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

        public async Task<List<Account>> SelectAsync()
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

        public async Task<Account> SelectAsync(int id)
        {
            var entity = new Account();
            const string strQuery = @"SELECT * FROM [dbo].[Accounts] WHERE AccountNumber = @AccountNumber";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand command = new SqlCommand(strQuery, conn);
                command.Parameters.AddWithValue("@AccountNumber", id);
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
                            entity = new Account
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
                                UpdatedBy = string.IsNullOrEmpty(row["UpdatedBy"].ToString()) ? string.Empty : row["UpdatedBy"].ToString().ToLower(),
                            };
                            if (entity.CreatedDate != null)
                            {
                                entity.CreatedDateStr = entity.CreatedDate.ToString("yyyy/MM/dd HH:mm:ss");
                            }
                            if (entity.UpdatedDate != null)
                            {
                                entity.UpdatedDateStr = entity.UpdatedDate.ToString("yyyy/MM/dd HH:mm:ss");
                            }
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
            return entity;
        }

        public async Task<Account> SelectAsync(string  name)
        {
            var entity = new Account();
            const string strQuery = @"SELECT * FROM [dbo].[Accounts] WHERE AccountName = @AccountName";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand command = new SqlCommand(strQuery, conn);
                command.Parameters.AddWithValue("@AccountName", name);
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
                            entity = new Account
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
                                UpdatedBy = string.IsNullOrEmpty(row["UpdatedBy"].ToString()) ? string.Empty : row["UpdatedBy"].ToString().ToLower(),
                            };
                            if (entity.CreatedDate != null)
                            {
                                entity.CreatedDateStr = entity.CreatedDate.ToString("yyyy/MM/dd HH:mm:ss");
                            }
                            if (entity.UpdatedDate != null)
                            {
                                entity.UpdatedDateStr = entity.UpdatedDate.ToString("yyyy/MM/dd HH:mm:ss");
                            }
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
            return entity;
        }

        public async Task<string> InsertAsync(Account entity)
        {
            var message = "ok";
            const string strQuery = @"IF ((SELECT COUNT(1) FROM [dbo].[Accounts] WHERE [AccountName] = @AccountName) > 0)
                                        begin
	                                       PRINT 'Account name duplicate error.' 
                                        end
                                      ELSE   
                                        begin
                                            INSERT INTO [dbo].[GeneratedID](Status) OUTPUT INSERTED.Generated_ID VALUES ('Used')
                                            INSERT INTO [dbo].[Accounts] (Account_ID
                                                                       ,[AccountName]
                                                                       ,[AccountType]
                                                                       ,[AccountNumber]
                                                                       ,[SavingsBalance]
                                                                       ,[CheckingBalance]
                                                                       ,[CreditBalance]
                                                                       ,[IsActive]
                                                                       ,[CreatedDate]
                                                                       ,[CreatedBy]
                                                                       ,[UpdatedDate]
                                                                       ,[UpdatedBy])
                                                                 VALUES
                                                                       ((SELECT SCOPE_IDENTITY())
                                                                       ,@AccountName
                                                                       ,@AccountType
                                                                       ,(SELECT SCOPE_IDENTITY())
                                                                       ,@SavingsBalance
                                                                       ,@CheckingBalance
                                                                       ,@CreditBalance
                                                                       ,@IsActive
                                                                       ,GetUtcDate()
                                                                       ,@CreatedBy
                                                                       ,GetUtcDate()
                                                                       ,@UpdatedBy)
                                         end";
            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand(strQuery, conn);
            try
            {
                //command.Parameters.AddWithValue("@XXXXXXXX", entity.XXXXXXXXX);

                command.Parameters.AddWithValue("@Account_ID", entity.Account_ID);
                command.Parameters.AddWithValue("@AccountName", entity.AccountName);
                command.Parameters.AddWithValue("@AccountType", entity.AccountType);
                //command.Parameters.AddWithValue("@AccountNumber", entity.AccountNumber);
                command.Parameters.AddWithValue("@SavingsBalance", entity.SavingsBalance);
                command.Parameters.AddWithValue("@CheckingBalance", entity.CheckingBalance);
                command.Parameters.AddWithValue("@CreditBalance", entity.CreditBalance);
                command.Parameters.AddWithValue("@IsActive", true);
                command.Parameters.AddWithValue("@CreatedBy", entity.CreatedBy);
                command.Parameters.AddWithValue("@UpdatedBy", entity.UpdatedBy);

                await conn.OpenAsync();
                conn.InfoMessage += (object obj, SqlInfoMessageEventArgs e) =>
                {
                    message = e.Message;
                };
                var result = await command.ExecuteNonQueryAsync();
                await conn.CloseAsync();
                await command.DisposeAsync();
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
                //throw ex;
            }
            return message;
        }

        public async Task<string> UpdateAsync(Account entity)
        {
            var message = "ok";
            const string strQuery = @"UPDATE [dbo].[Accounts] SET  [AccountType] = @AccountType
                                                                  ,[SavingsBalance] = @SavingsBalance
                                                                  ,[CheckingBalance] = @CheckingBalance
                                                                  ,[CreditBalance] = @CreditBalance
                                                                  ,[IsActive] = @IsActive
                                                                  ,[UpdatedDate] = GetUtcDate()
                                                                  ,[UpdatedBy] = @UpdatedBy
                                                             WHERE AccountNumber = @AccountNumber ";
            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand(strQuery, conn);
            try
            {
                //command.Parameters.AddWithValue("@XXXXXXXX", entity.XXXXXXXXX);
                command.Parameters.AddWithValue("@AccountNumber", entity.AccountNumber.Trim());
                command.Parameters.AddWithValue("@AccountType", entity.AccountType);
                command.Parameters.AddWithValue("@SavingsBalance", entity.SavingsBalance);
                command.Parameters.AddWithValue("@CheckingBalance", entity.CheckingBalance);
                command.Parameters.AddWithValue("@CreditBalance", entity.CreditBalance);
                command.Parameters.AddWithValue("@IsActive", 1);
                command.Parameters.AddWithValue("@UpdatedBy", "Admin");

              await  conn.OpenAsync();
                conn.InfoMessage += (object obj, SqlInfoMessageEventArgs e) =>
                {
                    message = e.Message;
                };
                var result = await command.ExecuteNonQueryAsync();
                await conn.CloseAsync();
                await command.DisposeAsync();
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
                //throw ex;
            }
            return message;
        }

        public async Task<string> DeleteByAccountNumberAsync(string number)
        {
            var message = "ok";
            const string strQuery = @"DELETE [dbo].[Accounts] WHERE AccountNumber = @AccountNumber";
            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand(strQuery, conn);
            try
            {
                command.Parameters.AddWithValue("@AccountNumber", number.Trim());
                await conn.OpenAsync();
                conn.InfoMessage += (object obj, SqlInfoMessageEventArgs e) =>
                {
                    message = e.Message;
                };
                var result = await command.ExecuteNonQueryAsync();
                await conn.CloseAsync();
                await command.DisposeAsync();
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
                //throw ex;
            }
            return message;
        }

        public async Task<string> DeleteByAccountNameAsync(string name)
        {
            var message = "ok";
            const string strQuery = @"DELETE [dbo].[Accounts] WHERE AccountName = @AccountName";
            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand(strQuery, conn);
            try
            {
                command.Parameters.AddWithValue("@AccountName", name.Trim());

                await conn.OpenAsync();
                conn.InfoMessage += (object obj, SqlInfoMessageEventArgs e) =>
                {
                    message = e.Message;
                };
                var result = await command.ExecuteNonQueryAsync();
                await conn.CloseAsync();
                await command.DisposeAsync();
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
                //throw ex;
            }
            return message;

        }
    }
}
