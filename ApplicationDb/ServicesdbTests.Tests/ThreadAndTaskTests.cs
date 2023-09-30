using Xunit;
using ModelsDb;
using Microsoft.EntityFrameworkCore;
using ExportTool;

namespace ServicesdbTests.Tests
{
    public class ThreadAndTaskTests
    {
        [Fact]
        public void ParallelImportAndExportTest()
        {
            ExportService exportService = new(Environment.CurrentDirectory, "export.csv");
            ExportService importService = new(Environment.CurrentDirectory, "import.csv");
            var locker = new object();

            using (BankServiceContext context = new())
            {
                importService.ImportDBToCSV(context.employee);
            }

            ThreadPool.QueueUserWorkItem(_ =>
            {
                using (BankServiceContext context = new())
                {
                    lock (locker)
                    {
                        exportService.ImportDBToCSV(context.employee);
                        context.employee.RemoveRange(context.employee);
                        context.SaveChanges();
                    }
                }
            });

            ThreadPool.QueueUserWorkItem(_ =>
            {
                using (BankServiceContext context = new())
                {
                    lock (locker)
                    {
                        context.AddRange(importService.ImportCSVToDB<Employee>());
                        context.SaveChanges();
                    }
                }
            });

            using (BankServiceContext context = new())
            {
                lock (locker)
                {
                    List<Employee> export = exportService.ImportCSVToDB<Employee>();
                    List<Employee> import = importService.ImportCSVToDB<Employee>();
                    bool? difference = export.Except(import).Any();
                    Assert.False(difference);
                }
            }
        }
        [Fact]
        public void ParallelPaymentTest()
        {
            Account account = new();
            Mutex mutex = new();
            CountdownEvent countdownEvent = new CountdownEvent(2);


            ThreadPool.QueueUserWorkItem(_ =>
            {
                for (int i = 0; i < 10; i++)
                {
                    mutex.WaitOne();
                    account.amount += 100;
                    mutex.ReleaseMutex();
                }
                countdownEvent.Signal();
            });

            ThreadPool.QueueUserWorkItem(_ =>
            {
                for (int i = 0; i < 10; i++)
                {
                    mutex.WaitOne();
                    account.amount += 100;
                    mutex.ReleaseMutex();
                }
                countdownEvent.Signal();
            });

            countdownEvent.Wait();
            Assert.Equal(2000, account.amount);
        }
    }
}
