﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repositories.Context;
using Repositories.Models.Base;

namespace Repositories.Repos.Base
{
    public abstract class RepoBase<T> : IRepo<T> where T : EntityBase, new()
    {
        public DbSet<T> Table { get; }
        public BloggingContext Context { get; }
        private readonly bool _disposeContext;

        protected RepoBase(BloggingContext context)
        {
            Context = context;
            Table = Context.Set<T>();
            _disposeContext = false;
        }

        protected RepoBase(DbContextOptions<BloggingContext> options) : this(new BloggingContext(options))
        {
            _disposeContext = true;
        }

        public virtual void Dispose()
        {
            if (_disposeContext)
            {
                Context.Dispose();
            }
        }

        public T Find(int? id) => Table.Find(id);
        public T FindAsNoTracking(int id) => Table.Where(x => x.Id == id).AsNoTracking().FirstOrDefault();
        public T FindIgnoreQueryFilters(int id) => Table.IgnoreQueryFilters().FirstOrDefault(x => x.Id == id);
        public virtual IEnumerable<T> GetAll() => Table;

        public virtual IEnumerable<T> GetAll(IList<(Expression<Func<T, object>> orderBy, bool desc)> orders)
        {
            IOrderedQueryable<T> query = default;
            for (int x = 0; x<orders.Count;x++)
            {
                switch (x)
                {
                    case 0:
                        query = orders[x].desc ? Table.OrderByDescending(orders[x].orderBy) : Table.OrderBy(orders[x].orderBy);
                        break;
                    default:
                        query = orders[x].desc ? query?.ThenByDescending(orders[x].orderBy) : query?.OrderBy(orders[x].orderBy);
                        break;
                }
            }

            return query;
        }

        public IEnumerable<T> GetRange(IQueryable<T> query, int skip, int take)
            => query.Skip(skip).Take(take);

        public virtual int Add(T entity, bool persist = true)
        {
            Table.Add(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int AddRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.AddRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Update(T entity, bool persist = true)
        {
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int UpdateRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public virtual int Delete(T entity, bool persist = true)
        {
            Table.Remove(entity);
            return persist ? SaveChanges() : 0;
        }

        public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
        {
            Table.RemoveRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //A concurrency error occurred
                //Should log and handle intelligently
                throw;// new SpyStoreConcurrencyException("A concurrency error happened.", ex);
            }
            catch (RetryLimitExceededException ex)
            {
                //DbResiliency retry limit exceeded
                //Should log and handle intelligently
                throw;// new SpyStoreRetryLimitExceededException("There is a problem with you connection.", ex);
            }
            catch (DbUpdateException ex)
            {
                //Should log and handle intelligently
                if (ex.InnerException is SqlException sqlException)
                {
                    if (sqlException.Message.Contains("FOREIGN KEY constraint", StringComparison.OrdinalIgnoreCase))
                    {
                        if (sqlException.Message.Contains("table \"Store.Products\", column 'Id'",
                            StringComparison.OrdinalIgnoreCase))
                        {
                            throw;// new SpyStoreInvalidProductException($"Invalid Product Id\r\n{ex.Message}", ex);
                        }

                        if (sqlException.Message.Contains("table \"Store.Customers\", column 'Id'",
                            StringComparison.OrdinalIgnoreCase))
                        {
                            throw;// new SpyStoreInvalidCustomerException($"Invalid Customer Id\r\n{ex.Message}", ex);
                        }
                    }
                }

                throw;// new SpyStoreException("An error occurred updating the database", ex);
            }
            catch (Exception ex)
            {
                //Should log and handle intelligently
                throw; //new SpyStoreException("An error occurred updating the database", ex);
            }
        }
    }
}