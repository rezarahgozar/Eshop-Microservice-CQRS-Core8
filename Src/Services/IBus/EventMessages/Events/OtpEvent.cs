using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMessages.Events
{
    public class OtpEvent : BaseEvent
    {
        public string MobileNumber { get; set; }
        public string OtpCode { get; set; }
    }
}
