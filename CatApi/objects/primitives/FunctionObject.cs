using CatApi.interpreting;
using CatApi.types;

namespace CatApi.objects.primitives
{
    public class FunctionObject : CallableDataObject
    {
        private readonly IInterpreter<IDataObject> _interpeter;

        public FunctionObject(IInterpreter<IDataObject> interpeter, TypeBox typeBox) : base(typeBox)
        {
            _interpeter = interpeter;
        }

        public override IDataObject Call(IDataObject[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}