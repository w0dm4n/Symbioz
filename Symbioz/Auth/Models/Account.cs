using Symbioz.Auth.Records;
using Symbioz.Network.Clients;
using Symbioz.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Auth.Models
{
    public class Account
    {


        public AccountInformationsRecord Informations { get { return AccountInformationsRecord.GetInformations(Id); } }

        public int Id { get; set; }

        public string Nickname { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public ServerRoleEnum Role { get; set; }

        public bool Banned { get; set; }

        public int MaxCharactersCount { get; set; }

        public int PointsCount { get; set; }

  
    }
}
