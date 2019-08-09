using goldStore.Areas.Panel.Models.Interface;
using goldStore.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace goldStore.Areas.Panel.Models.Repository
{
    public class whishRepository : IRepository<wishlist>
    {
        private goldstoreEntities _context;
        // private goldstoreEntities1 goldstoreEntities1;

        public whishRepository(goldstoreEntities Context)
        {
            this._context = Context;
        }

        public void Delete(wishlist model)
        {
            _context.wishlist.Remove(model);
            _context.SaveChanges();
        }

        public wishlist Get(int id)
        {

            return _context.wishlist.Find(id);
        }

        public List<wishlist> GetAll()
        {
            return _context.wishlist.ToList();
        }

        public void Save(wishlist model)
        {
            _context.wishlist.Add(model);
            _context.SaveChanges();
        }

        public void Update(wishlist model)
        {

            wishlist old = Get(model.id);
            _context.Entry(old).State = EntityState.Detached;
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        
    }
}