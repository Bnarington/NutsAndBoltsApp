using Infrastructure.DTOs;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Infrastructure.Services;

public class ProductService(ProductRepository productRepository, NutRepository nutRepository, BoltRepository boltRepository)
{
    private readonly ProductRepository _productRepository = productRepository;
    private readonly NutRepository _nutRepository = nutRepository;
    private readonly BoltRepository _boltRepository = boltRepository;


    public bool CreateProduct(Product product)
    {

        try
        {
            if (!_productRepository.Exists(x => x.ArticleNumber == product.ArticleNumber))
            {

                var nutEntity = _nutRepository.GetOne(x => x.NutName == product.NutName);
                var boltEntity = _boltRepository.GetOne(x => x.BoltName == product.BoltName);
                if (nutEntity == null && boltEntity == null)
                {
                    nutEntity = _nutRepository.Create(new NutEntity { NutName = product.NutName!, NutSize = product.NutSize });
                    boltEntity = _boltRepository.Create(new BoltEntity { BoltName = product.BoltName!, BoltSize = product.BoltSize });
                }

                var productEntity = new ProductEntity
                {
                    ArticleNumber = product.ArticleNumber,
                    Company = product.Company,
                    Description = product.Description,
                    Ingress = product.Ingress,
                    Price = product.Price,
                    BoltId = boltEntity.Id,
                    NutId = nutEntity!.Id,
                };

                var result = _productRepository.Create(productEntity);

                if (result != null)
                {
                    return true;
                }

            }
;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
    }



    public IEnumerable<Product> GetAllProducts()
    {

        var products = new List<Product>();

        try
        {
            var result = _productRepository.GetAll();


            foreach (var product in result)
            {
                products.Add(new Product
                {
                    Company = product.Company,
                    Description = product.Description,
                    Ingress = product.Ingress,
                    Price = product.Price,
                    BoltName = product.Bolt.BoltName,
                    BoltSize = product.Bolt.BoltSize,
                    NutName = product.Nut.NutName,
                    NutSize = product.Nut.NutSize
                });
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return products;
    }

    public IEnumerable<Product> GetOneProduct(Product productEntity)
    {

        var products = new List<Product>();

        try
        {
            var result = _productRepository.GetOne(x => x.Company ==  productEntity.Company);
            if (result != null)
            {
                var product = new Product();
                products.Add(new Product
                {
                    Company = product.Company,
                    Description = product.Description,
                    Ingress = product.Ingress,
                    Price = product.Price,
                    BoltName = product.BoltName,
                    BoltSize = product.BoltSize,
                    NutName = product.NutName,
                    NutSize = product.NutSize
                });
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return products;
    }

    public bool UpdateProduct(Product productEntity)
    {
        try
        {
            var product = _productRepository.GetOne(x => x.ArticleNumber == productEntity.ArticleNumber);
            if (product != null)
            {
                var productToUpdate = _productRepository.Update(product);
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
    }

    public bool DeleteProduct(Product productEntity)
    {

        try
        {
            var product = _productRepository.GetOne(x => x.ArticleNumber == productEntity.ArticleNumber);
            if (product != null)
            {
                var productToDelte = _productRepository.Delete(x => x.ArticleNumber == product.ArticleNumber);
                return true;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
    }
}

