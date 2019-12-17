using MeManga.Core.Business.Models;
using MeManga.Core.Business.Models.Base;
using MeManga.Core.Business.Models.Chapters;
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
    public interface IChapterService
    {
        Task<IEnumerable<ChapterViewModel>> GetAllChapterAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel);

        Task<PagedList<ChapterViewModel>> ListChapterAsync(ChapterRequestListViewModel chapterRequestListViewModel);

        Task<ResponseModel> GetChapterByIdAsync(Guid? id);

        Task<ResponseModel> CreateChapterAsync(ChapterManageModel chapterManageModel);

        Task<ResponseModel> UpdateChapterAsync(Guid id, ChapterManageModel chapterManageModel);

        Task<ResponseModel> DeleteChapterAsync(Guid id);
    }
    public class ChapterService : IChapterService
    {
        private readonly IRepository<Chapter> _chapterRepository;
        private readonly ILogger _logger;
        private readonly IOptions<AppSettings> _appSetting;

        public ChapterService(IRepository<Chapter> chapterRepository, ILogger<ChapterService> logger,
            IOptions<AppSettings> appSetting)
        {
            _chapterRepository = chapterRepository;
            _logger = logger;
            _appSetting = appSetting;
        }

        private IQueryable<Chapter> GetAll()
        {
            return _chapterRepository.GetAll()
                        .Include(x => x.Book)
                    .Where(x => !x.RecordDeleted);
        }

        private List<string> GetAllPropertyNameOfUserViewModel()
        {
            var chapterViewModel = new ChapterViewModel();
            var type = chapterViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        public async Task<IEnumerable<ChapterViewModel>> GetAllChapterAsync(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var list = await GetAll()
               .Where(x => (string.IsNullOrEmpty(baseRequestGetAllViewModel.Query)
                   || (x.Name.Contains(baseRequestGetAllViewModel.Query))
               ))
               .OrderBy(x => x.Name)
               .Select(x => new ChapterViewModel(x))
               .ToListAsync();

            return list;
        }

        public async Task<PagedList<ChapterViewModel>> ListChapterAsync(ChapterRequestListViewModel chapterRequestListViewModel)
        {
            var list = await GetAll()
            .Where(x => (!chapterRequestListViewModel.IsActive.HasValue || x.RecordActive == chapterRequestListViewModel.IsActive)
                && (string.IsNullOrEmpty(chapterRequestListViewModel.Query)
                    || (x.Name.Contains(chapterRequestListViewModel.Query)
                    )))
                .Select(x => new ChapterViewModel(x)).ToListAsync();

            var chapterViewModelProperties = GetAllPropertyNameOfUserViewModel();
            var requestPropertyName = !string.IsNullOrEmpty(chapterRequestListViewModel.SortName) ? chapterRequestListViewModel.SortName.ToLower() : string.Empty;
            string matchedPropertyName = chapterViewModelProperties.FirstOrDefault(x => x.ToLower() == requestPropertyName);

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Name";
            }

            var type = typeof(ChapterViewModel);
            var sortProperty = type.GetProperty(matchedPropertyName);

            list = chapterRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<ChapterViewModel>(list, chapterRequestListViewModel.Skip ?? CommonConstants.Config.DEFAULT_SKIP, chapterRequestListViewModel.Take ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        public async Task<ResponseModel> GetChapterByIdAsync(Guid? id)
        {
            var chapter = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (chapter != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new ChapterViewModel(chapter),
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

        public async Task<ResponseModel> CreateChapterAsync(ChapterManageModel chapterManageModel)
        {
            var chapter = await _chapterRepository.FetchFirstAsync(x => x.Name == chapterManageModel.Name);
            if (chapter != null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = MessageConstants.EXISTED_CREATED
                };
            }
            else
            {
                // Create Chapter
                chapter = AutoMapper.Mapper.Map<Chapter>(chapterManageModel);

                await _chapterRepository.InsertAsync(chapter);

                chapter = await GetAll().FirstOrDefaultAsync(x => x.Id == chapter.Id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Data = new ChapterViewModel(chapter),
                    Message = MessageConstants.CREATED_SUCCESSFULLY
                };
            }
        }

        public async Task<ResponseModel> UpdateChapterAsync(Guid id, ChapterManageModel chapterManageModel)
        {
            var chapter = await GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (chapter == null)
            {
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = MessageConstants.NOT_FOUND
                };
            }
            else
            {
                var exitstedChapter = await _chapterRepository.FetchFirstAsync(x => x.Name == chapterManageModel.Name && x.Id != id);
                if (exitstedChapter != null)
                {
                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = MessageConstants.EXISTED_CREATED
                    };
                }
                else
                {
                    chapterManageModel.SetDataToModel(chapter);
                    await _chapterRepository.UpdateAsync(chapter);

                    return new ResponseModel()
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Data = new ChapterViewModel(chapter),
                        Message = MessageConstants.UPDATED_SUCCESSFULLY
                    };
                }

            }
        }

        public async Task<ResponseModel> DeleteChapterAsync(Guid id)
        {
            var question = await _chapterRepository.GetByIdAsync(id);

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
                await _chapterRepository.DeleteAsync(id);
                return new ResponseModel()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = MessageConstants.DELETED_SUCCESSFULLY
                };
            }
        }
    }
}
