
using Symbioz.Utils;
using System;
namespace Symbioz.DofusProtocol.Messages
{
	public abstract class Message : IDisposable
	{
		private const byte BIT_RIGHT_SHIFT_LEN_PACKET_ID = 2;
		private const byte BIT_MASK = 3;

        public abstract ushort MessageId
		{
			get;
		}
		public void Unpack(ICustomDataInput reader)
		{
			this.Deserialize(reader);
		}
		public void Pack(ICustomDataOutput header)
		{
			var data = new CustomDataWriter ();
			Serialize (data);
			var size = data.Data.Length;
			var compute = ComputeTypeLen(size);
			short val = (short)((MessageId << 2) | compute);
			header.WriteShort (val);
			switch (compute)
			{
				case 1:
				header.WriteByte((byte)size);
                //header.WriteByte(0);//...
				break;
				case 2:
				header.WriteUShort((ushort)size);
				break;
				case 3:
				header.WriteByte((byte)((size >> 0x10) & 0xff));
				header.WriteUShort((ushort)(size & 0xffff));
				break;
			}
			header.WriteBytes (data.Data);
            data.Dispose();
			/*byte b = 3;
			short @short = (short)Message.SubComputeStaticHeader(this.MessageId, 3);
			writer.WriteShort(@short);
			for (int i = 3 - 1; i >= 0; i--)
			{
				writer.WriteByte(0);
			}
			this.Serialize(writer);
			long num = writer.Position - 5L;
			writer.Seek(2);
			for (int i = (int)(b - 1); i >= 0; i--)
			{
				writer.WriteByte((byte)(num >> 8 * i & 255L));
			}
			writer.Seek((int)num + 5);*/
		}
        public abstract void Serialize(ICustomDataOutput writer);
		public abstract void Deserialize(ICustomDataInput reader);

        private static byte ComputeTypeLen(int param1)
		{
			byte result;
			if (param1 > 65535)
			{
				result = 3;
			}
			else
			{
				if (param1 > 255)
				{
					result = 2;
				}
				else
				{
					if (param1 > 0)
					{
						result = 1;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}
		private static uint SubComputeStaticHeader(uint id, byte typeLen)
		{
			return id << 2 | (uint)typeLen;
		}
		public override string ToString()
		{
			return base.GetType().Name;
		}

		public void Dispose()
		{
		}
	}
}
