namespace Domain.Interfaces.Services
{
	public interface IEncryptService
	{
		string HashString(string input);
		public bool CheckHash(string input, string hashString);
	}
}
