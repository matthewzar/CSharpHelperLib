using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CollectionOfHelpers.UI
{
    /// <summary>
    /// A collection of common event handlers for common tasks, such as cancelling 
    /// certain types of input and spawning new dialogs on their own threads
    /// </summary>
    public static class CommonEventHandlers
    {
        #region TextBox numeric filter
        /// <summary>
        /// Keys that will accepted by number-only text boxes. 
        /// </summary>
        private static readonly Key[] NumericKeys =
        {
            Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.D0,
            Key.NumPad0, Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4,
            Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9, Key.Tab
        };

        /// <summary>
        /// Cancel non-numeric key down events by marking them as handled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void CancelNonNumericKeystrokes(object sender, KeyEventArgs e)
        {
            if (!NumericKeys.Contains(e.Key))
            {
                e.Handled = true;
            }
        }
        #endregion
    }
}
