using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore; // Не забудьте добавить этот using для Include
using up.Entities;

namespace up
{
    public partial class ProductsWindow : Window
    {
        private PostgresContext PostgresContext; // Используем соглашение об именовании
        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }

        public ProductsWindow()
        {
            InitializeComponent();
            PostgresContext = new PostgresContext(); // Инициализация контекста базы данных
            ShowTable(); // Загрузка данных при запуске окна
        }

        // Метод для отображения данных в таблице
        public void ShowTable()
        {
            Products = PostgresContext.Products
                .Include(p => p.IdKategoryNavigation)
                .Include(p => p.IdColorNavigation) 
                .Include(p => p.IdStorageNavigation)// Включаем связанные заказы
                .ToList();
            dataGridProduct.ItemsSource = Products; // Устанавливаем источник данных для DataGrid
        }

        // Обработчик события для удаления записи
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = dataGridProduct.SelectedItem as Product;
            if (selectedProduct == null)
                return;
            try
            {
                PostgresContext.Products.Remove(selectedProduct);
                PostgresContext.SaveChanges();
                ShowTable(); // Обновляем таблицу после удаления
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении: {ex.Message}");
            }
        }

        // Обработчик события для добавления новой записи
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new Product();
            var addWindow = new ProductsAddWindow(PostgresContext, newProduct)
            {
                WindowStartupLocation = this.WindowStartupLocation
            };
            addWindow.Show();
        }

        // Обработчик события для фильтрации данных
        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            string filterText = txtFilter.Text.ToLower(); // Получаем текст фильтра
            var filteredProducts = PostgresContext.Products
                .Include(p => p.Orders) // Включаем связанные заказы
                .Where(p => p.Name.ToLower().Contains(filterText)) // Фильтруем по названию продукта
                .ToList();

            dataGridProduct.ItemsSource = filteredProducts; // Обновляем источник данных для DataGrid
        }
    }
}