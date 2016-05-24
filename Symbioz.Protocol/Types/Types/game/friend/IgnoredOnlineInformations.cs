


















// Generated on 06/04/2015 18:45:33
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class IgnoredOnlineInformations : IgnoredInformations
{

public const short Id = 105;
public override short TypeId
{
    get { return Id; }
}

public uint playerId;
        public string playerName;
        public sbyte breed;
        public bool sex;
        

public IgnoredOnlineInformations()
{
}

public IgnoredOnlineInformations(int accountId, string accountName, uint playerId, string playerName, sbyte breed, bool sex)
         : base(accountId, accountName)
        {
            this.playerId = playerId;
            this.playerName = playerName;
            this.breed = breed;
            this.sex = sex;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(playerId);
            writer.WriteUTF(playerName);
            writer.WriteSByte(breed);
            writer.WriteBoolean(sex);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            playerId = reader.ReadVarUhInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            playerName = reader.ReadUTF();
            breed = reader.ReadSByte();
            if ((breed < (byte)Enums.PlayableBreedEnum.Feca) || (breed > (byte)Enums.PlayableBreedEnum.Eliotrope))
                throw new Exception("Forbidden value on breed = " + breed + ", it doesn't respect the following condition : (breed < (byte)Enums.PlayableBreedEnum.Feca) || (breed > (byte)Enums.PlayableBreedEnum.Eliotrope)");
            sex = reader.ReadBoolean();
            

}


}


}