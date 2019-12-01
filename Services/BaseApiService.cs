using ProjectStructure.Context;
using ProjectStructure.Contracts;
using System;
using Microsoft.Extensions.DependencyInjection;
using ProjectStructure.Models;
using System.Threading.Tasks;
using ProjectStructure.DTO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using ProjectStructure.Helpers;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;

namespace ProjectStructure.Services
{
    public abstract class BaseApiService<TEntity, TEntityDTO>
        where TEntity : BaseEntity, new()
        where TEntityDTO : BaseDTO
    {
        protected ProjectContext DB { get; set; }
        protected IAppContext AppCtx { get; set; }

        public BaseApiService(IServiceProvider services)
        {
            DB = services.GetService<ProjectContext>();
            AppCtx = services.GetService<IAppContext>();
        }

        public virtual async Task<TEntity> GetById(Guid id, bool disableTracking = true, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var entityQuery = DB.Set<TEntity>().AsQueryable();

            if (disableTracking)
            {
                entityQuery = entityQuery.AsNoTracking();
            }

            // We assign all passed includes (if there are any)
            entityQuery = entityQuery.AssignIncludes(include);

            var entity = await entityQuery
                .FirstOrDefaultAsync(x => x.Id == id)
            ;

            ProjectExceptions.ThrowNotFoundIfNull(entity);

            return entity;
        }

        public virtual async Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> predicate = null,
                                                     bool disableTracking = true,
                                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var entityQuery = DB.Set<TEntity>()
                .AsQueryable()
            ;

            if (predicate != null)
            {
                entityQuery = entityQuery.Where(predicate);
            }

            if (disableTracking)
            {
                entityQuery = entityQuery.AsNoTracking();
            }

            // We assign all passed includes (if there are any)
            entityQuery = entityQuery.AssignIncludes(include);
            var entitiesList = await entityQuery.ToListAsync();

            ProjectExceptions.ThrowNotFoundIfNull(entitiesList);

            return entitiesList;
        }


        public virtual async Task<TEntity> Save(TEntityDTO data, bool autoSave = true, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var isNewEntity = data.Id == null || data.Id.Equals(Guid.Empty);

            var entityToSave = isNewEntity
                ? new TEntity()
                : await GetById(data.Id, false, include)
            ;

            ProjectExceptions.ThrowNotFoundIfNull(entityToSave);

            Mapper.Map(data, entityToSave);


            if (isNewEntity)
            {
                await IsNewEntity(data, entityToSave);
                DB.Set<TEntity>().Add(entityToSave);
            }
            else
            {
                await IsExistingEntity(data, entityToSave);
                DB.Set<TEntity>().Update(entityToSave);
            }

            if (autoSave) { await DB.SaveChangesAsync(); }

            return entityToSave;
        }

        public virtual async Task<TEntity> Delete(Guid id, bool autoSave = true, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            var toDelete = await GetById(id, false, include);

            //toDelete.IsDeleted = true;
            // In most cases, we should fully delete our objects.
            // IsDeleted is maintained only for future cases.
            DB.Set<TEntity>().Remove(toDelete);

            if (autoSave) { await DB.SaveChangesAsync(); }

            return toDelete;
        }

        /// <summary>
        /// If you need more actions with Old entity, override this method
        /// </summary>
        /// <param name="data">Entity DTO</param>
        /// <param name="entityToSave">You entity from DB</param>
        public virtual async Task IsExistingEntity(TEntityDTO data, TEntity entityToSave)
        {
        }

        /// <summary>
        /// If you wanna add additional change, please override this method
        /// </summary>
        /// <param name="data">Entity DTO</param>
        /// <param name="entityToSave">You entity to create</param>
        /// <param name="repo">You repository</param>
        public virtual async Task IsNewEntity(TEntityDTO data, TEntity entityToSave)
        {
        }
    }
}
