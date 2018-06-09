using System;
using System.Text;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// ユニット定義情報を書式化するときのオプションのセットです。
    /// </summary>
    public class FormatOptions
    {
        string _newLine = Environment.NewLine;
        bool _softTab = false;
        int _tabSize = 4;

        /// <summary>
        /// 改行文字です。
        /// デフォルトでは<c>Environment.NewLine</c>と同じ値です。
        /// </summary>
        /// <value>The new line.</value>
        /// <exception cref="ArgumentNullException">値が<c>null</c>の場合</exception>
        public string NewLine
        {
            get => _newLine;
            set => _newLine = value ?? throw new ArgumentNullException(nameof(value));
        }
        /// <summary>
        /// ソフトタブが有効かどうかを示します。
        /// デフォルトでは<c>false</c>です。
        /// </summary>
        /// <value>有効である場合<c>true</c></value>
        public bool SoftTab 
        {
            get => _softTab;
            set => _softTab = value;
        }
        /// <summary>
        /// タブ幅を示します。
        /// デフォルトでは<c>4</c>です。
        /// </summary>
        /// <value>The size of the tab.</value>
        /// <exception cref="ArgumentOutOfRangeException">値が<c>0</c>より小さい場合</exception>
        public int TabSize 
        {
            get => _tabSize;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException
                    (nameof(value), "value must be greater than or equal 0.");
                _tabSize = value;
            }
        }
    }
}
