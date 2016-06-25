using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef
{
    public sealed class UnitType : IUnitType
    {
        private static readonly IDictionary<string, UnitType> dict = new Dictionary<string, UnitType>();

        /// <summary>
        /// ジョブネットグループまたはプランニンググループ
        /// </summary>
        public static readonly UnitType JobGroup = new UnitType("g", "JobGroup");
        /// <summary>
        /// マネージャージョブグループ
        /// </summary>
        public static readonly UnitType ManagerJobGroup = new UnitType("mg", "ManagerJobGroup");
        /// <summary>
        /// ジョブネット
        /// </summary>
        public static readonly UnitType Jobnet = new UnitType("n", "Jobnet");
        /// <summary>
        /// リカバリージョブネット
        /// </summary>
        public static readonly UnitType RecoveryJobnet = new UnitType("rn", "RecoveryJobnet");
        /// <summary>
        /// リモートジョブネット
        /// </summary>
        public static readonly UnitType RemoteJobnet = new UnitType("rm", "RemoteJobnet");
        /// <summary>
        /// リカバリーリモートジョブネット
        /// </summary>
        public static readonly UnitType RecoveryRemoteJobnet = new UnitType("rr", "RecoveryRemoteJobnet");
        /// <summary>
        /// ルートジョブネットの起動条件
        /// </summary>
        public static readonly UnitType RunCondition = new UnitType("rc", "RunCondition");
        /// <summary>
        /// マネージャージョブネット
        /// </summary>
        public static readonly UnitType ManagerJobnet = new UnitType("mg", "ManagerJobnet");
        /// <summary>
        /// UNIXジョブ
        /// </summary>
        public static readonly UnitType UnixJob = new UnitType("j", "UnixJob");
        /// <summary>
        /// リカバリーUNIXジョブ
        /// </summary>
        public static readonly UnitType RecoveryUnixJob = new UnitType("rj", "RecoveryUnixJob");
        /// <summary>
        /// PCジョブ
        /// </summary>
        public static readonly UnitType PcJob = new UnitType("pj", "PcJob");
        /// <summary>
        /// リカバリーPCジョブ
        /// </summary>
        public static readonly UnitType RecoveryPcJob = new UnitType("rp", "RecoveryPcJob");
        /// <summary>
        /// QUEUEジョブ
        /// </summary>
        public static readonly UnitType QueueJob = new UnitType("qj", "QueueJob");
        /// <summary>
        /// リカバリーQUEUEジョブ
        /// </summary>
        public static readonly UnitType RecoveryQueueJob = new UnitType("rq", "RecoveryQueueJob");
        /// <summary>
        /// 判定ジョブ
        /// </summary>
        public static readonly UnitType JudgementJob = new UnitType("jdj", "JudgementJob");
        /// <summary>
        /// リカバリー判定ジョブ
        /// </summary>
        public static readonly UnitType RecoveryJudgementJob = new UnitType("rjdj", "RecoveryJudgementJob");
        /// <summary>
        /// ORジョブ
        /// </summary>
        public static readonly UnitType OrJob = new UnitType("orj", "OrJob");
        /// <summary>
        /// リカバリーORジョブ
        /// </summary>
        public static readonly UnitType RecoveryOrJob = new UnitType("rorj", "RecoveryOrJob");
        /// <summary>
        /// JP1イベント受信監視ジョブ
        /// </summary>
        public static readonly UnitType EventWatchJob = new UnitType("evwj", "EventWatchJob");
        /// <summary>
        /// リカバリーJP1イベント受信監視ジョブ
        /// </summary>
        public static readonly UnitType RecoveryEventWatchJob = new UnitType("revwj", "RecoveryEventWatchJob");
        /// <summary>
        /// ファイル監視ジョブ
        /// </summary>
        public static readonly UnitType FileWatchJob = new UnitType("flwj", "FileWatchJob");
        /// <summary>
        /// リカバリーファイル監視ジョブ
        /// </summary>
        public static readonly UnitType RecoveryFileWatchJob = new UnitType("rflwj", "RecoveryFileWatchJob");
        /// <summary>
        /// メール受信監視ジョブ
        /// </summary>
        public static readonly UnitType MailWatchJob = new UnitType("mlwj", "MailWatchJob");
        /// <summary>
        /// リカバリーメール受信監視ジョブ
        /// </summary>
        public static readonly UnitType RecoveryMailWatchJob = new UnitType("rmlwj", "RecoveryMailWatchJob");
        /// <summary>
        /// メッセージキュー受信監視ジョブ
        /// </summary>
        public static readonly UnitType MessageQueueWatchJob = new UnitType("mqwj", "MessageQueueWatchJob");
        /// <summary>
        /// リカバリーメッセージキュー受信監視ジョブ
        /// </summary>
        public static readonly UnitType RecoveryMessageQueueWatchJob = new UnitType("rmqwj", "RecoveryMessageQueueWatchJob");
        /// <summary>
        /// MSMQ受信監視ジョブ
        /// </summary>
        public static readonly UnitType MsmqWatchJob = new UnitType("mswj", "MsmqWatchJob");
        /// <summary>
        /// リカバリーMSMQ受信監視ジョブ
        /// </summary>
        public static readonly UnitType RecoveryMsmqWatchJob = new UnitType("rmswj", "RecoveryMsmqWatchJob");
        /// <summary>
        /// ログファイル監視ジョブ
        /// </summary>
        public static readonly UnitType LogFileWatchJob = new UnitType("lfwj", "LogFileWatchJob");
        /// <summary>
        /// リカバリーログファイル監視ジョブ
        /// </summary>
        public static readonly UnitType RecoveryLogFileWatchJob = new UnitType("rlfwj", "RecoveryLogFileWatchJob");
        /// <summary>
        /// Windowsイベントログ監視ジョブ
        /// </summary>
        public static readonly UnitType WindowsEventLogWatchJob = new UnitType("ntwj", "WindowsEventLogWatchJob");
        /// <summary>
        /// リカバリーWindowsイベントログ監視ジョブ
        /// </summary>
        public static readonly UnitType RecoveryWindowsEventLogWatchJob = new UnitType("rntwj", "RecoveryWindowsEventLogWatchJob");
        /// <summary>
        /// 実行間隔制御ジョブ
        /// </summary>
        public static readonly UnitType TimeWatchJob = new UnitType("tmwj", "TimeWatchJob");
        /// <summary>
        /// リカバリー実行間隔制御ジョブ
        /// </summary>
        public static readonly UnitType RecoveryTimeWatchJob = new UnitType("rtmwj", "RecoveryTimeWatchJob");
        /// <summary>
        /// JP1イベント送信ジョブ
        /// </summary>
        public static readonly UnitType EventSendJob = new UnitType("evsj", "EventSendJob");
        /// <summary>
        /// リカバリーJP1イベント送信ジョブ
        /// </summary>
        public static readonly UnitType RecoveryEventSendJob = new UnitType("revsj", "RecoveryEventSendJob");
        /// <summary>
        /// メール送信ジョブ
        /// </summary>
        public static readonly UnitType MailSendJob = new UnitType("mlsj", "MailSendJob");
        /// <summary>
        /// リカバリーメール送信ジョブ
        /// </summary>
        public static readonly UnitType RecoveryMailSendJob = new UnitType("rmlsj", "RecoveryMailSendJob");
        /// <summary>
        /// メッセージキュー送信ジョブ
        /// </summary>
        public static readonly UnitType MessageQueueSendJob = new UnitType("mqsj", "MessageQueueSendJob");
        /// <summary>
        /// リカバリーメッセージキュー送信ジョブ
        /// </summary>
        public static readonly UnitType RecoveryMessageQueueSendJob = new UnitType("rmqsj", "RecoveryMessageQueueSendJob");
        /// <summary>
        /// MSMQ送信ジョブ
        /// </summary>
        public static readonly UnitType MsmqSendJob = new UnitType("mssj", "MsmqSendJob");
        /// <summary>
        /// リカバリーMSMQ送信ジョブ
        /// </summary>
        public static readonly UnitType RecoveryMsmqSendJob = new UnitType("rmssj", "RecoveryMsmqSendJob");
        /// <summary>
        /// JP1/Cm2状態通知ジョブ
        /// </summary>
        public static readonly UnitType Jp1cm2SendSendJob = new UnitType("cmsj", "Jp1cm2SendSendJob");
        /// <summary>
        /// リカバリーJP1/Cm2状態通知ジョブ
        /// </summary>
        public static readonly UnitType RecoveryJp1cm2SendSendJob = new UnitType("rcmsj", "RecoveryJp1cm2SendSendJob");
        /// <summary>
        /// ローカル電源制御ジョブ
        /// </summary>
        public static readonly UnitType LocalPowerControlJob = new UnitType("pwlj", "LocalPowerControlJob");
        /// <summary>
        /// リカバリーローカル電源制御ジョブ
        /// </summary>
        public static readonly UnitType RecoveryLocalPowerControlJob = new UnitType("rpwlj", "RecoveryLocalPowerControlJob");
        /// <summary>
        /// リモート電源制御ジョブ
        /// </summary>
        public static readonly UnitType RemotePowerControlJob = new UnitType("pwrj", "RemotePowerControlJob");
        /// <summary>
        /// リカバリーリモート電源制御ジョブ
        /// </summary>
        public static readonly UnitType RecoveryRemotePowerControlJob = new UnitType("rpwrj", "RecoveryRemotePowerControlJob");
        /// <summary>
        /// カスタムUNIXジョブ
        /// </summary>
        public static readonly UnitType CustomUnixJob = new UnitType("cj", "CustomUnixJob");
        /// <summary>
        /// リカバリーカスタムUNIXジョブ
        /// </summary>
        public static readonly UnitType RecoveryCustomUnixJob = new UnitType("rcj", "RecoveryCustomUnixJob");
        /// <summary>
        /// カスタムPCジョブ
        /// </summary>
        public static readonly UnitType CustomPcJob = new UnitType("cpj", "CustomPcJob");
        /// <summary>
        /// リカバリーカスタムPCジョブを
        /// </summary>
        public static readonly UnitType RecoveryCustomPcJob = new UnitType("rcpj", "RecoveryCustomPcJob");
        /// <summary>
        /// ホストリンクジョブネット
        /// </summary>
        public static readonly UnitType HostLinkJobnet = new UnitType("hln", "HostLinkJobnet");
        /// <summary>
        /// ジョブネットコネクタ
        /// </summary>
        public static readonly UnitType JobnetConnector = new UnitType("nc", "JobnetConnector");

        /// <summary>
        /// 指定された名称のインスタンスを返します。
        /// </summary>
        /// <param name="name">ユニット種別の名称（<code>"pj"</code>など）</param>
        /// <returns>インスタンス</returns>
        /// <exception cref="ArgumentException">未知の名称が指定された場合</exception>
        public static UnitType FromName(string name)
        {
            try
            {
                return dict[name];
            }
            catch (KeyNotFoundException e)
            {
                throw new ArgumentException("Unknown code as unit type.", e);
            }
        }
        /// <summary>
        /// 既知の名称のセットを返します。
        /// </summary>
        /// <returns>名称のセット</returns>
        public static ISet<string> Names()
        {
            return new HashSet<string>(dict.Keys);
        }
        /// <summary>
        /// インスタンスのセットを返します。
        /// </summary>
        /// <returns>インスタンスのセット</returns>
        public static ISet<UnitType> Instances()
        {
            return new HashSet<UnitType>(dict.Values);
        }

        public string Name { get; }
        public string LongName { get; }
        public bool IsRecoveryType { get; }

        private UnitType (string name, string longName)
        {
            Name = name;
            LongName = longName;
            IsRecoveryType = longName.StartsWith("Recovery");
            dict[name] = this;
        }

        public override string ToString()
        {
            return string.Format("UnitType(Name={0},LongName={1})", Name, LongName);
        }
        /// <summary>
        /// インスタンスが同一かどうかを判断します。
        /// </summary>
        /// <param name="obj">比較対象のオブジェクト</param>
        /// <returns>判断結果（参照が同一である場合<code>true</code>）</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            IUnitType that = obj as IUnitType;
            return that != null && that.Name.Equals(this.Name);
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
