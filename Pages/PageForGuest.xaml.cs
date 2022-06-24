using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace JewStore.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageForGuest.xaml
    /// </summary>
    public partial class PageForGuest : Page
    {
        string Manufacturer = "Все производители";
        public PageForGuest()
        {
            InitializeComponent();
            CreateLogo();
            CountAll.Text = CreateProductList("Все производители").ToString();
            ManufacturerList(Filtering);
            CountResult.Text = ProductList.Children.Count.ToString();
        }

        private void CreateLogo()
        {
            UserControls.LogoAndText LogoAndText = new UserControls.LogoAndText();
            LogoAndText.UserName.Text = "Гость";
            LogoAndText.HorizontalAlignment = HorizontalAlignment.Right;
            Logo.Children.Add(LogoAndText);
        }

        public int CreateProductList(string Manufacturer)
        {
                using (SqlConnection connection = new SqlConnection(Manager.ConnectionString))
                {
                    connection.Open();
                SqlCommand reading;
                if (Manufacturer == "Все производители")
                {
                    reading = new SqlCommand("SELECT * FROM Product", connection);
                }
                else reading = new SqlCommand($"SELECT * FROM Product WHERE ProductManufacturer = '{Manufacturer}'", connection);
                    SqlDataReader reader = reading.ExecuteReader();

                
                    while (reader.Read())
                    {
                        UserControls.ProductTemplate product = new UserControls.ProductTemplate();
                        if (reader.GetString(4) != "")
                        {
                            product.ProductImage.Source = new BitmapImage(new Uri(reader.GetString(4), UriKind.RelativeOrAbsolute));
                        }

                        product.ProductName.Text = reader.GetString(1);
                        product.Description.Text = reader.GetString(2);
                        product.Manufacturer.Text = reader.GetString(5);
                        product.Cost.Text = reader.GetDecimal(6).ToString();

                        if (reader.GetInt32(8) == 0)
                            product.Background = Brushes.Gray;
                        product.Stock.Text = $"{reader.GetInt32(8)} {reader.GetString(9)}";
                        ProductList.Children.Add(product);
                    }

                    connection.Close();
                }
                return ProductList.Children.Count;
        }

        public int CreateProductList(string Manufacturer, string Product)
        {
            using (SqlConnection connection = new SqlConnection(Manager.ConnectionString))
            {
                connection.Open();
                SqlCommand reading;
                if (Manufacturer == "Все производители")
                {
                    reading = new SqlCommand("SELECT * FROM Product", connection);
                }
                else reading = new SqlCommand($"SELECT * FROM Product WHERE ProductManufacturer = '{Manufacturer}' AND ProductName = '{Product}'", connection);
                SqlDataReader reader = reading.ExecuteReader();


                while (reader.Read())
                {
                    UserControls.ProductTemplate product = new UserControls.ProductTemplate();
                    if (reader.GetString(4) != "")
                    {
                        product.ProductImage.Source = new BitmapImage(new Uri(reader.GetString(4), UriKind.RelativeOrAbsolute));
                    }

                    product.ProductName.Text = reader.GetString(1);
                    product.Description.Text = reader.GetString(2);
                    product.Manufacturer.Text = reader.GetString(5);
                    product.Cost.Text = reader.GetDecimal(6).ToString();

                    if (reader.GetInt32(8) == 0)
                        product.Background = Brushes.Gray;
                    product.Stock.Text = $"{reader.GetInt32(8)} {reader.GetString(9)}";
                    ProductList.Children.Add(product);
                }

                connection.Close();
            }
            return ProductList.Children.Count;
        }

        private void ManufacturerList(ComboBox Filtering)
        {
            Filtering.Items.Clear();

            List<string> Manufacturers = new List<string>();
            Manufacturers.Add("Все производители");
            using (SqlConnection connection = new SqlConnection(Manager.ConnectionString))
            {
                connection.Open();
                SqlCommand reading = new SqlCommand("SELECT ProductManufacturer FROM Product", connection);
                SqlDataReader reader = reading.ExecuteReader();
                while (reader.Read())
                {
                    Manufacturers.Add(reader.GetString(0));
                }
                connection.Close();
            }
            Manufacturers = Manufacturers.Distinct().ToList();
            Filtering.ItemsSource = Manufacturers;
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this.IsLoaded)
            {
                return;
            }
            TextBox text = sender as TextBox;

            if (text.Text != "")
            {
                ProductList.Children.Clear();
                CreateProductList(Manufacturer, text.Text);
            }
            else CreateProductList(Manufacturer);
        }


        private void Filtering_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductList.Children.Clear();
            Manufacturer = Filtering.SelectedItem.ToString();
            CreateProductList(Filtering.SelectedItem.ToString());
            CountResult.Text = ProductList.Children.Count.ToString();
        }

        private void Sorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            Manager.SelectPage.Navigate(new LoginPage());
        }
    }
}
