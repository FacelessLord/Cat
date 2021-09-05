namespace CatAst.nodes
{
    public class PipelineOrNode : PipelineNode
    {
        public INode A { get; }
        public INode B { get; }

        public PipelineOrNode(INode a, INode b)
        {
            A = a;
            B = b;
        }
    }
}