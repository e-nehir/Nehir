using goldStore.Areas.Panel.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace goldStore.Areas.Panel.Models.Repository
{
    public class OrderDetailRepository : IRepository<orders>
    {

        private goldstoreEntities _context;
        // private goldstoreEntities1 goldstoreEntities1;

        public OrderDetailRepository(goldstoreEntities Context)
        {
            this._context = Context;
        }

        public void Delete(orders model)
        {

            if (model != null)
            {
                _context.orderDetals.Remove(model);
                _context.SaveChanges();

            }
        }

        public orders Get(int id)
        {
            return _context.orderDetals.Find(id);
        }

        public List<orders> GetAll()
        {
            return _context.orderDetals.AsNoTracking().ToList();
        }

        public void Save(orders model)
        {
            if (model != null)
            {
                _context.orderDetals.Add(model);
                _context.SaveChanges();

            }
        }

        public void Update(orders model)
        {
        }
    }
}