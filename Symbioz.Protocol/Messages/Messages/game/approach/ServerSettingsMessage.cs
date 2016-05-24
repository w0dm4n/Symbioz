


















// Generated on 06/04/2015 18:44:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ServerSettingsMessage : Message
{

public const ushort Id = 6340;
public override ushort MessageId
{
    get { return Id; }
}

public string lang;
        public sbyte community;
        public sbyte gameType;
        

public ServerSettingsMessage()
{
}

public ServerSettingsMessage(string lang, sbyte community, sbyte gameType)
        {
            this.lang = lang;
            this.community = community;
            this.gameType = gameType;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(lang);
            writer.WriteSByte(community);
            writer.WriteSByte(gameType);
            

}

public override void Deserialize(ICustomDataInput reader)
{

lang = reader.ReadUTF();
            community = reader.ReadSByte();
            if (community < 0)
                throw new Exception("Forbidden value on community = " + community + ", it doesn't respect the following condition : community < 0");
            gameType = reader.ReadSByte();
            if (gameType < 0)
                throw new Exception("Forbidden value on gameType = " + gameType + ", it doesn't respect the following condition : gameType < 0");
            

}


}


}