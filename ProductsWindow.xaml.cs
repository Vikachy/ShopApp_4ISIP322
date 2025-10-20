using System;
using System.Collections.Generic;
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

        private Entities2 context; 

        public ProductsWindow()
        {
            InitializeComponent();
            LoadData();
            context = new Entities2();
        }

        private void LoadData()
        {
            var products = context.Products.ToList(); // Загрузка всех продуктов
            var categories = context.Categories.ToList(); // Загрузка всех категорий

            dgProducts.ItemsSource = products; // Установка источника данных для DataGrid
            cmbCategory.ItemsSource = categories; // Установка источника данных для ComboBox

            cmbCategory.DisplayMemberPath = "CategoryName"; // Отображение имени категории
            cmbCategory.SelectedValuePath = "CategoryID"; // Установка значения, которое будет возвращаться
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
                LoadData(); // Обновление данных после добавления
            }
            else
            {
                MessageBox.Show("Пожалуйста, проверьте введенные данные.");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product selectedProduct)
            {
                if (decimal.TryParse(txtPrice.Text, out decimal price) &&
                    int.TryParse(txtQuantity.Text, out int quantity) &&
                    cmbCategory.SelectedItem is Category selectedCategory)
                {
                    selectedProduct.Name = txtProductName.Text;
                    selectedProduct.Category = selectedCategory.CategoryID;
                    selectedProduct.Price = price;
                    selectedProduct.Quantity = quantity;

                    context.SaveChanges();
                    LoadData(); // Обновление данных после обновления
                }
                else
                {
                    MessageBox.Show("Пожалуйста, проверьте введенные данные.");
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product selectedProduct)
            {
                context.Products.Remove(selectedProduct);
                context.SaveChanges();
                LoadData(); // Обновление данных после удаления
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
            cmbCategory.SelectedIndex = -1;
            txtPrice.Clear();
            txtQuantity.Clear();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}