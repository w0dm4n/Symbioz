using Symbioz.DofusProtocol.Types;
using Symbioz.ORM;
using Symbioz.World.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.World.Records
{
    [Table("CharactersJobs",true)]

    class CharacterJobRecord : ITable
    {
        public static List<CharacterJobRecord> CharactersJobs = new List<CharacterJobRecord>();

        [Primary]
        public int Id;
        public int CharacterId;
        public sbyte JobId;
        [Update]
        public byte JobLevel;
        [Update]
        public ulong JobExp;

        public CharacterJobRecord(int id,int characterid,sbyte jobid,byte joblevel,ulong jobexp)
        {
            this.Id = id;
            this.CharacterId = characterid;
            this.JobId = jobid;
            this.JobLevel=joblevel;
            this.JobExp =jobexp;
        }
        int GetNextId()
        {
            if (CharactersJobs.Count() > 0)
               return CharactersJobs.Last().Id + 1;
            else
                return 1;
        }
        public CharacterJobRecord(int characterid, sbyte jobid, byte joblevel, ulong jobexp)
        {
            this.Id = GetNextId();
            this.CharacterId = characterid;
            this.JobId = jobid;
            this.JobLevel = joblevel;
            this.JobExp = jobexp;
        }
        public JobExperience GetJobExperience()
        {
            return new JobExperience(JobId, JobLevel, JobExp, ExperienceRecord.GetExperienceForLevel(JobLevel), ExperienceRecord.GetExperienceForLevel((uint)(JobLevel + 1)));
        }
        public JobDescription GetJobDescription()
        {
            return new JobDescription(JobId, new SkillActionDescription[0]);
        }
        public JobCrafterDirectorySettings GetDirectorySettings()
        {
            return new JobCrafterDirectorySettings(JobId, 1, false);
        }

        public static JobsDetailedDatas GetCharacterJobsDatas(int characterid)
        {
           var jobs = CharactersJobs.FindAll(x => x.CharacterId == characterid);
           JobsDetailedDatas result = new JobsDetailedDatas();
           foreach (var job in jobs)
           {
               result.JobsDescriptions.Add(job.GetJobDescription());
               result.JobsExperiences.Add(job.GetJobExperience());
               result.JobSettings.Add(job.GetDirectorySettings());
           }
           return result;
        }
        public static CharacterJobRecord GetJob(int characterid,sbyte jobid)
        {
            return CharactersJobs.Find(x => x.CharacterId == characterid && x.JobId == jobid);
        }
        public static void RemoveAll(int characterid)
        {
            CharactersJobs.FindAll(x => x.CharacterId == characterid).ForEach(x => x.RemoveElement());
        }
      
    }
}
