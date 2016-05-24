


















// Generated on 06/04/2015 18:44:47
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TaxCollectorListMessage : AbstractTaxCollectorListMessage
{

public const ushort Id = 5930;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte nbcollectorMax;
        public IEnumerable<Types.TaxCollectorFightersInformation> fightersInformations;
        

public TaxCollectorListMessage()
{
}

public TaxCollectorListMessage(IEnumerable<Types.TaxCollectorInformations> informations, sbyte nbcollectorMax, IEnumerable<Types.TaxCollectorFightersInformation> fightersInformations)
         : base(informations)
        {
            this.nbcollectorMax = nbcollectorMax;
            this.fightersInformations = fightersInformations;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(nbcollectorMax);
            writer.WriteUShort((ushort)fightersInformations.Count());
            foreach (var entry in fightersInformations)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            nbcollectorMax = reader.ReadSByte();
            if (nbcollectorMax < 0)
                throw new Exception("Forbidden value on nbcollectorMax = " + nbcollectorMax + ", it doesn't respect the following condition : nbcollectorMax < 0");
            var limit = reader.ReadUShort();
            fightersInformations = new Types.TaxCollectorFightersInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 (fightersInformations as Types.TaxCollectorFightersInformation[])[i] = new Types.TaxCollectorFightersInformation();
                 (fightersInformations as Types.TaxCollectorFightersInformation[])[i].Deserialize(reader);
            }
            

}


}


}