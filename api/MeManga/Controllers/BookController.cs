using MeManga.Core.Business.Models.Base;
using MeManga.Core.Business.Models.Books;
using MeManga.Core.Business.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Controllers
{
    [Route("api/books")]
    [EnableCors("CorsPolicy")]
    public class BookController : BaseController
    {
        private readonly IBookService _bookService;
        
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(BookRequestListViewModel bookRequestListViewModel) 
        {
            var books = await _bookService.ListBookAsync(bookRequestListViewModel);
            return Ok(books);
        }

        [HttpGet("all-books")]
        public async Task<IActionResult> GetAllBook(BaseRequestGetAllViewModel baseRequestGetAllViewModel)
        {
            var books = await _bookService.GetAllBookAsync(baseRequestGetAllViewModel);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            var responseModel = await _bookService.GetBookByIdAsync(id);
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
        public async Task<IActionResult> Post([FromBody] BookManageModel bookManageModel)
        {
            var responseModel = await _bookService.CreateBookAsync(bookManageModel);
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
        public async Task<IActionResult> Update(Guid id, [FromBody] BookManageModel bookViewModel)
        {
            var responseModel = await _bookService.UpdateBookAsync(id, bookViewModel);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpPut("type/{id}")]
        public async Task<IActionResult> UpdateTypeBook(Guid id, [FromBody] BookTypeManageModel bookViewModel)
        {
            var responseModel = await _bookService.UpdateBookTypeAsync(id, bookViewModel);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpPut("writer/{id}")]
        public async Task<IActionResult> UpdateWriterBook(Guid id, [FromBody] BookWriterManageModel bookViewModel)
        {
            var responseModel = await _bookService.UpdateBookWriterAsync(id, bookViewModel);
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
            var responseModel = await _bookService.DeleteBookAsync(id);
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
