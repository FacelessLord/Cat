using System;
using System.Collections.Generic;
using System.Linq;
using CatApi.interpreting;
using CatApi.lexing;
using CatApi.utils;

namespace CatApi.ast.rules
{
    /**
     * Equivalent to all productions for non-terminal
     */
    public class CompositeRule : IRule
    {
        private string Name { get; }
        public List<Func<IRule, RuleChain>> Chains { get; set; }

        private bool _filled;

        public CompositeRule(string name, List<Func<IRule, RuleChain>> chains)
        {
            Name = name;
            Chains = chains;
            _filled = true;
        }

        private CompositeRule(string name)
        {
            Name = name;
        }

        private void Fill(List<Func<IRule, RuleChain>> chains)
        {
            if (_filled)
                throw new InvalidOperationException();
            _filled = true;
            Chains = chains;
        }

        public INode Read(BufferedEnumerable<IToken> tokens)
        {
            var depth = tokens.PushPointer();
            foreach (var chainGenerator in Chains)
            {
                var chain = chainGenerator(this);
                var node = chain.Read(tokens);
                if (node != null)
                {
                    return node;
                }

                tokens.PopPointer(depth + 1); // resetting to state before chain started reading
                tokens.ResetPointer();
            }

            tokens.PopPointer(depth);
            tokens.ResetPointer();
            return null;
        }

        public CompositeRule With(RuleChain chain)
        {
            Chains.Add(_ => chain);
            return this;
        }

        public CompositeRule With(Func<IRule, RuleChain> chain)
        {
            Chains.Add(chain);
            return this;
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
            var intermediateRule = new CompositeRule(rule.Name + "-inter");
            var ruleTails = recursiveRules
                .SelectMany(c => ProcessTailChains(c, tailRule, rule, intermediateRule)).ToList();
            tailRule.Fill(ruleTails);

            var intermediateRuleChains =
                chains.Where(c => c.Rules[0] != rule)
                    .SelectMany(c => ProcessHeaderChains(c, tailRule, rule, intermediateRule));
            intermediateRule.Fill(intermediateRuleChains.ToList());

            return Rule.Named(rule.Name).With(Chain.StartWith(intermediateRule)
                .CollectBy(nodes => collector(nodes[0] is NodeList nl ? nl.Nodes : new[] { nodes[0] })));
        }

        private static IEnumerable<Func<IRule, RuleChain>> ProcessTailChains(RuleChain c, CompositeRule ruleTail,
            CompositeRule mainRule, CompositeRule mainRuleReplacement)
        {
            yield return _ =>
                new RuleChain(
                    c.Rules.Skip(1).Select(r => r == mainRule ? mainRuleReplacement : r).Append(ruleTail).ToList(),
                    nodes => new NodeList(new[] { c.LeftRecursionCollector(nodes[..^1]) }, nodes[^1] as NodeList));
            yield return _ =>
                new RuleChain(c.Rules.Skip(1).Select(r => r == mainRule ? mainRuleReplacement : r).ToList(),
                    nodes => new NodeList(new[] { c.LeftRecursionCollector(nodes) }));
        }

        private static IEnumerable<Func<IRule, RuleChain>> ProcessHeaderChains(RuleChain c, CompositeRule ruleTail,
            CompositeRule mainRule, CompositeRule mainRuleReplacement)
        {
            yield return _ =>
                new RuleChain(c.Rules.Select(r => r == mainRule ? mainRuleReplacement : r).Append(ruleTail).ToList(),
                    nodes => new NodeList(new[] { c.Collector(nodes[..^1]) }, nodes[^1] as NodeList));
            yield return _ =>
                new RuleChain(c.Rules.Select(r => r == mainRule ? mainRuleReplacement : r).ToList(), c.Collector);
        }
    }
}