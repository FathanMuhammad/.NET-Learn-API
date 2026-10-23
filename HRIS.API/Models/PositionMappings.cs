using HRIS.API.Entities;

namespace HRIS.API.Models;

public static class PositionMappings
{
    public static PositionResponse ToResponse(this Position position) =>
        new(
            position.Id,
            position.Title,
            position.Level
        );
}
