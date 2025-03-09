using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Linq;
using Avalonia.Input;
using up.Entities;
using Microsoft.EntityFrameworkCore;

namespace up;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var login = this.FindControl<TextBox>("LoginTextBox").Text;
            var password = this.FindControl<TextBox>("PasswordTextBox").Text;

            if (login == "user1" && password == "user1")
            {
                var nextProduct = new ProductsWindow();
                nextProduct.Show();
                this.Close();
            }
            else
            {
                Console.WriteLine("Неверный логин или пароль.");
                return;
            }
        }
 }
   