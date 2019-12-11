using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MeManga.Core.Business.Filters;
using MeManga.Core.Business.Models.Comments;
using MeManga.Core.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Controllers
{
    [Route("api/comments")]
    [EnableCors("CorsPolicy")]
    [ValidateModel]
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetComment(Guid jobId, CommentRequestListViewModel commentRequestListViewModel)
        {
            var list = await _commentService.GetAllCommentByJob(jobId, commentRequestListViewModel);
            return Ok(list);
        }

        [HttpPost("{jobId}")]
        [CustomAuthorize]
        public async Task<IActionResult> PostComment(Guid jobId, [FromBody]CommentManageModel commentManageModel)
        {
            var responseModel = await _commentService.CreateCommentAsync(CurrentUserId, jobId, commentManageModel);
            if(responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel.Message);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpPut("{jobId}/{commentId}")]
        [CustomAuthorize]
        public async Task<IActionResult> UpdateComment(Guid jobId, Guid commentId, [FromBody] CommentManageModel commentManageModel)
        {
            var responseModel = await _commentService.UpdateCommentAsync(CurrentUserId, jobId, commentId, commentManageModel);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel.Message);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }

        [HttpDelete("{jobId}/{commentId}")]
        [CustomAuthorize]
        public async Task<IActionResult> DeleteComment(Guid jobId, Guid commentId)
        {
            var responseModel = await _commentService.DeleteCommentAsync(CurrentUserId, jobId, commentId);
            if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(responseModel.Message);
            }
            else
            {
                return BadRequest(new { Message = responseModel.Message });
            }
        }
    }
}