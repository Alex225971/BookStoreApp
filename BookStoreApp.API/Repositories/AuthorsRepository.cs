using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories
{
    public class AuthorsRepository : GenericRepository<Author>, IAuthorsRespository
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly IMapper _mapper;

        public AuthorsRepository(BookStoreDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<AuthorDetailsDto> GetAuthorDetailsAsync(int id)
        {
            var author = await _dbContext.Authors
                    .Include(a => a.Books)
                    .ProjectTo<AuthorDetailsDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(a => a.Id == id);
            return author;
        }
    }
}
