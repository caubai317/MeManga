using MeManga.Core.Business.Models;
using MeManga.Core.Business.Models.Base;
using MeManga.Core.Business.Models.FilePaths;
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
    public interface IFilePathService
    {
        Task<IEnumerable<FilePathViewModel>> GetAllFilePathAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel);

        Task<PagedList<FilePathViewModel>> ListFilePathAsync(FilePathRequestListViewModel filePathRequestListViewModel);

        Task<ResponseModel> GetFilePathByIdAsync(Guid? id);

        Task<ResponseModel> CreateFilePathAsync(FilePathManageModel filePathManageModel);

        Task<ResponseModel> UpdateFilePathAsync(Guid id, FilePathManageModel filePathManageModel);

        Task<ResponseModel> DeleteFilePathAsync(Guid id);
    }
    public class FilePathService : IFilePathService
    {
        private readonly IRepository<FilePath> _filePathRepository;
        private readonly ILogger _logger;
        private readonly IOptions<AppSettings> _appSetting;

        public FilePathService(IRepository<FilePath> filePathRepository, ILogger<FilePathService> logger,
            IOptions<AppSettings> appSetting)
        {
            _filePathRepository = filePathRepository;
            _logger = logger;
            _appSetting = appSetting;
        }

        private IQueryable<FilePath> GetAll()
        {
            return _filePathRepository.GetAll()
                        .Include(x => x.Chapter);
        }

        private List<string> GetAllPropertyNameOfUserViewModel()
        {
            var filePathViewModel = new FilePathViewModel();
            var type = filePathViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        public async Task<IEnumerable<FilePathViewModel>> GetAllFilePathAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var list = await GetAll()
               //.Where(x => (string.IsNullOrEmpty(baseRequestGetAllViewModel.Query)
               //    || (x.Name.Contains(baseRequestGetAllViewModel.Query))
               //))
               .OrderBy(x => x.PageNumber)
               .Select(x => new FilePathViewModel(x))
               .ToListAsync();

            return list;
        }

        public async Task<PagedList<FilePathViewModel>> ListFilePathAsync(FilePathRequestListViewModel filePathRequestListViewModel)
        {
            var list = await GetAll()
            //.Where(x => (!filePathRequestListViewModel.IsActive.HasValue || x.RecordActive == filePathRequestListViewModel.IsActive)
            //    && (string.IsNullOrEmpty(filePathRequestListViewModel.Query)
            //        || (x.Name.Contains(filePathRequestListViewModel.Query)
            //        )))
                .Select(x => new FilePathViewModel(x)).ToListAsync();

            var filePathViewModelProperties = GetAllPropertyNameOfUserViewModel();
            var requestPropertyName = !string.IsNullOrEmpty(filePathRequestListViewModel.SortName) ? filePathRequestListViewModel.SortName.ToLower() : string.Empty;
            string matchedPropertyName = filePathViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Name";
            }

            var type = typeof(FilePathViewModel);
            var sortProperty = type.GetProperty(matchedPropertyName);

            list = filePathRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<FilePathViewModel>(list, filePathRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, filePathRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        public async Task<ResponseModel> GetFilePathByIdAsync(Guid? id)
        {
            var filePath = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (filePath != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new FilePathViewModel(filePath),
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

        public async Task<ResponseModel> CreateFilePathAsync(FilePathManageModel filePathManageModel)
        {
            var filePath = await _filePathRepository.FetchFirstAsync(x => x.ChapterId == filePathManageModel.ChapterId && x.PageNumber == filePathManageModel.PageNumber);
            if (filePath != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = MessageConstants.EXISTED_CREATED
                };
            }
            else
            {
                // Create FilePath
                filePath = AutoMapper.Mapper.Map<FilePath>(filePathManageModel);

                await _filePathRepository.InsertAsync(filePath);

                filePath = await GetAll().FirstOrDefaultAsync(x => x.Id == filePath.Id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new FilePathViewModel(filePath),
                    Message = MessageConstants.CREATED_SUCCESSFULLY
                };
            }
        }

        public async Task<ResponseModel> UpdateFilePathAsync(Guid id, FilePathManageModel filePathManageModel)
        {
            var filePath = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (filePath == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                var exitstedFilePath = await _filePathRepository.FetchFirstAsync(x => x.ChapterId == filePathManageModel.ChapterId 
                                                                                    && x.PageNumber == filePathManageModel.PageNumber 
                                                                                    && x.Id != id);
                if (exitstedFilePath != null)
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = MessageConstants.EXISTED_CREATED
                    };
                }
                else
                {
                    filePathManageModel.SetDataToModel(filePath);
                    await _filePathRepository.UpdateAsync(filePath);

                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Data = new FilePathViewModel(filePath),
                        Message = MessageConstants.UPDATED_SUCCESSFULLY
                    };
                }

            }
        }

        public async Task<ResponseModel> DeleteFilePathAsync(Guid id)
        {
            var question = await _filePathRepository.GetByIdAsync(id);

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
                await _filePathRepository.DeleteAsync(id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
        }
    }
}
