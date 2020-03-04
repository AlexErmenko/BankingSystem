using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Policy;
using System.Text;

namespace GrpcService
{
	[DataContract]
	public class CurrencyDTO
	{
		[DataMember(Name = "ccy")]
		public string name { get; set; }

		[DataMember(Name = "base_ccy")]
		public string BaseCurrency { get; set; }

		[DataMember(Name = "buy")]
		public string Buy { get; set; }

		[DataMember(Name = "sale")]
		public string Sale { get; set; }

		public override string ToString()
		{
			return $"Name - {name} | Buy - {Buy}";
		}
	}
}
