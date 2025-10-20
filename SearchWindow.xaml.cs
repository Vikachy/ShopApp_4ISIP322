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
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private Entities2 context = new Entities2();

        public SearchWindow()
        {
            InitializeComponent();
            LoadCategories();
            LoadAllProducts();
        }

        private void LoadCategories()
        {
            cmbSearchCategory.ItemsSource = context.Categories.ToList();
        }

        private void LoadAllProducts()
        {
            dgSearchResults.ItemsSource = context.Products.ToList();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var query = context.Products.AsQueryable();

            // Поиск по названию
            if (!string.IsNullOrWhiteSpace(txtSearchName.Text))
            {
                query = query.Where(p => p.Name.Contains(txtSearchName.Text));
            }

            // Поиск по категории
            if (cmbSearchCategory.SelectedItem is Category selectedCategory)
            {
                query = query.Where(p => p.Category == selectedCategory.CategoryID);
            }

            dgSearchResults.ItemsSource = query.ToList();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearchName.Clear();
            cmbSearchCategory.SelectedIndex = -1;
            LoadAllProducts();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

