using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace AIS_Enterprise_Global.Helpers
{
    public static class DataGridHelper
    {
        public static DataGridCell GetCell(this DataGrid dg, int row, int column)
        {
            DataGridRow rowContainer = GetRow(dg, row);

            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);

                // try to get the cell but it may possibly be virtualized
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                if (cell == null)
                {
                    // now try to bring into view and retreive the cell
                    dg.ScrollIntoView(rowContainer, dg.Columns[column]);
                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                }
                return cell;
            }
            return null;
        }

        public static DataGridRow GetRow(this DataGrid dg, int index)
        {
            DataGridRow row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                dg.ScrollIntoView(dg.Items[index]);
                row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        private static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        #region IsSingleClickInCell

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static readonly DependencyProperty IsSingleClickInCellProperty =
            DependencyProperty.RegisterAttached("IsSingleClickInCell",
            typeof(bool), typeof(DataGrid),
            new FrameworkPropertyMetadata(false, OnIsSingleClickInCellSet));

        public static void SetIsSingleClickInCell(UIElement element, bool value)
        {
            element.SetValue(IsSingleClickInCellProperty, value);
        }

        public static bool GetIsSingleClickInCell(UIElement element)
        {
            return (bool)element.GetValue(IsSingleClickInCellProperty);
        }

        private static void OnIsSingleClickInCellSet(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                if ((bool)e.NewValue)
                {
                    var dataGrid = sender as DataGrid;

                    EventManager.RegisterClassHandler(typeof(DataGridCell),
                        DataGridCell.PreviewMouseLeftButtonUpEvent,
                        new RoutedEventHandler(OnPreviewMouseLeftButtonDown));
                }
            }
        }

        private static void OnPreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;

            if (cell != null && !cell.IsEditing && !cell.IsReadOnly)
            {
                var checkBoxes = FindVisualChildren<CheckBox>(cell);

                if (checkBoxes != null && checkBoxes.Count() > 0)
                {
                    foreach (var checkBox in checkBoxes)
                    {
                        if (checkBox.IsEnabled)
                        {
                            checkBox.Focus();
                            checkBox.IsChecked = !checkBox.IsChecked;

                            var bindingExpression = checkBox.GetBindingExpression(CheckBox.IsCheckedProperty);

                            if (bindingExpression != null)
                            {
                                bindingExpression.UpdateSource();
                            }
                        }
                        break;
                    }
                }
            }
        }

        #endregion
    }
}
