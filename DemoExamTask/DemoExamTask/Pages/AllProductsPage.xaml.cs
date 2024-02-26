using DemoExamTask.ADO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoExamTask.Pages
{
    /// <summary>
    /// Interaction logic for AllProductsPage.xaml
    /// </summary>
    public partial class AllProductsPage : Page
    {
        private string FilterName;
        private string SearchFilter = "";
        private string SortField;
        private bool IsAsc = false;

        public AllProductsPage()
        {
            InitializeComponent();
            Loaded += PageLoaded;
        }

        private void FillFilterData()
        {
            var filters = new List<ProductType> { new ProductType { Name = "Все типы" } };
            filters.AddRange(App.Connection.ProductType.ToList());
            cbFilter.ItemsSource = filters;
            cbFilter.SelectedIndex = 0;

            var sorts = new List<string> {
                "Без сортировки",
                "Наименование", "Номер производственного цеха",
                "Минимальная стоимость для агента"};
            cbSort.ItemsSource = sorts;
            cbSort.SelectedIndex = 0;

            cbSortOrders.ItemsSource = new List<string> { "По убыванию", "По возрастанию" };
            cbSortOrders.SelectedIndex = 0;
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            FillFilterData();
            UpdateSource();
        }

        private void UpdateSource()
        {
            List<Product> products;

            if (string.IsNullOrEmpty(FilterName) || FilterName == "Все типы")
            {
                products = App.Connection.Product.ToList();
            }
            else
            {
                products = App.Connection.Product.ToList().Where(x =>
                            x.ProductType.Name == FilterName).ToList();
            }

            products = products.Where(x => x.Name.ToLower().Contains(SearchFilter.ToLower())).ToList();

            if (IsAsc)
            {
                if (SortField == "Наименование")
                {
                    products = products.OrderBy(x => x.Name).ToList();
                }
                else if (SortField == "Номер производственного цеха")
                {
                    products = products.OrderBy(x => x.FactoryNumber).ToList();
                }
                else if (SortField == "Минимальная стоимость для агента")
                {
                    products = products.OrderBy(x => x.MinimalAgentPrice).ToList();
                }
            }
            else
            {
                if (SortField == "Наименование")
                {
                    products = products.OrderByDescending(x => x.Name).ToList();
                }
                else if (SortField == "Номер производственного цеха")
                {
                    products = products.OrderByDescending(x => x.FactoryNumber).ToList();
                }
                else if (SortField == "Минимальная стоимость для агента")
                {
                    products = products.OrderByDescending(x => x.MinimalAgentPrice).ToList();
                }
            }

            lvProducts.ItemsSource = products;
        }

        private void cbSortOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var order = (string)((ComboBox)sender).SelectedItem;
            if (order == "По возрастанию")
            {
                IsAsc = true;
            }
            else
            {
                IsAsc = false;
            }
            UpdateSource();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortField = (string)((ComboBox)sender).SelectedItem;
            UpdateSource();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ProductType)((ComboBox)sender).SelectedItem;
            if (selectedItem == null)
                return;
            FilterName = selectedItem.Name;
            UpdateSource();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ((TextBox)sender).Text;
            SearchFilter = text;
            UpdateSource();
        }
    }
}
