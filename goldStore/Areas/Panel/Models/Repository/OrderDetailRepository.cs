using goldStore.Areas.Panel.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace goldStore.Areas.Panel.Models.Repository
{
    public class OrderDetailRepository : IRepository<orderDetals>
    {

        private goldstoreEntities _context;
        // private goldstoreEntities1 goldstoreEntities1;

        public OrderDetailRepository(goldstoreEntities Context)
        {
            this._context = Context;
        }

        public void Delete(orderDetals model)
        {

            if (model != null)
            {
                _context.orderDetals.Remove(model);
                _context.SaveChanges();

            }
        }

        public orderDetals Get(int id)
        {
            return _context.orderDetals.Find(id);
        }

        public List<orderDetals> GetAll()
        {
            return _context.orderDetals.AsNoTracking().ToList();
        }

        public void Save(orderDetals model)
        {
            if (model != null)
            {
                _context.orderDetals.Add(model);
                _context.SaveChanges();

            }
        }

       

        public void Update(orderDetals model)
        {
            throw new NotImplementedException();
        }
    }
}