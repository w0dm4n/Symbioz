


















// Generated on 06/04/2015 18:45:23
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

    public class IdentifiedEntityDispositionInformations : EntityDispositionInformations
    {

        public const short Id = 107;
        public override short TypeId
        {
            get { return Id; }
        }

        public int id;

        public IdentifiedEntityDispositionInformations()
        {
        }

        public IdentifiedEntityDispositionInformations(short cellId, sbyte direction, int id)
            : base(cellId, direction)
        {
            this.id = id;
        }


        public override void Serialize(ICustomDataOutput writer)
        {

            base.Serialize(writer);
            writer.WriteInt(id);


        }

        public override void Deserialize(ICustomDataInput reader)
        {

            base.Deserialize(reader);
            id = reader.ReadInt();


        }


    }


}