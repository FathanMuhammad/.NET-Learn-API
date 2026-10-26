using HRIS.API.Entities;
using HRIS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Data.Repositories;

public class PositionRepository(HrisDbContext context) : Repository<Position>(context), IPositionRepository;
