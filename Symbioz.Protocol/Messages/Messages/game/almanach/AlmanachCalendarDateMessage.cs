


















// Generated on 06/04/2015 18:44:11
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AlmanachCalendarDateMessage : Message
{

public const ushort Id = 6341;
public override ushort MessageId
{
    get { return Id; }
}

public int date;
        

public AlmanachCalendarDateMessage()
{
}

public AlmanachCalendarDateMessage(int date)
        {
            this.date = date;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(date);
            

}

public override void Deserialize(ICustomDataInput reader)
{

date = reader.ReadInt();
            

}


}


}