namespace Bluefish.Blazor.Utility
{
    public class PathVisitor : ExpressionVisitor
    {
        private readonly Expression param;

        public string Path { get; private set; } = string.Empty;

        public PathVisitor(Expression parameter) => param = parameter;

        public override Expression Visit(Expression node)
        {
            if (node != null)
            {
                var chain = node.MemberClauses().ToList();
                if (chain.Any() && chain.First().Expression == param)
                {
                    Path = string.Join(".", chain.Select(mexpr => mexpr.Member.Name));
                    return node;
                }
            }
            return base.Visit(node);
        }
    }
}
