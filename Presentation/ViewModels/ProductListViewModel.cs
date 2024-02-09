using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Models;
using Prsenentation.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace Presentation.ViewModels
{
    public partial class ProductListViewModel : ObservableObject
    {
        private readonly ProductRepository _productRepository;
        private readonly ProductService _productService;
        private readonly IServiceProvider _serviceProvider;

        public ProductListViewModel(ProductService productService, IServiceProvider serviceProvider, ProductRepository productRepository)
        {
            _productService = productService;
            _serviceProvider = serviceProvider;
            _productRepository = productRepository;
            ShowProductList();
        }

        private ProductModel _selectedProduct;
        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                SetProperty(ref _selectedProduct, value);
            }
        }

        [RelayCommand]
        private void NavigateToAddProduct()
        {
            var mainViewModel = _serviceProvider.GetService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProductAddViewModel>();
        }

        [RelayCommand]
        private void NavigateToMainMenu()
        {
            var mainViewModel = _serviceProvider.GetService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<MainMenuViewModel>();
        }

        [RelayCommand]
        private void NavigateToUpdateProduct(ProductModel selectedProduct)
        {
            // Navigate to the update user view if a user is selected
            if (selectedProduct != null)
            {
                var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
                var productUpdateViewModel = _serviceProvider.GetRequiredService<ProductUpdateViewModel>();
                productUpdateViewModel.SelectedProduct = selectedProduct; // Pass the selected user to the update view model
                mainViewModel.CurrentViewModel = productUpdateViewModel;
            }
            else
            {
                MessageBox.Show("Please select a product to update.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ObservableCollection<ProductModel> ProductList { get; set; } = new ObservableCollection<ProductModel>();

        public void ShowProductList()
        {
            try
            {
                ProductList.Clear();

                var products = _productService.GetAllProducts();

                foreach ( var product in products )
                {
                    ProductList.Add(new ProductModel
                    {
                        ArticleNumber = product.ArticleNumber,
                        Company = product.Company,
                        Description = product.Description,
                        Ingress = product.Ingress,
                        Price = product.Price,
                        NutName = product.NutName,
                        NutSize = product.NutSize,
                        BoltName = product.BoltName,
                        BoltSize = product.BoltSize,
                    });

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading products: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void DeleteProduct(ProductModel product)
        {
            try
            {
                if (product != null)
                {
                    _productRepository.Delete(p => p.ArticleNumber == product.ArticleNumber);
                    ProductList.Remove(product);
                    MessageBox.Show("product deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No product selected for deletion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
                MessageBox.Show("An error occurred while deleting the product. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
