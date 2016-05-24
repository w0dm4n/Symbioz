// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.Classes
{
    public class Server : ID2OClass
    {
        [Cache]
        public static List<Server> Servers = new List<Server>();
        public Int32 id;
        public Int32 nameId;
        public Int32 commentId;
        public Double openingDate;
        public String language;
        public Int32 populationId;
        public Int32 gameTypeId;
        public Int32 communityId;
        public ArrayList restrictedToLanguages;
        public Server(Int32 id, Int32 nameId, Int32 commentId, Double openingDate, String language, Int32 populationId, Int32 gameTypeId, Int32 communityId, ArrayList restrictedToLanguages)
        {
            this.id = id;
            this.nameId = nameId;
            this.commentId = commentId;
            this.openingDate = openingDate;
            this.language = language;
            this.populationId = populationId;
            this.gameTypeId = gameTypeId;
            this.communityId = communityId;
            this.restrictedToLanguages = restrictedToLanguages;
        }
    }
}
