using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Application.Command.User
{
    public class UserCommand : IRequest<bool>
    {
        [Required(ErrorMessage ="This field is required")]
        [MinLength(10)]
        public required string FullName { get; set; }
        public required string MobileNumber { get; set; }
    }
}
