using System.Text;

namespace Bookmarket.Winforms
{
    public interface IMainFormView
    {
        event EventHandler Load;
        //event FormClosedEventHandler FormClosed;
        //event HelpEventHandler HelpRequested;
        //event KeyEventHandler KeyDown;

        Color BackColor { get; set; }
        void ShowEpisodeView();
        void ShowPodcastView();
    }

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnImportHtml_Click(object sender, EventArgs e)
        {
            ClearOutput();
            var inputText = txtImport.Text;
            if (String.IsNullOrWhiteSpace(inputText))
            {
                PrintToOutput("Please enter some text in the uppert text box");
                return;
            }
        }

        private void btnImportJson_Click(object sender, EventArgs e)
        {
            ClearOutput();
            var inputText = txtImport.Text;
            if (String.IsNullOrWhiteSpace(inputText))
            {
                PrintToOutput("Please enter some text in the uppert text box");
                return;
            }
        }

        private void PrintToOutput(string message)
        {
            var sb = new StringBuilder();
            sb.AppendLine(message);
            txtOutput.Text += sb.ToString();
        }

        private void ClearOutput()
        {
            txtOutput.Text = String.Empty;
        }
    }
}