using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace ShopApp_4ISIP322
{
    /// <summary>
    /// Логика взаимодействия для ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        private Entities2 context = new Entities2();

        public ProductsWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                // Асинхронно загружаем данные из базы данных
                dgProducts.ItemsSource = await context.Products.ToListAsync();
                cmbCategory.ItemsSource = await context.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(txtPrice.Text, out decimal price) &&
                int.TryParse(txtQuantity.Text, out int quantity) &&
                cmbCategory.SelectedItem is Category selectedCategory)
            {
                var product = new Product
                {
                    Name = txtProductName.Text,
                    Category = selectedCategory.CategoryID,
                    Price = price,
                    Quantity = quantity
                };

                context.Products.Add(product);
                context.SaveChanges();
                ClearForm();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product selectedProduct)
            {
                selectedProduct.Name = txtProductName.Text;
                selectedProduct.Category = ((Category)cmbCategory.SelectedItem).CategoryID;
                selectedProduct.Price = decimal.Parse(txtPrice.Text);
                selectedProduct.Quantity = int.Parse(txtQuantity.Text);

                context.SaveChanges();
                ClearForm();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product selectedProduct)
            {
                context.Products.Remove(selectedProduct);
                context.SaveChanges();
                ClearForm();
            }
        }

        private void dgProducts_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product selectedProduct)
            {
                txtProductName.Text = selectedProduct.Name;
                cmbCategory.SelectedValue = selectedProduct.Category;
                txtPrice.Text = selectedProduct.Price.ToString();
                txtQuantity.Text = selectedProduct.Quantity.ToString();
            }
        }

        private void ClearForm()
        {
            txtProductName.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            cmbCategory.SelectedIndex = -1;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}