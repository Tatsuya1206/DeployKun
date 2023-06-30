using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DeployKun
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var icon = GetResourceStream(new Uri("DeployKun.ico", UriKind.Relative)).Stream;
            var menu = new System.Windows.Forms.ContextMenuStrip();
            menu.Items.Add("デプロイ！", null, Deploy_Click);
            menu.Items.Add("デプロイくん表示", null, Show_window);
            menu.Items.Add("終了", null, Exit_Click);

            var notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Visible = true,
                Icon = new System.Drawing.Icon(icon),
                Text = "デプロイくん",
                ContextMenuStrip = menu
            };
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(NotifyIcon_Click);
        }
        private void NotifyIcon_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                var wnd = new MainWindow();
                wnd.Show();
            }
        }

        private void Deploy_Click(object sender, EventArgs e)
        {
            Deploy();
        }
        private void Deploy()
        {
            Deploy dep = new Deploy();
            bool isExec = false;
            if (dep.hasErroes())
            {
                string msg = "実行条件エラー：設定を確認してください。";
                string caption = "エラー";
                MessageBoxButton btn = MessageBoxButton.OK;
                MessageBoxImage img = MessageBoxImage.Error;
                dep.ShowDialogMsg(msg, caption, btn, img, out isExec);
                if (isExec)
                {
                    Show_Display();
                }

            }
            else
            {
                int errCode = dep.CallDeploy();

                if (errCode > 0)
                {
                    string msg = "デプロイ完了しました。" + Environment.NewLine + "実行されたバッチでエラーが発生しました。ログを確認してください。";
                    string caption = "エラー";
                    MessageBoxButton btn = MessageBoxButton.OK;
                    MessageBoxImage img = MessageBoxImage.Error;
                    dep.ShowDialogMsg(msg, caption, btn, img, out isExec);
                }
                else
                {
                    string msg = "デプロイ完了しました。";
                    string caption = "インフォメーション";
                    MessageBoxButton btn = MessageBoxButton.OK;
                    MessageBoxImage img = MessageBoxImage.Information;
                    dep.ShowDialogMsg(msg, caption, btn, img, out isExec);
                    if (isExec)
                    {
                        if (InputData.GrReStart)
                        {
                            dep.CallGrReStart();
                        }

                    }
                }

            }
        }

        private void Show_window(object sender, EventArgs e)
        {
            Show_Display();
        }
        private void Show_Display()
        {
            this.MainWindow.WindowState = WindowState.Normal;
            this.MainWindow.Show();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Shutdown();
        }
    }
}
