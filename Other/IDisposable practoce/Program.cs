using System.Runtime.InteropServices;
using Npgsql;
using System.Data.SqlClient;
using System.Configuration;

public class ConnectionAndMemory : IDisposable
{
    public static long TotalFreed { get; private set; }
    public static long TotalAllocated { get; private set; }
    private NpgsqlConnection _connection;
    private IntPtr _chunkHandle;
    private int _chunkSize;
    private bool _isFreed;
    private NpgsqlConnection GetConnection()
    {
        // Получаем строку подключения из конфигурации
        //var connectionString = ConfigurationManager.ConnectionStrings["Postgres"].ConnectionString;
        var connectionString = "Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=sewdaw;";
        //Создаем соединение
        var connection = new NpgsqlConnection(connectionString);
        return connection;
    }
    public ConnectionAndMemory(int chunkSize)
    {
        _connection = GetConnection();
        _connection.Open();
        _chunkSize = chunkSize;
        _chunkHandle = Marshal.AllocHGlobal(chunkSize);
        TotalAllocated += chunkSize;
    }
    private void ReleaseUnmanagedResources()
    {
        if (_isFreed) return;
        Marshal.FreeHGlobal(_chunkHandle);
        TotalFreed += _chunkSize;
        _isFreed = true;
    }
    public void DoWork() { } // Фиктивный метод. Подразумевается, что здесь вы работаете с ресурсами.
protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing)
        {
            _connection?.Dispose();
        }
    }
    public void Dispose()
    {
        Dispose(true);
    }
}

public class Program
{
    static void CreateConnectionsAndMemory(int count)
    {
        var random = new Random();
        for (int i = 0; i < count; i++)
        {
            var chunkSize = random.Next(4096);
            using (var connectionAndMemory = new ConnectionAndMemory(chunkSize))
            {
                connectionAndMemory.DoWork();
            }
        }
    }

    static void Main()
    {
        CreateConnectionsAndMemory(100);
        Console.WriteLine($"Total Allocated:{ ConnectionAndMemory.TotalAllocated}");
        Console.WriteLine($"Total Freed: {ConnectionAndMemory.TotalFreed}");
    }
}