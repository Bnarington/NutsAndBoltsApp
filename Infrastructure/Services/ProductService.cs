using Infrastructure.DTOs;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    /// <summary>
    /// Service responsible for managing products, nuts, and bolts.
    /// </summary>
    public class ProductService
    {
        private readonly ProductRepository _repository;
        private readonly NutRepository _nutRepository;
        private readonly BoltRepository _boltRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="repository">The repository for products.</param>
        /// <param name="nutRepository">The repository for nuts.</param>
        /// <param name="boltRepository">The repository for bolts.</param>
        public ProductService(ProductRepository repository, NutRepository nutRepository, BoltRepository boltRepository)
        {
            _repository = repository;
            _nutRepository = nutRepository;
            _boltRepository = boltRepository;
        }

        /// <summary>
        /// Creates a new product entity with the provided details.
        /// </summary>
        /// <param name="ArticleNumber">The article number of the product.</param>
        /// <param name="Company">The company of the product.</param>
        /// <param name="Description">The description of the product.</param>
        /// <param name="Ingress">The ingress of the product.</param>
        /// <param name="Price">The price of the product.</param>
        /// <param name="NutName">The name of the nut.</param>
        /// <param name="NutSize">The size of the nut.</param>
        /// <param name="BoltName">The name of the bolt.</param>
        /// <param name="BoltSize">The size of the bolt.</param>
        /// <returns>The newly created product entity.</returns>
        public ProductEntity CreateProduct(string ArticleNumber, string Company, string Description, string Ingress, decimal Price, string NutName, decimal NutSize, string BoltName, decimal BoltSize)
        {
            // Validation
            if (string.IsNullOrEmpty(ArticleNumber) || string.IsNullOrEmpty(Company))
            {
                throw new ArgumentException("One or more input parameters are null or empty.");
            }

            if (_repository.Exists(x => x.ArticleNumber == ArticleNumber))
            {
                throw new InvalidOperationException("A product with the provided articlenumber already exists.");
            }

            // Ensure NutEntity exists or create it
            var createNutEntity = _nutRepository.GetOne(x => x.NutName == NutName && x.NutSize == NutSize);
            if (createNutEntity == null)
            {
                createNutEntity = _nutRepository.Create(new NutEntity { NutName = NutName, NutSize = NutSize });
            }

            // Ensure BoltEntity exists or create it
            var createBoltEntity = _boltRepository.GetOne(x => x.BoltName == BoltName && x.BoltSize == BoltSize);
            if (createBoltEntity == null)
            {
                createBoltEntity = _boltRepository.Create(new BoltEntity { BoltName = BoltName, BoltSize = BoltSize });
            }

            // Fetch existing NutEntity and BoltEntity
            var nutNameEntity = _nutRepository.GetOne(x => x.NutName == NutName);
            var boltNameEntity = _boltRepository.GetOne(x => x.BoltName == BoltName);

            // Create ProductEntity
            var productEntity = new ProductEntity()
            {
                ArticleNumber = ArticleNumber,
                Company = Company,
                Description = Description,
                Ingress = Ingress,
                Price = Price,
                BoltId = boltNameEntity.Id,
                NutId = nutNameEntity.Id,
            };

            // Save ProductEntity to the database
            var result = _repository.Create(productEntity);
            if (result == null)
            {
                throw new Exception("Failed to create user.");
            }

            return result;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>An enumerable collection of products.</returns>
        public IEnumerable<Product> GetAllProducts()
        {
            var products = new List<Product>();

            try
            {
                var result = _repository.GetAll();

                foreach (var item in result)
                {
                    products.Add(new Product
                    {
                        ArticleNumber = item.ArticleNumber,
                        Company = item.Company,
                        Description = item.Description,
                        Ingress = item.Ingress,
                        Price = item.Price,
                        BoltName = item.Bolt.BoltName,
                        NutName = item.Nut.NutName,
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
            }

            return products;
        }


        /// <summary>
        /// Updates an existing product entity with the provided details.
        /// </summary>
        /// <param name="productEntity">The product entity to update.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public bool UpdateProduct(Product productEntity)
        {
            try
            {
                var product = _repository.GetOne(x => x.ArticleNumber == productEntity.ArticleNumber);
                if (product != null)
                {
                    product.ArticleNumber = productEntity.ArticleNumber;
                    product.Company = productEntity.Company;
                    product.Description = productEntity.Description;
                    product.Ingress = productEntity.Ingress;
                    product.Price = productEntity.Price;
                    product.Bolt = _boltRepository.SelectBoltName(productEntity.BoltName!);
                    product.Nut = _nutRepository.SelectNutName(productEntity.NutName!);

                    _repository.Update(x => x.ArticleNumber == productEntity.ArticleNumber, product);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Deletes an existing product entity.
        /// </summary>
        /// <param name="productEntity">The product entity to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public bool DeleteProduct(Product productEntity)
        {
            try
            {
                var product = _repository.GetOne(x => x.ArticleNumber == productEntity.ArticleNumber);
                if (product != null)
                {
                    _repository.Delete(x => x.ArticleNumber == productEntity.ArticleNumber);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
            }

            return false;
        }
    }
}
