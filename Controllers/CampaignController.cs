using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sadkah.Backend.Data;
using Sadkah.Backend.Dtos.Campaign;
using Sadkah.Backend.Mappers;

namespace Sadkah.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public CampaignsController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllCampaigns()
        {
            var campaigns = _context.Campaigns.Include(c => c.Owner).ToList().Select(c => c.ToCampaignDto());
            return Ok(campaigns);
        }

        [HttpGet("{id}")]
        public IActionResult GetCampaignById([FromRoute] int id)
        {
            var campaign = _context.Campaigns.Include(c => c.Owner).FirstOrDefault(c => c.Id == id);
            if (campaign == null) return NotFound();
            return Ok(campaign.ToCampaignDto());
        }

        [HttpPost]
        public IActionResult CreateCampaign([FromBody] CreateCampaignRequestDto createDto)
        {
            var campaign = createDto.ToCampaignFromCreateDto();
            _context.Campaigns.Add(campaign);
            _context.SaveChanges();

            var createdCampaign = _context.Campaigns.Include(c => c.Owner).FirstOrDefault(c => c.Id == campaign.Id);

            if (createdCampaign == null) return NotFound();

            return CreatedAtAction(
                nameof(GetCampaignById),
                new { id = createdCampaign.Id },
                createdCampaign.ToCampaignDto()
            );
        }

    }
}