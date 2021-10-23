using CatApi.objects;
using CatApi.objects.primitives;

namespace CatApi.bindings
{
    public class BoundMethod : ObjectMethod
    {
        private readonly AbstractMethodBinding _binding;

        public BoundMethod(AbstractMethodBinding binding) : base(binding.Type, binding.Name)
        {
            _binding = binding;
        }

        public override IDataObject Call(IDataObject[] args)
        {
            return _binding.Call(args);
        }
    }
}