using MHikeHybrid.Services;
using MHikeHybrid.ViewModels; 
using MHikeHybrid.Views; 
using Microsoft.Extensions.Logging;

namespace MHikeHybrid
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
         

            builder.Services.AddSingleton<DatabaseService>();

           
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddTransient<AddHikeViewModel>(); 
           
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<AddHikePage>(); 

            return builder.Build();
        }
    }
}