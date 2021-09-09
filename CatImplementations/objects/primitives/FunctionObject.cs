using CatApi.interpreting;
using CatApi.types;

namespace CatImplementations.objects.primitives
{
    public class FunctionObject : CallableDataObject
    {
        private readonly IInterpreter<IDataObject> _interpeter;

        public FunctionObject(IInterpreter<IDataObject> interpeter, ,IDataType type) : base(type)
        {
            _interpeter = interpeter;
        }

        public override IDataObject Call(IDataObject[] args)
        {
            throw new System.NotImplementedException();
        }
    }
}