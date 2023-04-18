using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Shop.Model;
using Shop.Raven;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace Shop
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("HEllo world");
            // CreateProduct("Apples", 9.10);
            // CreateProduct("Tea", 9.10);
            // CreateProduct("Soda", 9.10);

            //GetProduct("products/34-A");

            //GetAllProducts();

            // GetProducts(1, 2);

            //CreateCart("nicolas.rojasm@utadeo.edu.co");

            // AddProductToCart("nicolas.rojasm@utadeo.edu.co", "products/34-A", 5);
            

        }

        // Create
        static void CreateProduct(string name, double price)
        {
            Product p = new Product();
            p.Name = name;
            p.Price = price;




            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                session.Store(p);
                session.SaveChanges();
            }

            
        }

        // Read
        static void GetProduct(string id)
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
              Product p = session.Load<Product>(id);
                Console.WriteLine($"Product: {p.Name} \t\t price: {p.Price}");
            }
        }

        static void GetAllProducts()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                List<Product> all = session.Query<Product>().ToList();

                foreach (Product p in all)
                {
                   Console.WriteLine($"Product: {p.Name} \t\t price: {p.Price}");

                }


            }


        }

        static void GetProducts(int pageNdx, int pageSize)
        {

            int skip = (pageNdx-1) * pageSize;
            int take = pageSize;


            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                List<Product> page = session.Query<Product>()
                    .Statistics(out QueryStatistics stats)
                    .Skip(skip)
                    .Take(take)
                    .ToList();


                Console.WriteLine($"Showing results {skip + 1} to {skip + pageSize} of {stats.TotalResults}");


                foreach (Product p in page)
                {
                    Console.WriteLine($"Product: {p.Name} \t\t price: {p.Price} ");
                }

                Console.WriteLine($"This was produced in {stats.DurationInMs} ms");


            }
        }

        static void CreateCart(string customer)
        {
            Cart cart = new Cart();
            cart.Customer = customer;



            using(var session = DocumentStoreHolder.Store.OpenSession())
            {
                session.Store(cart);
                session.SaveChanges();

            }
        }
        static void AddProductToCart(string customer, string ProductId, int quantity)
        {

            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
              Cart cart= session.Query<Cart>().Single(x => x.Customer == customer);

                Product p = session.Load<Product>(ProductId);

                cart.Lines.Add (new CartLine
                {
                    ProductName = p.Name,
                    ProductPrice = p.Price,
                    Quantity = quantity          
                    

                });                      
       
                session.SaveChanges();

            }

        }

        static void DeleteProduct(string customer, string ProductId, int quantity)
        {

            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                session.Delete("products / 34 - A");

                session.SaveChanges();

            }

        }



    }
}

    