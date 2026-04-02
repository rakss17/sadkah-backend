using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sadkah.Backend.Data;
using Sadkah.Backend.Dtos.Donation;
using Sadkah.Backend.Mappers;

namespace Sadkah.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public DonationsController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllDonations()
        {
            var donations = _context.Donations.Include(d => d.Donor).ToList().Select(d => d.ToDonationDto());
            return Ok(donations);
        }

        [HttpGet("{id}")]
        public IActionResult GetDonationById([FromRoute] int id)
        {
            var donation = _context.Donations.Include(d => d.Donor).FirstOrDefault(d => d.Id == id);
            if (donation == null) return NotFound();
            return Ok(donation.ToDonationDto());
        }

        [HttpPost]
        public IActionResult CreateDonation([FromBody] CreateDonationRequestDto createDto)
        {
            var donation = createDto.ToDonationFromCreateDto();
            _context.Donations.Add(donation);
            _context.SaveChanges();

            var createdDonation = _context.Donations.Include(d => d.Donor).FirstOrDefault(d => d.Id == donation.Id);

            if (createdDonation == null) return NotFound();

            return CreatedAtAction(
                nameof(GetDonationById),
                new { id = createdDonation.Id },
                createdDonation.ToDonationDto()
            ); 
        }

        [HttpPut("{id}/anonymous")]
        public IActionResult UpdateAnonymousDonation([FromRoute] int id, [FromBody] UpdateAnonymousDonationRequestDto updateDto)
        {
            var donationModel = _context.Donations.FirstOrDefault(d => d.Id == id);

            if (donationModel == null) return NotFound();

            donationModel.IsAnonymous = updateDto.IsAnonymous;
            _context.SaveChanges();

            return Ok(new { message = "Donation anonymous status updated successfully." });
        }   
    }
}