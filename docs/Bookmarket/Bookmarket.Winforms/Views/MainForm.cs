using System.Collections;
using System.Text;
using HtmlAgilityPack;

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

        private void TestHtmlParse()
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            var node = HtmlNode.CreateNode("<html><head></head><body></body></html>");
            doc.DocumentNode.AppendChild(node);
            var html = "<p><b>Test1</b></p><p>Test1 paragraph</p><p><b>Test2</b></p><p>Test2 paragraph</p><p><b>Test3</b></p><p>Test3 paragraph</p>";
            var str = "";
            doc.LoadHtml(html);

            IEnumerable nodes = doc.DocumentNode.ChildNodes.Where(n => n.Name.Contains("p")).ToList();

            var items = new List<Item>();
            var item = new Item();

            foreach (var paraNode in nodes)
            {
                var htmlNode = paraNode as HtmlNode;
                if (htmlNode is null)
                {
                    continue;
                }
                if (htmlNode.SelectSingleNode("./b") is not null)
                {
                    item = new Item();
                    item.Title = htmlNode.SelectSingleNode("./b").InnerText;
                }
                else
                {
                    item.Text = htmlNode.InnerText.Trim();
                    items.Add(item);
                }
            }

            foreach (var i in items)
            {
                PrintToOutput("Title: " + i.Title + ", Text: " + i.Text);
            }

            // Console.WriteLine(str);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            TestHtmlParse();
        }
    }
    public class Item
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }
}