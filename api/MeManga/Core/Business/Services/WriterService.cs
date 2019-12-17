using MeManga.Core.Business.Models;
using MeManga.Core.Business.Models.Base;
using MeManga.Core.Business.Models.Writers;
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
    public interface IWriterService
    {
        Task<IEnumerable<WriterViewModel>> GetAllWriterAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel);

        Task<PagedList<WriterViewModel>> ListWriterAsync(WriterRequestListViewModel writerRequestListViewModel);

        Task<ResponseModel> GetWriterByIdAsync(Guid? id);

        Task<ResponseModel> CreateWriterAsync(WriterManageModel writerManageModel);

        Task<ResponseModel> UpdateWriterAsync(Guid id, WriterManageModel writerManageModel);

        Task<ResponseModel> DeleteWriterAsync(Guid id);
    }
    public class WriterService : IWriterService
    {
        private readonly IRepository<Writer> _writerRepository;
        private readonly ILogger _logger;
        private readonly IOptions<AppSettings> _appSetting;

        public WriterService(IRepository<Writer> writerRepository, ILogger<WriterService> logger,
            IOptions<AppSettings> appSetting)
        {
            _writerRepository = writerRepository;
            _logger = logger;
            _appSetting = appSetting;
        }

        private IQueryable<Writer> GetAll()
        {
            return _writerRepository.GetAll()
                        .Include(x => x.BookInWriters)
                            .ThenInclude( writer => writer.Book)
                    .Where(x => !x.RecordDeleted);
        }

        private List<string> GetAllPropertyNameOfUserViewModel()
        {
            var writerViewModel = new WriterViewModel();
            var type = writerViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        public async Task<IEnumerable<WriterViewModel>> GetAllWriterAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var list = await GetAll()
               .Where(x => (string.IsNullOrEmpty(baseRequestGetAllViewModel.Query)
                   || (x.Name.Contains(baseRequestGetAllViewModel.Query))
               ))
               .OrderBy(x => x.Name)
               .Select(x => new WriterViewModel(x))
               .ToListAsync();

            return list;
        }

        public async Task<PagedList<WriterViewModel>> ListWriterAsync(WriterRequestListViewModel writerRequestListViewModel)
        {
            var list = await GetAll()
            .Where(x => (!writerRequestListViewModel.IsActive.HasValue || x.RecordActive == writerRequestListViewModel.IsActive)
                && (string.IsNullOrEmpty(writerRequestListViewModel.Query)
                    || (x.Name.Contains(writerRequestListViewModel.Query)
                    )))
                .Select(x => new WriterViewModel(x)).ToListAsync();

            var writerViewModelProperties = GetAllPropertyNameOfUserViewModel();
            var requestPropertyName = !string.IsNullOrEmpty(writerRequestListViewModel.SortName) ? writerRequestListViewModel.SortName.ToLower() : string.Empty;
            string matchedPropertyName = writerViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Name";
            }

            var type = typeof(WriterViewModel);
            var sortProperty = type.GetProperty(matchedPropertyName);

            list = writerRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<WriterViewModel>(list, writerRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, writerRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        public async Task<ResponseModel> GetWriterByIdAsync(Guid? id)
        {
            var writer = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (writer != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new WriterViewModel(writer),
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

        public async Task<ResponseModel> CreateWriterAsync(WriterManageModel writerManageModel)
        {
            var writer = await _writerRepository.FetchFirstAsync(x => x.Name == writerManageModel.Name);
            if (writer != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = MessageConstants.EXISTED_CREATED
                };
            }
            else
            {
                // Create Writer
                writer = AutoMapper.Mapper.Map<Writer>(writerManageModel);

                await _writerRepository.InsertAsync(writer);

                writer = await GetAll().FirstOrDefaultAsync(x => x.Id == writer.Id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new WriterViewModel(writer),
                    Message = MessageConstants.CREATED_SUCCESSFULLY
                };
            }
        }

        public async Task<ResponseModel> UpdateWriterAsync(Guid id, WriterManageModel writerManageModel)
        {
            var writer = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (writer == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                var exitstedWriter = await _writerRepository.FetchFirstAsync(x => x.Name == writerManageModel.Name && x.Id != id);
                if (exitstedWriter != null)
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = MessageConstants.EXISTED_CREATED
                    };
                }
                else
                {
                    writerManageModel.SetDataToModel(writer);
                    await _writerRepository.UpdateAsync(writer);

                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Data = new WriterViewModel(writer),
                        Message = MessageConstants.UPDATED_SUCCESSFULLY
                    };
                }

            }
        }

        public async Task<ResponseModel> DeleteWriterAsync(Guid id)
        {
            var question = await _writerRepository.GetByIdAsync(id);

            if (question == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
            else
            {
                await _writerRepository.DeleteAsync(id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
        }
    }
}
