using System;
using System.IO;
using System.Text;

namespace Unclazz.Jp1ajs2.Unitdef.Parser
{
    /// <summary>
    /// パーサーの入力となるオブジェクトです。
    /// 文字列やファイル入力ストリームをラップします。
    /// EOFに到達するか処理中にIOエラーが発生した場合は自動でリソースを解放します。
    /// </summary>
    sealed class Input : IDisposable
    {
        /// <summary>
        /// 文字列から入力オブジェクトのインスタンスを生成します。
        /// </summary>
        /// <param name="s">文字列</param>
        /// <returns>インスタンス</returns>
        public static Input FromString(string s)
        {
            return new Input(new StringReader(s));
        }
        /// <summary>
        /// ファイルから入力オブジェクトのインスタンスを生成します。
        /// </summary>
        /// <param name="path">ファイルのパス</param>
        /// <param name="enc">エンコーディング</param>
        /// <returns>インスタンス</returns>
        public static Input FromFile(string path, Encoding enc)
        {
            return new Input(new StreamReader(path, enc));
        }
		/// <summary>
		/// ストリームから入力オブジェクトのインスタンスを生成します。
		/// </summary>
		/// <returns>インスタンス</returns>
		/// <param name="stream">ストリーム</param>
		/// <param name="enc">エンコーディング</param>
		public static Input FromStream(Stream stream, Encoding enc)
		{
			return new Input(new StreamReader(stream, enc));
		}

        private static readonly int CR = '\r';
        private static readonly int LF = '\n';
        private static readonly char Null = '\u0000';

        private readonly TextReader reader;
        private readonly StringBuilder lineBuff = new StringBuilder();
        private int position = -1;
        private bool closed = false;

        /// <summary>
        /// 行番号
        /// </summary>
        public int LineNumber { get; private set; }
        /// <summary>
        /// 行内の位置（<code>1</code>始まり）
        /// </summary>
        public int ColumnNumber {
            get
            {
                return position + 1;
            }
        }
        /// <summary>
        /// 現在位置から行末までの文字列
        /// </summary>
        public string RestOfLine
        {
            get
            {
                int count = lineBuff.Length - position;
                char[] restBuff = new char[count];
                lineBuff.CopyTo(position, restBuff, 0, count);
                return new String(restBuff);
            }
        }
        /// <summary>
        /// 現在位置の文字
        /// </summary>
        public char Current { get; private set; }
        /// <summary>
        /// EOFに到達している場合<code>true</code>
        /// </summary>
        public bool EndOfFile { get; private set; }
        /// <summary>
        /// EOLに到達している場合<code>true</code>
        /// </summary>
        public bool EndOfLine
        {
            get
            {
                return EndOfFile || Current == CR || Current == LF;
            }
        }

        private Input(TextReader r)
        {
            LineNumber = 0;
            Current = Null;
            reader = r;
            GoNext();
        }

        /// <summary>
        /// 現在位置を1つ前進させてその位置にある文字を返す
        /// </summary>
        /// <returns>前進後の現在位置の文字</returns>
        public char GoNext()
        {
            // EOF到達後ならすぐに現在文字（ヌル文字）を返す
            if (EndOfFile)
            {
                return Current;
            }
            // EOF到達前なら次の文字を取得する処理に入る
            // まず現在位置をインクリメント
            position += 1;
            // 現在位置が行バッファの末尾より後方にあるかチェック
            if (position >= lineBuff.Length)
            {
                // ストリームの状態をチェック
                if (closed)
                {
                    // すでにストリームが閉じられているならEOF
                    // フラグを立て、キャッシュを後始末
                    EndOfFile = true;
                    Current = Null;
                    position = 0;
                }
                else
                {
                    // まだオープン状態なら次の行をロードする
                    loadLine();
                }
            }
            // EOFの判定を実施
            if (EndOfFile)
            {
                // EOF到達済みの場合
                // 現在文字（ヌル文字）を返す
                return Current;
            }
            else
            {
                // EOF到達前の場合
                // 現在文字に新しく取得した文字を設定
                Current = lineBuff[position];
                // 現在文字を返す
                return Current;
            }
        }

        private void loadLine ()
        {
            try
            {
                // 現在位置を初期化
                position = 0;
                LineNumber += 1;
                // 行バッファをクリアする
                lineBuff.Clear();
                // 繰り返し処理
                while (!closed)
                {
                    // 次の文字を取得
                    int c0 = reader.Read();
                    // 文字のコード値が-1であるかどうか判定
                    if (c0 == -1)
                    {
                        // 文字のコード値が−1ならストリームの終了
                        // ストリームを即座にクローズする
                        closed = true;
                        reader.Dispose();
                        // バッファが空ならEOFでもある
                        EndOfFile = lineBuff.Length == 0;
                        if (EndOfFile)
                        {
                            Current = Null;
                        }
                        return;
                    }
                    // 読み取った文字をバッファに格納
                    lineBuff.Append((char)c0);
                    // 文字がLF・CRであるかどうか判定
                    if (c0 == LF)
                    {
                        // LFであればただちに読み取りを完了
                        return;
                    }
                    else if (c0 == CR)
                    {
                        int c1 = reader.Peek();
                        // 文字のコード値を判定
                        if (c1 == -1)
                        {
                            // 文字のコード値が−1ならストリームの終了
                            // ストリームを即座にクローズする
                            closed = true;
                            reader.Dispose();
                        }
                        else if (c1 == LF)
                        {
                            // LFであればそれもバッファに格納
                            // 読み取り位置も前進させる
                            lineBuff.Append((char)reader.Read());
                        }
                        // 読み取りを完了
                        return;
                    }
                } // End of while loop.
            }
            catch (IOException e)
            {
                reader.Dispose();
                throw new ParseException(this, "IO error.", e);
            }
        }
        /// <summary>
        /// このオブジェクトの文字列表現を返します。
        /// </summary>
        /// <returns>このオブジェクトの文字列表現</returns>
        public override string ToString()
        {
            return string.Format("Input(Source={0},LineNumber={1},ColumnNumer={2})",
                reader, LineNumber, ColumnNumber);
        }
        /// <summary>
        /// このオブジェクトが使用しているリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
