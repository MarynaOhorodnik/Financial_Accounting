using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Financial_Accounting
{
    /// <summary>
    /// Interaction logic for Add_income.xaml
    /// </summary>
    public partial class Add_income : Window
    {
        public Add_income()
        {
            InitializeComponent();

            Fill_Category();
            
        }

        private void Fill_Category()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `category_income` WHERE `is_delete` = '0'", db.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            CB_income_category.ItemsSource = table.DefaultView;
            CB_income_category.Text = default;
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            Total.Background = default;
            CB_income_category.Background = default;
            DatePick.Background = default;
            bool flag = false;

            if (!Total.Text.All(c => char.IsDigit(c)) || Total.Text == "")
            {
                Total.ToolTip = "Лише цифри";
                Total.Background = Brushes.MistyRose;
                flag = true;
            }
            if (CB_income_category.Text == default)
            {
                CB_income_category.Background = Brushes.MistyRose;
                flag = true;
            }
            if (DatePick.Text == "")
            {
                DatePick.Background = Brushes.MistyRose;
                flag = true;
            }

            if (flag)
                return;


            DataRowView oDataRowView = CB_income_category.SelectedItem as DataRowView;
            string str = "";

            if (oDataRowView != null)
            {
                str = oDataRowView.Row["name"] as string;
            }

            DB db = new DB();

            MySqlCommand command = new MySqlCommand("INSERT INTO `income` (`total`, `category_id`, `date`, `comments`, `is_delete`) VALUES (@total, @ctg, @date, @com, '0');", db.getConnection());
            command.Parameters.Add("@total", MySqlDbType.Double).Value = Total.Text;
            command.Parameters.Add("@ctg", MySqlDbType.Int32).Value = Find_Id(str);
            command.Parameters.Add("@date", MySqlDbType.Date).Value = DateFormat(DatePick.ToString());
            command.Parameters.Add("@com", MySqlDbType.VarChar).Value = Comment.Text;

            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Успіх!");
                Total.Text = "";
                CB_income_category.SelectedItem = default;
                DatePick.SelectedDate = default;
                Comment.Text = "";


            }
            else
                MessageBox.Show("Щось пішло не так!");

            db.closeConnection();
        }

        private int Find_Id(string str)
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `category_income` WHERE `name` = @name AND `is_delete` = '0'", db.getConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = str;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            int id = Convert.ToInt32(table.Rows[0][0]);

            return id;
        }

        private string DateFormat(string str)
        {
            string day = str.Substring(0, str.Length - 16);
            string month = str.Substring(3);
            month = month.Substring(0, str.Length - 16);
            string year = str.Substring(6);
            year = year.Substring(0, str.Length - 14);

            string result = year + "-" + month + "-" + day;
            return result;
        }

        void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();

        }
    }
}
