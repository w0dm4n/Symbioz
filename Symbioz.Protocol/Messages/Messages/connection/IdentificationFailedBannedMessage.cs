


















// Generated on 06/04/2015 18:44:03
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

    public class IdentificationFailedBannedMessage : IdentificationFailedMessage
    {

        public const ushort Id = 6174;
        public override ushort MessageId
        {
            get { return Id; }
        }

        public double banEndDate;


        public IdentificationFailedBannedMessage()
        {
        }

        public IdentificationFailedBannedMessage(sbyte reason, double banEndDate)
            : base(reason)
        {
            this.banEndDate = banEndDate;
        }


        public override void Serialize(ICustomDataOutput writer)
        {

            base.Serialize(writer);
            writer.WriteDouble(banEndDate);

        }

        public override void Deserialize(ICustomDataInput reader)
        {

            base.Deserialize(reader);
            banEndDate = reader.ReadDouble();
            if ((banEndDate < 0) || (banEndDate > 9.007199254740992E15))
                throw new Exception("Forbidden value on banEndDate = " + banEndDate + ", it doesn't respect the following condition : (banEndDate < 0) || (banEndDate > 9.007199254740992E15)");


        }


    }


}