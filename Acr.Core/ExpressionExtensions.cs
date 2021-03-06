﻿using System;
using System.Linq.Expressions;
using System.Reflection;


namespace Acr.Core
{
    public static class ExpressionExtensions
    {
        public static PropertyInfo GetPropertyInfo<TSender, TRet>(this TSender sender, Expression<Func<TSender, TRet>> expression)
        {
            var lambda = expression as LambdaExpression;
            var member = lambda.Body as MemberExpression;
            var property = sender.GetType().GetRuntimeProperty(member.Member.Name);
            return property;
        }
    }
}
