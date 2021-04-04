using Jarvis.Domain.Model;
using Microsoft.Bot.Builder;
using System.Threading;
using System.Threading.Tasks;

namespace Jarvis.Domain.Interfaces
{
    public interface ILuisService
    {
        Task<LuisResult> GetLuisResult(ITurnContext turnContext, CancellationToken cancellationToken);
    }
}
