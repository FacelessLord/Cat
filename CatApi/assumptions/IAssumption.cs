using System;

namespace CatApi.assumptions
{
    public interface IAssumption : IDisposable
    {
        public void GotToBeWrong();
    }
}