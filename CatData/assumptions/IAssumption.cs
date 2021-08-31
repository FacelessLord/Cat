using System;

namespace CatData.assumptions
{
    public interface IAssumption : IDisposable
    {
        public void GotToBeWrong();
    }
}