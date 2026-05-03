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
            try
            {
                var campaigns = await _campaignRepository.GetAllCampaignsAsync(query);
                var campaignDtos = campaigns.Items.Select(c => c.ToCampaignDto());
                if (!campaignDtos.Any()) return NotFound(new
                {
                    success = false,
                    message = "No campaigns found.",
                });
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving campaigns: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error while retrieving campaigns.",
                });
            }
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetCampaignById([FromRoute] Guid id)
        {
            try
            {
                var campaign = await _campaignRepository.GetCampaignByIdAsync(id);
                if (campaign == null) return NotFound(new
                {
                    success = false,
                    message = "Campaign not found.",
                });
                return Ok(new
                {
                    success = true,
                    message = "Campaign retrieved successfully.",
                    data = campaign.ToCampaignDto(),
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving campaign: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error while retrieving the campaign.",
                });
            }

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignRequestDto createDto)
        {
            try
            {
                var campaign = createDto.ToCampaignFromCreateDto();
            
                var createdCampaign = await _campaignRepository.CreateCampaignAsync(campaign);

                if (createdCampaign == null) return NotFound(new
                {
                    success = false,
                    message = "Failed to create campaign.",
                });

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating campaign: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error while creating the campaign.",
                });
            }
           
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateCampaign([FromRoute] Guid id, [FromBody] UpdateCampaignRequestDto updateDto)
        {
            try
            {
                var updatedCampaign = await _campaignRepository.UpdateCampaignAsync(id, updateDto);

                if (updatedCampaign == null) return NotFound(new
                    {
                        success = false,
                        message = "Campaign not found.",
                    }
                );

                return Ok(new
                {
                    success = true,
                    message = "Campaign updated successfully.",
                    data = updatedCampaign.ToCampaignFromUpdateResponseDto()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating campaign: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error while updating the campaign.",
                });
            }
            
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteCampaign([FromRoute] Guid id)
        {
            try
            {
                var deletedCampaign = await _campaignRepository.DeleteCampaignAsync(id);
            
                if (deletedCampaign == null) return NotFound(new
                {
                    success = false,
                    message = "Campaign not found.",
                });

                return Ok(new
                {
                    success = true,
                    message = "Campaign deleted successfully.",
                    data = deletedCampaign.ToCampaignDto()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting campaign: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error while deleting the campaign.",
                });
            }

        }
    }
}