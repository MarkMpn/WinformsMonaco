namespace MarkMpn.WinformsMonaco;

static class AsyncExtensions
{
    public static void ExecuteSync(this Task task)
    {
        var awaiter = task.GetAwaiter();
        while (!awaiter.IsCompleted)
            Application.DoEvents();
    }

    public static T ExecuteSync<T>(this Task<T> task)
    {
        var awaiter = task.GetAwaiter();
        while (!awaiter.IsCompleted)
            Application.DoEvents();
        return awaiter.GetResult();
    }
}
