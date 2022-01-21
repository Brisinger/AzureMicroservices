using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Search.Interfaces;
using ECommerce.API.Search.Models;


namespace ECommerce.API.Search.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpPost]
        public async Task<IActionResult> SearchAsync([FromBody] SearchTerm term)
        {
            var (IsSuccess, SearchResults) = await searchService.SearchAsync(term.CustomerId);
            if (IsSuccess)
            {
                return Ok(SearchResults);
            }
            return NotFound();
        }
    }
}