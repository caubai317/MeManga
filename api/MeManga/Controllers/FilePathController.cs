using MeManga.Core.Business.Models.Base;
using MeManga.Core.Business.Models.FilePaths;
using MeManga.Core.Business.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Controllers
{
    [Route("api/filepaths")]
    [EnableCors("CorsPolicy")]
    public class FilePathController : BaseController
    {
        private readonly IFilePathService _filePathService;

        public FilePathController(IFilePathService filePathService)
        {
            _filePathService = filePathService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(FilePathRequestListViewModel filePathRequestListViewModel)
        {
            var filePaths = await _filePathService.ListFilePathAsync(filePathRequestListViewModel);
            return Ok(filePaths);
        }

        [HttpGet("all-filePaths")]
        public async Task<IActionResult> GetAllFilePath(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var filePaths = await _filePathService.GetAllFilePathAsync(baseRequestGetAllViewModel);
            return Ok(filePaths);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilePathById(Guid id)
        {
            var responseModel = await _filePathService.GetFilePathByIdAsync(id);
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
        public async Task<IActionResult> Post([FromBody] FilePathManageModel filePathManageModel)
        {
            var responseModel = await _filePathService.CreateFilePathAsync(filePathManageModel);
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
        public async Task<IActionResult> Update(Guid id, [FromBody] FilePathManageModel filePathViewModel)
        {
            var responseModel = await _filePathService.UpdateFilePathAsync(id, filePathViewModel);
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
            var responseModel = await _filePathService.DeleteFilePathAsync(id);
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
