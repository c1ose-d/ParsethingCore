namespace ExportLibrary;

public static class ExportInstance
{
    public static void Run(DataGrid view)
    {
        Application excel = new() { Visible = true };
        Workbook workbook = excel.Workbooks.Add(Missing.Value);
        Worksheet sheet = (Worksheet)workbook.Sheets[1];

        for (int j = 0; j < view.Columns.Count; j++)
        {
            Range range = (Range)sheet.Cells[1, j + 1];
            range.Font.Bold = true;
            range.Font.Color = -15461187;
            range.Borders[xlEdgeLeft].LineStyle = xlLineStyleNone;
            range.Borders[xlEdgeRight].LineStyle = xlLineStyleNone;
            range.Borders[xlEdgeTop].LineStyle = xlLineStyleNone;
            range.Borders[xlEdgeBottom].LineStyle = xlContinuous;
            range.Borders[xlEdgeBottom].Weight = xlMedium;
            range.Borders[xlEdgeBottom].Color = -15461187;
            ((Range)sheet.Columns[j + 1]).ColumnWidth = 64;
            range.Value2 = view.Columns[j].Header;
        }

        for (int i = 0; i < view.Columns.Count; i++)
        {
            for (int j = 0; j < view.Items.Count; j++)
            {
                TextBlock b = (TextBlock)view.Columns[i].GetCellContent(view.Items[j]);
                Range range = (Range)sheet.Cells[j + 2, i + 1];
                range.Borders[xlEdgeLeft].LineStyle = xlLineStyleNone;
                range.Borders[xlEdgeRight].LineStyle = xlLineStyleNone;
                range.Borders[xlEdgeBottom].LineStyle = xlContinuous;
                range.Borders[xlEdgeBottom].Color = rgbBlack;
                range.Value2 = b.Text;
            }
        }
    }
}