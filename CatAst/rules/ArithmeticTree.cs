using System.Collections.Generic;
using System.Linq;
using Cat.lexing.tokens;
using CatAst.exceptions;
using CatAst.nodes;
using CatAst.nodes.arithmetics;
using static Cat.lexing.tokens.TokenTypes;

namespace CatAst.rules
{
    public static class ArithmeticTree
    {
        public static List<INode> Flatten(INode[] arr)
        {
            return arr.Length == 1
                ? new List<INode>() { arr[0] }
                : arr.SelectMany(n => Flatten(n is CompositeRule.NodeList nl ? nl.Nodes : new[] { n })).ToList();
        }

        private static readonly HashSet<ITokenType> UnaryOperations = new() { Tilda, Minus, Plus };

        private static readonly List<HashSet<ITokenType>> Operations = new List<HashSet<ITokenType>>()
        {
            new() { Is, As }, new() { Circumflex }, new() { Star, Divide }, new() { Plus, Minus }, new() { Percent },
            new() { TokenTypes.Equals, NotEquals }, new() { And }, new() { Or }
        };

        public static INode Collect(INode[] nodes)
        {
            var flatTree = Flatten(nodes);

            //actually these errors are impossible
            if (flatTree[^1] is TokenNode tnc && UnaryOperations.Contains(tnc.Token.Type))
            {
                throw new CatMisplacedOperatorException(tnc, true);
            }

            for (var i = flatTree.Count - 2; i >= 0; i--)
            {
                if (flatTree[i] is not TokenNode tn || !UnaryOperations.Contains(tn.Token.Type) || (i != 0 &&
                    (flatTree[i - 1] is not TokenNode tn2 || !UnaryOperations.Contains(tn2.Token.Type)))) continue;
                
                var unaryOperationNode = new ArithmeticUnaryOperationNode(flatTree[i + 1], tn);
                flatTree.RemoveRange(i, 2);
                flatTree.Insert(i, unaryOperationNode);
            }

            foreach (var operations in Operations)
            {
                //actually these errors are impossible
                if (flatTree[0] is TokenNode tnb1 && operations.Contains(tnb1.Token.Type))
                    throw new CatMisplacedOperatorException(tnb1, false);
                if (flatTree[^1] is TokenNode tnb2 && operations.Contains(tnb2.Token.Type))
                    throw new CatMisplacedOperatorException(tnb2, false);

                for (var j = 1; j < flatTree.Count-1; j++)
                {
                    if (flatTree[j] is not TokenNode tn || !operations.Contains(tn.Token.Type)) continue;

                    var binaryOperationNode = new ArithmeticBinaryOperationNode(flatTree[j - 1], tn, flatTree[j + 1]);

                    flatTree.RemoveRange(j - 1, 3);
                    flatTree.Insert(j - 1, binaryOperationNode);
                    j--;
                }
            }
            
            return flatTree.Single();
        }
    }
}