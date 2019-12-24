using MeManga.Core.Business.Models;
using MeManga.Core.Business.Models.Base;
using MeManga.Core.Business.Models.TypeBooks;
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
    public interface ITypeBookService
    {
        Task<IEnumerable<TypeBookViewModel>> GetAllTypeBookAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel);

        Task<PagedList<TypeBookViewModel>> ListTypeBookAsync(TypeBookRequestListViewModel typeBookRequestListViewModel);

        Task<ResponseModel> GetTypeBookByIdAsync(Guid? id);

        Task<ResponseModel> CreateTypeBookAsync(TypeBookManageModel typeBookManageModel);

        Task<ResponseModel> UpdateTypeBookAsync(Guid id, TypeBookManageModel typeBookManageModel);

        Task<ResponseModel> DeleteTypeBookAsync(Guid id);
    }
    public class TypeBookService : ITypeBookService
    {
        private readonly IRepository<TypeBook> _typeBookRepository;
        //private readonly IRepository<Writer> _writerRepository;
        private readonly ILogger _logger;
        private readonly IOptions<AppSettings> _appSetting;

        public TypeBookService(IRepository<TypeBook> typeBookRepository, ILogger<TypeBookService> logger, IOptions<AppSettings> appSetting)
        {
            _typeBookRepository = typeBookRepository;
            _logger = logger;
            _appSetting = appSetting;
        }

        private IQueryable<TypeBook> GetAll()
        {
            return _typeBookRepository.GetAll()
                        .Include(x => x.BookInTypes)
                            .ThenInclude(book => book.Book)
                    .Where(x => !x.RecordDeleted);
        }

        private List<string> GetAllPropertyNameOfUserViewModel()
        {
            var typeBookViewModel = new TypeBookViewModel();
            var type = typeBookViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        public async Task<IEnumerable<TypeBookViewModel>> GetAllTypeBookAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var list = await GetAll()
               .Where(x => (string.IsNullOrEmpty(baseRequestGetAllViewModel.Query)
                   || (x.Name.Contains(baseRequestGetAllViewModel.Query))
               ))
               .OrderBy(x => x.Name)
               .Select(x => new TypeBookViewModel(x))
               .ToListAsync();

            return list;
        }

        public async Task<PagedList<TypeBookViewModel>> ListTypeBookAsync(TypeBookRequestListViewModel typeBookRequestListViewModel)
        {
            var list = await GetAll()
            .Where(x => (!typeBookRequestListViewModel.IsActive.HasValue || x.RecordActive == typeBookRequestListViewModel.IsActive)
                && (string.IsNullOrEmpty(typeBookRequestListViewModel.Query)
                    || (x.Name.Contains(typeBookRequestListViewModel.Query)
                    )))
                .Select(x => new TypeBookViewModel(x)).ToListAsync();

            var typeBookViewModelProperties = GetAllPropertyNameOfUserViewModel();
            var requestPropertyName = !string.IsNullOrEmpty(typeBookRequestListViewModel.SortName) ? typeBookRequestListViewModel.SortName.ToLower() : string.Empty;
            string matchedPropertyName = typeBookViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Name";
            }

            var type = typeof(TypeBookViewModel);
            var sortProperty = type.GetProperty(matchedPropertyName);

            list = typeBookRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<TypeBookViewModel>(list, typeBookRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, typeBookRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        public async Task<ResponseModel> GetTypeBookByIdAsync(Guid? id)
        {
            var typeBook = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (typeBook != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new TypeBookViewByIdModel(typeBook),
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

        public async Task<ResponseModel> CreateTypeBookAsync(TypeBookManageModel typeBookManageModel)
        {
            var typeBook = await _typeBookRepository.FetchFirstAsync(x => x.Name == typeBookManageModel.Name);
            if (typeBook != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = MessageConstants.EXISTED_CREATED
                };
            }
            else
            {
                var writer = await _typeBookRepository.FetchFirstAsync(x => x.Name == typeBookManageModel.Name);

                // Create TypeBook
                typeBook = AutoMapper.Mapper.Map<TypeBook>(typeBookManageModel);

                await _typeBookRepository.InsertAsync(typeBook);

                typeBook = await GetAll().FirstOrDefaultAsync(x => x.Id == typeBook.Id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new TypeBookViewModel(typeBook),
                    Message = MessageConstants.CREATED_SUCCESSFULLY
                };
            }
        }

        public async Task<ResponseModel> UpdateTypeBookAsync(Guid id, TypeBookManageModel typeBookManageModel)
        {
            var typeBook = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (typeBook == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                var exitstedTypeBook = await _typeBookRepository.FetchFirstAsync(x => x.Name == typeBookManageModel.Name && x.Id != id);
                if (exitstedTypeBook != null)
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = MessageConstants.EXISTED_CREATED
                    };
                }
                else
                {
                    typeBookManageModel.SetDataToModel(typeBook);
                    await _typeBookRepository.UpdateAsync(typeBook);

                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Data = new TypeBookViewModel(typeBook),
                        Message = MessageConstants.UPDATED_SUCCESSFULLY
                    };
                }

            }
        }

        public async Task<ResponseModel> DeleteTypeBookAsync(Guid id)
        {
            var typeBook = await _typeBookRepository.GetByIdAsync(id);

            if (typeBook == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
            else
            {
                await _typeBookRepository.DeleteAsync(id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
        }
    }
}
