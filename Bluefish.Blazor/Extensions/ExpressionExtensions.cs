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
