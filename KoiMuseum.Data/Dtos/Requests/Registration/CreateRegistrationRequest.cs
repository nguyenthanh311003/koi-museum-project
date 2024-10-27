using KoiMuseum.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Dtos.Requests.Registration
{
    public class CreateRegistrationRequest
    {
        public string? IntroductionOfOwner { get; set; }

        public string? IntroductionOfKoi { get; set; }
    }
}
