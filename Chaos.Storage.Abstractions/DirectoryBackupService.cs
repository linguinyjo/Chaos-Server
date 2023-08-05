using System.Diagnostics;
using System.IO.Compression;
using Chaos.IO.FileSystem;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chaos.Storage.Abstractions;

/// <inheritdoc cref="IDirectoryBackupService" />
public class DirectoryBackupService<TOptions> : BackgroundService, IDirectoryBackupService
    where TOptions: class, IDirectoryBackupOptions
{
    /// <summary>
    ///     The logger used to log events
    /// </summary>
    protected ILogger Logger { get; }
    /// <summary>
    ///     The options used to configure the store
    /// </summary>
    protected TOptions Options { get; }

    /// <summary>
    ///     Creates a new instance of <see cref="DirectoryBackupService{TOptions}" />
    /// </summary>
    public DirectoryBackupService(IOptions<TOptions> options, ILogger<DirectoryBackupService<TOptions>> logger)
    {
        Options = options.Value;
        Logger = logger;
    }

    /// <inheritdoc />
    public virtual ValueTask HandleBackupRetentionAsync(string directory, CancellationToken token)
    {
        var directoryInfo = new DirectoryInfo(directory);

        if (!directoryInfo.Exists)
        {
            Logger.LogError("Failed to handle backup retention for path {@Directory} because it doesn't exist", directory);

            return default;
        }

        var deleteTime = DateTime.UtcNow.AddDays(-Options.BackupRetentionDays);

        foreach (var fileInfo in directoryInfo.EnumerateFiles())
            if (fileInfo.CreationTimeUtc < deleteTime)
                try
                {
                    Logger.LogTrace("Deleting backup {@Backup}", fileInfo.FullName);
                    fileInfo.Delete();
                } catch
                {
                    //ignored, not even worth logging
                }

        return default;
    }

    /// <inheritdoc />
    public virtual ValueTask TakeBackupAsync(string saveDirectory, CancellationToken token)
    {
        try
        {
            if (!Directory.Exists(Options.BackupDirectory))
                Directory.CreateDirectory(Options.BackupDirectory);

            var directoryInfo = new DirectoryInfo(saveDirectory);

            if (!directoryInfo.Exists)
            {
                Logger.LogError("Failed to take backup for path {@SaveDir} because it doesn't exist", saveDirectory);

                return default;
            }

            //don't take backups of things that haven't been modified
            if (directoryInfo.LastWriteTimeUtc < DateTime.UtcNow.AddMinutes(-Options.BackupIntervalMins))
                return default;

            var directoryName = Path.GetFileName(saveDirectory);
            var backupDirectory = Path.Combine(Options.BackupDirectory, directoryName);

            if (!Directory.Exists(backupDirectory))
                Directory.CreateDirectory(backupDirectory);

            var backupPath = Path.Combine(backupDirectory, $"{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}.zip");

            if (token.IsCancellationRequested)
                return default;

            saveDirectory.SafeExecute(
                saveDir =>
                {
                    Logger.LogTrace("Backing up directory {@SaveDir}", saveDirectory);
                    ZipFile.CreateFromDirectory(saveDir, backupPath);
                });
        } catch (Exception e)
        {
            Logger.LogError(e, "Failed to take backup for path {@SaveDir}", saveDirectory);
        }

        return default;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var dop = Math.Max(1, Environment.ProcessorCount / 4);
        var pOptions = new ParallelOptions { MaxDegreeOfParallelism = dop };
        var backupTimer = new PeriodicTimer(TimeSpan.FromMinutes(Options.BackupIntervalMins));

        while (!stoppingToken.IsCancellationRequested)
            try
            {
                await backupTimer.WaitForNextTickAsync(stoppingToken);
                var start = Stopwatch.GetTimestamp();

                Logger.LogDebug("Performing backup");

                await Parallel.ForEachAsync(
                    Directory.EnumerateDirectories(Options.Directory),
                    pOptions,
                    TakeBackupAsync);

                await Parallel.ForEachAsync(Directory.EnumerateDirectories(Options.BackupDirectory), pOptions, HandleBackupRetentionAsync);

                Logger.LogDebug("Backup completed, took {@Elapsed}", Stopwatch.GetElapsedTime(start));
            } catch (OperationCanceledException)
            {
                //ignore
                return;
            } catch (Exception e)
            {
                Logger.LogCritical(e, "Exception occurred while performing backup");
            }
    }
}