using System.Threading;
using System.Threading.Tasks;

namespace _current.Services
{
    public interface IInitializableAsync
    {
        Task InitializeAsync(CancellationToken cancellationToken = default);
    }
}