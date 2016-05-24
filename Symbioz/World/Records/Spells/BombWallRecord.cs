using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records.Spells
{
    [Table("BombWalls")]
    public class BombWallRecord : ITable
    {
        public static List<BombWallRecord> BombWalls = new List<BombWallRecord>();


        public sbyte WallId;

        public string WallName;

        public ushort SpellId;
        /// <summary>
        /// Id utilisé dans les SpellEffects pour determiner de quel mur il s'agit
        /// </summary>
        public int BombMonsterId;

        public int Color;

        public ushort CibleDetonationSpellId;

        public ushort DetonationSpellId;

        public BombWallRecord(sbyte wallId, string wallName, ushort spellid, int bombMonsterId, int color, ushort cibleDetonationSpellId, ushort detonationSpellId)
        {
            this.WallId = wallId;
            this.WallName = wallName;
            this.SpellId = spellid;
            this.BombMonsterId = bombMonsterId;
            this.Color = color;
            this.CibleDetonationSpellId = cibleDetonationSpellId;
            this.DetonationSpellId = detonationSpellId;
        }

        public static BombWallRecord GetWallRecord(int monsterId)
        {
            return BombWalls.Find(x => x.BombMonsterId == monsterId);
        }


    }
}
