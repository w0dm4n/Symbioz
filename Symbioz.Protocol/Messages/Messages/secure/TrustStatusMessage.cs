


















// Generated on 06/04/2015 18:45:15
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TrustStatusMessage : Message
{

public const ushort Id = 6267;
public override ushort MessageId
{
    get { return Id; }
}

public bool trusted;
        public bool certified;
        

public TrustStatusMessage()
{
}

public TrustStatusMessage(bool trusted, bool certified)
        {
            this.trusted = trusted;
            this.certified = certified;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, trusted);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, certified);
            writer.WriteByte(flag1);
            

}

public override void Deserialize(ICustomDataInput reader)
{

byte flag1 = reader.ReadByte();
            trusted = BooleanByteWrapper.GetFlag(flag1, 0);
            certified = BooleanByteWrapper.GetFlag(flag1, 1);
            

}


}


}