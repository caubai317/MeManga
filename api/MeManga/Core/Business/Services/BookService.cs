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
        Task<IEnumerable<BookViewModel>> GetAllBookAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel);

        Task<PagedList<BookViewModel>> ListBookAsync(BookRequestListViewModel bookRequestListViewModel);

        Task<ResponseModel> GetBookByIdAsync(Guid? id);

        Task<ResponseModel> CreateBookAsync(BookManageModel bookManageModel);

        Task<ResponseModel> UpdateBookAsync(Guid id, BookManageModel bookManageModel);

        Task<ResponseModel> UpdateBookTypeAsync(Guid id, BookTypeManageModel bookTypeManageModel);

        Task<ResponseModel> UpdateBookWriterAsync(Guid id, BookWriterManageModel bookWriterManageModel);

        Task<ResponseModel> DeleteBookAsync(Guid id);
    }
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BookInWriter> _bookInWriterRepository;
        private readonly IRepository<BookInType> _bookInTypeRepository;
        //private readonly IRepository<Writer> _writerRepository;
        private readonly ILogger _logger;
        private readonly IOptions<AppSettings> _appSetting;

        public BookService(IRepository<Book> bookRepository, IRepository<BookInWriter> bookInWriterRepository, IRepository<BookInType> bookInTypeRepository,
                                    ILogger<BookService> logger, IOptions<AppSettings> appSetting)
        {
            _bookRepository = bookRepository;
            _bookInWriterRepository = bookInWriterRepository;
            _bookInTypeRepository = bookInTypeRepository;
            _logger = logger;
            _appSetting = appSetting;
        }

        private IQueryable<Book> GetAll()
        {
            return _bookRepository.GetAll()
                        .Include(x => x.BookInWriters)
                            .ThenInclude(book => book.Writer)
                        .Include(x => x.Translator)
                        .Include(x => x.BookInTypes)
                            .ThenInclude(book => book.TypeBook)
                        .Include(x => x.Chapters)
                    .Where(x => !x.RecordDeleted);
        }

        private List<string> GetAllPropertyNameOfUserViewModel()
        {
            var bookViewModel = new BookViewModel();
            var type = bookViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        public async Task<IEnumerable<BookViewModel>> GetAllBookAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
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

        public async Task<ResponseModel> GetBookByIdAsync(Guid? id)
        {
            var book = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (book != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new BookViewByIdModel(book),
                };
            }
            else
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
        }

        public async Task<ResponseModel> CreateBookAsync(BookManageModel bookManageModel)
        {
            var book = await _bookRepository.FetchFirstAsync(x => x.Name == bookManageModel.Name);
            if (book != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = MessageConstants.EXISTED_CREATED
                };
            }
            else
            {
                //var writer = await _bookRepository.FetchFirstAsync(x => x.Name == bookManageModel.Name);

                // Create Book
                book = AutoMapper.Mapper.Map<Book>(bookManageModel);

                await _bookRepository.InsertAsync(book);

                foreach (var writerId in bookManageModel.WriterIds)
                {
                    var newBookInWriter = new BookInWriter
                    {
                        WriterId = writerId,
                        BookId = book.Id
                    };
                    await _bookInWriterRepository.InsertAsync(newBookInWriter);
                }

                foreach (var typeId in bookManageModel.TypeIds)
                {
                    var newBookInType = new BookInType
                    {
                        TypeBookId = typeId,
                        BookId = book.Id
                    };
                    await _bookInTypeRepository.InsertAsync(newBookInType);
                }

                book = await GetAll().FirstOrDefaultAsync(x => x.Id == book.Id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new BookViewModel(book),
                    Message = MessageConstants.CREATED_SUCCESSFULLY
                };
            }
        }

        public async Task<ResponseModel> UpdateBookAsync(Guid id, BookManageModel bookManageModel)
        {
            var book = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                var exitstedBook = await _bookRepository.FetchFirstAsync(x => x.Name == bookManageModel.Name && x.Id != id);
                if (exitstedBook != null)
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = MessageConstants.EXISTED_CREATED
                    };
                }
                else
                {
                    //if (bookManageModel.WriterId != book.WriterId)
                    //{
                    //    var bookInWriter = _bookInWriterRepository.GetAll().Where(x => x.WriterId == book.WriterId
                    //                                                                  && x.BookId == book.Id);
                    //    if (bookInWriter != null)
                    //    {
                    //        await _bookInWriterRepository.DeleteAsync(bookInWriter);

                    //        if(bookManageModel.WriterId != null)
                    //        {
                    //            var newBookInWriter = new BookInWriter
                    //            {
                    //                WriterId = bookManageModel.WriterId,
                    //                BookId = book.Id
                    //            };
                    //            await _bookInWriterRepository.InsertAsync(newBookInWriter);
                    //        }
                    //    }
                    //}

                    bookManageModel.SetDataToModel(book);
                    await _bookRepository.UpdateAsync(book);

                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Data = new BookViewModel(book),
                        Message = MessageConstants.UPDATED_SUCCESSFULLY
                    };
                }

            }
        }

        public async Task<ResponseModel> UpdateBookTypeAsync(Guid id, BookTypeManageModel bookTypeManageModel)
        {
            var book = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                var bookInType = _bookInTypeRepository.GetAll().Where(x => x.BookId == book.Id);
                if (bookInType != null)
                {
                    await _bookInTypeRepository.DeleteAsync(bookInType);
                }

                foreach (var typeId in bookTypeManageModel.TypeIds)
                {
                    var newBookInType = new BookInType
                    {
                        TypeBookId = typeId,
                        BookId = book.Id
                    };
                    await _bookInTypeRepository.InsertAsync(newBookInType);
                }

                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new BookViewByIdModel(book),
                    Message = MessageConstants.UPDATED_SUCCESSFULLY
                };
            }
        }

        public async Task<ResponseModel> UpdateBookWriterAsync(Guid id, BookWriterManageModel bookWriterManageModel)
        {
            var book = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                var bookInWriter = _bookInWriterRepository.GetAll().Where(x => x.BookId == book.Id);
                if (bookInWriter != null)
                {
                    await _bookInWriterRepository.DeleteAsync(bookInWriter);
                }

                foreach (var writerId in bookWriterManageModel.WriterIds)
                {
                    var newBookInWriter = new BookInWriter
                    {
                        WriterId = writerId,
                        BookId = book.Id
                    };
                    await _bookInWriterRepository.InsertAsync(newBookInWriter);
                }

                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new BookViewByIdModel(book),
                    Message = MessageConstants.UPDATED_SUCCESSFULLY
                };
            }
        }

        public async Task<ResponseModel> DeleteBookAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
            else
            {
                var bookInWriter = _bookInWriterRepository.GetAll().Where(x => x.BookId == book.Id);
                if (bookInWriter != null)
                {
                    await _bookInWriterRepository.DeleteAsync(bookInWriter);
                }

                await _bookRepository.DeleteAsync(id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
        }
    }
}
