using System.Collections.Generic;
using System.Linq;
using CatApi.ast.rules;
using CatApi.exceptions;
using CatApi.interpreting;
using CatApi.lexing;
using CatApi.lexing.tokens;
using static CatApi.lexing.tokens.TokenTypes;

namespace CatApi.ast.nodes.arithmetics
{
    public static class ArithmeticTree
    {
        public static List<INode> Flatten(INode[] arr)
        {
            var l = new List<INode> { arr[0] };
            if (arr.Length == 1)
                return l;
            if (arr[1] is NodeList nl)
                l.AddRange(Flatten(nl.Nodes));
            else
                l.Add(arr[1]);
            return l;
        }

        private static readonly HashSet<ITokenType> UnaryOperations = new() { Tilda, Minus, Plus };

        private static readonly List<HashSet<ITokenType>> Operations = new()
        {
            new HashSet<ITokenType>() { Is, As }, new HashSet<ITokenType>() { Circumflex }, new HashSet<ITokenType>() { Star, Divide }, new HashSet<ITokenType>() { Plus, Minus }, new HashSet<ITokenType>() { Percent },
            new HashSet<ITokenType>() { TokenTypes.Equals, NotEquals }, new HashSet<ITokenType>() { Circumflex }, new HashSet<ITokenType>() { And }, new HashSet<ITokenType>() { Or }
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

                for (var j = 1; j < flatTree.Count - 1; j++)
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