using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.GenericRepository;
using License.Model.Model;

namespace License.Logic.ServiceLogic
{
    public class OrganizationLogic : BaseLogic
    {

        public List<Organization> GetTeams()
        {
            List<Organization> teams = new List<Organization>();
            var teamlist = Work.TeamLicenseRepository.GetData();
            foreach (License.Core.Model.Organization t in teamlist)
            {
                teams.Add(AutoMapper.Mapper.Map<License.Core.Model.Organization, Organization>(t));
            }
            return teams;
        }

        public Organization GetTeamById(object id)
        {
            var obj = Work.TeamLicenseRepository.GetById(id);
            return AutoMapper.Mapper.Map<License.Core.Model.Organization, Organization>(obj);
        }

        public Organization GetTeamByName(object name)
        {
            var obj =
                Work.TeamLicenseRepository.GetData().FirstOrDefault(t => t.Name.ToLower() == name.ToString().ToLower());
            if (obj == null)
                return null;
            return AutoMapper.Mapper.Map<License.Core.Model.Organization, Organization>(obj);
        }

        public Organization CreateTeam(Organization team)
        {
            var _team = AutoMapper.Mapper.Map<Organization, License.Core.Model.Organization>(team);
            _team = Work.TeamLicenseRepository.Create(_team);
            Work.Save();
            team = AutoMapper.Mapper.Map<License.Core.Model.Organization, Organization>(_team);
            return team;
        }

        public Organization UpdateTeam(Organization team)
        {
            var _team = AutoMapper.Mapper.Map<Organization, License.Core.Model.Organization>(team);
            _team = Work.TeamLicenseRepository.Update(_team);
            Work.Save();
            team = AutoMapper.Mapper.Map<License.Core.Model.Organization, Organization>(_team);
            return team;
        }

        public bool DeleteTeam(object id)
        {
            var status = Work.TeamLicenseRepository.Delete(id);
            Work.Save();
            return status;
        }
    }
}
