using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sadkah.Backend.Dtos.Donation
{
    public class UpdateAnonymousDonationRequestDto
    {
        [Required]
        public bool? IsAnonymous { get; set; }
    }
}