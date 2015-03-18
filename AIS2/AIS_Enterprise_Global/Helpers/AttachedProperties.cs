using System.Windows;
using System.Windows.Controls;

namespace AIS_Enterprise_Global.Helpers
{
    public class AttachedProperties : DependencyObject
    {

        #region RegisterBlackoutDates

        // Adds a collection of command bindings to a date picker's existing BlackoutDates collection, since the collections are immutable and can't be bound to otherwise.
        //
        // Usage: <DatePicker hacks:AttachedProperties.RegisterBlackoutDates="{Binding BlackoutDates}" >

        public static DependencyProperty RegisterBlackoutDatesProperty = DependencyProperty.RegisterAttached("RegisterBlackoutDates", typeof(CalendarBlackoutDatesCollection), typeof(AttachedProperties), new PropertyMetadata(null, OnRegisterCommandBindingChanged));

        public static void SetRegisterBlackoutDates(UIElement element, CalendarBlackoutDatesCollection value)
        {
            if (element != null)
                element.SetValue(RegisterBlackoutDatesProperty, value);
        }
        public static CalendarBlackoutDatesCollection GetRegisterBlackoutDates(UIElement element)
        {
            return (element != null ? (CalendarBlackoutDatesCollection)element.GetValue(RegisterBlackoutDatesProperty) : null);
        }
        private static void OnRegisterCommandBindingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DatePicker element = sender as DatePicker;
            if (element != null)
            {
                CalendarBlackoutDatesCollection bindings = e.NewValue as CalendarBlackoutDatesCollection;
                if (bindings != null)
                {
                    element.BlackoutDates.Clear();
                    foreach (var dateRange in bindings)
                    {
                        element.BlackoutDates.Add(dateRange);
                    }
                }
            }
        }

        #endregion
    }
}
