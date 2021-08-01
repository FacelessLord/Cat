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
        public static LazyChain Lazy(Func<IRuleChain> chain) => new(chain);

        public static RuleReader Reader => RuleReader.Create();
    }

    public class RuleReader : IRule
    {
        public static RuleReader Create() => new();
        private readonly List<IRuleChain> _chains;

        private RuleReader()
        {
            _chains = new List<IRuleChain>();
        }

        private RuleReader(RuleReader reader, IRuleChain parser)
        {
            _chains = new List<IRuleChain>(reader._chains) {parser};
        }

        public RuleReader With(IRuleChain chain)
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

        private readonly List<IRule> _rules;

        private RuleChainConfig()
        {
            _rules = new List<IRule>();
        }

        private RuleChainConfig(RuleChainConfig chainConfig, IRule rule)
        {
            _rules = new List<IRule>(chainConfig._rules) {rule};
        }

        public RuleChainConfig Then(IRule rule)
        {
            return new(this, rule);
        }

        public RuleChain Collect(Func<INode[], INode> collector)
        {
            return new RuleChain(_rules, collector);
        }
    }

    public interface IRuleChain
    {
        public bool TryRead(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode collectedNode);
    }

    public class RuleChain: IRuleChain
    {
        private readonly List<IRule> _rules;
        private readonly Func<INode[], INode> _collector;

        public RuleChain(List<IRule> rules, Func<INode[], INode> collector)
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
                if (!rule.TryRead(tokens, onError, out var node)) return false;
                nodes[i] = node;
            }

            collectedNode = _collector(nodes);

            return true;
        }
    }

    public class LazyChain : IRuleChain
    {
        private readonly Func<IRuleChain> _chain;

        public LazyChain(Func<IRuleChain> chain)
        {
            _chain = chain;
        }
        public bool TryRead(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode collectedNode)
        {
            return _chain().TryRead(tokens, onError, out collectedNode);
        }
    }

    public class ChainStarter
    {
        public RuleChainConfig StartWith(IRule rule)
        {
            return RuleChainConfig.Create().Then(rule);
        }
    }
}