using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("NpcsReplies",true)]
    public class NpcReplyRecord : ITable
    {
        public static List<NpcReplyRecord> NpcsReplies = new List<NpcReplyRecord>();

        public ushort MessageId;
        [Primary]
        public int ReplyId;
        public string ActionType;
        public string OptionalValue1;
        public string OptionalValue2;
        public string Condition;
        public string ConditionExplanation;
        public NpcReplyRecord(ushort messageid,int replyid,string actiontype,string optionalvalue1,string optionalvalue2,string condition,string conditionexplanation)
        {
            this.MessageId = messageid;
            this.ReplyId = replyid;
            this.ActionType = actiontype;
            this.OptionalValue1 = optionalvalue1;
            this.OptionalValue2 = optionalvalue2;
            this.Condition = condition;
            this.ConditionExplanation = conditionexplanation;
        }

        public static List<NpcReplyRecord> GetNpcRepliesData(int replyid)
        {
            return NpcsReplies.FindAll(x => x.ReplyId == replyid);
        }
        public static List<NpcReplyRecord> GetNpcReplies(ushort messageid)
        {
            return NpcsReplies.FindAll(x => x.MessageId == messageid);
        }
    }
}
