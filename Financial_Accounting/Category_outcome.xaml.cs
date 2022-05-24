﻿using MySql.Data.MySqlClient;
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
    /// Interaction logic for Category_outcome.xaml
    /// </summary>
    public partial class Category_outcome : Window
    {
        public Category_outcome()
        {
            InitializeComponent();

            ReloadTable();
        }

        private void ReloadTable()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `category_outcome` WHERE `is_delete` = '0'", db.getConnection());

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

        private void Button_save_Click(object sender, RoutedEventArgs e)
        {
            if (isCategoryExists())
                return;

            DB db = new DB();

            MySqlCommand command = new MySqlCommand("INSERT INTO `category_outcome` (`name`, `comments`, `is_delete`) VALUES (@name, @comments, '0');", db.getConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = Name.Text;
            command.Parameters.Add("@comments", MySqlDbType.VarChar).Value = Comment.Text;

            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Успіх!");
                ReloadTable();
                Name.Text = "";
                Comment.Text = "";
            }
            else
                MessageBox.Show("Щось пішло не так!");

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
                MessageBox.Show("Така категорія вже існує. Введіть іншу.");
                return true;
            }
            else
                return false;
        }

        private void buttonDel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultMes = MessageBox.Show("Ви бажаєте видалити дану категорію?", "Підтвердження", MessageBoxButton.YesNo);
            switch (resultMes)
            {
                case MessageBoxResult.Yes:

                    DB db = new DB();

                    int id = Convert.ToInt32(((Button)(sender)).Tag);

                    MySqlCommand command = new MySqlCommand("UPDATE `category_outcome` SET `is_delete` = '1' WHERE `category_outcome`.`id` = @id;", db.getConnection());
                    command.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;

                    db.openConnection();

                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Успіх!");
                        ReloadTable();
                    }
                    else
                        MessageBox.Show("Щось пішло не так! Спробуйте ще раз.");

                    db.closeConnection();
                    break;

                case MessageBoxResult.No:
                    break;
            }
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
