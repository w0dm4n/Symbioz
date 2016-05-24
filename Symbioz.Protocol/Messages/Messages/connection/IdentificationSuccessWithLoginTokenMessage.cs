


















// Generated on 06/04/2015 18:44:03
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class IdentificationSuccessWithLoginTokenMessage : IdentificationSuccessMessage
{

public const ushort Id = 6209;
public override ushort MessageId
{
    get { return Id; }
}

public string loginToken;
        

public IdentificationSuccessWithLoginTokenMessage()
{
}

public IdentificationSuccessWithLoginTokenMessage(bool hasRights, bool wasAlreadyConnected, string login, string nickname, int accountId, sbyte communityId, string secretQuestion, double accountCreation, double subscriptionElapsedDuration, double subscriptionEndDate, string loginToken)
         : base(hasRights, wasAlreadyConnected, login, nickname, accountId, communityId, secretQuestion, accountCreation, subscriptionElapsedDuration, subscriptionEndDate)
        {
            this.loginToken = loginToken;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUTF(loginToken);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            loginToken = reader.ReadUTF();
            

}


}


}