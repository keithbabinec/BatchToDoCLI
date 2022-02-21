namespace BatchToDoCLI
{
    public class Variables : Dictionary<string, string>
    {
        public Variables(string kvpListItems)
        {
            if (string.IsNullOrEmpty(kvpListItems))
            {
                return;
            }

            var kvpItems = kvpListItems.Split(";");

            foreach (var item in kvpItems)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var splitKvp = item.Split("=");

                    if (splitKvp.Length == 2)
                    {
                        this.Add(splitKvp[0], splitKvp[1]);
                    }
                }
            }
        }
    }
}
