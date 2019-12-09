using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStructure.Config.Profiles
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings(IServiceProvider services)
        {
            Mapper.Initialize(c =>
            {
                // This is needed for automapper to be able create models
                // with injectable services
                c.AddProfile<LikeProfile>();
                c.AddProfile<PostProfile>();
                c.AddProfile<CommentProfile>();
                c.AddProfile<UserProfile>();


            });
        }
    }

    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }
}
