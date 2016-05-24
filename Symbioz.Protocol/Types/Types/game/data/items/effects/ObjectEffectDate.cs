


















// Generated on 06/04/2015 18:45:33
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class ObjectEffectDate : ObjectEffect
{

public const short Id = 72;
public override short TypeId
{
    get { return Id; }
}

public ushort year { get; set; }
public sbyte month { get;set; }
public sbyte day { get; set; }
public sbyte hour { get; set; }
public sbyte minute { get; set; }
        

public ObjectEffectDate()
{
}

public ObjectEffectDate(ushort actionId, ushort year, sbyte month, sbyte day, sbyte hour, sbyte minute)
         : base(actionId)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(year);
            writer.WriteSByte(month);
            writer.WriteSByte(day);
            writer.WriteSByte(hour);
            writer.WriteSByte(minute);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            year = reader.ReadVarUhShort();
            if (year < 0)
                throw new Exception("Forbidden value on year = " + year + ", it doesn't respect the following condition : year < 0");
            month = reader.ReadSByte();
            if (month < 0)
                throw new Exception("Forbidden value on month = " + month + ", it doesn't respect the following condition : month < 0");
            day = reader.ReadSByte();
            if (day < 0)
                throw new Exception("Forbidden value on day = " + day + ", it doesn't respect the following condition : day < 0");
            hour = reader.ReadSByte();
            if (hour < 0)
                throw new Exception("Forbidden value on hour = " + hour + ", it doesn't respect the following condition : hour < 0");
            minute = reader.ReadSByte();
            if (minute < 0)
                throw new Exception("Forbidden value on minute = " + minute + ", it doesn't respect the following condition : minute < 0");
            

}


}


}