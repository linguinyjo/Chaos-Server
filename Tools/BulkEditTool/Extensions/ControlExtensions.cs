using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace BulkEditTool.Extensions;

public static class ControlExtensions
{
    public static T? FindVisualChild<T>(this DependencyObject obj) where T: DependencyObject
    {
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            var child = VisualTreeHelper.GetChild(obj, i);

            if (child is T dependencyObject)
                return dependencyObject;

            var childOfChild = FindVisualChild<T>(child);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (childOfChild is not null)
                return childOfChild;
        }

        return null;
    }

    public static DataGridCell? GetCell(this DataGrid dataGrid, DataGridRow? rowContainer, int column)
    {
        if (rowContainer != null)
        {
            var presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);

            if (presenter == null)
            {
                /* if the row has been virtualized away, call its ApplyTemplate() method
                 * to build its visual tree in order for the DataGridCellsPresenter
                 * and the DataGridCells to be created */
                rowContainer.ApplyTemplate();
                presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
            }

            if (presenter != null)
            {
                var cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;

                if (cell == null)
                {
                    /* bring the column into view
                     * in case it has been virtualized away */
                    dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                    cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                }

                return cell;
            }
        }

        return null;
    }

    public static void SelectCellByIndex(
        this DataGrid dataGrid,
        int rowIndex,
        int columnIndex,
        bool focus = true
    )
    {
        if (!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.Cell))
            throw new ArgumentException("The SelectionUnit of the DataGrid must be set to Cell.");

        if ((rowIndex < 0) || (rowIndex > dataGrid.Items.Count - 1))
            throw new ArgumentException($"{rowIndex} is an invalid row index.");

        if ((columnIndex < 0) || (columnIndex > dataGrid.Columns.Count - 1))
            throw new ArgumentException($"{columnIndex} is an invalid column index.");

        dataGrid.SelectedCells.Clear();

        var item = dataGrid.Items[rowIndex];
        var row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;

        if (row == null)
        {
            dataGrid.ScrollIntoView(item);
            row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
        }

        if (row != null)
        {
            var cell = GetCell(dataGrid, row, columnIndex);

            if (cell != null)
            {
                var dataGridCellInfo = new DataGridCellInfo(cell);
                dataGrid.SelectedCells.Add(dataGridCellInfo);

                if (focus)
                    cell.Focus();
            }
        }
    }
}