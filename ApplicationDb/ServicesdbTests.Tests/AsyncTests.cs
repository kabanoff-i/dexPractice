using Xunit;
using Xunit.Abstractions;

namespace ServicesdbTests.Tests
{
    public class AsyncTests
    {
        private readonly ITestOutputHelper output;
        public AsyncTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public async Task Test()
        {
            if (!ThreadPool.SetMaxThreads(Environment.ProcessorCount, Environment.ProcessorCount))
                throw new Exception("Error setting max threads");
            List<Task> tasks = new();
            for (int i = 0; i < 15; i++)
            {
                tasks.Add(ProcessTaskAsync(i+1));
                await Task.Delay(1000);
            }
            await Task.WhenAll(tasks);
        }
        private async Task ProcessTaskAsync(int taskNumber)
        {
            output.WriteLine($"Запуск {taskNumber} задачи");
            ThreadPool.GetAvailableThreads(out int avThreads, out int avIOThreads);
            output.WriteLine($"Свободных потоков {avThreads} {avIOThreads}");
            await Task.Delay(2000);
            output.WriteLine($"Завершение {taskNumber} задачи");
        }
    }
}
