using System;
using System.Collections;
using System.Collections.Generic;

namespace Cat.ast.api
{
    public class BufferedEnumerable<T> : IEnumerable<T>
    {
        private List<T> _buffer = new();
        private int _pointer = 0;
        private Stack<int> pointerStack = new();
        private IEnumerator<T> _wrapped;
        private bool _enumerableEnded;

        public BufferedEnumerable(IEnumerable<T> wrapped)
        {
            _wrapped = wrapped.GetEnumerator();
        }

        public void ResetPointer()
        {
            _pointer = pointerStack.Count > 0 ? pointerStack.Peek() : 0;
            _enumerableEnded = _buffer.Count == 0;
        }

        public int PushPointer()
        {
            pointerStack.Push(_pointer);
            return pointerStack.Count;
        }

        public void PopPointer(int depth)
        {
            while (pointerStack.Count >= depth)
                pointerStack.Pop();
        }

        private (T, bool) Next()
        {
            if (_pointer < _buffer.Count)
            {
                return (_buffer[_pointer++], _enumerableEnded && _pointer == _buffer.Count);
            }

            _enumerableEnded = !_wrapped.MoveNext();
            if (_enumerableEnded)
                return (default, _enumerableEnded);

            var elem = _wrapped.Current;
            _pointer++;
            _buffer.Add(elem);
            return (elem, _enumerableEnded);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new BufferedEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        class BufferedEnumerator<T> : IEnumerator<T>
        {
            private readonly BufferedEnumerable<T> _bufferedEnumerable;

            public BufferedEnumerator(BufferedEnumerable<T> bufferedEnumerable)
            {
                _bufferedEnumerable = bufferedEnumerable;
            }

            public bool MoveNext()
            {
                var (elem, enumerationEnded) = _bufferedEnumerable.Next();
                if (!enumerationEnded)
                    Current = elem;
                return !enumerationEnded;
            }

            public void Reset()
            {
                _bufferedEnumerable.ResetPointer();
            }

            public T Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}