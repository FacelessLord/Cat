using System;
using Cat.lexing;

namespace Cat
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexer = new Lexer();

            var tokens = lexer.ParseCode("24124\"214421\"124\'saf214fas\'");
            var a = 0;
        }
    }
}