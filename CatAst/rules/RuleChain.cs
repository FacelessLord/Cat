using System;
using System.Collections.Generic;
using Cat.lexing.tokens;
using CatAst.api;

namespace CatAst.rules
{
    public class RuleChain
    {
        public List<IRule> Rules { get; }
        public Func<INode[], INode> Collector { get; }
        /**
         * gets all but first node
         * associated with betas in left-recursive rule scheme
         */
        public Func<INode[], INode> LeftRecursionCollector { get; }

        public RuleChain(List<IRule> rules, Func<INode[], INode> collector = null, Func<INode[], INode> leftRecursionCollector = null)
        {
            Rules = rules;
            Collector = collector;
            LeftRecursionCollector = leftRecursionCollector;
        }

        public INode Read(BufferedEnumerable<Token> tokens)
        {
            var nodes = new INode[Rules.Count];
            for (var i = 0; i < Rules.Count; i++)
            {
                var rule = Rules[i];
                var node = rule.Read(tokens);
                if (node == null)
                {
                    return null;
                }

                nodes[i] = node;
            }

            return Collector(nodes);
        }
    }
}