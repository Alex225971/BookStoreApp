using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models;
using Microsoft.EntityFrameworkCore;
using YamlDotNet.Core.Tokens;

namespace BookStoreApp.API.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly BookStoreDbContext _dbContext;
        private readonly IMapper _mapper;

        public GenericRepository(BookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public BookStoreDbContext DbContext { get; }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<VirtualizeResponse<TResult>> GetAllAsync<TResult>(QueryParams queryParams) where TResult : class
        {
            var totalSize = await _dbContext.Set<T>().CountAsync();
            var items = await _dbContext.Set<T>()
                .Skip(queryParams.StartIndex)
                .Take(queryParams.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new VirtualizeResponse<TResult> { Results = items, TotalSize = totalSize };
        }

        public async Task<T> GetAsync(int? id)
        {
            if(id == null)
            {
                return null;
            }
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
