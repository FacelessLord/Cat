﻿using CatData.types;
using CatDi.di;
using CatLexing;

namespace Cat.di
{
    public static class Di
    {
        private static Kernel _instance;

        public static T Resolve<T>()
        {
            return _instance.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return _instance.Resolve<T>(name);
        }
        
        public static void Configure()
        {
            _instance = new Kernel();

            _instance.Register<TypeStorage>().AsSingleton<ITypeStorage>();
            _instance.Register<Lexer>().AsSingleton<ILexer>();
            // _instance.Register<AstBuilder>().AsSingleton<IParser>();

        }
    }
}