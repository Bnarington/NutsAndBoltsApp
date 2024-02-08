using Infrastructure.DTOs;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    /// <summary>
    /// Service responsible for managing products, nuts, and bolts.
    /// </summary>
    public class ProductService
    {
        private readonly BaseRepository<ProductEntity> _productRepository;
        private readonly BaseRepository<NutEntity> _nutRepository;
        private readonly BaseRepository<BoltEntity> _boltRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productRepository">The repository for products.</param>
        /// <param name="nutRepository">The repository for nuts.</param>
        /// <param name="boltRepository">The repository for bolts.</param>
        public ProductService(BaseRepository<ProductEntity> productRepository, BaseRepository<NutEntity> nutRepository, BaseRepository<BoltEntity> boltRepository)
        {
            _productRepository = productRepository;
            _nutRepository = nutRepository;
            _boltRepository = boltRepository;
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">The product to create.</param>
        /// <returns>True if the product is created successfully; otherwise, false.</returns>
        public bool CreateProduct(Product product)
        {
            try
            {
                if (!_productRepository.Exists(x => x.ArticleNumber == product.ArticleNumber))
                {
                    NutEntity nutEntity = null!;
                    BoltEntity boltEntity = null!;

                    if (string.IsNullOrEmpty(product.NutName) && string.IsNullOrEmpty(product.BoltName))
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
                        BoltId = boltEntity!.Id,
                        NutId = nutEntity!.Id
                    };

                    var result = _productRepository.Create(productEntity);

                    return result != null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>A collection of all products.</returns>
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
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
            }

            return products;
        }

        /// <summary>
        /// Retrieves a single product by company name.
        /// </summary>
        /// <param name="productEntity">The product entity containing company name.</param>
        /// <returns>A collection containing a single product.</returns>
        public IEnumerable<Product> GetOneProduct(Product productEntity)
        {
            var products = new List<Product>();

            try
            {
                var result = _productRepository.GetOne(x => x.Company == productEntity.Company);
                if (result != null)
                {
                    products.Add(new Product
                    {
                        Company = result.Company,
                        Description = result.Description,
                        Ingress = result.Ingress,
                        Price = result.Price,
                        BoltName = result.Bolt.BoltName,
                        BoltSize = result.Bolt.BoltSize,
                        NutName = result.Nut.NutName,
                        NutSize = result.Nut.NutSize
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
        /// Updates an existing product.
        /// </summary>
        /// <param name="productEntity">The product entity to update.</param>
        /// <returns>True if the product is updated successfully; otherwise, false.</returns>
        public bool UpdateProduct(ProductEntity productEntity)
        {
            try
            {
                var product = _productRepository.GetOne(x => x.ArticleNumber == productEntity.ArticleNumber);
                if (product != null)
                {
                    var productToUpdate = _productRepository.Update(x => x.ArticleNumber == product.ArticleNumber, productEntity);
                    return productToUpdate != null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR :: " + ex.Message);
            }

            return false;
        }


        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="productEntity">The product entity to delete.</param>
        /// <returns>True if the product is deleted successfully; otherwise, false.</returns>
        public bool DeleteProduct(Product productEntity)
        {
            try
            {
                var product = _productRepository.GetOne(x => x.ArticleNumber == productEntity.ArticleNumber);
                if (product != null)
                {
                    _productRepository.Delete(x => x.ArticleNumber == product.ArticleNumber);
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
