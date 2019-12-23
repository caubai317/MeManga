using MeManga.Core.Business.Models.Base;
using MeManga.Core.Business.Models.TypeBooks;
using MeManga.Core.Business.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Controllers
{
    [Route("api/typebooks")]
    [EnableCors("CorsPolicy")]
    public class TypeBookController : BaseController
    {
        private readonly ITypeBookService _typeBookService;

        public TypeBookController(ITypeBookService typeBookService)
        {
            _typeBookService = typeBookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(TypeBookRequestListViewModel typeBookRequestListViewModel)
        {
            var typeBooks = await _typeBookService.ListTypeBookAsync(typeBookRequestListViewModel);
            return Ok(typeBooks);
        }

        [HttpGet("all-typeBooks")]
        public async Task<IActionResult> GetAllTypeBook(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var typeBooks = await _typeBookService.GetAllTypeBookAsync(baseRequestGetAllViewModel);
            return Ok(typeBooks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTypeBookById(Guid id)
        {
            var responseModel = await _typeBookService.GetTypeBookByIdAsync(id);
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
        public async Task<IActionResult> Post([FromBody] TypeBookManageModel typeBookManageModel)
        {
            var responseModel = await _typeBookService.CreateTypeBookAsync(typeBookManageModel);
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
        public async Task<IActionResult> Update(Guid id, [FromBody] TypeBookManageModel typeBookViewModel)
        {
            var responseModel = await _typeBookService.UpdateTypeBookAsync(id, typeBookViewModel);
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
            var responseModel = await _typeBookService.DeleteTypeBookAsync(id);
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
