


















// Generated on 06/04/2015 18:45:37
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class StartupActionAddObject
{

public const short Id = 52;
public virtual short TypeId
{
    get { return Id; }
}

public int uid;
        public string title;
        public string text;
        public string descUrl;
        public string pictureUrl;
        public IEnumerable<Types.ObjectItemInformationWithQuantity> items;
        

public StartupActionAddObject()
{
}

public StartupActionAddObject(int uid, string title, string text, string descUrl, string pictureUrl, IEnumerable<Types.ObjectItemInformationWithQuantity> items)
        {
            this.uid = uid;
            this.title = title;
            this.text = text;
            this.descUrl = descUrl;
            this.pictureUrl = pictureUrl;
            this.items = items;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(uid);
            writer.WriteUTF(title);
            writer.WriteUTF(text);
            writer.WriteUTF(descUrl);
            writer.WriteUTF(pictureUrl);
            writer.WriteUShort((ushort)items.Count());
            foreach (var entry in items)
            {
                 entry.Serialize(writer);
            }
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

uid = reader.ReadInt();
            if (uid < 0)
                throw new Exception("Forbidden value on uid = " + uid + ", it doesn't respect the following condition : uid < 0");
            title = reader.ReadUTF();
            text = reader.ReadUTF();
            descUrl = reader.ReadUTF();
            pictureUrl = reader.ReadUTF();
            var limit = reader.ReadUShort();
            items = new Types.ObjectItemInformationWithQuantity[limit];
            for (int i = 0; i < limit; i++)
            {
                 (items as Types.ObjectItemInformationWithQuantity[])[i] = new Types.ObjectItemInformationWithQuantity();
                 (items as Types.ObjectItemInformationWithQuantity[])[i].Deserialize(reader);
            }
            

}


}


}