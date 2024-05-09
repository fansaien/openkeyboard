using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace OpenKeyboard{
    public partial class App : Application{

        private static Mutex _mutex = null;
        public void App_Startup(object sender, StartupEventArgs e) {
            DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
        }//event

        public void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            if(e.Exception.InnerException != null) vLogger.Exception("app.UnhandledException", e.Exception.InnerException);
            else vLogger.Exception("app.UnhandledException", e.Exception);

            e.Handled = true;
            Application.Current.Shutdown();
        }//func

        // WPF Single Instances
        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "OpenKeyboard";
            bool createdNew;

            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                Application.Current.Shutdown();
            }

            base.OnStartup(e);
        }
    }//cls
}//ns
