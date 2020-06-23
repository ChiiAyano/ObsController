using Newtonsoft.Json;
using System.IO;

namespace ObsController.Core
{
    /// <summary>
    /// 設定の読み書きを提供します。
    /// </summary>
    public class SettingLoader
    {
        /// <summary>
        /// 設定をロードします。ファイルがない場合は既定値で新しく生成します。
        /// </summary>
        /// <returns></returns>
        public static T Load<T>() where T : new()
        {
            var json = string.Empty;

            if (!File.Exists(General.SettingPath))
            {
                var def = new T();
                Save(def);
                return def;
            }

            using (var sr = new StreamReader(General.SettingPath))
            {
                json = sr.ReadToEnd();
            }

            var data = JsonConvert.DeserializeObject<T>(json);
            return data;
        }

        /// <summary>
        /// 設定を保存します。
        /// </summary>
        /// <param name="data"></param>
        public static void Save<T>(T data) where T : new()
        {
            var json = JsonConvert.SerializeObject(data);

            using var wr = new StreamWriter(General.SettingPath, false);

            wr.WriteLine(json);
        }
    }
}
