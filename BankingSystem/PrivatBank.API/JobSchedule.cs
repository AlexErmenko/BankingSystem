using System;

namespace CurrencyService
{
	public class JobSchedule
	{
		public JobSchedule(Type jobType, string cronExpression)
		{
			JobType        = jobType;
			CronExpression = cronExpression;
		}

		public Type   JobType        { get; set; }
		public string CronExpression { get; }
	}
}