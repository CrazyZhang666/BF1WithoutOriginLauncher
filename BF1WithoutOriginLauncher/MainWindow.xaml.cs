using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Win32;

using BF1WithoutOriginLauncher.Utils;
using BF1WithoutOriginLauncher.Helper;

namespace BF1WithoutOriginLauncher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<BitmapImage> BackImageList = new List<BitmapImage>();

        private int Index = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 主窗口加载完毕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Main_Loaded(object sender, RoutedEventArgs e)
        {
            BackImageList.Add(new BitmapImage(new Uri("/Assets/Images/Background/MP_Beachhead_BGLoop-4f04c02e.jpg", UriKind.RelativeOrAbsolute)));
            BackImageList.Add(new BitmapImage(new Uri("/Assets/Images/Background/MP_Harbor_BGLoop-70b9c5f4.jpg", UriKind.RelativeOrAbsolute)));
            BackImageList.Add(new BitmapImage(new Uri("/Assets/Images/Background/MP_Naval_BGLoop-dd6e9a89.jpg", UriKind.RelativeOrAbsolute)));
            BackImageList.Add(new BitmapImage(new Uri("/Assets/Images/Background/MP_Ridge_BGLoop-22333a03.jpg", UriKind.RelativeOrAbsolute)));

            Index = new Random().Next(4);

            ChangeBackgroundImage();

            /////////////////////////////////////////

            var resStream = Application.GetResourceStream(new Uri("/Assets/Arrow.cur", UriKind.Relative));
            Mouse.OverrideCursor = new Cursor(resStream.Stream, true);

            /////////////////////////////////////////

            TextBox_BF1GameDir.Text = IniHelper.ReadValue("Config", "BF1GameDir");
            TextBox_BF1RunArgs.Text = IniHelper.ReadValue("Config", "BF1RunArgs");

            CoreUtil.BF1_Game_Dir = TextBox_BF1GameDir.Text.Trim();
        }

        /// <summary>
        /// 主窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Main_Closing(object sender, CancelEventArgs e)
        {
            IniHelper.WriteValue("Config", "BF1GameDir", TextBox_BF1GameDir.Text.Trim());
            IniHelper.WriteValue("Config", "BF1RunArgs", TextBox_BF1RunArgs.Text.Trim());
        }

        /// <summary>
        /// 超链接请求导航事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            ProcessHelper.OpenLink(e.Uri.OriginalString);
            e.Handled = true;
        }

        /// <summary>
        /// 命令绑定 切换背景图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBinding_ChangeBackgroundImage(object sender, RoutedEventArgs e)
        {
            ChangeBackgroundImage();
        }

        /// <summary>
        /// 切换背景图片
        /// </summary>
        private void ChangeBackgroundImage()
        {
            if (Index >= BackImageList.Count)
                Index = 0;

            Image_Background.Source = BackImageList[Index++];
        }

        /// <summary>
        /// 选择战地1游戏所在文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_SelectBF1GameDir_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Title = "选择战地1游戏所在文件夹",
                RestoreDirectory = true,
                Multiselect = false,
                Filter = "可以执行程序|*.exe",
                FileName = "bf1.exe"
            };

            if (fileDialog.ShowDialog() == true)
            {
                TextBox_BF1GameDir.Text = Path.GetDirectoryName(fileDialog.FileName);

                CoreUtil.BF1_Game_Dir = TextBox_BF1GameDir.Text.Trim();
            }
        }

        /// <summary>
        /// 打开数据目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OpenAppDataDir_Click(object sender, RoutedEventArgs e)
        {
            ProcessHelper.OpenLink(CoreUtil.Root);
        }

        /// <summary>
        /// 打开战地1游戏目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OpenBF1GameDir_Click(object sender, RoutedEventArgs e)
        {
            if (!CoreUtil.IsExistsBF1GameDir())
                return;

            ProcessHelper.OpenLink(CoreUtil.BF1_Game_Dir);
        }

        /// <summary>
        /// 打开战地1文档目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OpenBF1DocDir_Click(object sender, RoutedEventArgs e)
        {
            if (!CoreUtil.IsExistsBF1DocDir())
                return;

            ProcessHelper.OpenLink(CoreUtil.BF1_Doc_Dir);
        }

        /// <summary>
        /// 编辑Origin模拟器配置文件（Cookies）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_EditOriginEmuConfig_Click(object sender, RoutedEventArgs e)
        {
            if (!CoreUtil.IsExistsBF1GameDir())
                return;

            ProcessHelper.OpenProcess(CoreUtil.Origin_config_ini, "notepad");
        }

        /// <summary>
        /// 使用战地1免Origin补丁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_UseWithoutOriginPath_Click(object sender, RoutedEventArgs e)
        {
            if (!CoreUtil.IsExistsBF1GameDir())
                return;

            try
            {
                File.Copy(CoreUtil.Patch_bf1_without_origin_exe, CoreUtil.Game_bf1_exe, true);
                File.Copy(CoreUtil.Patch_dinput8_dll, CoreUtil.Game_dinput8_dll, true);
                File.Copy(CoreUtil.Patch_dinput8_org_dll, CoreUtil.Game_dinput8_org_dll, true);
                File.Copy(CoreUtil.Patch_originemu_dll, CoreUtil.Game_originemu_dll, true);

                MsgBoxHelper.Information("使用战地1免Origin补丁成功");
            }
            catch (Exception ex)
            {
                MsgBoxHelper.Exception(ex);
            }
        }

        /// <summary>
        /// 恢复战地1原版文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_UseBackupBF1MainApp_Click(object sender, RoutedEventArgs e)
        {
            if (!CoreUtil.IsExistsBF1GameDir())
                return;

            try
            {
                if (File.Exists(CoreUtil.Game_bf1_without_origin_exe))
                    File.Delete(CoreUtil.Game_bf1_without_origin_exe);

                if (File.Exists(CoreUtil.Game_dinput8_dll))
                    File.Delete(CoreUtil.Game_dinput8_dll);

                if (File.Exists(CoreUtil.Game_dinput8_org_dll))
                    File.Delete(CoreUtil.Game_dinput8_org_dll);

                if (File.Exists(CoreUtil.Game_originemu_dll))
                    File.Delete(CoreUtil.Game_originemu_dll);

                File.Copy(CoreUtil.Backup_bf1_exe, CoreUtil.Game_bf1_exe, true);

                MsgBoxHelper.Information("恢复战地1原版文件成功");
            }
            catch (Exception ex)
            {
                MsgBoxHelper.Exception(ex);
            }
        }

        /// <summary>
        /// 运行战地1繁体中文注册表修复工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_UseBF1RegeditFix_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("你确定要运行战地1繁体中文注册表修复工具吗？这个一般在修改战地1语言为繁体中文时使用",
                "注册表修复工具", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                if (!CoreUtil.IsExistsBF1GameDir())
                    return;

                try
                {
                    File.Copy(CoreUtil.Tools_EA_Game_RegFix_exe, CoreUtil.Game_EA_Game_RegFix_exe, true);

                    ProcessHelper.OpenProcess(CoreUtil.Game_EA_Game_RegFix_exe);
                }
                catch (Exception ex)
                {
                    MsgBoxHelper.Exception(ex);
                }
            }
        }

        /// <summary>
        /// 第一步 启动Origin模拟器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_RunOriginEmu_Click(object sender, RoutedEventArgs e)
        {
            ProcessHelper.OpenProcess(CoreUtil.Origin_OriginDebug_exe);
        }

        /// <summary>
        /// 第二步 启动战地1游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_RunBF1Game_Click(object sender, RoutedEventArgs e)
        {
            if (!CoreUtil.IsExistsBF1GameDir())
                return;

            if (!ProcessHelper.IsAppRun("OriginDebug"))
            {
                MsgBoxHelper.Warning("请先启动Origin模拟器");
                return;
            }

            if (!CoreUtil.IsExistsBF1OriginEmuPath())
            {
                MsgBoxHelper.Warning("缺少战地1免Origin补丁");
                return;
            }

            var args = TextBox_BF1RunArgs.Text.Trim();

            ProcessHelper.OpenProcess(CoreUtil.Game_bf1_exe, args);
        }
    }
}
