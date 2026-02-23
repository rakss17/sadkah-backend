using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sadkah.Backend.Dtos.Campaign;
using Sadkah.Backend.Models;

namespace Sadkah.Backend.Mappers
{
    public static class CampaignMappers
    {
        public static CampaignDto ToCampaignDto(this Campaign campaignModel)
        {
            return new CampaignDto
            {
                Id = campaignModel.Id,
                Title = campaignModel.Title,
                Description = campaignModel.Description,
                TargetAmount = campaignModel.TargetAmount,
                CurrentAmount = campaignModel.CurrentAmount,
                Deadline = campaignModel.Deadline,
                Status = campaignModel.Status,
                IsVerified = campaignModel.IsVerified,
                OwnerName = campaignModel.Owner != null ? $"{campaignModel.Owner.FirstName} {campaignModel.Owner.LastName}" : "Unknown User"
            };
        }

        public static Campaign ToCampaignFromCreateDto(this CreateCampaignRequestDto createDto)
        {
            return new Campaign
            {
                OwnerId = createDto.OwnerId,
                Title = createDto.Title,
                Description = createDto.Description,
                TargetAmount = createDto.TargetAmount,
                Deadline = createDto.Deadline,
                Status = Enums.CampaignStatus.Active,
                IsVerified = false,
            };
        }
    }
}