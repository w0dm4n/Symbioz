


















// Generated on 06/04/2015 18:45:33
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class ProtectedEntityWaitingForHelpInfo
{

public const short Id = 186;
public virtual short TypeId
{
    get { return Id; }
}

public int timeLeftBeforeFight;
        public int waitTimeForPlacement;
        public sbyte nbPositionForDefensors;
        

public ProtectedEntityWaitingForHelpInfo()
{
}

public ProtectedEntityWaitingForHelpInfo(int timeLeftBeforeFight, int waitTimeForPlacement, sbyte nbPositionForDefensors)
        {
            this.timeLeftBeforeFight = timeLeftBeforeFight;
            this.waitTimeForPlacement = waitTimeForPlacement;
            this.nbPositionForDefensors = nbPositionForDefensors;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(timeLeftBeforeFight);
            writer.WriteInt(waitTimeForPlacement);
            writer.WriteSByte(nbPositionForDefensors);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

timeLeftBeforeFight = reader.ReadInt();
            waitTimeForPlacement = reader.ReadInt();
            nbPositionForDefensors = reader.ReadSByte();
            if (nbPositionForDefensors < 0)
                throw new Exception("Forbidden value on nbPositionForDefensors = " + nbPositionForDefensors + ", it doesn't respect the following condition : nbPositionForDefensors < 0");
            

}


}


}