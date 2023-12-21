using CliWrap;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.WPF.Services;

public class CommandConstruct
{
    public string Exectuable { get; set; } = String.Empty;
    public string Parameters { get; set; } = String.Empty;
    public string WorkingDirectory { get; set; } = String.Empty;

    public bool Isvalid()
    {
        return !String.IsNullOrWhiteSpace(Exectuable);
    }
}

public interface ICommandLineService
{
    Task<string?> RunCommandReturnStringAsync(CommandConstruct commandConstruct);
}

public class CommandLineService : ICommandLineService
{
    public async Task<string?> RunCommandReturnStringAsync(CommandConstruct commandConstruct)
    {
        if (commandConstruct == null || !commandConstruct.Isvalid())
        {
            return null;
        }
        await Task.Delay(2000); // Simulate a bit of work
        var stdOut = string.Empty;
        try
        {
            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();

            var result = await Cli.Wrap(commandConstruct.Exectuable)
                .WithArguments(new[] { commandConstruct.Parameters })
                .WithWorkingDirectory(commandConstruct.WorkingDirectory)
                // This can be simplified with `ExecuteBufferedAsync()`
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                .ExecuteAsync();

            // Access stdout & stderr buffered in-memory as strings
            stdOut = stdOutBuffer.ToString();
            var stdErr = stdErrBuffer.ToString();
        }
        catch (Exception ex)
        {
            var message = ex.Message;
        }

        return stdOut;
    }
}