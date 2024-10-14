using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDP.Domain.Entities.BaseEntities
{
    public class BaseEntity
    {
        [Key]
        public Int64 Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public BaseEntity()
        {
            this.CreateDate = DateTime.UtcNow;
        }

    }
}
