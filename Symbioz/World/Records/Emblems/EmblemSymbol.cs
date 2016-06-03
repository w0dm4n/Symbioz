using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Emblems
{
    [Table("EmblemsSymbols")]
    public class EmblemSymbol : ITable
    {
        public static List<EmblemSymbol> EmblemsSymbols = new List<EmblemSymbol>();

        public int IconId;
        public int SkindId;
        public int Order;
        public int CategoryId;
        public int Colorizable;

        public EmblemSymbol(int iconId, int skinId, int order, int categoryId, int colorizable)
        {
            this.IconId = iconId;
            this.SkindId = skinId;
            this.Order = order;
            this.CategoryId = categoryId;
            this.Colorizable = colorizable;
        }

        public static int GetSkinId(int iconId)
        {
            int skinId = -1;

            var emblemSymbol = EmblemsSymbols.FirstOrDefault(x => x.IconId == iconId);
            if(emblemSymbol != null)
            {
                skinId = emblemSymbol.SkindId;
            }

            return skinId;
        }
    }
}
