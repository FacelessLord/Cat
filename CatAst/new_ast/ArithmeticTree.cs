using System.Collections.Generic;
using System.Linq;
using Cat.ast.nodes;
using Cat.ast.nodes.arithmetics;
using Cat.lexing.tokens;
using static Cat.lexing.tokens.TokenTypes;

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

        public static HashSet<ITokenType> UnaryOperations = new() { Tilda, Minus, Plus };

        public static List<HashSet<ITokenType>> Operations = new List<HashSet<ITokenType>>()
        {
            new() { Is, As }, new() { Circumflex }, new() { Star, Divide }, new() { Plus, Minus }, new() { Percent },
            new() { TokenTypes.Equals, NotEquals }, new() { And }, new() { Or }
        };

        public static HashSet<ITokenType> AllOperations =
            UnaryOperations.Concat(Operations.SelectMany(n => n)).ToHashSet();

        public static INode Collect(INode[] nodes)
        {
            var flatTree = Flatten(nodes);

            //todo process errors with hang operators
            for (int i = flatTree.Count - 1; i >= 0; i--) // todo count - 2 after error checks
            {
                if (flatTree[i] is TokenNode tn && UnaryOperations.Contains(tn.Token.Type) && (i == 0 ||
                    flatTree[i - 1] is TokenNode tn2 && UnaryOperations.Contains(tn2.Token.Type)))
                {
                    var unaryOperationNode = new ArithmeticUnaryOperationNode(flatTree[i + 1], tn);
                    flatTree.RemoveRange(i, 2);
                    flatTree.Insert(i, unaryOperationNode);
                }
            }

            for (int i = 0; i < Operations.Count; i++)
            {
                var operations = Operations[i];
                for (int j = 0; j < flatTree.Count; j++) // todo count - 1 and j = 1 after error checks
                {
                    if (flatTree[j] is not TokenNode tn || !operations.Contains(tn.Token.Type)) continue;

                    var binaryOperationNode = new ArithmeticBinaryOperationNode(flatTree[j - 1], tn, flatTree[j + 1]);

                    flatTree.RemoveRange(j - 1, 3);
                    flatTree.Insert(j - 1, binaryOperationNode);
                    j--;
                }
            }
            
            //todo add custom collector for parens
            // parens here wouldn't be processed or expected
            return null;
        }
    }
}