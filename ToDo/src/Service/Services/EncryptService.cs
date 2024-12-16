﻿using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
	public class EncryptService : IEncryptService
	{
		public string HashString(string input)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					builder.Append(bytes[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}

		public bool CheckHash(string input, string hashString)
		{
			string hashInput = HashString(input); 
			return hashString.Equals(hashInput, StringComparison.OrdinalIgnoreCase);
		}
	}
}
