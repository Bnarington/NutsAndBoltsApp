using Infrastructure.Entities;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class ProductService(ProductRepository productRepository, NutRepository nutRepository, BoltRepository boltRepository )
{
    private readonly ProductRepository _productRepository = productRepository;
    private readonly NutRepository _nutRepository = nutRepository;
    private readonly BoltRepository _boltRepository = boltRepository;


    public bool CreateProduct(string articleNumber, string title, string description, string ingress, decimal price, string nutName, string nutSize, string boltName, string boltSize)
    {
        if (!_productRepository.Exists(x => x.ArticleNumber == articleNumber))
        {

            var nutEntity = _nutRepository.GetOne(x => x.NutName == nutName);
            var boltEntity = _boltRepository.GetOne(x => x.BoltName == boltName);
            if (nutEntity == null && boltEntity == null)
            {
                nutEntity = _nutRepository.Create(new NutEntity { NutName = nutName, NutSize = nutSize });
                boltEntity = _boltRepository.Create(new BoltEntity {  BoltName = boltName, BoltSize = boltSize });
            }

            var productEntity = new ProductEntity
            {
                ArticleNumber = articleNumber,
                Title = title,
                Description = description,
                Ingress = ingress,
                Price = price,
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
}
