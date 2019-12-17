using MeManga.Core.Business.Models.Base;
using MeManga.Core.Business.Models.Chapters;
using MeManga.Core.Business.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Controllers
{
    [Route("api/chapters")]
    [EnableCors("CorsPolicy")]
    public class ChapterController : BaseController
    {
        private readonly IChapterService _chapterService;

        public ChapterController(IChapterService chapterService)
        {
            _chapterService = chapterService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(ChapterRequestListViewModel chapterRequestListViewModel)
        {
            var chapters = await _chapterService.ListChapterAsync(chapterRequestListViewModel);
            return Ok(chapters);
        }

        [HttpGet("all-chapters")]
        public async Task<IActionResult> GetAllChapter(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var chapters = await _chapterService.GetAllChapterAsync(baseRequestGetAllViewModel);
            return Ok(chapters);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChapterById(Guid id)
        {
            var responseModel = await _chapterService.GetChapterByIdAsync(id);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return NotFound(new { message = responseModel.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChapterManageModel chapterManageModel)
        {
            var responseModel = await _chapterService.CreateChapterAsync(chapterManageModel);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ChapterManageModel chapterViewModel)
        {
            var responseModel = await _chapterService.UpdateChapterAsync(id, chapterViewModel);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var responseModel = await _chapterService.DeleteChapterAsync(id);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }
    }
}
