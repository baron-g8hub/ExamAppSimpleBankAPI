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
    public class TransactionsDataSource
    {
        private static string? connectionString;

        public TransactionsDataSource(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Transaction>> SelectAsync()
        {
            var list = new List<Transaction>();
            const string strQuery = @"SELECT * FROM [dbo].[Posted_Transactions]  Order By PostingDate Desc";

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
                            var entity = new Transaction
                            {
                                Transaction_ID = string.IsNullOrEmpty(row["Transaction_ID"].ToString()) ? 0 : int.Parse(row["Transaction_ID"].ToString()),
                                PostingDate = (Convert.IsDBNull(row["PostingDate"].ToString())) ? DateTime.Now : (row["PostingDate"].ToString() == "") ? DateTime.Now : Convert.ToDateTime(row["PostingDate"].ToString()),
                                TransactionType_ID = string.IsNullOrEmpty(row["TransactionType_ID"].ToString()) ? 0 : int.Parse(row["TransactionType_ID"].ToString()),
                                AccountNumber = string.IsNullOrEmpty(row["AccountNumber"].ToString()) ? string.Empty : row["AccountNumber"].ToString(),
                                AccountType = string.IsNullOrEmpty(row["AccountType"].ToString()) ? 0 : int.Parse(row["AccountType"].ToString()),
                                Amount = string.IsNullOrEmpty(row["Amount"].ToString()) ? 0.00 : double.Parse(row["Amount"].ToString()),
                                RunningBalance = string.IsNullOrEmpty(row["RunningBalance"].ToString()) ? 0.00 : double.Parse(row["RunningBalance"].ToString()),
                                Description = string.IsNullOrEmpty(row["Description"].ToString()) ? string.Empty : row["Description"].ToString(),
                                DestinationAccount = string.IsNullOrEmpty(row["DestinationAccount"].ToString()) ? string.Empty : row["DestinationAccount"].ToString(),
                            };
                            if (entity.PostingDate != null)
                            {
                                entity.PostingDateStr = entity.PostingDate.ToString("yyyy-MM-dd | HH:mm:ss");
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

        public async Task<Transaction> SelectAsync(int id)
        {
            var entity = new Transaction();
            const string strQuery = @"SELECT * FROM [dbo].[Posted_Transactions]  Order By PostingDate Desc";

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
                            entity = new Transaction
                            {
                                Transaction_ID = string.IsNullOrEmpty(row["Transaction_ID"].ToString()) ? 0 : int.Parse(row["Transaction_ID"].ToString()),
                                PostingDate = (Convert.IsDBNull(row["PostingDate"].ToString())) ? DateTime.Now : (row["PostingDate"].ToString() == "") ? DateTime.Now : Convert.ToDateTime(row["PostingDate"].ToString()),
                                TransactionType_ID = string.IsNullOrEmpty(row["TransactionType_ID"].ToString()) ? 0 : int.Parse(row["TransactionType_ID"].ToString()),
                                AccountNumber = string.IsNullOrEmpty(row["AccountNumber"].ToString()) ? string.Empty : row["AccountNumber"].ToString(),
                                AccountType = string.IsNullOrEmpty(row["AccountType"].ToString()) ? 0 : int.Parse(row["AccountType"].ToString()),
                                Amount = string.IsNullOrEmpty(row["Amount"].ToString()) ? 0.00 : double.Parse(row["Amount"].ToString()),
                                RunningBalance = string.IsNullOrEmpty(row["RunningBalance"].ToString()) ? 0.00 : double.Parse(row["RunningBalance"].ToString()),
                                Description = string.IsNullOrEmpty(row["Description"].ToString()) ? string.Empty : row["Description"].ToString(),
                                DestinationAccount = string.IsNullOrEmpty(row["DestinationAccount"].ToString()) ? string.Empty : row["DestinationAccount"].ToString(),
                            };
                            if (entity.PostingDate != null)
                            {
                                entity.PostingDateStr = entity.PostingDate.ToString("yyyy-MM-dd | HH:mm:ss");
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

        public async Task<string> InsertTransactionAsync(Transaction entity)
        {
            var message = "ok";
            const string strQuery = @" DECLARE @currentbalance money
                                     SET @currentbalance = (SELECT SUM(SavingsBalance) FROM [dbo].[Accounts] WHERE AccountNumber = @AccountNumber)

                                     IF ((SELECT COUNT(1) FROM [dbo].[Accounts] WHERE AccountNumber = @AccountNumber) < 1)
	                                    begin
		                                    PRINT 'Invalid source account.'
	                                    end
                                    ELSE IF ((SELECT COUNT(1) FROM [dbo].[Accounts] WHERE AccountNumber = @DestinationAccount) < 1)
	                                    begin
		                                    PRINT 'Invalid destination account.'
	                                    end
                                    ELSE IF (@Amount > @currentbalance)
	                                    begin
		                                    PRINT 'Insufficient funds.'
	                                    end
                                    ELSE
	                                    begin                                             
                                            IF (@currentbalance > @Amount OR @currentbalance = @Amount)
	                                            begin
                                                    DECLARE @updated int
                                                    UPDATE [dbo].[Accounts]
                                                    set [SavingsBalance] = [SavingsBalance] + @Amount
                                                    WHERE AccountNumber = @DestinationAccount
                                                    SET @updated = @@ROWCOUNT
                                                    IF (@updated = 1)
                                                        begin
														    UPDATE [dbo].[Accounts]
															set [SavingsBalance] = [SavingsBalance] - @Amount
															WHERE AccountNumber = @AccountNumber

                                                          INSERT INTO [dbo].[Posted_Transactions]  ([PostingDate], TransactionType_ID
                                                                                                   ,[AccountNumber]
                                                                                                   ,[Description]
                                                                                                   ,[AccountType]
                                                                                                   ,[Amount]
                                                                                                   ,[DestinationAccount]
                                                                                                   ,[RunningBalance])
                                                                                             VALUES
                                                                                                   (GetUtcDate(), @TransactionType_ID
                                                                                                   ,@AccountNumber
                                                                                                   ,@Description
                                                                                                   ,@AccountType
                                                                                                   ,@Amount
                                                                                                   ,@DestinationAccount
                                                                                                   ,(SELECT SUM(SavingsBalance) FROM [dbo].[Accounts] WHERE AccountNumber = @AccountNumber))
															SELECT 1 as Counted, 'Balance Transfered' as Remarks
                                                        end
                                                    ELSE
                                                        PRINT 'Funds not transfered.' 
	                                            end
                                        end";

            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand command = new SqlCommand(strQuery, conn);
            try
            {
                //command.Parameters.AddWithValue("@XXXXXXXX", entity.XXXXXXXXX);
                command.Parameters.AddWithValue("@TransactionType_ID", entity.TransactionType_ID);
                command.Parameters.AddWithValue("@AccountNumber", entity.AccountNumber);
                command.Parameters.AddWithValue("@Description", entity.Description);
                command.Parameters.AddWithValue("@AccountType", entity.AccountType);
                command.Parameters.AddWithValue("@Amount", entity.Amount);
                command.Parameters.AddWithValue("@DestinationAccount", entity.DestinationAccount);

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
                await conn.CloseAsync();
                await command.DisposeAsync();
                message = ex.Message.ToString();
                //throw ex;
            }
            return message;
        }
    }
}
