﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Emprise.Application.Map.Dtos;
using Emprise.Application.Map.Services;
using Emprise.Domain.Core.Enums;
using Emprise.Domain.Core.Models;
using Emprise.Web.Areas.Admin.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Emprise.Web.Areas.Admin.Pages.Map
{
    public class EditModel : BaseAdminPageModel
    {
        private readonly IMapAppService _mapAppService;
        private readonly AppConfig _appConfig;
        private readonly IMapper _mapper;
        public EditModel(
            ILogger<EditModel> logger,
            IMapAppService mapAppService,
            IMapper mapper,
            IOptionsMonitor<AppConfig> appConfig)
            : base(logger)
        {
            _mapper = mapper;
            _mapAppService = mapAppService;
            _appConfig = appConfig.CurrentValue;

        }

        [BindProperty]
        public MapInput Map { get; set; }

        public string ErrorMessage { get; set; }





        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id > 0)
            {
                var map = await _mapAppService.Get(id);

                Map = _mapper.Map<MapInput>(map);

                return Page();
            }
            else
            {
                return RedirectToPage("/Map/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            ErrorMessage = "";
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _mapAppService.Update(id, Map);
            if (!result.IsSuccess)
            {
                ErrorMessage = result.Message;
                return Page();
            }
            else
            {
                return RedirectToPage("/Map/Index");
            }

        }
    }
}