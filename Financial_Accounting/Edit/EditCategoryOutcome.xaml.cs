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
    /// Interaction logic for EditCategoryOutcome.xaml
    /// </summary>
    public partial class EditCategoryOutcome : Window
    {
        string name;
        public EditCategoryOutcome()
        {
            InitializeComponent();

            Fill_Values();
        }

        private void Fill_Values()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `category_outcome` WHERE `id` = @id AND `is_delete` = '0'", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = Value_Total.Id_current;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            name = table.Rows[0][1].ToString();
            Name.Text = table.Rows[0][1].ToString();
            Comment.Text = table.Rows[0][2].ToString();


        }


        void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Category_outcome income = new Category_outcome();
            income.Show();
            this.Hide();

        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            Name.Background = default;
            if (isCategoryExists())
                return;
            if (Name.Text == "")
            {
                Name.Background = Brushes.MistyRose;
                return;
            }

            DB db = new DB();

            MySqlCommand command = new MySqlCommand("UPDATE `category_outcome` SET `name` = @name, `comments` = @comn WHERE `category_outcome`.`id` = @id;", db.getConnection());
            command.Parameters.Add("@id", MySqlDbType.VarChar).Value = Value_Total.Id_current;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = Name.Text;
            command.Parameters.Add("@comn", MySqlDbType.VarChar).Value = Comment.Text;

            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Успіх!");
            }
            else
                MessageBox.Show("Щось пішло не так! Спробуйте ще раз.");

            db.closeConnection();
        }

        public Boolean isCategoryExists()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `category_outcome` WHERE `name` = @name AND `is_delete` = '0'", db.getConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = Name.Text;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                if (table.Rows[0][1].ToString() == name)
                    return false;
                else
                    return true;
            }
            else
                return false;
        }
    }
}
