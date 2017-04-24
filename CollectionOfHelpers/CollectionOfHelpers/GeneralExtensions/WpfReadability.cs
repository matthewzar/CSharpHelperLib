using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CollectionOfHelpers.GeneralExtensions
{
    /// <summary>
    /// This class provides extension methods that make complex-looking WPF operations much simpler to read.
    /// The logic behind these methods is usually not complex, but they obfuscate syntactic complexity.
    /// </summary>
    public static class WpfReadability
    {
        /// <summary>
        /// Returns True only if the ToggleButton in question is fully checked - bypassing the normal 3 state check.
        /// </summary>
        /// <param name="toggleButton"></param>
        /// <returns></returns>
        public static bool IsFullyChecked(this System.Windows.Controls.Primitives.ToggleButton toggleButton)
        {
            return toggleButton.IsChecked == true;
        }
    }
}
