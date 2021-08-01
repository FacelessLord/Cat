namespace Cat.ast.nodes
{
    public class PipelineAndNode : PipelineNode
    {
        public readonly INode A;
        public readonly INode B;

        public PipelineAndNode(INode a, INode b)
        {
            A = a;
            B = b;
        }
    }
}