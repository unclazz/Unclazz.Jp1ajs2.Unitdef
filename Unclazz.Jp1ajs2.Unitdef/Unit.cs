using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unclazz.Jp1ajs2.Unitdef.Parser;
using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef
{
    /// <summary>
    /// <code>IUnit</code>のイミュータブルなデフォルト実装です。
    /// </summary>
    public sealed partial class Unit : IUnit
    {

        static readonly Parser<IUnit> parser = new UnitParser2();

        /// <summary>
        /// 文字列からユニット定義を読み取ります。
        /// </summary>
        /// <returns>ユニット定義</returns>
        /// <param name="value">ユニット定義を含む文字列</param>
        public static IUnit FromString(string value)
        {
            var result = parser.Parse(value ?? throw new ArgumentNullException(nameof(value)));
            if (result.Successful) return result.Capture;
            throw new ParseException2(result);
        }
        /// <summary>
        /// ファイルからユニット定義を読み取ります。
        /// </summary>
        /// <returns>ユニット定義</returns>
        /// <param name="path">ファイルパス</param>
        /// <param name="enc">エンコーディング</param>
        public static IUnit FromFile(string path, Encoding enc)
        {
            var result = parser.Parse(Reader.From(path ?? throw new ArgumentNullException(nameof(path)),
                                                  enc ?? throw new ArgumentNullException(nameof(enc))));
            if (result.Successful) return result.Capture;
            throw new ParseException2(result);
        }
		/// <summary>
		/// ストリームからユニット定義を読み取ります。
		/// </summary>
		/// <returns>ユニット定義</returns>
		/// <param name="stream">ストリーム</param>
		/// <param name="enc">エンコーディング</param>
		public static IUnit FromStream(Stream stream, Encoding enc)
		{
            var result = parser.Parse(Reader.From(stream ?? throw new ArgumentNullException(nameof(stream)),
                                                  enc ?? throw new ArgumentNullException(nameof(enc))));
            if (result.Successful) return result.Capture;
            throw new ParseException2(result);
		}

        Unit(FullName fqn, Attributes attributes, IParameter ty, IParameter cm, 
            List<IParameter> parameters, List<IUnit> subUnits)
        {
            _fqn = fqn;
            _name = attributes.UnitName;
            _attrs = attributes;
            _type = UnitType.FromName(ty.Values[0].StringValue);
            _comment = cm == null ? string.Empty : cm.Values[0].StringValue;
            Parameters = new NonNullCollection<IParameter>(parameters.AsReadOnly());
            SubUnits = new NonNullCollection<IUnit>(subUnits.AsReadOnly());
        }

        readonly FullName _fqn;
        readonly Attributes _attrs;
        readonly string _comment;
        readonly string _name;
        readonly UnitType _type;

        /// <summary>
        /// ユニット属性パラメータ
        /// </summary>
        public Attributes Attributes
        {
            get => _attrs;
            set => throw new NotSupportedException();
        }
        /// <summary>
        /// コメント
        /// </summary>
        public string Comment
        {
            get => _comment;
            set => throw new NotSupportedException();
        }
        /// <summary>
        /// ユニット完全名
        /// </summary>
        public FullName FullName
        {
            get => _fqn;
            set => throw new NotSupportedException();
        }
        /// <summary>
        /// ユニット名
        /// </summary>
        public string Name
        {
            get => _name;
            set => throw new NotSupportedException();
        }
        /// <summary>
        /// ユニット定義パラメータのリスト
        /// </summary>
        public NonNullCollection<IParameter> Parameters { get; }
        /// <summary>
        /// 下位ユニットのリスト
        /// </summary>
        public NonNullCollection<IUnit> SubUnits { get; }
        /// <summary>
        /// ユニット種別
        /// </summary>
        public UnitType Type
        {
            get => _type;
            set => throw new NotSupportedException();
        }
        /// <summary>
        /// このオブジェクトの文字列表現を返します。
        /// </summary>
        /// <returns>このオブジェクトの文字列表現</returns>
        public override string ToString()
        {
            return UnitdefUtil.ToString(this);
        }
    }
}
