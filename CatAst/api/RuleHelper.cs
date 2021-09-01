﻿using System;
using System.Collections.Generic;
using Cat.lexing.tokens;

namespace Cat.ast.api
{
    public static class RuleHelper
    {
        public static ChainStarter Chain => new();
        public static ChainStarter ComplementingChain => new();
        public static LazyChain Lazy(Func<IRuleChain> chain) => new(chain);

        public static ReaderNamingConfig Reader=> new ();
    }

    public class ReaderNamingConfig{
        
        internal ReaderNamingConfig(){}
        public RuleReader Named(string name) => RuleReader.Create(name);
    }

    public class RuleReader : IRule
    {
        public string Name { get; private set; }
        public static RuleReader Create(string name) => new(name);
        private List<IRuleChain> _chains;
        private List<IRuleChain> _complementingChains;
        

        protected RuleReader(string name)
        {
            Name = name;
            _chains = new List<IRuleChain>();
            _complementingChains = new List<IRuleChain>();
        }

        internal void FillFrom(RuleReader reader)
        {
            Name = reader.Name;
            _chains = reader._chains;
            _complementingChains = reader._complementingChains;
        }

        public RuleReader With(IRuleChain chain)
        {
            _chains.Add(chain);
            return this;
        }
        
        public RuleReader With(Func<RuleReader, IRuleChain> chain)
        {
            _chains.Add(chain(this));
            return this;
        }
        public RuleReader ComplementBy(IRuleChain chain)
        {
            _complementingChains.Add(chain);
            return this;
        }
        
        public RuleReader ComplementBy(Func<RuleReader, IRuleChain> chain)
        {
            _complementingChains.Add(chain(this));
            return this;
        }

        public bool TryRead(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode node)
        {
            var stackDepth = tokens.PushPointer();
            foreach (var chain in _chains)
            {
                if (chain.TryRead(tokens, onError, out var intermediateNode))
                {
                    ComplementNode(tokens, onError, out node, intermediateNode);
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

        private void ComplementNode(BufferedEnumerable<Token> tokens, Action<Token,string> onError, out INode node, INode intermediateNode)
        {
            var prevNode = intermediateNode;
            node = intermediateNode;

            do
            {
                foreach (var chain in _complementingChains)
                {
                    if (chain.TryRead(tokens, onError, out var newNode, node))
                    {
                        prevNode = node;
                        node = newNode;
                    }
                }
            } while (prevNode != node);
        }

        public override string ToString()
        {
            return Name;
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

        public RuleChain CollectBy(Func<INode[], INode> collector)
        {
            return new RuleChain(_rules, collector);
        }
        public ComplementingRuleChain CollectBy(Func<INode, INode[], INode> collector)
        {
            return new ComplementingRuleChain(_rules, collector);
        }
    }
    
    public interface IRuleChain
    {
        public bool TryRead(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode collectedNode, INode previousNode = null);
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

        public bool TryRead(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode collectedNode, INode previousNode = null)
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
    
    public class ComplementingRuleChain: IRuleChain
    {
        private readonly List<IRule> _rules;
        private readonly Func<INode, INode[], INode> _collector;

        public ComplementingRuleChain(List<IRule> rules, Func<INode, INode[], INode> collector)
        {
            _rules = rules;
            _collector = collector;
        }

        public bool TryRead(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode collectedNode, INode previousNode = null)
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

            collectedNode = _collector(previousNode, nodes);

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
        public bool TryRead(BufferedEnumerable<Token> tokens, Action<Token, string> onError, out INode collectedNode, INode previousNode = null)
        {
            return _chain().TryRead(tokens, onError, out collectedNode, previousNode);
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