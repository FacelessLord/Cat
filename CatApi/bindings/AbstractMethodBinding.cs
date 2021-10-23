using System;
using CatApi.objects;
using CatApi.types;

namespace CatApi.bindings
{
    public abstract class AbstractMethodBinding
    {
        public AbstractMethodBinding(string name, TypeBox type)
        {
            this.Name = name;
            this.Type = type;
        }

        public abstract IDataObject Call(IDataObject[] args);
        public string Name { get; init; }
        public TypeBox Type { get; init; }

        public void Deconstruct(out string name, out TypeBox type)
        {
            name = this.Name;
            type = this.Type;
        }
    }

    public class MethodBinding : AbstractMethodBinding
    {
        private readonly Func<IDataObject[], IDataObject> _body;

        public MethodBinding(string name, TypeBox type, Func<IDataObject[], IDataObject> body) : base(name, type)
        {
            _body = body;
        }

        public override IDataObject Call(IDataObject[] args)
        {
            return _body(args);
        }
    }
}