


















// Generated on 06/04/2015 18:44:20
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

    public class GameFightPlacementSwapPositionsMessage : Message
    {

        public const ushort Id = 6544;
        public override ushort MessageId
        {
            get { return Id; }
        }

        public IEnumerable<Types.IdentifiedEntityDispositionInformations> dispositions;


        public GameFightPlacementSwapPositionsMessage()
        {
        }

        public GameFightPlacementSwapPositionsMessage(IEnumerable<Types.IdentifiedEntityDispositionInformations> dispositions)
        {
            this.dispositions = dispositions;
        }


        public override void Serialize(ICustomDataOutput writer)
        {

            foreach (var entry in dispositions)
            {
                entry.Serialize(writer);
            }


        }

        public override void Deserialize(ICustomDataInput reader)
        {
            dispositions = new Types.IdentifiedEntityDispositionInformations[2];
            for (int i = 0; i < 2; i++)
            {
                (dispositions as Types.IdentifiedEntityDispositionInformations[])[i] = new Types.IdentifiedEntityDispositionInformations();
                (dispositions as Types.IdentifiedEntityDispositionInformations[])[i].Deserialize(reader);
            }


        }


    }


}