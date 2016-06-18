using Symbioz.World.Records.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Models.Fights
{
    public class SpellHistory
    {
        private List<CastedSpell> m_castedSpells = new List<CastedSpell>();
    
        public void Add(SpellLevelRecord level,int targetId)
        {
            m_castedSpells.Add(new CastedSpell(level.SpellId,targetId,level.MinCastInterval));
        }
        public void Clear()
        {
            m_castedSpells.Clear();
        }
        public bool CanCast(SpellLevelRecord level,int targetId)
        {
            int count = m_castedSpells.FindAll(x => x.SpellId == level.SpellId).Count(); // nombre de fois lancer ce tour
            int countPerTarget = m_castedSpells.FindAll(x => x.SpellId == level.SpellId && x.TargetId == targetId).Count(); // nombre de fois lancer ce tour
            var @default = m_castedSpells.FirstOrDefault(x => x.SpellId == level.SpellId);
            if (@default != null)
            {
                if (@default.CastInterval > 0)
                {
                    Logger.Log("LOCK BY CASTINTERVAL");
                    return false;
                }
            }
            if (count > level.MaxCastPerTurn && count != 0 && level.MaxCastPerTurn != 0)//a verifier
            {
                Logger.Log("LOCK BY MAXPERTURN Count:" + count + " > maxperturn:" + level.MaxCastPerTurn);
                return false;
            }
            if (count > level.MaxCastPerTarget && targetId != 0 && count != 0 && level.MaxCastPerTarget != 0)//a verifier
            {
                Logger.Log("LOCK BY MAXCASTPERTARGET Count:" + count + " > maxpertarget" + level.MaxCastPerTarget);
                return false;
            }
            return true;
        }
        public void OnTurnEnded()
        {
            m_castedSpells.ForEach(x => x.CastInterval--);
            m_castedSpells.RemoveAll(x => x.CastInterval <= 0);
        }


    }
    public class CastedSpell
    {
        public CastedSpell(ushort spellId,int targetId,short castInterval)
        {
            this.SpellId = spellId;
            this.TargetId = targetId;
            this.CastInterval = castInterval;
        }

        public short CastInterval { get; set; }
        public ushort SpellId { get; set; }
        public int TargetId { get; set; }
    }
}
