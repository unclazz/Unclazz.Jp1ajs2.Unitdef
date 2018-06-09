# Unclazz.Jp1ajs2.Unitdef

[![Build Status](https://travis-ci.org/unclazz/Unclazz.Jp1ajs2.Unitdef.svg?branch=master)](https://travis-ci.org/unclazz/Unclazz.Jp1ajs2.Unitdef)

`Unclazz.Jp1ajs2.Unitdef`は日立ソリューションズの製造・販売する[JP1/AJS2](http://www.hitachi-solutions.co.jp/jp1/sp/?cid=aws0004461)の定義情報をパースし、C#オブジェクトとしてアクセスするためのAPIです。Java向けの同趣旨のAPIである[unclazz-jp1ajs2-unitdef](https://github.com/unclazz/unclazz-jp1ajs2-unitdef)をC#向けに実装しなおしたものです。

APIのアセンブリは[NuGet Gallery](https://www.nuget.org/packages/Unclazz.Jp1ajs2.Unitdef/)から取得することができます。

## 使い方

出発点は`Unclazz.Jp1ajs2.Unitdef.Unit`です。このクラスはユニット定義を表すインターフェース`IUnit`のデフォルト実装であり、かつ、文字列もしくはファイルからユニット定義をパースしてC#オブジェクトに変換するユーティリティです。

以下のサンプルでは`Unit.FromString(string)`で文字列からユニット定義をパースしたあと、`IUnit.Query(IQuery<IUnit,TResult>)`を通じて各種の値を問い合わせしています：

```C#
// 文字列もしくはファイルからユニット定義をパースする
IUnit u = Unit.FromString(
	"unit=XXXX0000,,,;\n" +
	"{   ty=n;\n" +
	"    cm=\"comment text...\";\n" +
	"    unit=XXXX1000,,,;\n" +
	"    {   ty=n;\n" +
	"        unit=XXXX1100,,,;{ty=pj;sc=\"x:\\path\\to\\script\";prm=\"param list\";tho=0;}\n" +
	"        unit=XXXX1200,,,;{ty=j;sc=\"/path/to/script\";prm=\"param list\";tho=1;}\n" +
	"    }\n" +
	"    unit=XXXX2000,,,;{ty=j;sc=\"/path/to/script\";tho=2;}\n" +
	"}");

// 直属の下位ユニットのうち名前が1000で終わるものすべて
IEnumerable<IUnit> childrenNameEndsWith1000 = u.Children().NameEndsWith("1000");
// ...そのうち1件だけ（存在しない場合は例外をスローする）
IUnit child0NameEndsWith1000 = u.Children().NameEndsWith("1001").First();

// 直属・非直属の下位ユニットのうち種別がUNIXジョブであるもののscパラメータすべて
IEnumerable<IParameter> paramsScOfDescendantsTypeIsUnixJob = 
	u.Descendants(UnitType.UnixJob).SelectMany(v => v.Parameters).NameEquals("sc");
// ...そのうち1件だけ（存在しない場合はnullを返す）
IParameter param0ScOfDescendantsTypeIsUnixJob = 
	u.Descendants(UnitType.UnixJob).SelectMany(v => v.Parameters)
	.NameEquals("sc").FirstOrDefault();

// 当該ユニットおよび下位ユニットのうちscパラメータを持ちその値が"/path/to"で始まるものすべて
IEnumerable<IUnit> unitsScStartsWithPathTo = 
	u.Descendants(UnitType.UnixJob)
	.Where(v => w.Parameters.Any(p => p.Name == "sc" && 
	p.Values[0].StringValue.StartsWith("/path/to")));
```

## 名前空間

`Unclazz.Jp1ajs2.Unitdef`─この名前空間にはユニット定義を構成するユニット、ユニット属性パラメータ、ユニット定義パラメータなどに対応するインターフェースとそのデフォルト実装が含まれています。中核となるのはインターフェース`IUnit`およびその実装である`Unit`です。`Unit.FromString(string)`と`Unit.FromFile(string)`を通じてユニット定義のパースを行うことができます。

下位ユニット `IUnit.SubUnits` やユニット属性パラメータ `IUnit.Parameters` 、そのパラメータの値 `IParameter.Values` は、いずれも `NonNullCollection<T>` クラスのインスタンスにくるまれています。このクラスは名前の通りnull非許容のコレクションで、内包する要素に応じて `IList<T>` には存在しないいくつかの追加のメソッドも提供しています。

膨大な量のユニットを必要に応じて並列で処理することも想定して、パース結果はイミュータブルなオブジェクトを返すように実装されています。しかし `IUnit.AsMutable()` により容易にミュータブルなバージョンを得られます。その逆に `IUnit.AsImmutable()` によりイミュータブルなバージョンに変換することも可能です。

`Unclazz.Jp1ajs2.Unitdef.Parser`─この名前空間にはユニット定義のパース処理に関連するクラスが含まれています。`UnitParser`は文字通りユニット定義のパーサーであり、`Unit.From...`と異なって複数のルート・ユニットを持つユニット定義をパースし、結果をリストとして取得することが可能です。

## 使用方法

`Unclazz.Jp1ajs2.Unitdef` が提供するAPIの使用方法をケース別にサンプルコードを提示して説明します。

### ユニット定義情報をパースする

ユニット定義情報は文字列、ファイル、そして任意のストリームからパースすることができます：

```C#
var u0 = Unit.FromString("..."); // 文字列から
var u1 = Unit.FromFile("/path/to/file", Encoding.GetEncoding("...")); // ファイルから
var u2 = Unit.FromStream(..., Encoding.GetEncoding("...")); // 任意のストリームから
```

### ユニット定義情報を変更する

ユニット定義情報 `IUnit` にはイミュータブルな実装とミュータブルな実装が存在し、相互に変換が可能です：

```C#
var u0 = Unit.FromString("..."); // イミュータブルなインスタンス
var u1 = u0.AsMutable(); // ミュータブルなインスタンス

u1.Type = UnitType.PcJob; // ユニット定義パラメータtyの値の変更
u1.Comment = "new comment here"; // ユニット定義パラメータcmの値の変更
u1.Parameters.Add(...); // その他のユニット定義パラメータの変更
u1.SubUnits.Add(...); // 下位ユニットの変更
```

### コンポーネントを作成する

ユニット定義情報 `IUnit` のインスタンス生成にはビルダーオブジェクトやファクトリーメソッドを使用します：

```C#
// ビルダーを使用してイミュータブルなインスタンスを作成
var u0 = Unit.Builder.Create()
		.Attributes(...)
		...
		.Build();

// ファクトリーメソッドを使用してミュータブルなインスタンスを作成
var u1 = MutableUnit.Create("...");
```

ユニット定義パラメータ `IParameter` とその値 `IParameterValue` にも、
それぞれのビルダーオブジェクトやファクトリーメソッドが提供されています：

```C#
var u0 = Unit.FromString("...").AsMutable();
var p0 = Parameter.Builder.Create().Name("...")
		.AddValue(RawStringParameterValue.OfValue("raw_string"))
		.AddValue(QuotedStringParameterValue.OfValue("quoted string"))
		.AddValue(TupleParameterValue.OfValue(...))
		.Build();
u0.Parameters.Add(p0);
```

タプル `ITuple` にもイミュータブルな実装とミュータブルな実装が存在し、相互に変換が可能。
インスタンス生成にはファクトリーメソッドを使用します：

```C#
// イミュータブルなインスタンスの生成
var t0 = Tuple.FromCollection(new []{
	TupleEntry.OfValue("value0"),
	TupleEntry.OfPair("key1", "value1"),
	TupleEntry.OfPair("key2", "value2")
});

// イミュータブルかつ空のインスタンスの生成
var t1 = Tuple.Empty;

var t2 = t0.AsMutable(); // ミュータブルなインスタンスへ変換
t2.Add("value3");
t2.Add("key4", "value4");

var t3 = MutableTuple.Empty; // ミュータブルかつ空のインスタンスの生成
t3.Add("value0");
```

### ユニット定義情報を書き出す

`IUnit` の内容は文字列やファイル、任意のストリームに書き出すことができます：

```C#
IUnit u0 = ...;
TextWriter w0 = new StringWriter(); 
u.WriteTo(w0); // 書き出す（システムデフォルトの改行文字、インデントはタブ文字）

u.WriteTo(w0, new FormatOptions{
	NewLine = "\r\n", // 改行文字はCRLF
	SoftTab = true,   // インデントはソフトタブ
	TabSize = 2       // タブ幅は2
});
```

### ユニット定義情報を検索する

`IUnit` は子孫ユニットからなる木構造を持ちます。
ルートになるユニットとこれらの子孫ユニットから構成される集合から、特定の条件にマッチするユニットを探すには次のようにします：

```C#
IUnit u0 = ...;

IEnumerable<IUnit> c0 = u0.Children(); // 子ユニット（下位ユニット）＝IUnit.SubUnits
IEnumerable<IUnit> c1 = u0.Children(UnitType.PcJob); // 特定種別の子ユニットのみ
IEnumerable<IUnit> c2 = u0.ItSelfAndChildren(); // 自身と子ユニット

IEnumerable<IUnit> c0 = u0.Descendants(); // 子孫ユニット
IEnumerable<IUnit> c1 = u0.Descendants(UnitType.PcJob); // 特定種別の子孫ユニットのみ
IEnumerable<IUnit> c2 = u0.ItSelfAndDescendants(); // 自身と子孫ユニット
```

いずれのAPIも `IEnumetable<IUnit>` を戻り値とするので、ここからLINQによる絞り込みが可能です。


## JP1/AJS2製造・販売元との関係

Java版のAPI同様、JP1/AJS2製造・販売元に対する本プロジェクト開発者の立場は単なる「ユーザー」です。したがって、本プロジェクトで開発・配布するコードは製造・販売元とは一切関わりがありません。

本プロジェクトで開発・配布するコードは、製造・販売元で公開している定義情報[リファレンス](http://www.hitachi.co.jp/Prod/comp/soft1/manual/pc/d3K2543/AJSO0001.HTM)に基づき、これを参考にして、定義ファイルのパースや各種パラメータのアクセサのロジックを実装していますが、同リファレンスの誤読もしくはリファレンスとJP1/AJS2間の実装齟齬などにより、実際の動作に差異が存在する可能性があります。
