using System;
using System.Collections.Generic;
using CatApi.interpreting;

namespace CatApi.ast.rules
{
    public static class Chain
    {
        public static ChainConfig StartWith(IRule rule)
        {
            return new(rule);
        }
    }

    public class ChainConfig
    {
        private List<IRule> Rules { get; }

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
            return new(Rules, collector);
        }
        public RuleChain CollectTailBy(Func<INode[], INode> collector)
        {
            return new(Rules, null, collector);
        }
    }
}