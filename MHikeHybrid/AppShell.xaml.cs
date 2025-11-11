using MHikeHybrid.Views; 

namespace MHikeHybrid;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(AddHikePage), typeof(AddHikePage));
    }
}