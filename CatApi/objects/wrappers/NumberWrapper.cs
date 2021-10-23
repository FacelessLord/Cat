using CatApi.exceptions;

namespace CatApi.objects.wrappers
{
    public class NumberWrapper
    {
        public decimal Value { get; }

        public NumberWrapper(decimal number)
        {
            Value = number;
        }

        public NumberWrapper(double number)
        {
            Value = (decimal) number;
        }

        public NumberWrapper(float number)
        {
            Value = (decimal) number;
        }

        public NumberWrapper(long number)
        {
            Value = number;
        }

        public NumberWrapper(int number)
        {
            Value = number;
        }

        public NumberWrapper(short number)
        {
            Value = number;
        }

        public NumberWrapper(byte number)
        {
            Value = number;
        }

        public static NumberWrapper FromObject(object src)
        {
            return src switch
            {
                byte b => new NumberWrapper(b),
                short s => new NumberWrapper(s),
                int i => new NumberWrapper(i),
                long l => new NumberWrapper(l),
                float f => new NumberWrapper(f),
                double d => new NumberWrapper(d),
                decimal d => new NumberWrapper(d),
                _ => throw new CatNumberParsingException(src)
            };
        }
    }
}