using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public class RandomService : IRandomService
    {
        private readonly int _seed;
        private readonly TestDbContext _ctx;

        public RandomService(TestDbContext ctx)
        {
            _seed = Guid.NewGuid().GetHashCode();
            _ctx = ctx;
        }

        public async Task<int> GetRandom()
        {
            int number = new Random(_seed).Next(100);
            var entity = new RandomNumber { Number = number };
            _ctx.Numbers.Add(entity);

            int tentativas = 0;

            while (tentativas < 100) // no máximo 100 tentativas
            {
                try
                {
                    await _ctx.SaveChangesAsync();
                    return entity.Number; // sucesso
                }
                catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
                {
                    entity.Number++;
                    if (entity.Number > 99)
                        entity.Number = 0;

                    tentativas++;
                }
            }

            // todos os números já existem no banco
            return -1;
        }

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx)
            {
                return sqlEx.Number == 2627 || sqlEx.Number == 2601;
            }
            return false;
        }

    }
}
