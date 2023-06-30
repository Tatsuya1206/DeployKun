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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using Path = System.IO.Path;

namespace DeployKun
{
    /// <summary>
    /// Deploy.xaml の相互作用ロジック
    /// </summary>
    public partial class Deploy : Page
    {
        private const string XML_FILE_FOLODER_NAME = "ExecCondition";
        private const string AUTO_SAVE_NAME = "AutoSaveExecCondition.xml";
        private const string BIN_DEPLOY_BAT = "\\Bin_Deploy.bat";
        private const string ASPX_DEPLOY_BAT = "\\Aspx_Deploy.bat";
        private const string JS_DEPLOY_BAT = "\\Js_Deploy.bat";
        private const string GR_RESTART_BAT = "\\GRReStart.bat";
        private const string BLANK = " ";
        private const string MSG_FOLDER = "フォルダ";
        private const string MSG_FILE = "ファイル";
        private const string IMAGE_DEPLOY_KUN_PATH = "Images/DeployKun.png";
        private const string IMAGE_DEPLOY_OK_PATH = "Images/DeployOK.png";
        private const string IMAGE_DEPLOY_NG_PATH = "Images/DeployNG.png";
        private const string IMAGE_DEPLOY_ING_PATH = "Images/Deploying.png";
        private const string INFO_CAPTION = "インフォメーション";
        private const string BROWSER_CHROME = "1";
        private const string BROWSER_EDGE = "2";
        private const string BROWSER_EDGE_IE_MODE = "3";
        private const string NOT_EXISTS = " は存在しません。";

        //private bool grReStartCheck = false;

        //public bool GrRestartCheck
        //{
        //    get { return grReStartCheck; }
        //    set { grReStartCheck = value; }
        //}

        

        public Deploy()
        {
            InitializeComponent();
            // 前回実行条件読込
            string filePath = Path.Combine(XML_FILE_FOLODER_NAME, AUTO_SAVE_NAME);
            Chrome.IsChecked = true;
            LoadInputData(filePath);
            // 読取専用項目設定
            setReadOnlyText();
            // 項目活性制御
            setEnableBrowserCheck();
            // 初期デプロイ君イメージ設定
            setImage(IMAGE_DEPLOY_KUN_PATH);
            // 必須項目設定
            setRequiredColor();
            // 初期フォーカス
            ReadSetFile.Focus();
        }

        /// <summary>
        /// binパス参照ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefBinPathBtn_Click(object sender, RoutedEventArgs e)
        {
            // V1.2改修　From
            string tmpPath = getPath(MSG_FOLDER, BinPathText.Text);
            BinPathText.Text = string.IsNullOrEmpty(tmpPath) ? BinPathText.Text : tmpPath;
            // V1.2改修　To
            // 項目の必須状態設定
            setRequiredColor();
        }
        /// <summary>
        /// ファイルパス参照ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefFilePathBtn_Click(object sender, RoutedEventArgs e)
        {
            // V1.2改修　From
            string tmpPath = getPath(MSG_FOLDER, FilePathText.Text);
            FilePathText.Text = string.IsNullOrEmpty(tmpPath) ? FilePathText.Text : tmpPath;
            // V1.2改修　To
            // 項目の必須状態設定
            setRequiredColor();
        }
        /// <summary>
        /// アプリケーションパス参照ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefAppPathBtn_Click(object sender, RoutedEventArgs e)
        {
            // V1.2改修　From
            string tmpPath = getPath(MSG_FOLDER, AppPathText.Text);
            AppPathText.Text = string.IsNullOrEmpty(tmpPath) ? AppPathText.Text : tmpPath;
            // V1.2改修　To
            // 項目の必須状態設定
            setRequiredColor();
        }
        /// <summary>
        /// 前回実行条件読込ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadSetFile_Click(object sender, RoutedEventArgs e)
        {
            string readFilePath = GetXmlFilePath(MSG_FILE);
            if (!string.IsNullOrEmpty(readFilePath))
            {
                LoadInputData(readFilePath);
                ShowInfoMsg("指定の実行条件を読込ました。");
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// フォルダパス取得
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string getPath(string msg,string initialDirectory)
        {
            string fileName = string.Empty;

            using (var cofd = new CommonOpenFileDialog()
            {
                Title = msg + "を選択してください",
                // フォルダ選択モードにする
                IsFolderPicker = true,
                // V1.2改修　From
                InitialDirectory = initialDirectory
                // V1.2改修　To
            })
            {
                if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return fileName;
                }

                // FileNameで選択されたフォルダを取得する
                fileName = cofd.FileName;
            }

            return fileName;
        }

        /// <summary>
        /// xmlファイルパスの取得
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string GetXmlFilePath(string msg)
        {
            string filePath = string.Empty;

            using (var cofd = new CommonOpenFileDialog()
            {
                Title = msg + "を選択してください",
                // ファイル選択モードにする
                IsFolderPicker = false,
                // ファイル選択フィルターを追加
                DefaultExtension = ".xml",
                EnsureFileExists = true,
                EnsurePathExists = true,
                // V1.2改修　From
                InitialDirectory = Directory.GetCurrentDirectory() + "\\" + XML_FILE_FOLODER_NAME
                // V1.2改修　To
            })
            {
                cofd.Filters.Add(new CommonFileDialogFilter("XML ファイル", "*.xml"));
                if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return filePath;
                }

                // 選択されたファイルパスを取得する
                filePath = cofd.FileName;
            }

            return filePath;
        }

        /// <summary>
        /// リストボックス追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddIDBtn_Click(object sender, RoutedEventArgs e)
        {
            // V1.2改修　From
            if (!string.IsNullOrEmpty(TagetIDText.Text))
            {
                TargetIDList.Items.Add(new TargetList { No = TargetIDList.Items.Count + 1, ID = TagetIDText.Text });
            }
            else
            {
                ShowErrMsg("機能IDを指定してください。");
                setImage(IMAGE_DEPLOY_NG_PATH);
            }
            // V1.2改修　To

        }

        /// <summary>
        /// リストボックス削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteIDBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TargetIDList.SelectedItems.Count == 0) return;

            TargetIDList.Items.RemoveAt(TargetIDList.SelectedIndex);

            setTargetIDNo();
        }

        // V1.2改修　From
        private void setTargetIDNo()
        {
            List<string> ID = new List<string>();
            foreach(TargetList item in TargetIDList.Items)
            {
                ID.Add(item.ID);
            }
            TargetIDList.Items.Clear();

            for (int i = 0; i < ID.Count; i++) 
            {
                TargetIDList.Items.Add(new TargetList { No = i + 1, ID = ID[i] });
            }
        }
        // V1.2改修　To

        /// <summary>
        /// デプロイボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void DeployBtn_Click(object sender, RoutedEventArgs e)
        {
            // エラークリア
            ClearErrs();
            
            if (hasErroes())
            {
                // エラーがある場合
                setImage(IMAGE_DEPLOY_NG_PATH);
                return;
            }
            else
            {
                // エラーがない場合
                try
                {
                    // 実行条件の自動保存
                    SaveInputData(SaveConditionCheckBox.IsChecked ?? false);
                    // デプロイ実施
                    setImage(IMAGE_DEPLOY_ING_PATH);
                    ShowInfoMsg("デプロイ実行中");
                    await Task.Delay(1000);

                    ClearErrs();
                    int code = CallDeploy();
                    if (code != 0)
                    {
                        // バッチ群でエラーが発生している場合、ログ確認させる。
                        // エラーを出しててもデプロイ出来ている場合があるため。
                        setImage(IMAGE_DEPLOY_NG_PATH);
                        ShowInfoMsg("デプロイ完了しました。実行されたバッチでエラーが発生しました。ログを確認してください。");
                        return;
                    }
                    else
                    {
                        // 正常終了の場合
                        setImage(IMAGE_DEPLOY_OK_PATH);
                        ShowInfoMsg("デプロイ完了しました。");
                    }

                    if (GRReStart.IsChecked ?? false)
                    {
                        // バッチ群でエラーが発生している場合、ログ確認させる。
                        // エラーを出しててもデプロイ出来ている場合があるため。
                        // そのため、再起動のチェックがある場合、強制的に再起動に入る。
                        CallGrReStart();
                    }
                }
                catch (Exception ex)
                {
                    ShowErrMsg("例外エラー：" + ex.Message);
                    setImage(IMAGE_DEPLOY_NG_PATH);
                }

            }
        }
        /// <summary>
        /// エラーチェック
        /// </summary>
        /// <returns></returns>
        public bool hasErroes()
        {
            bool ret = false;

            // 必須チェック
            ret = hasRequiredErr();

            if (!ret)
            {
                // 条件チェック
                ret = hasConditionsErr();
            }

            return ret;
        }
        /// <summary>
        /// 画像の切り替え
        /// </summary>
        /// <param name="path"></param>
        private void setImage(string path)
        {
            BitmapImage bmpImage = new BitmapImage();
            using (FileStream stream = File.OpenRead(path))
            {
                bmpImage.BeginInit();
                bmpImage.StreamSource = stream;
                bmpImage.DecodePixelWidth = 500;
                bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                bmpImage.CreateOptions = BitmapCreateOptions.None;
                bmpImage.EndInit();
                bmpImage.Freeze();
            }

            DeployKun.Source = bmpImage;
        }
        /// <summary>
        /// デプロイバッチ群の呼出し。
        /// </summary>
        /// <returns></returns>
        public int CallDeploy()
        {
            int errCode = 0;

            string currentDir = Directory.GetCurrentDirectory();

            try
            {
                // .bin,.pdbのデプロイ
                if (Bin_Pdb_CheckBox.IsChecked ?? false)
                {
                    Process binDeploy = new Process();
                    binDeploy.StartInfo.FileName = currentDir + BIN_DEPLOY_BAT;
                    binDeploy.StartInfo.Verb = "RunAs";
                    binDeploy.StartInfo.CreateNoWindow = true; // コマンドプロンプトを表示
                    binDeploy.StartInfo.UseShellExecute = false; // シェル機能オフ
                    StringBuilder binArgs = new StringBuilder();

                    binArgs.Append(BinPathText.Text);   //binのパス
                    binArgs.Append(BLANK);
                    binArgs.Append(AppPathText.Text);   //アプリケーションのパス

                    binDeploy.StartInfo.Arguments = binArgs.ToString();
                    // 実行
                    binDeploy.Start();
                    // 待機
                    binDeploy.WaitForExit();
                    // 戻り値
                    errCode = binDeploy.ExitCode;
                }

                // .aspxのデプロイ
                if (Aspx_CheckBox.IsChecked ?? false)
                {
                    Process aspxDeploy = new Process();
                    aspxDeploy.StartInfo.FileName = currentDir + ASPX_DEPLOY_BAT;
                    aspxDeploy.StartInfo.Verb = "RunAs";
                    aspxDeploy.StartInfo.CreateNoWindow = true; // コマンドプロンプトを表示
                    aspxDeploy.StartInfo.UseShellExecute = false; // シェル機能オフ
                    StringBuilder aspxArgs = new StringBuilder();

                    aspxArgs.Append(FilePathText.Text);     // Aspxのファイルパス
                    aspxArgs.Append(BLANK);
                    aspxArgs.Append(AppPathText.Text);      // アプリケーションパス

                    if (TargetIDList.Items.Count > 0)
                    {
                        foreach (TargetList item in TargetIDList.Items)
                        {
                            // 指定された機能IDの数だけデプロイ実施
                            aspxDeploy.StartInfo.Arguments = aspxArgs.ToString() + BLANK + item.ID;
                            aspxDeploy.Start();
                            aspxDeploy.WaitForExit();
                            errCode = aspxDeploy.ExitCode;
                        }
                    }
                    else
                    {
                        // 機能IDが指定されていない場合、全ファイルをデプロイ
                        aspxDeploy.StartInfo.Arguments = aspxArgs.ToString();
                        aspxDeploy.Start();
                        aspxDeploy.WaitForExit();
                        errCode = aspxDeploy.ExitCode;
                    }
                }

                // .jsのデプロイ
                if (Js_CheckBox.IsChecked ?? false)
                {
                    Process jsDeploy = new Process();
                    jsDeploy.StartInfo.FileName = currentDir + JS_DEPLOY_BAT;
                    jsDeploy.StartInfo.Verb = "RunAs";
                    jsDeploy.StartInfo.CreateNoWindow = true; // コマンドプロンプトを表示
                    jsDeploy.StartInfo.UseShellExecute = false; // シェル機能オフ
                    StringBuilder jsArgs = new StringBuilder();

                    jsArgs.Append(FilePathText.Text);   //jsファイルのパス
                    jsArgs.Append(BLANK);
                    jsArgs.Append(AppPathText.Text);    //アプリケーションパス

                    if (TargetIDList.Items.Count > 0)
                    {
                        foreach (TargetList item in TargetIDList.Items)
                        {
                            // 指定された機能IDの数だけデプロイ実施
                            jsDeploy.StartInfo.Arguments = jsArgs.ToString() + BLANK + item.ID;
                            jsDeploy.Start();
                            jsDeploy.WaitForExit();
                            errCode = jsDeploy.ExitCode;
                        }
                    }
                    else
                    {
                        // 機能IDが指定されていない場合、全ファイルをデプロイ
                        jsDeploy.StartInfo.Arguments = jsArgs.ToString();
                        jsDeploy.Start();
                        jsDeploy.WaitForExit();
                        errCode = jsDeploy.ExitCode;
                    }
                }
                return errCode;
            }
            catch
            {
                errCode = 99999;
                throw;
            }
        }

        /// <summary>
        /// GR再起動バッチの呼出し。
        /// </summary>
        public void CallGrReStart()
        {
            string currentDir = Directory.GetCurrentDirectory();

            Process grReStart = new Process();
            grReStart.StartInfo.FileName = currentDir + GR_RESTART_BAT;
            grReStart.StartInfo.Verb = "RunAs";
            grReStart.StartInfo.CreateNoWindow = true; // コマンドプロンプトを表示
            grReStart.StartInfo.UseShellExecute = false; // シェル機能オフ
            StringBuilder grReStartArgs = new StringBuilder();
            string path = AppPathText.Text;
            string folderName = path.Substring(path.LastIndexOf('\\') + 1);
            string appPath = Path.Combine("http://localhost/", folderName);
            grReStartArgs.Append(appPath);
            grReStartArgs.Append(BLANK);
            //grReStartArgs.Append(Chrome.IsChecked ?? false ? "1" : "0");

            if(Chrome.IsChecked ?? false)
            {
                grReStartArgs.Append(BROWSER_CHROME);
            }
            else if(Edge.IsChecked ?? false)
            {
                grReStartArgs.Append(BROWSER_EDGE);
            }
            else
            {
                grReStartArgs.Append(BROWSER_EDGE_IE_MODE);
            }

            grReStart.StartInfo.Arguments = grReStartArgs.ToString();
            grReStart.Start();
        }

        /// <summary>
        /// 画面実行条件の保存
        /// </summary>
        /// <param name="saveFlg"></param>
        public void SaveInputData(bool saveFlg)
        {
            //ファイルの保存先とファイル名を指定
            string fileName = string.Empty;
            if (saveFlg)
            {
                // 実行条件の保存をする場合、指定のファイル名で保存する。
                fileName = Path.Combine(XML_FILE_FOLODER_NAME, ExecConditionName.Text + ".xml");
            }
            else
            {
                // 実行条件の保存をしない場合、自動保存
                fileName = Path.Combine(XML_FILE_FOLODER_NAME, AUTO_SAVE_NAME);
            }
            // XMLファイルを作成または開く
            XDocument xml = new XDocument();
            XElement root = new XElement("Root");
            xml.Add(root);

            // テキストボックスの値をXMLに保存
            root.Add(new XElement("BinPathText", BinPathText.Text));
            root.Add(new XElement("FilePathText", FilePathText.Text));
            root.Add(new XElement("AppPathText", AppPathText.Text));

            // チェックボックスの値をXMLに保存
            root.Add(new XElement("Bin_Pdb_CheckBox", Bin_Pdb_CheckBox.IsChecked.ToString()));
            root.Add(new XElement("Aspx_CheckBox", Aspx_CheckBox.IsChecked.ToString()));
            root.Add(new XElement("Js_CheckBox", Js_CheckBox.IsChecked.ToString()));

            // チェックボックスの値をXMLに保存
            root.Add(new XElement("GRReStart", GRReStart.IsChecked.ToString()));
            root.Add(new XElement("Chrome", Chrome.IsChecked.ToString()));
            root.Add(new XElement("Edge", Edge.IsChecked.ToString()));
            root.Add(new XElement("EdgeIEMode", EdgeIEMode.IsChecked.ToString()));

            // リストボックスの選択肢をXMLに保存
            var listBoxItems = TargetIDList.Items.OfType<TargetList>();
            foreach (TargetList item in listBoxItems)
            {
                XElement listBoxItemElement = new XElement("TargetIDListItem");
                listBoxItemElement.Add(new XElement("No", item.No));
                listBoxItemElement.Add(new XElement("ID", item.ID));
                root.Add(listBoxItemElement);
            }
            try
            {
                xml.Save(fileName);
            }
            catch (Exception ex)
            {
                ShowErrMsg("例外エラー：" + ex.Message);
                setImage(IMAGE_DEPLOY_NG_PATH);
                throw;
            }
            
        }

        /// <summary>
        /// 画面実行条件の読込
        /// </summary>
        /// <param name="fileName"></param>
        private void LoadInputData(string fileName)
        {
            if (File.Exists(fileName))
            {
                // XMLファイルから画面入力項目を読み込み
                XDocument xml = XDocument.Load(fileName);

                // テキストボックスの値を復元
                BinPathText.Text = GetValueFromXml(xml, "BinPathText");
                FilePathText.Text = GetValueFromXml(xml, "FilePathText");
                AppPathText.Text = GetValueFromXml(xml, "AppPathText");

                // チェックボックスの状態を復元
                Bin_Pdb_CheckBox.IsChecked = GetBooleanValueFromXml(xml, "Bin_Pdb_CheckBox");
                Aspx_CheckBox.IsChecked = GetBooleanValueFromXml(xml, "Aspx_CheckBox");
                Js_CheckBox.IsChecked = GetBooleanValueFromXml(xml, "Js_CheckBox");

                GRReStart.IsChecked = GetBooleanValueFromXml(xml, "GRReStart");
                Chrome.IsChecked = GetBooleanValueFromXml(xml, "Chrome");
                Edge.IsChecked = GetBooleanValueFromXml(xml, "Edge");
                EdgeIEMode.IsChecked = GetBooleanValueFromXml(xml, "EdgeIEMode");

                // V1.2改修　From
                // 実行条件読込時に機能IDリストが残る不具合修正
                TargetIDList.Items.Clear();
                // V1.2改修　To

                if (xml.Root.Elements("TargetIDListItem").Any())
                {
                    var listBoxItems = xml.Root.Elements("TargetIDListItem")
                        .Select(element => new TargetList
                        {
                            No = int.Parse(GetValueFromXml(element, "No")),
                            ID = GetValueFromXml(element, "ID")
                        });

                    TargetIDList.Items.Clear();
                    foreach (var item in listBoxItems)
                    {
                        TargetIDList.Items.Add(item);
                    }
                }
            }

            BinPathText.Focus();
        }

        /// <summary>
        /// XMLファイルより、テキストボックスの値を取得
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        private string GetValueFromXml(XDocument xml, string elementName)
        {
            XElement element = xml.Root.Element(elementName);
            return element?.Value;
        }

        /// <summary>
        /// XMLファイルより、リストボックスの値を取得
        /// </summary>
        /// <param name="element"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        private string GetValueFromXml(XElement element, string elementName)
        {
            XElement childElement = element.Element(elementName);
            return childElement?.Value;
        }
        /// <summary>
        /// XMLファイルより、チェックボックスの値を取得
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        private bool GetBooleanValueFromXml(XDocument xml, string elementName)
        {
            XElement element = xml.Root.Element(elementName);
            if (element != null && bool.TryParse(element.Value, out bool value))
            {
                return value;
            }
            return false;
        }

        /// <summary>
        /// 実行条件保存にチェック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveConditionCheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            setReadOnlyText();
        }

        /// <summary>
        /// 読取専用の切り替え
        /// </summary>
        private void setReadOnlyText()
        {
            if (SaveConditionCheckBox.IsChecked ?? false)
            {
                ExecConditionName.IsReadOnly = false;
                ExecConditionName.Background = Brushes.White;
            }
            else
            {
                ExecConditionName.IsReadOnly = true;
                ExecConditionName.Text = string.Empty;
                ExecConditionName.Background = Brushes.LightGray;
            }
        }

        /// <summary>
        /// GR再起動クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GRReStart_Click(object sender, RoutedEventArgs e)
        {
            InputData.GrReStart = GRReStart.IsChecked ?? false;
            setEnableBrowserCheck();
        }
        /// <summary>
        /// ブラウザ選択チェックボックスの活性制御
        /// </summary>
        private void setEnableBrowserCheck()
        {
            if (GRReStart.IsChecked ?? false)
            {
                Chrome.IsEnabled = true;
                Edge.IsEnabled = true;
                EdgeIEMode.IsEnabled = true;
            }
            else
            {
                Chrome.IsEnabled = false;
                Edge.IsEnabled = false;
                EdgeIEMode.IsEnabled = false;
            }
        }
        /// <summary>
        /// 必須項目チェック
        /// </summary>
        /// <returns>true:エラー有り/false:エラー無し</returns>
        private bool hasRequiredErr()
        {
            bool ret = false;

            if (string.IsNullOrEmpty(BinPathText.Text))
            {
                ShowErrMsg(BinPathLabel.Content.ToString() + "を指定してください。");
                BinPathText.Focus();
                ret = true;
                return ret;
            }
            // V1.2改修　From
            else if (!Directory.Exists(BinPathText.Text))
            {
                ShowErrMsg(BinPathText.Text + NOT_EXISTS);
                BinPathText.Focus();
                ret = true;
                return ret;
            }
            // V1.2改修　To

            if (string.IsNullOrEmpty(FilePathText.Text))
            {
                ShowErrMsg(FilePathLabel.Content.ToString() + "を指定してください。");
                BinPathText.Focus();
                ret = true;
                return ret;
            }
            // V1.2改修　From
            else if (!Directory.Exists(FilePathText.Text))
            {
                ShowErrMsg(FilePathText.Text + NOT_EXISTS);
                FilePathText.Focus();
                ret = true;
                return ret;
            }
            // V1.2改修　To

            if (string.IsNullOrEmpty(AppPathText.Text))
            {
                ShowErrMsg(AppPathLabel.Content.ToString() + "を指定してください。");
                BinPathText.Focus();
                ret = true;
                return ret;
            }
            // V1.2改修　From
            else if (!Directory.Exists(AppPathText.Text))
            {
                ShowErrMsg(AppPathText.Text + NOT_EXISTS);
                AppPathText.Focus();
                ret = true;
                return ret;
            }
            // V1.2改修　To

            // 拡張子
            if (Bin_Pdb_CheckBox.IsChecked == false &&
                Aspx_CheckBox.IsChecked == false &&
                Js_CheckBox.IsChecked == false)
            {
                ShowErrMsg("デプロイする拡張子を指定してください。");
                Bin_Pdb_CheckBox.Focus();
                ret = true;
                return ret;
            }
            return ret;
        }
        /// <summary>
        /// エラーメッセージ表示
        /// </summary>
        /// <param name="errMsg"></param>
        private void ShowErrMsg(string errMsg)
        {
            MsgLabel.Visibility = Visibility.Visible;
            MsgLabel.Content = errMsg;
            MsgLabel.Background = Brushes.Red;
            MsgLabel.Foreground = Brushes.White;
        }
        private void ShowInfoMsg(string infoMsg)
        {
            MsgLabel.Visibility = Visibility.Visible;
            MsgLabel.Content = infoMsg;
            MsgLabel.Background = Brushes.Yellow;
            MsgLabel.Foreground = Brushes.Black;
        }
        /// <summary>
        /// 条件チェック
        /// </summary>
        /// <returns>true:エラー有り/false:エラー無し</returns>
        private bool hasConditionsErr()
        {
            bool ret = false;
            if (SaveConditionCheckBox.IsChecked == true)
            {
                //if (string.IsNullOrEmpty(ExecConditionName.Text))
                //{
                //    ShowErrMsg("実行条件を保存する場合は、ファイル名を指定してください。");
                //    ExecConditionName.Focus();
                //    ret = true;
                //    return ret;
                //}
                if (string.IsNullOrEmpty(ExecConditionName.Text))
                {
                    ShowErrMsg("実行条件を保存する場合は、ファイル名を指定してください。");
                    ExecConditionName.Focus();
                    ret = true;
                    return ret;
                }
                else if (ExecConditionName.Text.IndexOf("/") > 0)
                {
                    ShowErrMsg("ファイル名が不正です。");
                    ExecConditionName.Focus();
                    ret = true;
                    return ret;
                }
            }
            
            if ((Aspx_CheckBox.IsChecked ?? false == true) || (Js_CheckBox.IsChecked ?? false == true))
            {
                //aspx,jsにチェックが入っている場合、全デプロイの有無を確認する。

                if (TargetIDList.Items.Count == 0)
                {
                    if (!isAllDeploy())
                    {
                        // 機能IDを指定せず、全デプロイを拒否した場合、エラーとする。
                        ShowErrMsg("個別にデプロイする場合は、機能IDを追加してください。");
                        TagetIDText.Focus();
                        ret = true;
                        return ret;
                    }
                }
            }
            
            //if (GRReStart.IsChecked == true)
            //{
            //    if (Chrome.IsChecked == false && Edge.IsChecked == false && EdgeIEMode.IsChecked == false)
            //    {
            //        ShowErrMsg("GRを再起動する場合は、対象ブラウザを指定してください。");
            //        Chrome.Focus();
            //        ret = true;
            //        return ret;
            //    }
            //}
            return ret;
        }
        /// <summary>
        /// 実行対象の確認
        /// </summary>
        /// <returns>true:全ファイル実行/false:実行条件エラー</returns>
        private bool isAllDeploy()
        {
            bool isExec = false;

            string msg = "機能IDが追加されていません。" + Environment.NewLine + "すべてのファイルをデプロイしてよろしいですか？";
            string caption = INFO_CAPTION;
            MessageBoxButton btn = MessageBoxButton.OKCancel;
            MessageBoxImage img = MessageBoxImage.Information;
            ShowDialogMsg(msg, caption, btn, img, out isExec);

            return isExec;
        }
        /// <summary>
        /// ダイアログメッセージ表示
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <param name="caption">メッセージタイトル</param>
        /// <param name="btn">ボタンの種類</param>
        /// <param name="icon">表示アイコン</param>
        /// <param name="isExec">OUT:実行状態</param>
        public void ShowDialogMsg(string msg, string caption, MessageBoxButton btn, MessageBoxImage icon, out bool isExec)
        {
            isExec = false;
            MessageBoxResult ret = MessageBox.Show(msg, caption, btn, icon);
            switch (ret)
            {
                case MessageBoxResult.Yes:
                case MessageBoxResult.OK:
                    isExec = true;
                    break;
                case MessageBoxResult.None:
                case MessageBoxResult.No:
                case MessageBoxResult.Cancel:
                    isExec = false;
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// エラーメッセージの非表示
        /// </summary>
        private void ClearErrs()
        {
            MsgLabel.Visibility = Visibility.Hidden;
            // 一度、全実行状態にすると値が引き継がれるため、
            // デプロイボタン押下時に初期値に戻す。
            setImage(IMAGE_DEPLOY_KUN_PATH);
        }

        private void setRequiredColor()
        {
            BinPathText.Background = string.IsNullOrEmpty(BinPathText.Text) ? Brushes.LightPink : Brushes.White;
            FilePathText.Background = string.IsNullOrEmpty(FilePathText.Text) ? Brushes.LightPink : Brushes.White;
            AppPathText.Background = string.IsNullOrEmpty(AppPathText.Text) ? Brushes.LightPink : Brushes.White;
        }

        private void BinPathText_LostFocus(object sender, RoutedEventArgs e)
        {
            setRequiredColor();
        }

        private void FilePathText_LostFocus(object sender, RoutedEventArgs e)
        {
            setRequiredColor();
        }

        private void AppPathText_LostFocus(object sender, RoutedEventArgs e)
        {
            setRequiredColor();
        }

        //private void Chrome_Checked(object sender, RoutedEventArgs e)
        //{
        //    if(Chrome.IsChecked ?? false == true)
        //    {
        //        Edge.IsChecked = false;
        //    }
        //}

        //private void Edge_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (Edge.IsChecked ?? false == true)
        //    {
        //        Chrome.IsChecked = false;
        //    }
        //}
    }
    /// <summary>
    /// リストボックスのItemクラス
    /// </summary>
    public class TargetList
    {
        public int No { get; set; }
        public string ID { get; set; }

    }

    public static class InputData
    {
        public static bool GrReStart { get; set; }
    }
}
