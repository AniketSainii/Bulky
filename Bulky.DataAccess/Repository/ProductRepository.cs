using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository(ApplicationDbContext db) : Repository<Product>(db),IProductRepository
    {
        private readonly ApplicationDbContext _db = db;

        

        public void Update(Product obj)
        {
            _db.Update(obj);
        }
    }
}
