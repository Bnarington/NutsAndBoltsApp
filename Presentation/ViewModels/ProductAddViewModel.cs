using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Prsenentation.Models;
using System.Windows;

namespace Presentation.ViewModels
{
    public partial class ProductAddViewModel : ObservableObject
    {
        
        private readonly IServiceProvider _serviceProvider;

        public ProductAddViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [RelayCommand]
        private void NavigateToProductList()
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
        }

        [ObservableProperty]
        private ProductModel _productForm = new();

        [RelayCommand]
        private void CreateProduct()
        {
            try
            {
                var productServices = _serviceProvider.GetRequiredService<ProductService>();

                var newProduct = productServices.CreateProduct(
                    ProductForm.ArticleNumber,
                    ProductForm.Company,
                    ProductForm.Description!,
                    ProductForm.Ingress!,
                    ProductForm.Price,
                    ProductForm.NutName!,
                    ProductForm.NutSize,
                    ProductForm.BoltName!,
                    ProductForm.BoltSize);

                if (newProduct != null)
                {
                    NavigateToProductList();
                }
                else
                {
                    MessageBox.Show("Product creation failed. Please check your input and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while creating a user. Please try again later." + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }
    }
}
   

