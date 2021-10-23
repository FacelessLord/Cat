using System;
using System.Collections.Generic;
using CatApi.interpreting;

namespace CatApi.ast.rules
{
    public static class Rule
    {
        public static CompositeRuleNamingConfig Named(string name)
        {
            return new(name);
        }
    }

    public class CompositeRuleNamingConfig
    {
        private string Name { get; }

        public CompositeRuleNamingConfig(string name)
        {
            Name = name;
        }

        public CompositeRule With(RuleChain chain)
        {
            return new(Name, new List<Func<IRule, RuleChain>> { _ => chain });
        }

        public CompositeRule With(Func<IRule, RuleChain> chain)
        {
            return new(Name, new List<Func<IRule, RuleChain>> { chain });
        }
    }
}