using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using hw.DebugFormatter;
using hw.Helper;

namespace Taabus.Data
{
    sealed class QueryProvider : DumpableObject, IQueryProvider
    {
        [DisableDump]
        internal readonly Server Server;

        internal QueryProvider(Server server) { Server = server; }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            NotImplementedMethod(expression);
            return null;
        }
        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            if(typeof(TElement) == typeof(DataRecord))
                return (IQueryable<TElement>) (IQueryable<DataRecord>) new ExpressionQuery(this, expression);
            NotImplementedMethod(expression);
            return null;
        }

        object IQueryProvider.Execute(Expression expression)
        {
            NotImplementedMethod(expression);
            return null;
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            var mce = expression as MethodCallExpression;
            if(mce != null && mce.Arguments.Count == 1)
                return Result<TResult>(CreateSQL(mce.Arguments[0]), mce.Method);

            NotImplementedMethod(expression);
            return default(TResult);
        }

        TResult Result<TResult>(string objectSql, MethodInfo m)
        {
            if(m.Name == "Count" && typeof(TResult) == typeof(int))
                return Server.Select("select count(*) from {0}".ReplaceArgs(objectSql), r => (TResult) r[0]).Single();
            NotImplementedMethod(objectSql, m);
            return default(TResult);
        }

        string CheckedCreate(string result, MethodCallExpression expression)
        {
            if(result != null || expression == null)
                return result;

            var objectExpression = expression.Object;
            var arguments = expression.Arguments;
            var method = expression.Method;
            var sqlConverter = method.GetAttribute<SQLConverter>(true);

            if(sqlConverter != null)
                return sqlConverter.Convert(this, objectExpression, arguments.ToArray());

            if(objectExpression == null && method.Name == "Where" && arguments.Count == 2)
                return CreateWhere(arguments[0], arguments[1]);

            NotImplementedMethod(result, expression);
            return null;
        }

        internal string CreateWhere(string objectSQL, Expression funcExpression)
        {
            var ue = funcExpression as UnaryExpression;
            if(ue != null)
                if(ue.NodeType == ExpressionType.Quote)
                    return CreateWhereQuote(objectSQL, ue.Operand);

            NotImplementedMethod(objectSQL, funcExpression);
            return null;
        }

        internal string CreateWhereQuote(string objectSQL, Expression funcExpression)
        {
            var lambdaExpression = funcExpression as Expression<Func<DataRecord, bool>>;
            if(lambdaExpression != null)
                return CreateWhereLambda(objectSQL, lambdaExpression.Parameters.Single(), lambdaExpression.Body);
            NotImplementedMethod(objectSQL, funcExpression, "type", funcExpression.GetType().PrettyName());
            return null;
        }

        string CreateWhere(Expression objectExpression, Expression funcExpression) { return CreateWhere(CreateSQL(objectExpression), funcExpression); }

        internal string CreateSQL(Expression expression)
        {
            string result = null;
            result = CheckedCreate(result, expression as ConstantExpression);
            result = CheckedCreate(result, expression as MethodCallExpression);
            if(result != null)
                return result;
            NotImplementedMethod(expression);
            return null;
        }

        string CreateWhereLambda(string objectSQL, ParameterExpression parameter, Expression body)
        {
            return "select * from {0} {1} where {2}"
                .ReplaceArgs
                (
                    objectSQL,
                    parameter.Name,
                    CreateSQL(body)
                );
        }

        string CheckedCreate(string result, ConstantExpression expression)
        {
            if(result != null || expression == null)
                return result;

            var typeQuery = expression.Value as TypeQuery;
            if(typeQuery != null)
                return typeQuery.SubStatement;

            NotImplementedMethod(result, expression);
            return null;
        }

        internal static IEnumerable<string> CreateFieldNames(string name, Expression expression)
        {
            return expression
                .Eval<IEnumerable<Field>>()
                .Select(f => name + "." + f.Name)
                ;
        }

        internal static string CreateValue(Expression expression)
        {
            return expression
                .Eval<string>()
                .SQLFormat();
        }
    }
}