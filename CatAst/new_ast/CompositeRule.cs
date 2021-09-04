using System;
using System.Collections.Generic;
using System.Linq;
using Cat.ast.api;
using Cat.lexing.tokens;

namespace Cat.ast.new_ast
{
    /**
     * Equivelent to all productions for non-terminal
     */
    public class CompositeRule : IRule
    {
        public string Name { get; }
        public List<Func<IRule, RuleChain>> Chains { get; private set; }

        public bool Filled = false;

        public CompositeRule(string name, List<Func<IRule, RuleChain>> chains)
        {
            Name = name;
            Chains = chains;
            Filled = true;
        }

        public CompositeRule(string name)
        {
        }

        public void Fill(List<Func<IRule, RuleChain>> chains)
        {
            if (Filled)
                throw new InvalidOperationException();
            Filled = true;
            Chains = chains;
        }

        public INode Read(BufferedEnumerable<Token> tokens)
        {
            var depth = tokens.PushPointer();
            foreach (var chain in Chains)
            {
                var node = chain(this).Read(tokens);
                if (node != null)
                {
                    return node;
                }

                tokens.PopPointer(depth + 1); // reseting to state before chain started reading
                tokens.ResetPointer();
            }

            tokens.PopPointer(depth);
            tokens.ResetPointer();
            return null;
        }

        public CompositeRule With(RuleChain chain)
        {
            Chains.Add((thisRule) => chain);
            return this;
        }

        public CompositeRule With(Func<IRule, RuleChain> chain)
        {
            Chains.Add(chain);
            return this;
        }

        public class NodeList : INode
        {
            public INode[] Nodes { get; }

            public NodeList(INode[] nodes)
            {
                Nodes = nodes;
            }

            public NodeList(INode[] nodes, NodeList list)
            {
                Nodes = nodes.Concat(list.Nodes).ToArray();
            }
        }

        /**
         * Creates new right-recursive rule
         * collector accepts 
         * Fixes only one non-terminal on the left and only immediate recursion
         */
        public static IRule FixLeftRecursion(CompositeRule rule, Func<INode[], INode> collector)
        {
            var chains = rule.Chains.Select(c => c(rule)).ToList();
            var recursiveRules = chains.Where(c => c.Rules[0] == rule);

            var tailRule = new CompositeRule(rule.Name + "-rTail");
            var ruleTails = recursiveRules
                .SelectMany(c => ProcessTailChains(c, tailRule)).ToList();
            tailRule.Fill(ruleTails);
            
            var intermediateRuleChains = chains.Where(c => c.Rules[0] != rule).SelectMany(c => ProcessHeaderChains(c, tailRule));
            var intermediateRule = new CompositeRule(rule.Name+"-inter", intermediateRuleChains.ToList());
            
            return Rule.Named(rule.Name).With(Chain.StartWith(intermediateRule).CollectBy(nodes => collector((nodes[0] as NodeList)?.Nodes)));
        }

        private static IEnumerable<Func<IRule, RuleChain>> ProcessTailChains(RuleChain c, CompositeRule RuleTail)
        {
            yield return _ => new RuleChain(c.Rules.Skip(1).Append(RuleTail).ToList(),
                nodes => new NodeList(new[]{c.LeftRecursionCollector(nodes[..^1])}, nodes[^1] as NodeList));
            yield return _ => new RuleChain(c.Rules.Skip(1).ToList(), nodes => new NodeList(new[]{c.LeftRecursionCollector(nodes)}));
        }
        private static IEnumerable<Func<IRule, RuleChain>> ProcessHeaderChains(RuleChain c, CompositeRule RuleTail)
        {
            yield return _ => new RuleChain(c.Rules.Append(RuleTail).ToList(),
                nodes => new NodeList(new[]{c.Collector(nodes[..^1])}, nodes[^1] as NodeList));
            yield return _ => new RuleChain(c.Rules.ToList(), c.Collector);
        }
    }
}