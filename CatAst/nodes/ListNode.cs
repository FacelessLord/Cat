﻿namespace CatAst.nodes
{
    public class ListNode : INode
    {
        public INode[] Nodes { get; }

        public ListNode(ExpressionListNode listInternals)
        {
            Nodes = listInternals.Nodes.ToArray();
        }
    }
}