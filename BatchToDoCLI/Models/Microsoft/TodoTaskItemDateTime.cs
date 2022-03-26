namespace BatchToDoCLI.Models.Microsoft
{
    public class TodoTaskItemDateTime
    {
        /// <summary>
        /// A combined date and time format: {date}T{time}
        /// Example: 2017-08-29T04:00:00.0000000
        /// </summary>
        public string dateTime { get; set; }

        /// <summary>
        /// Must be a value from Default Time Zones list on Microsoft developer docs.
        /// https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/default-time-zones?view=windows-11
        /// </summary>
        public string timeZone { get; set; }
    }
}
