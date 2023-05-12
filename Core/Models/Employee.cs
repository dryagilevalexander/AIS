using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Employee
    {
      public int Id { get; set; }
      public string Name { get; set; } = null!;
      public string FirstName { get; set; } = null!;
      public string LastName { get; set; } = null!;
      public string Address { get; set; } = null!;
      public string PhoneNumber { get; set; } = null!;
      public string Email { get; set; } = null!;
      public int? PartnerId { get; set; }
      public Partner? Partner { get; set; }
    }
}
