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
    }
}