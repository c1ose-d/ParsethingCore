﻿namespace ParsethingCore.UI.ListView_Custom;

public partial class ProcurementsList : UserControl, IView
{
    public ProcurementsList()
    {
        InitializeComponent();
        GetView();
    }

    public void GetView()
    {
        List<Procurement>? procurements = GET.View.ProcurementSources(), items = new();
        try
        {
            if (procurements != null)
                for (int i = 0; i < 20; i++)
                    items.Add(procurements[i]);
        }
        catch { }
        View.ItemsSource = items;
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        if (View.SelectedIndex != -1 && new ApprovalFlyout(((Procurement)View.SelectedItem).Number).ShowDialog() == true)
        {
            PULL.Procurement_ProcurementState((Procurement)View.SelectedItem, 21);
            GetView();
        }
    }

    public void Edit()
    {
        if (View.SelectedIndex != -1)
            new ProcurementCard((Procurement)View.SelectedItem).ShowDialog();
    }

    public void Delete()
    {
        if (View.SelectedIndex != -1 && new DeleteFlyout(((Procurement)View.SelectedItem).Number).ShowDialog() == true)
        {
            DELETE.Procurement((Procurement)View.SelectedItem);
            GetView();
        }
    }

    public void Export() { }

    public void Search(string searchString)
    {
        List<Procurement>? procurements = GET.View.ProcurementSources()?
            .Where(p => p.Number.ToLower().Contains(searchString) ||
            p.Object.ToLower().Contains(searchString))
            .ToList(),
            items = new();
        try
        {
            if (procurements != null)
                for (int i = 0; i < 20; i++)
                    items.Add(procurements[i]);
        }
        catch { }
        View.ItemsSource = items;
    }

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (View.SelectedIndex != -1)
            new ProcurementCard((Procurement)View.SelectedItem).ShowDialog();
    }
}