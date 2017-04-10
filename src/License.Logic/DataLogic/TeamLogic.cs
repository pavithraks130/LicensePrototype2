using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License;

namespace License.Logic.DataLogic
{
    public class TeamLogic : BaseLogic
    {
        public TeamLogic()
        {

        }

        public List<DataModel.Team> GetTeam()
        {
            var objList = Work.TeamRepository.GetData();
            List<DataModel.Team> teamList = new List<DataModel.Team>();
            foreach (var obj in objList)
            {
                var data = AutoMapper.Mapper.Map<DataModel.Team>(obj);
                teamList.Add(data);
            }
            return teamList;
        }

        public List<DataModel.Team> GetTeamsByAdmin(string adminId)
        {
            var objList = Work.TeamRepository.GetData(t => t.AdminId == adminId).ToList();
            List<DataModel.Team> teamList = new List<DataModel.Team>();
            foreach (var obj in objList)
            {
                var data = AutoMapper.Mapper.Map<DataModel.Team>(obj);
                teamList.Add(data);
            }
            return teamList;
        }

        public List<DataModel.Team> GetTeamsByUser(string userId)
        {
            var teamIdList = Work.TeamMemberRepository.GetData(tm => tm.InviteeUserId == userId).ToList().Select(t => t.TeamId).ToList();
            var objList = Work.TeamRepository.GetData(t => teamIdList.Contains(t.Id)).ToList();
            List<DataModel.Team> teamList = new List<DataModel.Team>();
            foreach (var obj in objList)
            {
                var data = AutoMapper.Mapper.Map<DataModel.Team>(obj);
                teamList.Add(data);
            }
            return teamList;
        }

        public DataModel.Team GetTeamById(int id)
        {
            TeamMemberLogic memLogic = new TeamMemberLogic();
            var obj = Work.TeamRepository.GetData(r => r.Id == id, null, "TeamMember").FirstOrDefault();
            DataModel.Team teamObj = AutoMapper.Mapper.Map<DataModel.Team>(obj);
            teamObj.TeamMembers = memLogic.GetTeamMembers(id);
            return teamObj;
        }

        public DataModel.Team CreateTeam(DataModel.Team model)
        {
            var obj = AutoMapper.Mapper.Map<Core.Model.Team>(model);
            obj = Work.TeamRepository.Create(obj);
            Work.TeamRepository.Save();
            if (obj.Id > 0)
                model = AutoMapper.Mapper.Map<DataModel.Team>(obj);
            return model;
        }

        public DataModel.Team UpdateTeam(int id, DataModel.Team model)
        {
            var obj = Work.TeamRepository.GetById(id);
            obj.Name = model.Name;
            obj = Work.TeamRepository.Update(obj);
            Work.TeamRepository.Save();
            return AutoMapper.Mapper.Map<DataModel.Team>(obj);
        }

        public bool DeleteTeam(int id)
        {
            var data = Work.TeamMemberRepository.GetData(tm => tm.TeamId == id).ToList();
            if (data.Count > 0)
            {
                int i = 0;
                foreach (var mem in data)
                {
                    Work.TeamMemberRepository.Delete(mem);
                    i++;
                }
                if (i > 0)
                    Work.TeamMemberRepository.Save();
            }
            var deletestatus = Work.TeamRepository.Delete(id);
            Work.TeamRepository.Save();
            return deletestatus;
        }
    }
}
