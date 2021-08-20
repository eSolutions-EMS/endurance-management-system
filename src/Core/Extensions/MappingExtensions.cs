using AutoMapper;
using System;
using System.Linq.Expressions;

namespace EnduranceJudge.Core.Extensions
{
    public static class MappingExtensions
    {
        public static IMappingExpression<TSource, TDestination> MapMember<
            TSource,
            TDestination,
            TSourceMember,
            TDestinationMember>(
            this IMappingExpression<TSource, TDestination> mapping,
            Expression<Func<TDestination, TDestinationMember>> destinationMemberSelector,
            Expression<Func<TSource, TSourceMember>> sourceMemberSelector)
        {
            return mapping.ForMember(destinationMemberSelector, opt => opt.MapFrom(sourceMemberSelector));
        }
    }
}
