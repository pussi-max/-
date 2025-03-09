using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using up.Entities;


namespace up
{
    public partial class ProductsAddWindow : Window
    {
        public PostgresContext PostgresContext { get; set; }
        public Product Product { get; set; }

        public ProductsAddWindow(PostgresContext context, Product product)
        {
            InitializeComponent();
            this.PostgresContext = context;
            this.Product = product;
            DataContext = Product; // Устанавливаем контекст данных
            FillKategory();
            FillColor();
            FillStorage();
        }
        public void FillKategory(){
            var kategories = PostgresContext.Kategories.ToList();
            var cmbKategory = this.FindControl<ComboBox>("cmbKategory");
            cmbKategory.ItemsSource=kategories;
        }
                public void FillColor(){
            var colors = PostgresContext.Colors.ToList();
            var cmbColor = this.FindControl<ComboBox>("cmbColor");
            cmbColor.ItemsSource=colors;
        }
                public void FillStorage(){
            var storages = PostgresContext.Storages.ToList();
            var cmbStorage = this.FindControl<ComboBox>("cmbStorage");
            cmbStorage.ItemsSource=storages;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var txtName = this.FindControl<TextBox>("txtName");
            var cmbKategory = this.FindControl<ComboBox>("cmbKategory");
            var cmbColor = this.FindControl<ComboBox>("cmbColor");
            var cmbStorage = this.FindControl<ComboBox>("cmbStorage");
            var txtGarantiya = this.FindControl<TextBox>("txtGarantiya");
            var txtPrice = this.FindControl<TextBox>("txtPrice");

            var kategory = cmbKategory.SelectedItem as Kategory;
            var color = cmbColor.SelectedItem as Color;
            var storage = cmbStorage.SelectedItem as Storage;

            // Проверяем, что цена может быть распознана как int
            if (!int.TryParse(txtPrice.Text, out var price))
            {
                // Обработка ошибки, если цена неверна
                return;
            }

            try
            {
                this.Product.Name=txtName.Text;
                this.Product.IdKategory = kategory.Id;
                this.Product.IdColor = color.Id;
                this.Product.IdStorage = storage.Id;
                this.Product.Garantiya = txtGarantiya.Text;
                this.Product.Price = price;
                PostgresContext.SaveChanges();
            }

            catch
            {
                Console.Write("Error");
            }
        }
         private void btnBack_Click(object sender, RoutedEventArgs e){
            this.Close();
         }
    }
}
