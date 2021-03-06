using System;
using System.Collections.Generic;
using System.Linq;
using CatApi.interpreting;

namespace CatApi.ast.nodes
{
    public class ExpressionListNode : INode
    {
        public List<INode> Nodes { get; }

        public ExpressionListNode(INode[] nodes)
        {
            Nodes = nodes.ToList();
        }
        
        public ExpressionListNode(INode singleElement)
        {
            Nodes = new List<INode> { singleElement };
        }

        public ExpressionListNode(INode singleElement, INode latter)
        {
            Nodes = new List<INode> { singleElement };
            if (latter == null) return;
            
            if (latter is ExpressionListNode expList) //have to always be this
            {
                Nodes.AddRange(expList.Nodes);
            }
            else
                throw new ArgumentException("Latter have to be ExpressionListNode, not " + latter.GetType());
        }
    }
}