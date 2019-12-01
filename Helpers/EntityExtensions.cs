using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using ProjectStructure.DTO;
using ProjectStructure.Models;
using ProjectStructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Helpers
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Преобразует текущий объект в желаемый тип, используя AutoMapper
        /// </summary>
        /// <typeparam name="TSource">Тип источника</typeparam>
        /// <typeparam name="TDestination">Тип, в который будет преобразован объект</typeparam>
        /// <param name="entity">Объект, который нужно преобразовать</param>
        /// <returns>Объект желаемого типа</returns>
        public static TDestination Map<TSource, TDestination>(this TSource entity)
            where TSource : BaseEntity
            where TDestination : BaseVM
        {
            return Mapper.Map<TSource, TDestination>(entity);
        }

        /// <summary>
        /// Переносит поля из источника в указанный объект, используя AutoMapper
        /// </summary>
        /// <typeparam name="TSource">Тип источника</typeparam>
        /// <typeparam name="TDestination">Тип целевого объекта</typeparam>
        /// <param name="vm">Источник информации</param>
        /// <param name="entity">Целевой объект</param>
        /// <returns></returns>
        public static TDestination CopyTo<TSource, TDestination>(this TSource vm, TDestination entity)
            where TSource : BaseDTO
        {
            return Mapper.Map(vm, entity);
        }

        public static IQueryable<TEntity> AssignIncludes<TEntity>(this IQueryable<TEntity> query, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null) where TEntity : BaseEntity
        {
            if (include != null)
            {
                query = include(query);
            }
            return query;
        }
    }
}
