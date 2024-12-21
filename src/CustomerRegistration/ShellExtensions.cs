using System;

namespace CustomerRegistration;

public static class ShellExtensions
{
    public static async Task GoToAsync(this Shell shell, ShellNavigationState state, bool removePreviousPage = false)
    {
        await shell.GoToAsync(state);

        var stack = shell.Navigation.ModalStack[0];

        stack = null;


    }

    public static async Task ToastAlert(this Shell shell, string message, TimeSpan timeSpan = default)
    {
        if (timeSpan == default)
        {
            timeSpan = TimeSpan.FromSeconds(2);
        }

        // Criação do Border
        var toast = new Border
        {
            Opacity = 0,
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb("#75C82864"),
            StrokeThickness = .5,
            Margin = new Thickness(5, 10),
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Start,
            MinimumHeightRequest = 60,
            MaximumWidthRequest = 500,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 7 }
        };

        // Criação do Grid
        var grid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition { Width = GridLength.Star }
            },
            Padding = new Thickness(10)
        };

        // Criação do Ellipse
        var ellipse = new Microsoft.Maui.Controls.Shapes.Ellipse
        {
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Start,
            WidthRequest = 30,
            HeightRequest = 30
        };

        // Criação do Label Icon
        var icon = new Label
        {
            WidthRequest = 30,
            HeightRequest = 30,
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Start,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            FontSize = 24, // Tamanho de fonte equivalente a "Title"
            FontFamily = "Icomoon",
            Text = "vdfvdf"
        };

        // Criação do Label Message
        var labelMessage = new Label
        {
            Margin = new Thickness(10, 5),
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Start,
            TextColor = Colors.Black,
            FontSize = 13,
            Text = message
        };

        grid.Children.Add(ellipse);
        grid.Children.Add(icon);
        grid.Add(labelMessage, 1, 0);

        // Adicionando Grid ao Border
        toast.Content = grid;

        var absoluteLayout = new AbsoluteLayout { InputTransparent = true};
        AbsoluteLayout.SetLayoutBounds(toast, new Rect(0, 0, 1, 1));
        AbsoluteLayout.SetLayoutFlags(toast, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);

        var currentPage = shell.CurrentPage as ContentPage;
        if (currentPage?.Content is Layout layout)
        {
            absoluteLayout.Children.Add(toast);
            layout.Children.Add(absoluteLayout);

            var timer = CreateTimer(toast, timeSpan, layout);
            timer.Start();
            toast.TranslationY = -50;

            await Task.WhenAll(
                toast.FadeTo(1, 500, Easing.CubicInOut),
                toast.TranslateTo(0, 0, 250, Easing.CubicInOut)
            );
        }
    }

    private static IDispatcherTimer CreateTimer(View toast, TimeSpan timeSpan, Layout layout)
    {
        var timer = Application.Current!.Dispatcher.CreateTimer();
        timer.Interval = timeSpan;
        timer.Tick += async (s, e) =>
        {
            await Task.WhenAll(
                toast.FadeTo(0, 150, Easing.CubicInOut),
                toast.TranslateTo(0, -50, 200, Easing.CubicInOut));
            layout.Remove(toast.Parent as Layout);
            timer.Stop();
        };
        return timer;
    }

}
