using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ObsController.Core.Objects
{
    public class TextProperty
    {
        /// <summary>
        /// テキストオブジェクトの名前
        /// </summary>
        [JsonProperty("source")]
        public string? Source { get; set; }

        /// <summary>
        /// 設定する文字列
        /// </summary>
        [JsonProperty("text")]
        public string? Text { get; set; }
    }
}
