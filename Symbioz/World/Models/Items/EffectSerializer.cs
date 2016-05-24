using Symbioz.DofusProtocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Symbioz.World.Models.Items
{
    public class EffectSerializer
    {
        public static void Serialize(List<ObjectEffect> effects)
        {
            foreach (var effect in effects)
            {
                 YAXSerializer serializer = new YAXSerializer(effect.GetType());
                 string contents = serializer.Serialize(effect);
            }
        }
    }
}
