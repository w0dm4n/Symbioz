
using Symbioz.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.Auth.Records
{
    [Table("AccountsInformations", true)]
    public class AccountInformationsRecord : ITable
    {
        public static List<AccountInformationsRecord> AccountsInformations = new List<AccountInformationsRecord>();

        [Primary]
        public int Id;
        [Update]
        public uint BankKamas;

        public AccountInformationsRecord(int id,uint bankkamas)
        {
            this.Id = id;
            this.BankKamas = bankkamas;
        }
        public static void AddAccountInformations(int accountid,int startbankkamas)
        {
            AccountsProvider.CreateAccountInformation(accountid,startbankkamas);
            AccountsInformations.Add(new AccountInformationsRecord(accountid,(uint)startbankkamas));
        }
        public static void CheckAccountInformations(int accountid,int startbankkamas)
        {
           
           if (AccountsInformations.Find(x=>x.Id == accountid) == null)
           {
               AddAccountInformations(accountid,startbankkamas);
           }
        }
        public static AccountInformationsRecord GetInformations(int accountid)
        {
            return AccountsInformations.Find(x => x.Id == accountid);
        }
    }
}
