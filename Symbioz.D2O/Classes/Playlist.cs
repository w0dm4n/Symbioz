// automatic generation Symbioz.Sync 2015

using Symbioz.D2O.InternalClasses;
using Symbioz.DofusProtocol.D2O;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Playlist
    {
        [Cache]
        public static List<Playlist> Playlists = new List<Playlist>();

        public Int32 id;
        public UInt32 silenceDuration;
        public Int32 iteration;
        public Int32 type;
        public PlaylistSound[] sounds;
        public Playlist(Int32 id, UInt32 silenceDuration, Int32 iteration, Int32 type,object[] sounds)
        {
            this.id = id;
            this.silenceDuration = silenceDuration;
            this.iteration = iteration;
            this.type = type;
            this.sounds = sounds.Cast<PlaylistSound>().ToArray();
        }
    }
}
