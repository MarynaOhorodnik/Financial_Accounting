using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.ComponentModel;

namespace Financial_Accounting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currency = " грн.";
        public MainWindow()
        {
            InitializeComponent();
            UpdateMain();
        }

        public void UpdateMain()
        {
            ReloadTableIN();
            ReloadTableOUT();
            Reload_total();
        }


        private void Button_income_Click(object sender, RoutedEventArgs e)
        {
            Add_income add_income = new Add_income();
            add_income.Show();
            this.Hide();
        }

        private void Button_outcome_Click(object sender, RoutedEventArgs e)
        {
            Add_outcome add_outcome = new Add_outcome();
            add_outcome.Show();
            this.Hide();
        }

        private void Button_ctg_income_Click(object sender, RoutedEventArgs e)
        {
            Category_income category_income = new Category_income();
            category_income.Show();
            this.Hide();
        }

        private void Button_ctg_outcome_Click(object sender, RoutedEventArgs e)
        {
            Category_outcome category_outcome = new Category_outcome();
            category_outcome.Show();
            this.Hide();
        }


        

        private void buttonDel_In_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultMes = MessageBox.Show("Ви бажаєте видалити?", "Підтвердження", MessageBoxButton.YesNo);
            switch (resultMes)
            {
                case MessageBoxResult.Yes:

                    DB db = new DB();

                    int id = Convert.ToInt32(((Button)(sender)).Tag);

                    MySqlCommand command = new MySqlCommand("UPDATE `income` SET `is_delete` = '1' WHERE `income`.`id` = @id;", db.getConnection());
                    command.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;

                    db.openConnection();

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Успіх!");
                        UpdateMain();
                    }

                    else
                        MessageBox.Show("Щось пішло не так! Спробуйте ще раз.");

                    db.closeConnection();

                    break;

                case MessageBoxResult.No:
                    break;
            }
        }

        private void buttonDel_Out_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultMes = MessageBox.Show("Ви бажаєте видалити?", "Підтвердження", MessageBoxButton.YesNo);
            switch (resultMes)
            {
                case MessageBoxResult.Yes:

                    DB db = new DB();

                    int id = Convert.ToInt32(((Button)(sender)).Tag);

                    MySqlCommand command = new MySqlCommand("UPDATE `outcome` SET `is_delete` = '1' WHERE `outcome`.`id` = @id;", db.getConnection());
                    command.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;

                    db.openConnection();

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Успіх!");
                        UpdateMain();
                    }

                    else
                        MessageBox.Show("Щось пішло не так! Спробуйте ще раз.");

                    db.closeConnection();

                    break;

                case MessageBoxResult.No:
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateMain();
        }

        private void ReloadTableIN()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT INC.id, INC.total, INC.category_id, DATE_FORMAT(INC.date, '%d.%m.%Y') AS date , INC.comments, INC.is_delete, CTG.name FROM income AS INC, category_income AS CTG WHERE INC.category_id = CTG.id AND INC.is_delete = '0'", db.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            objectsListIncome.ItemsSource = table.DefaultView;

            if (table.Rows.Count == 0)
            {
                ResultTxtIncome.Text = "Немає результатів";
            }
            else
            {
                ResultTxtIncome.Text = default;
            }
        }

        private void ReloadTableOUT()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT INC.id, INC.total, INC.category_id, DATE_FORMAT(INC.date, '%d.%m.%Y') AS date , INC.comments, INC.is_delete, CTG.name FROM outcome AS INC, category_outcome AS CTG WHERE INC.category_id = CTG.id AND INC.is_delete = '0'", db.getConnection());


            adapter.SelectCommand = command;
            adapter.Fill(table);


            objectsListOutcome.ItemsSource = table.DefaultView;

            if (table.Rows.Count == 0)
            {
                ResultTxtOutcome.Text = "Немає результатів";
            }
            else
            {
                ResultTxtOutcome.Text = default;
            }
        }

        private void Reload_total()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command1 = new MySqlCommand("SELECT is_delete, SUM(total) AS sum FROM income WHERE is_delete = '0' GROUP BY is_delete", db.getConnection());

            adapter.SelectCommand = command1;
            adapter.Fill(table);
            double total_in = Convert.ToDouble(table.Rows[0][1]);

            MySqlCommand command2 = new MySqlCommand("SELECT is_delete, SUM(total) AS sum FROM outcome WHERE is_delete = '0' GROUP BY is_delete", db.getConnection());

            adapter.SelectCommand = command2;
            adapter.Fill(table);
            double total_out = Convert.ToDouble(table.Rows[1][1]);


            income_tot.Text = total_in.ToString() + currency;
            outcome_tot.Text = total_out.ToString() + currency;
            balance_tot.Text = (total_in - total_out).ToString() + currency;

            Value_Total.Income_total = total_in;
            Value_Total.Outcome_total = total_out;

            ContControl.DataContext = new PieChart();
        }

        void Window_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
