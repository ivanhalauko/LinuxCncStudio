using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace LinuxCncStudio.Gui;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Точка входа для графического интерфейса
            desktop.MainWindow = new Avalonia.Controls.Window
            {
                Title = "LinuxCNC Studio Engine - Canvas Editor",
                Width = 1024,
                Height = 768,
                Content = new LinuxCncStudio.Gui.Views.NodeCanvasView()
            };
        }
        base.OnFrameworkInitializationCompleted();
    }
}
