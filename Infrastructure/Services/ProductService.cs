using Infrastructure.DTOs;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Runtime.InteropServices;

namespace Infrastructure.Services;

public class ProductService(ProductRepository productRepository, NutRepository nutRepository, BoltRepository boltRepository )
{
    private readonly ProductRepository _productRepository = productRepository;
    private readonly NutRepository _nutRepository = nutRepository;
    private readonly BoltRepository _boltRepository = boltRepository;


    public bool CreateProduct(Product product )
    {
        if (!_productRepository.Exists(x => x.ArticleNumber == product.ArticleNumber))
        {

            var nutEntity = _nutRepository.GetOne(x => x.NutName == product.NutName);
            var boltEntity = _boltRepository.GetOne(x => x.BoltName == product.BoltName);
            if (nutEntity == null && boltEntity == null)
            {
                nutEntity = _nutRepository.Create(new NutEntity { NutName = product.NutName, NutSize = product.NutSize });
                boltEntity = _boltRepository.Create(new BoltEntity {  BoltName = product.BoltName, BoltSize = product.BoltSize });
            }

            var productEntity = new ProductEntity
            {
                ArticleNumber = product.ArticleNumber,
                Company = product.Company,
                Description = product.Description,
                Ingress = product.Ingress,
                Price = product.Price,
                BoltId = boltEntity.Id,
                NutId = nutEntity.Id,
            };

            var result = _productRepository.Create(productEntity);

            if (result != null)
            {
                return true;
            }

        }

        return false;

    }


    public IEnumerable<Product> GetAllProducts()
    {
        var result = _productRepository.GetAll();

        var products = new List<Product>();
        foreach (var product in result)
        {
            products.Add(new Product
            {
                Title = product.Title,
                Description = product.Description,
                Ingress = product.Ingress,
                Price = product.Price,
                BoltName = product.,

            });
        }
    }
}
