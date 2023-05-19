using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CamundaClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static string camundaRestBaseUrl = "http://192.168.8.104:8080/engine-rest";

        static async Task Main(string[] args)
        {
            while (true)
            {
                //Fetch and lock a task
                var fetchAndLockRequest = new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        workerId = "someWorkerId",
                        maxTasks = 1,
                        usePriority = true,
                        topics = new[]
                        {
                            new 
                            {
                                topicName = "creditScoreChecker",
                                lockDuration = 10000,
                            },
                        },
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync($"{camundaRestBaseUrl}/external-task/fetchAndLock", fetchAndLockRequest);

                var content = await response.Content.ReadAsStringAsync();
                dynamic deserializedContent = JsonConvert.DeserializeObject(content);
                // test if deserializedContent is an array and if it has at least one element
                if (deserializedContent is JArray && deserializedContent.Count > 0)
                {
                    Console.WriteLine($"Fetched task with id {deserializedContent[0].id}");
                }
                else
                {
                    Console.WriteLine("No task fetched");
                    continue;
                }
                var taskId = deserializedContent[0]?.id;

                if (taskId == null)
                {
                    Console.WriteLine("No task fetched");
                    continue;
                }

                Console.WriteLine($"Fetched task with id {taskId}");

                // Retrieve the variable 'defaultScore'
                dynamic firstObject = deserializedContent[0];
                dynamic variables = firstObject.variables;
                dynamic defaultScoreObj = variables.defaultScore;
                var defaultScore = defaultScoreObj.value;

                Console.WriteLine($"Retrieved defaultScore with value {defaultScore}");

                // Complete the task
                var completeRequest = new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        workerId = "someWorkerId",
                        variables = new
                        {
                            creditScores = new
                            {
                                value = JsonConvert.SerializeObject(new[] { defaultScore, 9, 1, 4, 10 }),
                                type = "Json",
                            },
                        },
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                // log the request
                Console.WriteLine($"Complete request: {await completeRequest.ReadAsStringAsync()}");
                
                var completeResponse = await client.PostAsync($"{camundaRestBaseUrl}/external-task/{taskId}/complete", completeRequest);

                if (completeResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Task completed successfully");
                }
                else
                {
                    var errorContent = await completeResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to complete task. Server response: {errorContent}");
                }
                // Wait for 5 seconds
                await Task.Delay(5000);
            }
        }
    }
}
