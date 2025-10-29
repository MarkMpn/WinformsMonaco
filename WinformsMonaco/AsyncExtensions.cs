namespace WinformsMonaco;

static class AsyncExtensions
{
    public static T ExecuteSync<T>(this Task<T> task)
    {
        var awaiter = task.GetAwaiter();
        while (!awaiter.IsCompleted)
            Application.DoEvents();
        return awaiter.GetResult();
    }
}
