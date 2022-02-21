using BatchToDoCLI.Models;
using YamlDotNet.Serialization;

namespace BatchToDoCLI.Definitions
{
    public class Loader
    {
        public TaskBatch LoadFromYaml(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException(nameof(path) + " must be provided");
            }

            var contents = File.ReadAllText(path);

            var yamlDeserializer = new DeserializerBuilder().Build();

            return yamlDeserializer.Deserialize<TaskBatch>(contents);
        }
    }
}
