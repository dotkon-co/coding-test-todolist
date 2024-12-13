using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Requests.Register
{
	public class RegisterRequest
	{
		public string Name { get; set; } = null!;
		public string User { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}
