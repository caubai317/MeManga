﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MeManga.Core.Business.Filters;
using MeManga.Core.Business.Models.Roles;
using MeManga.Core.Business.Services;
using MeManga.Core.Common.Constants;
using System.Threading.Tasks;

namespace MeManga.Controllers
{
    [Route("api/roles")]
    [EnableCors("CorsPolicy")]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;

        }

        [HttpGet]
        public async Task<IActionResult> Get(RoleRequestListViewModel roleRequestListViewModel)
        {
            var roles = await _roleService.ListRoleAsync(roleRequestListViewModel);
            return Ok(roles);
        }

        [HttpGet("by-name")]
        public async Task<IActionResult> GetbyName(string name)
        {
            var role = await _roleService.GetRoleByNameAsync(name);
            if (role == null)
            {
                return NotFound("Nội dung không tìm thấy trong hệ thống!");
            }
            return Ok(role);
        }
    }
}