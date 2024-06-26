// MIT License.

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace WebForms.Compiler.Dynamic;

internal sealed class WebFormsCompilationService : BackgroundService
{
    private readonly ILogger<WebFormsCompilationService> _logger;
    private readonly ManualResetEventSlim _event;
    private readonly DynamicSystemWebCompilation _compiler;
    private readonly IOptions<PageCompilationOptions> _options;

    public WebFormsCompilationService(
        DynamicSystemWebCompilation compiler,
        IOptions<PageCompilationOptions> options,
        ILogger<WebFormsCompilationService> logger)
    {
        _compiler = compiler;
        _options = options;
        _logger = logger;
        _event = new ManualResetEventSlim(true);
    }

    public override void Dispose()
    {
        base.Dispose();
        _event.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var files = _compiler.Files;

        using var matcher = new CompositeWatcher(files, OnFileChange);

        foreach (var extension in _options.Value.Parsers.Keys)
        {
            matcher.AddExtension(extension);
        }

        await ProcessChanges(stoppingToken).ConfigureAwait(false);
    }

    private void OnFileChange()
    {
        _event.Set();
        _logger.LogInformation("File change detected");
    }

    private async Task ProcessChanges(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await _event.WaitHandle.WaitAsync(token).ConfigureAwait(false);

            try
            {
                _compiler.CompilePages(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error compiling assets");
            }

            _event.Reset();
        }
    }

    private sealed class CompositeWatcher(IFileProvider files, Action action) : IDisposable
    {
        private readonly List<IDisposable> _disposables = [];

        public void AddExtension(string extension)
        {
            var disposable = ChangeToken.OnChange(() => files.Watch($"**/*{extension}*"), action);

            _disposables.Add(disposable);
        }

        public void Dispose()
        {
            foreach (var d in _disposables)
            {
                d.Dispose();
            }
        }
    }
}
