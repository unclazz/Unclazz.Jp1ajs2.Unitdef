using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// ユニット定義を構成する部品を表すインターフェースです。
    /// <see cref="IUnit"/>、<see cref="IParameter"/>、<see cref="IParameterValue"/>などはいずれも
    /// このインターフェースを拡張・実装して、ユニット定義ファイル上の表現形式へのアクセスを提供します。
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// オブジェクトの保持する情報を文字シーケンス化します。
        /// このメソッドが返すシーケンスは<code>object.ToString()</code>と異なってデバッグ用途で利用されるものではありません。
        /// このメソッドが返すシーケンスは当該コンポーネントのユニット定義ファイル上における
        /// 表現形式で表されたものでなくてはなりません。
        /// </summary>
        /// <returns></returns>
        string Serialize();
    }
}
