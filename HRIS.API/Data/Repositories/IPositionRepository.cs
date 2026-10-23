using HRIS.API.Entities;
using HRIS.API.Models;

namespace HRIS.API.Data.Repositories;

public interface IPositionRepository : IRepository<Position>
{
    Task<IReadOnlyCollection<PositionResponse>> GetResponseAsync(CancellationToken ct = default);
}
