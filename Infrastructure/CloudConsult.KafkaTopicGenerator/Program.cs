using CloudConsult.KafkaTopicGenerator;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;

try
{
    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    var appSettings = new AppSettings();
    configuration.Bind(nameof(appSettings), appSettings);

    using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = appSettings.BootstrapServers }).Build())
    {
        foreach(var topic in appSettings.KafkaTopics)
        {
            var topicName = topic.Name;
            var partitions = topic.PartitionCount;
            var replicaFactor = topic.ReplicationFactor;

            Console.WriteLine($"Creating topic {topicName} with {partitions} partition(s) having a replication factor of {replicaFactor}");
            try
            {
                await adminClient.CreateTopicsAsync(new TopicSpecification[] { new TopicSpecification
                { 
                    Name = topicName,
                    ReplicationFactor = replicaFactor,
                    NumPartitions = partitions
                }});

                Console.WriteLine($"{topic.Name} topic created successfully");
            }
            catch (CreateTopicsException e)
            {
                Console.WriteLine($"An error occured while creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }
        }
    }

}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.StackTrace);
}
Console.WriteLine("Press any key to exit");
Console.ReadKey();