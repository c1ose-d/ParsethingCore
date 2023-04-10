namespace LogWriterLibrary;

public static class LogWriter
{
    public static void Initialize()
    {
        FileInfo trace = new("Log.txt");
        trace.Create().Close();
        _ = Trace.Listeners.Add(new TextWriterTraceListener(trace.OpenWrite()));
        Trace.AutoFlush = true;
    }

    public static void Write(Exception exception)
    {
        string value = $"{DateTime.Now}\n{exception.Source}\n{exception.Message}\n";
        if (exception.InnerException != null)
        {
            value += $"{exception.InnerException}\n";
        }
        Trace.WriteLine(value);
    }
}