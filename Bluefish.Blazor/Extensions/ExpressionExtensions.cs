using System.Reflection;

namespace Bluefish.Blazor.Extensions;

public static class ExpressionExtensions
{
    /// <summary>
    /// Get MemberInfo even if the referenced member is a primitive value nestled inside
    /// a UnaryExpression which is a Convert operation.
    /// </summary>
    /// <param name="p_body">
    /// The Expression Body which is evaluated as a Member expression
    /// </param>
    /// <returns>
    /// A FieldInfo or PropertyInfo representing the member described in the expression.
    /// </returns>
    public static MemberInfo GetMemberInfo(this Expression p_body)
    {
        MemberExpression memberExpr = p_body as MemberExpression;

        if (p_body is UnaryExpression)
        {
            var unaryBody = p_body as UnaryExpression;
            if (unaryBody.NodeType != ExpressionType.Convert &&
                unaryBody.NodeType != ExpressionType.ConvertChecked)
                throw new ArgumentException("A Non-Convert Unary Expression was found.");

            memberExpr = unaryBody.Operand as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException
                ("The target of the Convert operation was not a MemberExpression.");
        }
        else if (memberExpr == null)
        {
            throw new ArgumentException("The Expression must identify a single member.");
        }

        var member = memberExpr.Member;
        if (!(member is FieldInfo || member is PropertyInfo))
            throw new ArgumentException
            ("The member specified was not a Field or Property: " + member.GetType());

        return memberExpr.Member;
    }

    /// <summary>
    ///     Gets the corresponding <see cref="PropertyInfo" /> from an <see cref="Expression" />.
    /// </summary>
    /// <param name="expression">The expression that selects the property to get info on.</param>
    /// <returns>The property info collected from the expression.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="expression" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">The expression doesn't indicate a valid property."</exception>
    public static PropertyInfo GetPropertyInfo<TItem>(this Expression<Func<TItem, object>> expression) => expression?.Body switch
    {
        null => throw new ArgumentNullException(nameof(expression)),
        UnaryExpression ue when ue.Operand is MemberExpression me => (PropertyInfo)me.Member,
        MemberExpression me => (PropertyInfo)me.Member,
        _ => throw new ArgumentException($"The expression doesn't indicate a valid property. [ {expression} ]")
    };

    public static IEnumerable<MemberExpression> MemberClauses(this Expression expr)
    {
        if (expr is not MemberExpression mexpr)
        {
            yield break;
        }
        foreach (var item in MemberClauses(mexpr.Expression))
        {
            yield return item;
        }
        yield return mexpr;
    }
}
