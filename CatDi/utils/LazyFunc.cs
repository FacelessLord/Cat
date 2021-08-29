using System;

namespace CatDi.utils
{
    public class LazyFunc<T, R>
    {
        private readonly Func<T, R> _generator;
        private bool _initialized = false;
        private R _value;

        public LazyFunc(Func<T, R> generator)
        {
            _generator = generator;
        }

        public R GetValue(T t)
        {
            if (_initialized)
            {
                return _value;
            }

            _value = _generator(t);
            _initialized = true;
            return _value;
        }
    }

    public class LazyFunc<T1, T2, R>
    {
        private readonly Func<T1, T2, R> _generator;
        private bool _initialized = false;
        private R _value;

        public LazyFunc(Func<T1, T2, R> generator)
        {
            _generator = generator;
        }

        public R GetValue(T1 t1, T2 t2)
        {
            if (_initialized)
            {
                return _value;
            }

            _value = _generator(t1, t2);
            _initialized = true;
            return _value;
        }
    }
}