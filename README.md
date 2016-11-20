# Unclazz.Jp1ajs2.Unitdef

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
IEnumerable<IUnit> childrenNameEndsWith1000 = u.Query(Q.Children().NameEndsWith("1000"));
// ...そのうち1件だけ（存在しない場合は例外をスローする）
IUnit child0NameEndsWith1000 = u.Query(Q.Children().NameEndsWith("1001").One());

// 直属・非直属の下位ユニットのうち種別がUNIXジョブであるもののscパラメータすべて
IEnumerable<IParameter> paramsScOfDescendantsTypeIsUnixJob = u.Query(Q
	.Descendants().TypeIs(UnitType.UnixJob).TheirParameters.NameEquals("sc"));
// ...そのうち1件だけ（存在しない場合はnullを返す）
IParameter param0ScOfDescendantsTypeIsUnixJob = u.Query(Q
	.Descendants().TypeIs(UnitType.UnixJob)
	.TheirParameters.NameEquals("sc").One(true));
// 当該ユニットおよび下位ユニットのうちscパラメータを持ちその値が"/path/to"で始まるものすべて
IEnumerable<IUnit> unitsScStartsWithPathTo = u.Query(Q
	.Descendants().TypeIs(UnitType.UnixJob)
	.HasParameter("sc").StartsWith("/path/to").One(true));
```

## 名前空間の構成

`Unclazz.Jp1ajs2.Unitdef`─この名前空間にはユニット定義を構成するユニット、ユニット属性パラメータ、ユニット定義パラメータなどに対応するインターフェースとそのデフォルト実装が含まれています。中核となるのはインターフェース`IUnit`およびその実装である`Unit`です。`Unit.FromString(string)`と`Unit.FromFile(string)`を通じてユニット定義のパースを行うことができます。

`Unclazz.Jp1ajs2.Unitdef.Parser`─この名前空間にはユニット定義のパース処理に関連するクラスが含まれています。`UnitParser`は文字通りユニット定義のパーサーであり、`Unit.From...`と異なって複数のルート・ユニットを持つユニット定義をパースし、結果をリストとして取得することが可能です。

`Unclazz.Jp1ajs2.Unitdef.Query`─この名前空間にはユニット定義を構成するオブジェクトに対して問合せをして、何かしらの値を抽出するためのインターフェース`IQuery<T,TResult>`およびそのデフォルト実装を提供するユーティリティ`Q`などが含まれています。

## JP1/AJS2製造・販売元との関係

Java版のAPI同様、JP1/AJS2製造・販売元に対する本プロジェクト開発者の立場は単なる「ユーザー」です。したがって、本プロジェクトで開発・配布するコードは製造・販売元とは一切関わりがありません。

本プロジェクトで開発・配布するコードは、製造・販売元で公開している定義情報[リファレンス](http://www.hitachi.co.jp/Prod/comp/soft1/manual/pc/d3K2543/AJSO0001.HTM)に基づき、これを参考にして、定義ファイルのパースや各種パラメータのアクセサのロジックを実装していますが、同リファレンスの誤読もしくはリファレンスとJP1/AJS2間の実装齟齬などにより、実際の動作に差異が存在する可能性があります。
