namespace Cat.ast.nodes
{
    public class PipelineOrNode : PipelineNode
    {
        public readonly INode A;
        public readonly INode B;

        public PipelineOrNode(INode a, INode b)
        {
            A = a;
            B = b;
        }
    }
}