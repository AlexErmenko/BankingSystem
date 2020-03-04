namespace ApplicationCore.Dto
{
	public class CurrencyDto
	{
		public int Id { get; set; }
		public string  ShortName { get; set; }
		public decimal Buy       { get; set; }
		public decimal Sale      { get; set; }
	}
}