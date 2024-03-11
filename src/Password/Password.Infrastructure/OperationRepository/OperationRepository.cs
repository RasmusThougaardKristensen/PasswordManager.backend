﻿using PasswordManager.Password.Infrastructure.PasswordRepository;
using PasswordManager.Password.Infrastructure.BaseRepository;

namespace PasswordManager.Password.Infrastructure.OperationRepository;
public class OperationRepository : BaseRepository<Operation, OperationEntity>, IOperationRepository
{
    public OperationRepository(PasswordContext context) : base(context)
    {
    }

    public async Task<Operation?> GetByRequestId(string requestId)
    {
        var operationEntity = await GetOperationsDbSet()
            .Where(x => x.RequestId == requestId)
            .AsNoTracking()
            .SingleOrDefaultAsync();
        return operationEntity is null ? null : Map(operationEntity);
    }

    public async Task<ICollection<Operation>> GetPasswordOperations(Guid passwordId)
    {
        var password = await GetOperationsDbSet()
                    .Where(x => x.PasswordId == passwordId)
                    .AsNoTracking()
                    .ToListAsync();
        return password.Select(Map).ToImmutableHashSet();
    }

    private DbSet<OperationEntity> GetOperationsDbSet()
    {
        if (Context.Operations is null)
            throw new InvalidOperationException("Database configuration not setup correctly");
        return Context.Operations;
    }

    protected override Operation Map(OperationEntity entity)
    {
        return OperationMapper.Map(entity);
    }

    protected override OperationEntity Map(Operation model)
    {
        return OperationMapper.Map(model);
    }

  
}
