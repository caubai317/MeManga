using MeManga.Core.Business.Models.Base;
using MeManga.Core.Business.Models.Writers;
using MeManga.Core.Business.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Controllers
{
    [Route("api/writers")]
    [EnableCors("CorsPolicy")]
    public class WriterController : BaseController
    {
        private readonly IWriterService _writerService;

        public WriterController(IWriterService writerService)
        {
            _writerService = writerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(WriterRequestListViewModel writerRequestListViewModel)
        {
            var writers = await _writerService.ListWriterAsync(writerRequestListViewModel);
            return Ok(writers);
        }

        [HttpGet("all-writers")]
        public async Task<IActionResult> GetAllWriter(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var writers = await _writerService.GetAllWriterAsync(baseRequestGetAllViewModel);
            return Ok(writers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWriterById(Guid id)
        {
            var responseModel = await _writerService.GetWriterByIdAsync(id);
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
        public async Task<IActionResult> Post([FromBody] WriterManageModel writerManageModel)
        {
            var responseModel = await _writerService.CreateWriterAsync(writerManageModel);
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
        public async Task<IActionResult> Update(Guid id, [FromBody] WriterManageModel writerViewModel)
        {
            var responseModel = await _writerService.UpdateWriterAsync(id, writerViewModel);
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
            var responseModel = await _writerService.DeleteWriterAsync(id);
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
