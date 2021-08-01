namespace Cat.ast.nodes
{
    public class PipelineAndNode : PipelineNode
    {
        public INode A { get; }
        public INode B { get; }

        public PipelineAndNode(INode a, INode b)
        {
            A = a;
            B = b;
        }
    }
}