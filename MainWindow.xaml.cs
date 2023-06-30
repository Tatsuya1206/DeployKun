using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeployKun
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Uri uri = new Uri("/Deploy.xaml", UriKind.Relative);
            frame.Source = uri;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Deploy dep = new Deploy();
            //dep.SaveInputData(false);
            e.Cancel = true;  // ウィンドウのクローズをキャンセル
            this.WindowState = WindowState.Minimized;  // ウィンドウを最小化
            // 常駐状態だと実行条件の変更が、キャッチ出来ない為、
            // ×ボタン押下時に実行条件を自動保存
            // 常駐状態では、自動保存された実行条件を使用する。
            
        }
    }
}
