namespace Cat.ast.nodes
{
    public class ListNode : INode
    {
        public ExpressionListNode ListInternals { get; }

        public ListNode(ExpressionListNode listInternals)
        {
            ListInternals = listInternals;
        }
    }
}