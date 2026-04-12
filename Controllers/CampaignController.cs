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
        private readonly ICampaignRepository _campaignRepository;
        public CampaignsController(ICampaignRepository campaignRepository)
        {
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
            var campaign = await _campaignRepository.GetCampaignByIdAsync(id);
            if (campaign == null) return NotFound();
            return Ok(campaign.ToCampaignDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignRequestDto createDto)
        {
            var campaign = createDto.ToCampaignFromCreateDto();
            
            var createdCampaign = await _campaignRepository.CreateCampaignAsync(campaign);

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
            var updatedCampaign = await _campaignRepository.UpdateCampaignAsync(id, updateDto);

            if (updatedCampaign == null) return NotFound();

            return Ok(updatedCampaign.ToCampaignDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampaign([FromRoute] int id)
        {
            var deletedCampaign = await _campaignRepository.DeleteCampaignAsync(id);
            
            if (deletedCampaign == null) return NotFound();

            return Ok(new { message = "Campaign deleted successfully." });
        }
    }
}