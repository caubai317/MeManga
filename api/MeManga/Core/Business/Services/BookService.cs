using MeManga.Core.Business.Models;
using MeManga.Core.Business.Models.Base;
using MeManga.Core.Business.Models.Books;
using MeManga.Core.Common.Constants;
using MeManga.Core.Common.Reflections;
using MeManga.Core.DataAccess.Repositories.Base;
using MeManga.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookViewModel>> GetAllUserAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel);

        Task<PagedList<BookViewModel>> ListBookAsync(BookRequestListViewModel bookRequestListViewModel);

        //Task<ResponseModel> AddBookAsync(BookManageModel bookManageModel);

        //Task<ResponseModel> UpdateProfileAsync(Guid id, UserUpdateProfileModel userUpdateProfileModel);

        //Task<ResponseModel> DeleteUserAsync(Guid id);

        //Task<Book> GetByWriterAsync(Guid? writerId);

        //Task<Book> GetByIdAsync(Guid? id);
    }
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly ILogger _logger;
        private readonly IOptions<AppSettings> _appSetting;

        public BookService(IRepository<Book> bookRepository, ILogger<BookService> logger,
            IOptions<AppSettings> appSetting)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _appSetting = appSetting;
        }

        private IQueryable<Book> GetAll()
        {
            return _bookRepository.GetAll()
                        .Include(x => x.BookInWriters)
                            .ThenInclude(book => book.Writer)
                    .Where(x => !x.RecordDeleted);
        }

        private List<string> GetAllPropertyNameOfUserViewModel()
        {
            var bookViewModel = new BookViewModel();
            var type = bookViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        public async Task<IEnumerable<BookViewModel>> GetAllUserAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var list = await GetAll()
               .Where(x => (string.IsNullOrEmpty(baseRequestGetAllViewModel.Query)
                   || (x.Name.Contains(baseRequestGetAllViewModel.Query))
               ))
               .OrderBy(x => x.Name)
               .Select(x => new BookViewModel(x))
               .ToListAsync();

            return list;
        }

        public async Task<PagedList<BookViewModel>> ListBookAsync(BookRequestListViewModel bookRequestListViewModel)
        {
            var list = await GetAll()
            .Where(x => (!bookRequestListViewModel.IsActive.HasValue || x.RecordActive == bookRequestListViewModel.IsActive)
                && (string.IsNullOrEmpty(bookRequestListViewModel.Query)
                    || (x.Name.Contains(bookRequestListViewModel.Query)
                    )))
                .Select(x => new BookViewModel(x)).ToListAsync();

            var bookViewModelProperties = GetAllPropertyNameOfUserViewModel();
            var requestPropertyName = !string.IsNullOrEmpty(bookRequestListViewModel.SortName) ? bookRequestListViewModel.SortName.ToLower() : string.Empty;
            string matchedPropertyName = bookViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Name";
            }

            var type = typeof(BookViewModel);
            var sortProperty = type.GetProperty(matchedPropertyName);

            list = bookRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<BookViewModel>(list, bookRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, bookRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
        }
    }
}
