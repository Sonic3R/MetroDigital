using MetroDigital.Application.Common.Base;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace MetroDigital.Application.Common.Factories
{
    public class ValidationErrorBuilder<TRequest, TResponse>
        where TRequest : Request<TResponse>
        where TResponse : Response, new()
    {
        private List<ValidationError> _validationResults = new();
        private static Regex _listIndexRegex = new(@"\.get_Item\((\d+)\)", RegexOptions.Compiled);
        private static Regex _prefixRemoverRegex = new(@"value\([^)]+\)\.\w+\.", RegexOptions.Compiled);

        private readonly EvaluateListIndexValueVisitor _visitor = new();

        private string GetPropertyName(Expression<Func<object>> expression)
        {
            var body = (expression.Body as UnaryExpression)?.Operand ?? expression.Body;
            body = _visitor.Visit(body);

            var name = body.ToString();
            name = _prefixRemoverRegex.Replace(name, "");
            return _listIndexRegex.Replace(name, "[$1]");
        }

        public void Add(Expression<Func<object>> expression, string errorMessage)
        {
            Add(GetPropertyName(expression), errorMessage);
        }

        public void Add(string propName, string errorMessage)
        {
            var validationResult = new ValidationError
            {
                PropertyName = propName,
                ErrorMessage = errorMessage
            };
            _validationResults.Add(validationResult);
        }

        public List<ValidationError> GetValidationFailures()
        {
            return _validationResults;
        }
    }

    internal class EvaluateListIndexValueVisitor : ExpressionVisitor
    {
        private bool IsListIndexer(MethodCallExpression call)
        {
            if (!call.Method.DeclaringType.IsGenericType) return false;
            if (call.Method.Name != "get_Item") return false;
            return typeof(List<>) == call.Method.DeclaringType.GetGenericTypeDefinition();
        }
        public override Expression Visit(Expression node)
        {
            if (node is MethodCallExpression call)
            {
                if (!IsListIndexer(call)) return node;

                var argument = call.Arguments.Single();
                var value = Expression.Lambda(argument).Compile().DynamicInvoke();
                return call.Update(call.Object, new[] { Expression.Constant(value) });
            }

            return base.Visit(node);
        }
    }
}