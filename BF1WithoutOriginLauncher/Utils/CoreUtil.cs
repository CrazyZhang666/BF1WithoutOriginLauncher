using BF1WithoutOriginLauncher.Helper;
using System.IO;

namespace BF1WithoutOriginLauncher.Utils
{
    public static class CoreUtil
    {
        private const string Root = ".\\AppData";

        public const string Backup_bf1_exe = Root + "\\Backup\\bf1.exe";

        public const string Config_Config_ini = Root + "\\Config\\Config.ini";

        public const string Origin_OriginDebug_exe = Root + "\\Origin\\OriginDebug.exe";
        public const string Origin_config_ini = Root + "\\Origin\\config.txt";

        public const string Patch_bf1_without_origin_exe = Root + "\\Patch\\bf1_without_origin.exe";
        public const string Patch_dinput8_dll = Root + "\\Patch\\dinput8.dll";
        public const string Patch_dinput8_org_dll = Root + "\\Patch\\dinput8_org.dll";
        public const string Patch_originemu_dll = Root + "\\Patch\\originemu.dll";

        public const string Tools_EA_Game_RegFix_exe = Root + "\\Tools\\EA_Game_RegFix.exe";

        ////////////////////////////////////////////////

        public static string BF1_Dir { get; set; }

        public static string Game_bf1_exe
        {
            get { return Path.Combine(BF1_Dir, "bf1.exe"); }
        }

        public static string Game_bf1_without_origin_exe
        {
            get { return Path.Combine(BF1_Dir, "bf1_without_origin.exe"); }
        }

        public static string Game_dinput8_dll
        {
            get { return Path.Combine(BF1_Dir, "dinput8.dll"); }
        }

        public static string Game_dinput8_org_dll
        {
            get { return Path.Combine(BF1_Dir, "dinput8_org.dll"); }
        }

        public static string Game_originemu_dll
        {
            get { return Path.Combine(BF1_Dir, "originemu.dll"); }
        }

        public static string Game_EA_Game_RegFix_exe
        {
            get { return Path.Combine(BF1_Dir, "EA_Game_RegFix.exe"); }
        }

        ////////////////////////////////////////////////

        /// <summary>
        /// 判断战地1游戏目录是否存在
        /// </summary>
        /// <returns></returns>
        public static bool IsExistsBF1GameDir()
        {
            if (!Directory.Exists(BF1_Dir))
            {
                MsgBoxHelper.Warning($"当前战地1游戏目录不存在\n{BF1_Dir}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检测战地1免Origin补丁是否存在
        /// </summary> 
        /// <returns></returns>
        public static bool IsExistsBF1OriginEmuPath()
        {
            if (!Directory.Exists(BF1_Dir))
                return false;

            if (!File.Exists(Game_bf1_exe))
                return false;

            if (!File.Exists(Game_dinput8_dll))
                return false;
            if (!File.Exists(Game_dinput8_org_dll))
                return false;
            if (!File.Exists(Game_originemu_dll))
                return false;

            return true;
        }
    }
}
