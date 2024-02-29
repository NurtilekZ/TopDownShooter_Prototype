using System.Threading.Tasks;

namespace Infrastructure.Factories.Interfaces
{
    public interface IFactory
    {
        Task WarmUp();
        void CleanUp();
    }
}