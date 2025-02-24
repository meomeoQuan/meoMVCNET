﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using meo.DataAccess.Data;
using meo.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace meo.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
            _db.Products.Include(u => u.Category);
            //_db.orderHeaders.Include(u => u.applicationUser);
        }
        public void Add(T entity)
        {
           dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            // Category,CategoryID ->    {"Category","CategoryID"}
            return query.FirstOrDefault();
        }


        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null,string ? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query.Where(filter);

            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includeProp in includeProperties.
                   Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries))
                    {
                    query = query.Include(includeProp);
                }
            }
           
            return query.ToList();
        }
  
        public void Remove(T entity)
        {
           dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
           dbSet.RemoveRange(entities);
        }


    }
}
