using System.Threading.Tasks;

namespace _current.Infrastructure.Factories.Interfaces
{
    public interface IFactory
    {
        Task WarmUp();
        void CleanUp();
    }
}