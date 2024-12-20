﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Data;
using SocialMedia.Data.Interfaces;

namespace SocialMedia.Data.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext db;
        internal DbSet<T> dbSet;
        protected Repository(ApplicationDbContext dbContext)
        {
            db = dbContext;
            dbSet = db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public List<T> GetAll()
        {
            return dbSet.ToList();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public T GetBy(Expression<Func<T, bool>> expression)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(expression);
            return query.FirstOrDefault();
        }
        public List<T> GetAllBy(Expression<Func<T, bool>> expression)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(expression);
            return query.ToList();
        }
    }
}
