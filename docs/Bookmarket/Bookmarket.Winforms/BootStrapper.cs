using Autofac;

namespace Bookmarket.Winforms
{
    internal class BootStrapper
    {
        public IContainer BootStrap()
        {
            var builder = new ContainerBuilder();
            // builder.RegisterType<MainForm>()
            //     .As<IMainFormView>().SingleInstance();

            builder.RegisterType<MainForm>().AsSelf();

            return builder.Build();
        }
    }
}
