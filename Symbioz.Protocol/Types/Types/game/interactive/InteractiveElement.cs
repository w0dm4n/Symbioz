


















// Generated on 06/04/2015 18:45:35
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class InteractiveElement
{
    public int MapId { get; set; }
public int OptionalValue { get; set; }
public int OptionalValue2 { get; set; }
public const short Id = 80;
public virtual short TypeId
{
    get { return Id; }
}

public int elementId;
        public int elementTypeId;
        public IEnumerable<Types.InteractiveElementSkill> enabledSkills;
        public IEnumerable<Types.InteractiveElementSkill> disabledSkills;
        

public InteractiveElement()
{
}

public InteractiveElement(int elementId, int elementTypeId, IEnumerable<Types.InteractiveElementSkill> enabledSkills, IEnumerable<Types.InteractiveElementSkill> disabledSkills)
        {
            this.elementId = elementId;
            this.elementTypeId = elementTypeId;
            this.enabledSkills = enabledSkills;
            this.disabledSkills = disabledSkills;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(elementId);
            writer.WriteInt(elementTypeId);
            writer.WriteUShort((ushort)enabledSkills.Count());
            foreach (var entry in enabledSkills)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)disabledSkills.Count());
            foreach (var entry in disabledSkills)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

elementId = reader.ReadInt();
            if (elementId < 0)
                throw new Exception("Forbidden value on elementId = " + elementId + ", it doesn't respect the following condition : elementId < 0");
            elementTypeId = reader.ReadInt();
            var limit = reader.ReadUShort();
            enabledSkills = new Types.InteractiveElementSkill[limit];
            for (int i = 0; i < limit; i++)
            {
                 (enabledSkills as Types.InteractiveElementSkill[])[i] = Types.ProtocolTypeManager.GetInstance<Types.InteractiveElementSkill>(reader.ReadShort());
                 (enabledSkills as Types.InteractiveElementSkill[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            disabledSkills = new Types.InteractiveElementSkill[limit];
            for (int i = 0; i < limit; i++)
            {
                 (disabledSkills as Types.InteractiveElementSkill[])[i] = Types.ProtocolTypeManager.GetInstance<Types.InteractiveElementSkill>(reader.ReadShort());
                 (disabledSkills as Types.InteractiveElementSkill[])[i].Deserialize(reader);
            }
            

}


}


}