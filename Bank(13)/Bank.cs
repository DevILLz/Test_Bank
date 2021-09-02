using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;

namespace Bank_13_
{
    public interface IBank
    {
        public void CreateBank(MainWindow w);
        public void DataBinding(MainWindow w);
        public void Imitation(object sender, EventArgs e);
        public void AddNewClient(MainWindow w);
        public void Transfer(int c1, int c2, int money);
        public void NewCredit(int i, int money);
        public void Repayment(int i);
        public void UpdateBankAccount(int index, int money, bool outsid);
        public void Withdrawal(int i, int money);
        public void Update();
        public void DeleteClient(object o);
    }
    /// <summary>
    /// Набор функций взаимодействия с данными БД
    /// </summary>
    public class Bank: IBank
    {


        Task t;
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

            try
            {
                con.Open();
                con.Close();
            }
            catch { throw new Exception(""); }

            t = new Task(() =>
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Update(i);
                }
                da.Update(dt);
            });
            dt = new DataTable();
            da = new SqlDataAdapter();
            dtl = new DataTable();
            dal = new SqlDataAdapter();
            #region Комманды для таблицы с клиентами

            var sql = @"SELECT [Id], [Type], [FullName], CAST([MainAccount] AS int) as MainAccount, [Address], CAST([BankAccount] AS int) as BankAccount, [Reliability], [Credit], [PhoneNumber], [Current] FROM Clients Order By Clients.Id";
            
            da.SelectCommand = new SqlCommand(sql, con);


            sql = @"INSERT INTO Clients (Type, FullName,  MainAccount,  [Address], BankAccount, Reliability, Credit, PhoneNumber, [Current]) 
                                 VALUES (@Type, @FullName, @MainAccount, @Address, @BankAccount, @Reliability, CAST(0.0000 AS Money), @PhoneNumber, CAST(0 AS TinyInt)); 
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
            #region Комманды для таблицы с логами

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
            dal.DeleteCommand.Parameters.Add("@Id", SqlDbType.Int, 7, "Id");
     
            #endregion
            da.Fill(dt);
            dal.Fill(dtl);
            
        }
        public void DataBinding(MainWindow w)
        {
            w.Dispatcher.Invoke(() =>
            {
                w.ClientsList.DataContext = dt.DefaultView;
                w.OperationList.DataContext = dtl.DefaultView;
            });
        }
        /// <summary>
        /// Создание новой ДБ
        /// </summary>
        /// <param name="w"></param>
        public void CreateBank(MainWindow w)
        {
            try
            {
                lock (dt)
                {
                    con.Open();
                    new SqlCommand("truncate table Clients; truncate table Logs;", con).ExecuteNonQuery();
                    dt.Clear();
                    int n = 6_000;
                    Task<string>[] tasks = new Task<string>[n / 500];
                    for (int d = 0; d < n / 500; d++)
                    {
                        tasks[d] = Task<string>.Factory.StartNew(() =>
                        {
                            string sql = default;
                            for (int i = 0; i < 500; i++)
                            {
                                sql += CBNW();
                            }
                            return sql;
                        });
                    }

                    w.Dispatcher.Invoke(() =>
                    {
                        w.PB.Visibility = Visibility.Visible;
                    });
                    Task.WaitAll(tasks);
                    for (int d = 0; d < n / 500; d++)
                    {

                        new SqlCommand(tasks[d].Result, con).ExecuteNonQuery();
                        w.Dispatcher.Invoke(() =>
                        {
                            w.PB.Value = Map(d, 0, n / 500, 0, 100);
                        });
                    }
                    w.Dispatcher.Invoke(() =>
                    {
                        w.LoadInfo.Visibility = Visibility.Hidden;
                        w.PB.Visibility = Visibility.Hidden;
                    });

                    w.Dispatcher.Invoke(() =>
                    {
                        dt.Clear();
                        da.Fill(dt);
                        dtl.Clear();
                        dal.Fill(dtl);
                    });
                    con.Close();
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        /// <summary>
        /// Рандомизатор добавления клиентов
        /// </summary>
        private string CBNW()
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
            return sql;
        }
        public void AddNewClient(MainWindow w)
        {
            DataRow r = dt.NewRow();
            NewClient window = new NewClient(r, w);
            window.ShowDialog();

            if (window.DialogResult.Value)
            {
                dt.Rows.Add(r);
                da.Update(dt);
            }
        }
        public static int Map(int value, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            int s = (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
            return s;
        }
        /// <summary>
        /// Перевод денег между счетами 2х клиентов
        /// </summary>
        /// <param name="c1">Клиент 1 (sender)</param>
        /// <param name="c2">Клиент 2 (Recipient)</param>
        /// <param name="money">Сумма</param>
        public void Transfer(int c1, int c2, int money)
        {
            byte cc = 0;
            bool flag = false;
            foreach (DataRow e in dt.Rows) if (Convert.ToInt32(e[0]) == c2) flag = true;
            if (flag && Convert.ToInt32(dt.Rows[c1].ItemArray[3]) >= money)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int c = Convert.ToInt32(dt.Rows[i][0]);
                    if (c == c1)
                    {
                        dt.Rows[i][3] = Convert.ToInt32(dt.Rows[i][3]) - money;
                        cc++;
                        if (cc == 2) break;
                    }
                    if (c == c2)
                    {
                        dt.Rows[i][3] = Convert.ToInt32(dt.Rows[i][3]) + money;
                        cc++;
                        if (cc == 2) break;
                    }
                }
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
        /// <summary>
        /// Новый кредит (последующие добавляются сверху)
        /// </summary>
        /// <param name="i">Элемент БД</param>
        /// <param name="money">Сумма</param>
        public void NewCredit(int i, int money)
        {
            float LR = Convert.ToString(dt.Rows[i][1]) switch
            {
                "VIP" => 9,
                "Entitie" => 5,
                _ => 15,
            };
            dt.Rows[i][3] = Convert.ToInt32(dt.Rows[i][3]) + money;
            if (!Convert.ToBoolean(dt.Rows[i][6])) dt.Rows[i][7] = Convert.ToInt32(dt.Rows[i][7]) + (money + (money * (LR / 100)));
            else dt.Rows[i][7] = Convert.ToInt32(dt.Rows[i][7]) + (money + (money * (LR / 125)));//для надёжных клиентов, ставка по кредиту ниже
            da.Update(dt);
        }
        /// <summary>
        /// Погашение кредита
        /// </summary>
        /// <param name="i">Элемент БД</param>
        public void Repayment(int i)
        {
            if (Convert.ToInt32(dt.Rows[i][7]) > 0 && Convert.ToInt32(dt.Rows[i][3]) >= Convert.ToInt32(dt.Rows[i][7]))
            {
                dt.Rows[i][3] = Convert.ToInt32(dt.Rows[i][3]) - Convert.ToInt32(dt.Rows[i][7]);
                dt.Rows[i][7] = 0;
                dt.Rows[i][9] = 0;
                dt.Rows[i][6] = true;
                da.Update(dt);
            }
        }
        /// <summary>
        /// Добавить/снять деньги на счет/со счета
        /// </summary>
        /// <param name="money">Сумма</param>
        public void UpdateBankAccount(int index, int money, bool outside)
        {
            if (outside)
            {
                if (Convert.ToInt32(dt.Rows[index][5]) >= money)
                {
                    dt.Rows[index][3] = Convert.ToInt32(dt.Rows[index][3]) + money;
                    dt.Rows[index][5] = Convert.ToInt32(dt.Rows[index][5]) - money;
                }
            }
            else
            {
                if (Convert.ToInt32(dt.Rows[index][3]) >= money)
                {
                    dt.Rows[index][5] = Convert.ToInt32(dt.Rows[index][5]) + money;
                    dt.Rows[index][3] = Convert.ToInt32(dt.Rows[index][3]) - money;
                }
            }
            da.Update(dt);
        }
        /// <summary>
        /// Условное снятие наличных
        /// </summary>
        /// <param name="i">Элемент БД</param>
        /// <param name="money">Сумма</param>
        public void Withdrawal(int i, int money)
        {
            if (Convert.ToInt32(dt.Rows[i][3]) >= money)
            {
                dt.Rows[i][3] = Convert.ToInt32(dt.Rows[i][3]) - money;
            }
        }
        /// <summary>
        /// Ежемесячная проверка счёта
        /// </summary>
        /// <param name="i">Индекс элемента БД</param>
        public void Update(int i)
        {
            lock (dt.Rows[i])
            {
                int Money = Convert.ToInt32(dt.Rows[i][3]);
                int BankAccount = Convert.ToInt32(dt.Rows[i][5]);
                bool Reliability = Convert.ToBoolean(dt.Rows[i][6]);
                int Credit = Convert.ToInt32(dt.Rows[i][7]);
                byte count = Convert.ToByte(dt.Rows[i][9]);

                float AIR = Convert.ToString(dt.Rows[i][1]) switch
                {
                    "VIP" => 13,
                    "Entitie" => 15,
                    _ => 9,
                };

                if (Reliability)
                {
                    dt.Rows[i][5] = BankAccount + (int)(BankAccount * (AIR / 100 / 12));
                    dt.Rows[i][3] = (int)(Money * 1.01);//каддый месяц 1% от остатка
                }//Т.К. в теории оно происходит каждый день, нет смысла делать дополнительные проверки

                if (Credit > 0)
                {
                    if (Money > Credit / 10)
                    {
                        dt.Rows[i][3] = Credit - (Credit / 10);//каждый месяц выплачивается 10% от остатка кредита
                        dt.Rows[i][7] = Credit - (Credit / 10);
                        if (count > 0) dt.Rows[i][9] = count - 1;
                        else dt.Rows[i][6] = true;//клиент становится надёжным, если не просрочил хотя бы один месяц
                    }
                    if (Credit < 100 && Money >= 100) { dt.Rows[i][3] = Money - Credit; dt.Rows[i][7] = 0; } //последние 100 Рублей снимаются сами, выходя из бесконечного цикла
                }
                if (count >= 5) dt.Rows[i][6] = false;//если просрочил кредит 5 месяцев к ряду, надёжность пропадает
                
            }
        }
        /// <summary>
        /// Имитация пересылки денег между клиентами. По одной операции в условный месяц
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Imitation(object sender, EventArgs e)
        {

            if (dt.Rows.Count > 0)
            {
                int c1 = r.Next(0, dt.Rows.Count - 1);
                int c2 = r.Next(0, dt.Rows.Count - 1);
                int tempM = r.Next(100, 1000);
                Transfer(c1, c2, tempM);
            }

            if (t.Status == TaskStatus.RanToCompletion)
                t.Start();

        }

        public void Update()
        {
            da.Update(dt);
        }
        public void DeleteClient(object r)
        {
            da.Update(dt);
        }
    }
}
