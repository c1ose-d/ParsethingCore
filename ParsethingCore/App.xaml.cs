﻿namespace ParsethingCore;

public partial class App : Application
{
    public App() =>
        AppContext.SetSwitch("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", false);
}