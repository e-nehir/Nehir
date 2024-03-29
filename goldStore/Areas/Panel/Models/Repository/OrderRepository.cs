﻿using goldStore.Areas.Panel.Models.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace goldStore.Areas.Panel.Models.Repository
{
    public class OrderRepository : IRepository<orders>


    {

        private goldstoreEntities _context;
        // private goldstoreEntities1 goldstoreEntities1;

        public OrderRepository(goldstoreEntities Context)
        {
            this._context = Context;
        }


        public void Delete(orders model)
        {
           if(model != null)
            {
                _context.orders.Remove(model);
                _context.SaveChanges();

            }
        }

        public orders Get(int id)
        {
            return _context.orders.Find(id);
        }

        public List<orders> GetAll()
        {
            return _context.orders.AsNoTracking().ToList();
        }

        public void Save(orders model)
        {
            if(model !=null)
            {
                _context.orders.Add(model);
                _context.SaveChanges();

            }
        }

        public void Update(orders model)
        {
            
        }
    }
}