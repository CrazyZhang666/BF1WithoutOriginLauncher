using System.IO;
using System.Windows;
using System.Windows.Navigation;
using System.ComponentModel;
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
            TextBox_BF1GameDir.Text = IniHelper.ReadValue("Config", "BF1GameDir");
        }

        /// <summary>
        /// 主窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Main_Closing(object sender, CancelEventArgs e)
        {
            IniHelper.WriteValue("Config", "BF1GameDir", TextBox_BF1GameDir.Text.Trim());
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
            }
        }

        /// <summary>
        /// 打开战地1游戏目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OpenBF1GameDir_Click(object sender, RoutedEventArgs e)
        {
            var bf1Dir = TextBox_BF1GameDir.Text.Trim();

            if (!Directory.Exists(bf1Dir))
            {
                MsgBoxHelper.Warning("战地1目录不存在");
                return;
            }

            ProcessHelper.OpenLink(bf1Dir);
        }

        /// <summary>
        /// 编辑Origin模拟器配置文件（Cookies）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_EditOriginEmuConfig_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(CoreUtil.Origin_config_ini))
            {
                MsgBoxHelper.Warning("Origin模拟器配置文件不存在");
                return;
            }

            ProcessHelper.OpenProcess(CoreUtil.Origin_config_ini, "notepad");
        }

        /// <summary>
        /// 使用战地1免Origin补丁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_UseWithoutOriginPath_Click(object sender, RoutedEventArgs e)
        {
            var bf1Dir = TextBox_BF1GameDir.Text.Trim();

            if (!Directory.Exists(bf1Dir))
            {
                MsgBoxHelper.Warning("战地1目录不存在");
                return;
            }

            File.Copy(CoreUtil.Patch_bf1_without_origin_exe, Path.Combine(bf1Dir, "bf1.exe"), true);
            File.Copy(CoreUtil.Patch_dinput8_dll, Path.Combine(bf1Dir, "dinput8.dll"), true);
            File.Copy(CoreUtil.Patch_dinput8_org_dll, Path.Combine(bf1Dir, "dinput8_org.dll"), true);
            File.Copy(CoreUtil.Patch_originemu_dll, Path.Combine(bf1Dir, "originemu.dll"), true);

            MsgBoxHelper.Information("使用战地1免Origin补丁成功");
        }

        /// <summary>
        /// 恢复战地1原版文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_UseBackupBF1MainApp_Click(object sender, RoutedEventArgs e)
        {
            var bf1Dir = TextBox_BF1GameDir.Text.Trim();

            if (!Directory.Exists(bf1Dir))
            {
                MsgBoxHelper.Warning("战地1目录不存在");
                return;
            }

            File.Delete(Path.Combine(bf1Dir, "bf1_without_origin.exe"));
            File.Delete(Path.Combine(bf1Dir, "dinput8.dll"));
            File.Delete(Path.Combine(bf1Dir, "dinput8_org.dll"));
            File.Delete(Path.Combine(bf1Dir, "originemu.dll"));

            File.Copy(".\\AppData\\Backup\\bf1.exe", Path.Combine(bf1Dir, "bf1.exe"), true);

            MsgBoxHelper.Information("恢复战地1原版文件成功");
        }

        /// <summary>
        /// 运行战地1注册表恢复工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_UseBF1RegeditFix_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("你确定要运行战地1注册表恢复工具吗？",
                "运行提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                var bf1Dir = TextBox_BF1GameDir.Text.Trim();

                if (!Directory.Exists(bf1Dir))
                {
                    MsgBoxHelper.Warning("战地1目录不存在");
                    return;
                }

                var regFixPath = Path.Combine(bf1Dir, CoreUtil.Tools_EA_Game_RegFix_exe);

                File.Copy(CoreUtil.Tools_EA_Game_RegFix_exe, regFixPath, true);

                ProcessHelper.OpenProcess(regFixPath);
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
            var bf1Dir = TextBox_BF1GameDir.Text.Trim();

            if (!Directory.Exists(bf1Dir))
            {
                MsgBoxHelper.Warning("战地1目录不存在");
                return;
            }

            if (!ProcessHelper.IsAppRun("OriginDebug"))
            {
                MsgBoxHelper.Warning("请先启动Origin模拟器");
                return;
            }

            if (!CheckBF1PathIsSucess())
            {
                MsgBoxHelper.Warning("缺少战地1免Origin补丁");
                return;
            }

            var args = TextBox_RunArgs.Text.Trim();

            ProcessHelper.OpenProcess(Path.Combine(bf1Dir, "bf1.exe"), args);
        }

        /// <summary>
        /// 检测战地1免Origin补丁是否成功
        /// </summary>
        /// <returns></returns>
        private bool CheckBF1PathIsSucess()
        {
            var bf1Dir = TextBox_BF1GameDir.Text.Trim();
            if (!Directory.Exists(bf1Dir))
                return false;

            CoreUtil.BF1_Dir = bf1Dir;

            if (!File.Exists(CoreUtil.Game_bf1_exe))
                return false;

            if (!File.Exists(CoreUtil.Game_dinput8_dll))
                return false;
            if (!File.Exists(CoreUtil.Game_dinput8_org_dll))
                return false;
            if (!File.Exists(CoreUtil.Game_originemu_dll))
                return false;

            return true;
        }
    }
}
