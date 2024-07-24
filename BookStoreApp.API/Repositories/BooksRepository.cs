using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Book;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories
{
    public class BooksRepository : GenericRepository<Book> , IBooksRepository
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly IMapper _mapper;

        public BooksRepository(BookStoreDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<BookReadOnlyDto>> GetAllReadOnlyAsync()
        {
            return await _dbContext.Books.Include(b => b.Author).ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<BookDetailsDto> GetDetailsAsync(int id)
        {
            return await _dbContext.Books.Include(b => b.Author).ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(b => b.Id == id);
        }
    }
}
