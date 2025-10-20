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
    /// Логика взаимодействия для StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        private Entities2 context = new Entities2();

        public StatisticsWindow()
        {
            InitializeComponent();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            var products = context.Products.ToList();

            // Общая статистика
            int totalProducts = products.Sum(p => p.Quantity);
            decimal totalValue = products.Sum(p => p.Price * p.Quantity);

            txtTotalProducts.Text = $"Общее количество товаров: {totalProducts}";
            txtTotalValue.Text = $"Общая стоимость: {totalValue:C}";

            // Статистика по категориям
            var categoryStats = products
                .GroupBy(p => p.Category1.CategoryName)
                .Select(g => new
                {
                    Key = g.Key,
                    Value = g.Average(p => p.Price)
                })
                .ToList();

            dgCategoryStats.ItemsSource = categoryStats;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadStatistics();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
