using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.DTOs;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Prsenentation.Models;
using System.Diagnostics;
using System.Windows;

namespace Presentation.ViewModels;

public partial class ProductUpdateViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ProductService _productService;

    public ProductUpdateViewModel(ProductService productService, IServiceProvider serviceProvider)
    {
        _productService = productService;
        _serviceProvider = serviceProvider;
    }

    private ProductModel _selectedProduct;

    public ProductModel SelectedProduct
    {
        get { return _selectedProduct; }
        set { SetProperty(ref _selectedProduct, value); }
    }

    [RelayCommand]
    private void NavigateToProductList()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
    }

    [RelayCommand]
    private void UpdateProduct()
    {
        try
        {
            if(SelectedProduct != null)
            {
                var product = new Product
                {
                    ArticleNumber = SelectedProduct.ArticleNumber,
                    Company = SelectedProduct.Company,
                    Description = SelectedProduct.Description,
                    Ingress = SelectedProduct.Ingress,
                    Price = SelectedProduct.Price,
                    BoltName = SelectedProduct.BoltName,
                    BoltSize = SelectedProduct.BoltSize,
                    NutName = SelectedProduct.NutName,
                    NutSize = SelectedProduct.NutSize,
                };

                var result = _productService.UpdateProduct(product);

                if (result)
                {
                    // Show success message and reload the user list
                    MessageBox.Show("User updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    var productListViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
                    productListViewModel.ShowProductList();
                    NavigateToProductList();
                }
                else
                {
                    MessageBox.Show("Failed to update product. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No product selected for update.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            Debug.WriteLine("ERROR :: " + ex.Message);
            MessageBox.Show("An error occurred while updating the product. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
