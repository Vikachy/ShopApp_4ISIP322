using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Data.Entity;

namespace ShopApp_4ISIP322
{
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow()
        {
            InitializeComponent();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            // Получаем единственный экземпляр контекста
            var context = Entities2.GetContext();

            // Загружаем продукты вместе с категориями
            var products = context.Products.Include(p => p.Category1).ToList();

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
                    Category = g.Key,
                    AveragePrice = g.Average(p => p.Price)
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
