using System;
using System.Collections.Generic;

namespace Cat.ast.new_ast
{
    public static class Rule
    {
        public static CompositeRuleNamingConfig Named(string name)
        {
            return new CompositeRuleNamingConfig(name);
        }
    }

    public class CompositeRuleNamingConfig
    {
        public string Name { get; }

        public CompositeRuleNamingConfig(string name)
        {
            Name = name;
        }

        public CompositeRule With(RuleChain chain)
        {
            return new CompositeRule(Name, new List<Func<IRule, RuleChain>> { (rule) => chain });
        }

        public CompositeRule With(Func<IRule, RuleChain> chain)
        {
            return new CompositeRule(Name, new List<Func<IRule, RuleChain>> { chain });
        }
    }
}