using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        private IEnumerable<string> GetTags()
        {
            return new[]
            {
                "C#",
                "Web Dev",
                "DevOps",
                "Recreation",
                "Miscellaneous",
                "FromHtml"
            };
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (var tag in GetTags())
            {
                clbTags.Items.Add(tag);
            }
        }

        private void btnAddTag_Click(object sender, EventArgs e)
        {
            var tagText = txtNewTag.Text;
            if (String.IsNullOrWhiteSpace(tagText))
            {
                return;
            }

            foreach (var item in clbTags.Items)
            {
                var existingTag = (string)item;
                if (existingTag.ToLower() == tagText.ToLower())
                {
                    return;
                }
            }
            clbTags.Items.Add(tagText);
            txtNewTag.Text = String.Empty;
        }
    }
}