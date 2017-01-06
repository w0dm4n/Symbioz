using Symbioz.ORM;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("CharactersInvisible", true)]
    public class CharactersInvisibleRecord : ITable
    {
        public static List<CharactersInvisibleRecord> CharactersInvisible = new List<CharactersInvisibleRecord>();
        [Primary]
        public int Id;
        [UpdateAttribute]
        public int CharacterId;
        [UpdateAttribute]
        public string LookBefore;
        [UpdateAttribute]
        public int MovementAuthorized;

        public CharactersInvisibleRecord(int id, int characterId, string lookBefore, int movementAuthorized)
        {
            this.Id = id;
            this.CharacterId = characterId;
            this.LookBefore = lookBefore;
            this.MovementAuthorized = movementAuthorized;
        }

        public static bool CheckInvisible(int characterId)
        {
            foreach (var character in CharactersInvisible)
            {
                if (character.CharacterId == characterId)
                    return true;
            }
            return false;
        }

        public static bool CanStillBeInvisible(int characterId)
        {
            foreach (var character in CharactersInvisible)
            {
                if (character.CharacterId == characterId)
                    if (character.MovementAuthorized > 0)
                        return true;
            }
            return false;
        }

        public static void DecreaseMovementAuthorized(int characterId)
        {
            foreach (var character in CharactersInvisible)
            {
                if (character.CharacterId == characterId)
                {
                    character.MovementAuthorized -= 1;
                    SaveTask.UpdateElement(character);
                    break;
                }
            }
        }

        public static int getAuthorizedMovement(int characterId)
        {
            foreach (var character in CharactersInvisible)
            {
                if (character.CharacterId == characterId)
                    return character.MovementAuthorized;
            }
            return 0;
        }

        public static void DeleteInvisibleCharacter(int characterId)
        {
            foreach (var character in CharactersInvisible)
            {
                if (character.CharacterId == characterId)
                {
                    SaveTask.RemoveElement(character);
                    break;
                }
            }
        }

        public static string getCharacterLook(int characterId)
        {
            foreach (var character in CharactersInvisible)
            {
                if (character.CharacterId == characterId)
                    return character.LookBefore;
            }
            return "{1}";
        }

        public static int PopId()
        {
            int Id = 1;
            foreach (var tmp in CharactersInvisible)
                Id++;
            Id++;
            return Id;
        }

    }
}
