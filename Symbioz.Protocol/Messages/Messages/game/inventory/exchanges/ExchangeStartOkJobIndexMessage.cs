


















// Generated on 06/04/2015 18:44:56
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeStartOkJobIndexMessage : Message
{

public const ushort Id = 5819;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<uint> jobs;
        

public ExchangeStartOkJobIndexMessage()
{
}

public ExchangeStartOkJobIndexMessage(IEnumerable<uint> jobs)
        {
            this.jobs = jobs;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)jobs.Count());
            foreach (var entry in jobs)
            {
                 writer.WriteVarUhInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            jobs = new uint[limit];
            for (int i = 0; i < limit; i++)
            {
                 (jobs as uint[])[i] = reader.ReadVarUhInt();
            }
            

}


}


}