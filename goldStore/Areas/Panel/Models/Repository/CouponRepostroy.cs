using goldStore.Areas.Panel.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace goldStore.Areas.Panel.Models.Repository
{
    public class CouponRepostroy : IRepository<coupons>
    {
        private goldstoreEntities _context;
        // private goldstoreEntities1 goldstoreEntities1;

        public CouponRepostroy(goldstoreEntities Context)
        {
            this._context = Context;
        }

        public void Delete(coupons model)
        {

            _context.coupons.Remove(model);
            _context.SaveChanges();
        }

        public coupons Get(int id)
        {
            return _context.coupons.Find(id);
        }

        public List<coupons> GetAll()
        {
            return _context.coupons.ToList();
        }

        public void Save(coupons model)
        {
            _context.coupons.Add(model);
            _context.SaveChanges();
        }

        public void Update(coupons model)
        {

            coupons old = Get(model.Id);
            _context.Entry(old).State = EntityState.Detached;
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}