using System;
using System.Collections.Generic;
using System.Linq;
using Cat.ast.rules;
using Cat.lexing.tokens;

namespace Cat.ast.api
{
    public static class RuleHelper
    {
        public static ChainStarter Chain => new();

        public static RuleReader Reader => RuleReader.Create();
    }

    public class RuleReader
    {
        public static RuleReader Create() => new();
        private readonly List<RuleChain> _chains;

        private RuleReader()
        {
            _chains = new List<RuleChain>();
        }

        private RuleReader(RuleReader reader, RuleChain parser)
        {
            _chains = new List<RuleChain>(reader._chains) {parser};
        }

        public RuleReader With(RuleChain chain)
        {
            return new(this, chain);
        }

        public bool TryRead(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode node)
        {
            var stackDepth = tokens.PushPointer();
            foreach (var chain in _chains)
            {
                if (chain.TryRead(tokens, onError, out node))
                {
                    return true;
                }

                tokens.PopPointer(stackDepth+1);
                tokens.ResetPointer();
            }
            node = null;
            tokens.PopPointer(stackDepth);
            tokens.ResetPointer();

            return false;
        }
    }

    public class RuleChainConfig
    {
        public static RuleChainConfig Create() => new();

        private readonly List<IContext> _rules;

        private RuleChainConfig()
        {
            _rules = new List<IContext>();
        }

        private RuleChainConfig(RuleChainConfig chainConfig, IContext rule)
        {
            _rules = new List<IContext>(chainConfig._rules) {rule};
        }

        public RuleChainConfig Then(IContext rule)
        {
            return new(this, rule);
        }

        public RuleChain Collect(Func<INode[], INode> collector)
        {
            return new RuleChain(_rules, collector);
        }
    }

    public class RuleChain
    {
        private readonly List<IContext> _rules;
        private readonly Func<INode[], INode> _collector;

        public RuleChain(List<IContext> rules, Func<INode[], INode> collector)
        {
            _rules = rules;
            _collector = collector;
        }

        public bool TryRead(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode collectedNode)
        {
            collectedNode = null;
            var nodes = new INode[_rules.Count];
            for (var i = 0; i < _rules.Count; i++)
            {
                var rule = _rules[i];
                // ReSharper disable once PossibleMultipleEnumeration
                if (!rule.TryParse(tokens, onError, out var node)) return false;
                nodes[i] = node;
            }

            collectedNode = _collector(nodes);

            return true;
        }
    }

    public class ChainStarter
    {
        public RuleChainConfig StartWith(IContext rule)
        {
            return RuleChainConfig.Create().Then(rule);
        }
    }
}