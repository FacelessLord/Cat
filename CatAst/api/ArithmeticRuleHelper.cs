using System;
using System.Collections.Generic;

namespace Cat.ast.api
{
    public static class ArithmeticRuleHelper
    {
        public static ArithmeticRuleReaderNamingConfig StartRule() => new ArithmeticRuleReaderNamingConfig();
    }

    public class ArithmeticRuleReaderNamingConfig
    {
        public ArithmeticRuleReaderConfigStarter Named(string name) => new ArithmeticRuleReaderConfigStarter(name);
    }
    public class ArithmeticRuleReaderConfigStarter
    {
        private readonly string _name;

        public ArithmeticRuleReaderConfigStarter(string name)
        {
            _name = name;
        }
        
        public ArithmeticRuleReaderConfig StartWith(Func<RuleReader, RuleReader> startingRule) => ArithmeticRuleReaderConfig.Create(_name, startingRule);
    }

    public class ArithmeticRuleReaderConfig
    {
        public static ArithmeticRuleReaderConfig Create(string name, Func<RuleReader, RuleReader> startingRule) => new(name, startingRule);

        private readonly string _name;
        private List<Func<RuleReader, RuleReader>> _ruleGenerators;

        private ArithmeticRuleReaderConfig(string name, Func<RuleReader, RuleReader> startingRule)
        {
            _name = name;
            _ruleGenerators = new List<Func<RuleReader, RuleReader>> {startingRule};
        }
        private ArithmeticRuleReaderConfig(ArithmeticRuleReaderConfig previousConfig, Func<RuleReader, RuleReader> startingRule)
        {
            _ruleGenerators = new List<Func<RuleReader, RuleReader>>(previousConfig._ruleGenerators) {startingRule};
        }

        public ArithmeticRuleReaderConfig Then(Func<RuleReader, RuleReader> nextRule)
        {
            return new ArithmeticRuleReaderConfig(this, nextRule);
        }

        public RuleReader Recurse()
        {
            var bottomReader = RuleHelper.Reader.Named(_name);
            var previousReader = bottomReader;
            
            for (int i = _ruleGenerators.Count - 1; i >= 0; i--)
            {
                previousReader = _ruleGenerators[i](previousReader);
            }
            bottomReader.FillFrom(previousReader);

            return bottomReader;
        }
    }
}