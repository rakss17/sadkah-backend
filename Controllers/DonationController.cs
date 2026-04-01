using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sadkah.Backend.Data;
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
    }
}