using System.Collections.Generic;
using System.Linq;

namespace Cat.ast.new_ast
{
    public static class ArithmeticTree
    {
        public static List<INode> Flatten(INode[] arr)
        {
            return arr.Length == 1 
                ? new List<INode>() { arr[0] } 
                : arr.SelectMany(n => Flatten(n is CompositeRule.NodeList nl ? nl.Nodes : new[] { n })).ToList();
        }

        public static INode Collect(INode[] nodes)
        {
            var flatTree = Flatten(nodes);
            return null;
        }
    }
}