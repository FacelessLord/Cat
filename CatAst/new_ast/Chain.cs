using System;
using System.Collections.Generic;

namespace Cat.ast.new_ast
{
    public class Chain
    {
        public static ChainConfig StartWith(IRule rule)
        {
            return new ChainConfig(rule);
        }
    }

    public class ChainConfig
    {
        public List<IRule> Rules { get; }

        public ChainConfig(IRule rule)
        {
            Rules = new List<IRule> { rule };
        }

        public ChainConfig Then(IRule rule)
        {
            Rules.Add(rule);
            return this;
        }

        public RuleChain CollectBy(Func<INode[], INode> collector)
        {
            return new RuleChain(Rules, collector);
        }
        public RuleChain CollectTailBy(Func<INode[], INode> collector)
        {
            return new RuleChain(Rules, null, collector);
        }
    }
}