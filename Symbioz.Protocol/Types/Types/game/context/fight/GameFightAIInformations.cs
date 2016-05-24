


















// Generated on 06/04/2015 18:45:25
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

    public class GameFightAIInformations : GameFightFighterInformations
    {

        public const short Id = 151;
        public override short TypeId
        {
            get { return Id; }
        }



        public GameFightAIInformations()
        {
        }

        public GameFightAIInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, sbyte teamId, sbyte wave, bool alive, Types.GameFightMinimalStats stats, IEnumerable<ushort> previousPositions)
            : base(contextualId, look, disposition, teamId, wave, alive, stats, previousPositions)
        {
        }


        public override void Serialize(ICustomDataOutput writer)
        {

            base.Serialize(writer);


        }

        public override void Deserialize(ICustomDataInput reader)
        {

            base.Deserialize(reader);


        }


    }


}