using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.DataAccess.Models;
using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Services
{
    public class PagesService : IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PagesService(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }
        public async Task<List<PageDto>> GetPagesAsync()
        {
            var pages = await _pageRepository.GetAllAsync();
            return pages.Select(p => MapOperation(p)).ToList();
        }

        private PageDto MapOperation(Page page)
        {
            return new PageDto()
            {
                Id = page.Id,
                PageName = page.PageName,
                IconClass = page.IconClass,
                Route = page.Route
            };
        }
    }
}
