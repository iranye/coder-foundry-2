using Autofac;
using Bookmarket.Winforms.BusinessLogic;

namespace Bookmarket.Winforms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += ApplicationOnThreadException;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            ApplicationConfiguration.Initialize();

            var bootStrapper = new BootStrapper();
            var container = bootStrapper.BootStrap();
            var mainWindow = container.Resolve<MainForm>();
            Application.Run(mainWindow);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exceptionMessage = ((Exception)e.ExceptionObject).Message;
            var message = $"Something went wrong. Please contact support.\r\n{exceptionMessage}";

            // TODO: Write to log file
            Console.WriteLine($"ERROR {DateTimeOffset.Now}: {exceptionMessage}");

            MessageBox.Show(message, "Unexpected Error");
        }

        private static void ApplicationOnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var message = $"Something went wrong. Please contact support.\r\n{e.Exception.Message}";

            // TODO: Write to log file
            Console.WriteLine($"ERROR {DateTimeOffset.Now}: {e.Exception}");

            MessageBox.Show(message, "Unexpected Error");
        }
    }
}