using System.Windows;
using System.Windows.Controls;

namespace DreamCatcher.Dialogs
{
    /// <summary>
    /// Interaction logic for ConfirmationDialog.xaml
    /// </summary>
    public partial class ConfirmationDialog : UserControl
    {
        private static DependencyProperty MessageProperty = DependencyProperty.Register("Message",
            typeof(string), typeof(ConfirmationDialog));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        private static DependencyProperty HeaderProperty = DependencyProperty.Register("Header",
            typeof(string), typeof(ConfirmationDialog));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public ConfirmationDialog()
        {
            InitializeComponent();
        }
    }
}
