using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
	public class EncryptService : IEncryptService
	{
		public string EncryptString(string input)
		{
			return input;
		}
	}
}
