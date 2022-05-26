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
    /// Interaction logic for EditTableOutcome.xaml
    /// </summary>
    public partial class EditTableOutcome : Window
    {
        public EditTableOutcome()
        {
            InitializeComponent();

            Fill_Category();
            Fill_Values();
        }

        private void Fill_Category()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `category_outcome` WHERE `is_delete` = '0'", db.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            CB_outcome_category.ItemsSource = table.DefaultView;
            CB_outcome_category.Text = default;
        }

        private void Fill_Values()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT INC.id, INC.total, INC.category_id, CTG.name, DATE_FORMAT(INC.date, '%d.%m.%Y') AS date , INC.comments FROM outcome AS INC, category_outcome AS CTG WHERE INC.category_id = CTG.id AND INC.is_delete = '0' AND INC.id = @id", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = Value_Total.Id_current;

            adapter.SelectCommand = command;
            adapter.Fill(table);


            Total.Text = table.Rows[0][1].ToString().Substring(1);
            CB_outcome_category.SelectedIndex = IndexComboBox(table.Rows[0][3].ToString());
            DatePick.SelectedDate = Convert.ToDateTime(table.Rows[0][4].ToString());
            Comment.Text = table.Rows[0][5].ToString();

        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            Total.Background = default;
            CB_outcome_category.Background = default;
            DatePick.Background = default;
            bool flag = false;

            if (!Total.Text.All(c => char.IsDigit(c)) || Total.Text == "")
            {
                Total.ToolTip = "Лише цифри";
                Total.Background = Brushes.MistyRose;
                flag = true;
            }
            if (CB_outcome_category.Text == default)
            {
                CB_outcome_category.Background = Brushes.MistyRose;
                flag = true;
            }
            if (DatePick.Text == "")
            {
                DatePick.Background = Brushes.MistyRose;
                flag = true;
            }

            if (flag)
                return;


            DataRowView oDataRowView = CB_outcome_category.SelectedItem as DataRowView;
            string str = "";
            double total = Convert.ToDouble(Total.Text);
            total = total - 2 * total;

            if (oDataRowView != null)
            {
                str = oDataRowView.Row["name"] as string;
            }

            DB db = new DB();

            MySqlCommand command = new MySqlCommand("UPDATE `outcome` SET `total` = @total, `category_id` = @ctg, `date` = @date, `comments` = @com WHERE `outcome`.`id` = @id;", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = Value_Total.Id_current;
            command.Parameters.Add("@total", MySqlDbType.Double).Value = total.ToString();
            command.Parameters.Add("@ctg", MySqlDbType.Int32).Value = Find_Id(str);
            command.Parameters.Add("@date", MySqlDbType.Date).Value = DateFormat(DatePick.ToString());
            command.Parameters.Add("@com", MySqlDbType.VarChar).Value = Comment.Text;

            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                //MessageBox.Show("Успіх!");
            }
            else
                MessageBox.Show("Щось пішло не так!");

            db.closeConnection();
        }

        private int IndexComboBox(string name)
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `category_outcome` WHERE `is_delete` = '0'", db.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][1].ToString() == name)
                {
                    return i;
                }
            }
            return -1;
        }

        private int Find_Id(string str)
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `category_outcome` WHERE `name` = @name AND `is_delete` = '0'", db.getConnection());
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
            MainWindow main = new MainWindow();
            main.Show();
            this.Hide();
        }
    }
}