using System;
using System.Collections.Generic;
using CatApi.interpreting;

namespace CatImplementations.ast.nodes
{
    public class ValuePairListNode : INode
    {
        public List<ValuePairNode> Nodes { get; }

        public ValuePairListNode(ValuePairNode singleNode)
        {
            Nodes = new List<ValuePairNode>() {singleNode};
        }

        public ValuePairListNode(ValuePairNode singleElement, INode latter)
        {
            Nodes = new List<ValuePairNode> {singleElement};
            if (latter is ValuePairListNode expList) //have to always be this
            {
                Nodes.AddRange(expList.Nodes);
            }
            else
                throw new ArgumentException("Latter have to be ExpressionListNode, not " + latter.GetType());
        }
    }
}