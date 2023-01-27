using System.Reflection;
using System.Windows.Controls;

namespace Bookmarket.UI.Controls
{
    /// <summary>
    /// Interaction logic for HeaderControl.xaml
    /// </summary>
    public partial class HeaderControl : UserControl
    {
        public HeaderControl()
        {
            InitializeComponent();

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var versionStr = version is null ? "v1.0.1" : version.ToString().Substring(0, version.ToString().LastIndexOf("."));
            Version.Text = $"v{versionStr}";
        }
    }
}
