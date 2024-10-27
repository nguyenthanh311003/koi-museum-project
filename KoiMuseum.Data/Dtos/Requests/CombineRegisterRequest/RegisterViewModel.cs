using KoiMuseum.Data.Dtos.Requests.RegisterDetail;
using KoiMuseum.Data.Dtos.Requests.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Dtos.Requests.CombineRegisterRequest
{
    public class RegisterViewModel
    {
        public CreateRegistrationRequest createRegistrationRequest {  get; set; }
        public CreateRegisterDetailRequest CreateRegisterDetailRequest { get; set; }
    }
}
