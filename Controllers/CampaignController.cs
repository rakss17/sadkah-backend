using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sadkah.Backend.Data;
using Sadkah.Backend.Dtos.Campaign;
using Sadkah.Backend.Mappers;
using Sadkah.Backend.Interfaces;

namespace Sadkah.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ICampaignRepository _campaignRepository;
        public CampaignsController(ApplicationDBContext context, ICampaignRepository campaignRepository)
        {
            _context = context;
            _campaignRepository = campaignRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCampaigns()
        {
            var campaigns = await _campaignRepository.GetAllCampaignsAsync();
            var campaignDtos = campaigns.Select(c => c.ToCampaignDto());
            return Ok(campaignDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampaignById([FromRoute] int id)
        {
            var campaign = await _context.Campaigns.Include(c => c.Owner).FirstOrDefaultAsync(c => c.Id == id);
            if (campaign == null) return NotFound();
            return Ok(campaign.ToCampaignDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignRequestDto createDto)
        {
            var campaign = createDto.ToCampaignFromCreateDto();
            await _context.Campaigns.AddAsync(campaign);
            await _context.SaveChangesAsync();

            var createdCampaign = await _context.Campaigns.Include(c => c.Owner).FirstOrDefaultAsync(c => c.Id == campaign.Id);

            if (createdCampaign == null) return NotFound();

            return CreatedAtAction(
                nameof(GetCampaignById),
                new { id = createdCampaign.Id },
                createdCampaign.ToCampaignDto()
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCampaign([FromRoute] int id, [FromBody] UpdateCampaignRequestDto updateDto)
        {
            var campaignModel = await _context.Campaigns.Include(c => c.Owner).FirstOrDefaultAsync(c => c.Id == id);

            if (campaignModel == null) return NotFound();

            campaignModel.Title = updateDto.Title ?? campaignModel.Title;
            campaignModel.OwnerId = updateDto.OwnerId ?? campaignModel.OwnerId;
            campaignModel.Description = updateDto.Description ?? campaignModel.Description;
            campaignModel.TargetAmount = updateDto.TargetAmount ?? campaignModel.TargetAmount;
            campaignModel.Deadline = updateDto.Deadline ?? campaignModel.Deadline;

            await _context.SaveChangesAsync();

            return Ok(campaignModel.ToCampaignDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampaign([FromRoute] int id)
        {
            var campaignModel = await _context.Campaigns.FirstOrDefaultAsync(c => c.Id == id);
            if (campaignModel == null) return NotFound();

            campaignModel.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}