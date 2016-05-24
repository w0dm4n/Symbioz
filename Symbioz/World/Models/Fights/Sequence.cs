using Symbioz.DofusProtocol.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights
{
    public class Sequence
    {
        public bool Started = false;
        public Sequence(Fight fight,int sourceid,sbyte sequencetype)
        {
            this.SourceId = sourceid;
            this.SequenceType = sequencetype;
        }
        public int SourceId;
        public sbyte SequenceType;
        public void End(Fight fight,ushort actionId)
        {
            fight.Send(new SequenceEndMessage(actionId, SourceId, SequenceType));
        }
        public void Start(Fight fight)
        {
            fight.Send(new SequenceStartMessage(SequenceType, SourceId));
            this.Started = true;
        }
    }
}
