using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace Bank_13_
{
    /// <summary>
    /// Набор функций взаимодействия с данными БД
    /// </summary>
    public class Bank
    {
        Random r = new();
        public SqlConnection con;
        public SqlDataAdapter da, dal;
        public DataTable dt, dtl;
        public DataRowView row;
        public Bank()
        {
            SqlConnectionStringBuilder connect = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "MSSQLLocalDemo",
                Pooling = true
            };
            con = new SqlConnection(connect.ConnectionString);
            dt = new DataTable();
            da = new SqlDataAdapter();
            dtl = new DataTable();
            dal = new SqlDataAdapter();
            #region commands

            var sql = @"SELECT [Id], [Type], [FullName], CAST([MainAccount] AS int) as MainAccount, [Address], CAST([BankAccount] AS int) as BankAccount, [Reliability], [Credit], [PhoneNumber], [Current] FROM Clients Order By Clients.Id";
            
            da.SelectCommand = new SqlCommand(sql, con);


            sql = @"INSERT INTO Clients (Type, FullName,  MainAccount,  [Address], BankAccount, Reliability, Credit, PhoneNumber, [Current]) 
                                 VALUES (@Type, @FullName, @MainAccount, @Address, @BankAccount, @Reliability, 0, @PhoneNumber, 0); 
                     SET @Id = @@IDENTITY;";

            da.InsertCommand = new SqlCommand(sql, con);

            da.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 7, "Id").Direction = ParameterDirection.Output;
            da.InsertCommand.Parameters.Add("@Type", SqlDbType.NChar, 7, "Type");
            da.InsertCommand.Parameters.Add("@FullName", SqlDbType.NVarChar, 80, "FullName");
            da.InsertCommand.Parameters.Add("@MainAccount", SqlDbType.Money, 15, "MainAccount");
            da.InsertCommand.Parameters.Add("@Address", SqlDbType.NVarChar, 100, "Address");
            da.InsertCommand.Parameters.Add("@BankAccount", SqlDbType.Money, 15, "BankAccount");
            da.InsertCommand.Parameters.Add("@Reliability", SqlDbType.Bit, 1, "Reliability");
            da.InsertCommand.Parameters.Add("@PhoneNumber", SqlDbType.NChar, 14, "PhoneNumber");

            sql = @"UPDATE Clients SET 
                           FullName = @FullName,
                           MainAccount = @MainAccount, 
                           [Address] = @Address,
                           BankAccount = @BankAccount,
                           Reliability = @Reliability,
                           Credit = @Credit,
                           PhoneNumber = @PhoneNumber,
                           [Current] = @Current
                    WHERE Id = @Id";

            da.UpdateCommand = new SqlCommand(sql, con);
            da.UpdateCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id").SourceVersion = DataRowVersion.Original;
            da.UpdateCommand.Parameters.Add("@FullName", SqlDbType.NVarChar, 80, "FullName");
            da.UpdateCommand.Parameters.Add("@MainAccount", SqlDbType.Money, 15, "MainAccount");
            da.UpdateCommand.Parameters.Add("@Address", SqlDbType.NVarChar, 100, "Address");
            da.UpdateCommand.Parameters.Add("@BankAccount", SqlDbType.Money, 15, "BankAccount");
            da.UpdateCommand.Parameters.Add("@Credit", SqlDbType.Money, 15, "Credit");
            da.UpdateCommand.Parameters.Add("@Reliability", SqlDbType.Bit, 1, "Reliability");
            da.UpdateCommand.Parameters.Add("@PhoneNumber", SqlDbType.NChar, 14, "PhoneNumber");
            da.UpdateCommand.Parameters.Add("@Current", SqlDbType.TinyInt, 2, "Current");

            sql = "DELETE FROM Clients WHERE Id = @Id";

            da.DeleteCommand = new SqlCommand(sql, con);
            da.DeleteCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id");

            #endregion
            #region commands2

            sql = @"SELECT [Id], [Date], CAST([MoneyAmount] AS int) AS MoneyAmount, [SenderID], [RecipientID], [Successful] FROM Logs Order By Logs.Id";
            dal.SelectCommand = new SqlCommand(sql, con);

            sql = @"INSERT INTO Logs ([Date], MoneyAmount,  SenderID,  RecipientID, Successful) 
                                 VALUES (@Date, @MoneyAmount, @SenderID, @RecipientID, @Successful); 
                     SET @Id = @@IDENTITY;";

            dal.InsertCommand = new SqlCommand(sql, con);
            dal.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 7, "Id").Direction = ParameterDirection.Output;
            dal.InsertCommand.Parameters.Add("@Date", SqlDbType.DateTime2, 20, "Date");
            dal.InsertCommand.Parameters.Add("@MoneyAmount", SqlDbType.Money, 15, "MoneyAmount");
            dal.InsertCommand.Parameters.Add("@SenderID", SqlDbType.Int, 7, "SenderID");
            dal.InsertCommand.Parameters.Add("@RecipientID", SqlDbType.Int, 7, "RecipientID");
            dal.InsertCommand.Parameters.Add("@Successful", SqlDbType.Bit, 1, "Successful");

            sql = "DELETE FROM Logs WHERE Id = @Id";

            dal.DeleteCommand = new SqlCommand(sql, con);
            dal.DeleteCommand.Parameters.Add("@Id", SqlDbType.Int, 4, "Id");

            #endregion
            da.Fill(dt);
            dal.Fill(dtl);
        }
        public void CreateBank(MainWindow w)
        {
            con.Open();
            new SqlCommand("truncate table Clients", con).ExecuteNonQuery();
            dt.Clear();
            int n = 6_000;
            //Task[] tasks = new Task[n];
            for (int i = 0; i < n; i++)
            {
                //tasks[i] = Task.Factory.StartNew(CBNW2);
                CBNW();
            }
            //Task.WaitAll(tasks);

            w.Dispatcher.Invoke(() =>
            {
                dt.Clear();
                da.Fill(dt);
            });
            con.Close();
        }
        private void CBNW()
        {
            string sql;
            int rand = new Random().Next(500, 800);
            switch (new Random().Next(3))
            {
                case 0://VIP
                    sql = @$"INSERT INTO Clients (Type, FullName,  MainAccount,  [Address], BankAccount, Reliability, Credit, PhoneNumber, [Current]) 
                                 VALUES ({"'VIP'"}, N{"'Василий vip'"}, {rand}, N{"'Г. Москва, ул. Советская 63'"},  {rand}, 1, 0, {"'+79995554444'"}, 0);";
                    break;
                case 1:// CLIENT
                    sql = @$"INSERT INTO Clients (Type, FullName,  MainAccount,  [Address], BankAccount, Reliability, Credit, PhoneNumber, [Current]) 
                                 VALUES ({"'Client'"}, N{"'Павел Александров'"}, {rand}, N{"'Г. Москва, ул. Пушкина 22'"},  {rand}, 0, 0, {"'+79992456585'"}, 0);";
                    break;
                default://Entitie
                    sql = @$"INSERT INTO Clients (Type, FullName,  MainAccount,  [Address], BankAccount, Reliability, Credit, PhoneNumber, [Current]) 
                                 VALUES ({"'Entitie'"}, N{"'Завод'"}, {rand}, N{"'Г. Москва, ул. Строителей 8'"},  {rand}, 1, 0, {"'+79384205543'"}, 0);";
                    break;
            }
            new SqlCommand(sql, con).ExecuteNonQuery();

        }
        /// <summary>
        /// Перевод денег между счетами 2х клиентов
        /// </summary>
        /// <param name="c1">Клиент 1 (sender)</param>
        /// <param name="c2">Клиент 2 (Recipient)</param>
        /// <param name="money">Сумма</param>
        public void Transfer(int c1, int c2, int money)
        {
            if (Convert.ToInt32(dt.Rows[c1-1].ItemArray[3]) >= money)
            {
                dt.Rows[c1 - 1][3] = Convert.ToInt32(dt.Rows[c1 - 1][3]) - money;
                dt.Rows[c2 - 1][3] = Convert.ToInt32(dt.Rows[c2 - 1][3]) + money;
                da.Update(dt);
                DataRow r = dtl.NewRow();
                r["Date"] = DateTime.Now;
                r["MoneyAmount"] = money;
                r["SenderID"] = c1;
                r["RecipientID"] = c2;
                r["Successful"] = 1;
                dtl.Rows.Add(r);
                dal.Update(dtl);
            }
            else
            {
                DataRow r = dtl.NewRow();
                r["Date"] = DateTime.Now.ToString("yy.MM.dd HH:mm:ss");
                r["MoneyAmount"] = money;
                r["SenderID"] = c1;
                r["RecipientID"] = c2;
                r["Successful"] = 0;
                dtl.Rows.Add(r);
                dal.Update(dtl);
            }

        }
        public void Imitation(object sender, EventArgs e)
        {
            //if (ClientBase.Count > 0)
            //{
            //    int c1 = r.Next(0, ClientBase.Count - 1);
            //    Thread.Sleep(100);
            //    int c2 = r.Next(0, ClientBase.Count - 1);
            //    long tempM = r.Next(100, 1000);
            //    long t = ClientBase[c1].Transfer(tempM);
            //    if (t > 0)
            //    {
            //        ClientBase[c2].AddMoney(t);
            //        OperationList.Add(new Log(ClientBase[c1].Id, ClientBase[c2].Id, t, true));
            //    }
            //    else OperationList.Add(new Log(ClientBase[c1].Id, ClientBase[c2].Id, tempM, false));
            //}
            //Date = Date.AddMonths(1);
            //Update(Date);
        }
    }
}
