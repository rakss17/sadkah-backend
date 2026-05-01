using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize]
        public async Task<IActionResult> GetAllCampaigns([FromQuery] QueryObject query)
        {
            var campaigns = await _campaignRepository.GetAllCampaignsAsync(query);
            var campaignDtos = campaigns.Items.Select(c => c.ToCampaignDto());
            return Ok(new
            {
                success = true,
                message = "Campaigns retrieved successfully.",
                data = campaignDtos,
                metadata = new
                {
                    totalCount = campaigns.TotalCount,
                    pageSize = campaigns.PageSize,
                    currentPage = campaigns.CurrentPage,
                    totalPages = campaigns.TotalPages
                }
            });
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetCampaignById([FromRoute] Guid id)
        {
            var campaign = await _campaignRepository.GetCampaignByIdAsync(id);
            if (campaign == null) return NotFound();
            return Ok(new
            {
                success = true,
                message = "Campaign retrieved successfully.",
                data = campaign.ToCampaignDto(),
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignRequestDto createDto)
        {
            var campaign = createDto.ToCampaignFromCreateDto();
            
            var createdCampaign = await _campaignRepository.CreateCampaignAsync(campaign);

            if (createdCampaign == null) return NotFound();

            return CreatedAtAction(
                nameof(GetCampaignById),
                new { id = createdCampaign.Id },
                new
                {
                    success = true,
                    message = "Campaign created successfully.",
                    data = createdCampaign.ToCampaignDto()
                }
            );
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateCampaign([FromRoute] Guid id, [FromBody] UpdateCampaignRequestDto updateDto)
        {
            var updatedCampaign = await _campaignRepository.UpdateCampaignAsync(id, updateDto);

            if (updatedCampaign == null) return NotFound();

            return Ok(new
            {
                success = true,
                message = "Campaign updated successfully.",
                data = updatedCampaign.ToCampaignFromUpdateResponseDto()
            });
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteCampaign([FromRoute] Guid id)
        {
            var deletedCampaign = await _campaignRepository.DeleteCampaignAsync(id);
            
            if (deletedCampaign == null) return NotFound();

            return Ok(new
            {
                success = true,
                message = "Campaign deleted successfully.",
                data = deletedCampaign.ToCampaignDto()
            });
        }
    }
}