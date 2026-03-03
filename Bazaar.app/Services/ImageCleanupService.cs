
namespace Bazaar.app.Services
{
    public class ImageCleanupService : BackgroundService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ImageCleanupService> _logger;
        private readonly string _tempPath;

        public ImageCleanupService(IWebHostEnvironment env, ILogger<ImageCleanupService> logger)
        {
            _env = env;
            _logger = logger;
            _tempPath = Path.Combine(_env.WebRootPath, "temp");
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("خدمة تنظيف الصور المؤقتة بدأت العمل...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    CleanTempFolder();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "حدث خطأ أثناء تنظيف المجلد المؤقت");
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
        private void CleanTempFolder()
        {
            if (!Directory.Exists(_tempPath)) return;

            var directory = new DirectoryInfo(_tempPath);
            var files = directory.GetFiles();

            int deletedCount = 0;
            foreach (var file in files)
            {
                if (file.CreationTime < DateTime.Now.AddDays(-1))
                {
                    file.Delete();
                    deletedCount++;
                }
            }
            if (deletedCount > 0)
            {
                _logger.LogInformation($"{deletedCount} image delete successfully");
            }
        }
    }
}
