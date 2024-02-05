using Infrastructure.DTOs;
using Infrastructure.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ProductService _productService;

        public MainWindow(ProductService productService)
        {
            InitializeComponent();
            _productService = productService;

            CreateDemoproduct();
        }

        private void CreateDemoproduct ()
        {
            var result = _productService.CreateProduct(new Product
            {
                ArticleNumber = "A1",
                Company = "Test AB",
                Description = "Description",
                Ingress = "Ingress",
                Price = 200,
                NutName = "Test Nut",
                NutSize = 2,
                BoltName = "Test Bolt",
                BoltSize = 2,
            });

            if (result)
                MessageBox.Show("Lyckades!");
            else
                MessageBox.Show("Misslyckades!");
        }
    }
}