using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.GenericRepository;
using License.Model.Model;

namespace License.Logic.ServiceLogic
{
    public class TeamLogic : BaseLogic
    {


        public List<Team> GetTeams()
        {
            List<Team> teams = new List<Team>();
            var teamlist = Work.TeamLicenseRepository.GetData();
            foreach (License.Core.Model.Team t in teamlist)
            {
                teams.Add(AutoMapper.Mapper.Map<License.Core.Model.Team, Team>(t));
            }
            return teams;
        }

        public Team GetTeamById(object id)
        {
            var obj = Work.TeamLicenseRepository.GetById(id);
            return AutoMapper.Mapper.Map<License.Core.Model.Team, Team>(obj);
        }

        public Team GetTeamByName(object name)
        {
            var obj =
                Work.TeamLicenseRepository.GetData().FirstOrDefault(t => t.Name.ToLower() == name.ToString().ToLower());
            return AutoMapper.Mapper.Map<License.Core.Model.Team, Team>(obj);
        }

        public Team CreateTeam(Team team)
        {
            var _team = AutoMapper.Mapper.Map<Team, License.Core.Model.Team>(team);
            _team = Work.TeamLicenseRepository.Create(_team);
            return AutoMapper.Mapper.Map<License.Core.Model.Team, Team>(_team);
        }

        public bool DeleteTeam(object id)
        {
            return Work.TeamLicenseRepository.Delete(id);
        }
    }
}
