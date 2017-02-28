using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;
namespace LicenseServer.Logic
{
    public class OrganizationLogic : BaseLogic
    {
        public List<Organization> GetTeams()
        {
            List<Organization> teams = new List<Organization>();
            var teamlist = Work.OrganizationRepository.GetData();
            foreach (LicenseServer.Core.Model.Organization t in teamlist)
            {
                teams.Add(AutoMapper.Mapper.Map<LicenseServer.Core.Model.Organization, Organization>(t));
            }
            return teams;
        }

        public Organization GetTeamById(object id)
        {
            var obj = Work.OrganizationRepository.GetById(id);
            return AutoMapper.Mapper.Map<LicenseServer.Core.Model.Organization, Organization>(obj);
        }

        public Organization GetTeamByName(object name)
        {
            var obj =
                Work.OrganizationRepository.GetData().FirstOrDefault(t => t.Name.ToLower() == name.ToString().ToLower());
            if (obj == null)
                return null;
            return AutoMapper.Mapper.Map<LicenseServer.Core.Model.Organization, Organization>(obj);
        }

        public Organization CreateTeam(Organization team)
        {
            var _team = AutoMapper.Mapper.Map<Organization, LicenseServer.Core.Model.Organization>(team);
            _team = Work.OrganizationRepository.Create(_team);
            Work.OrganizationRepository.Save();
            team = AutoMapper.Mapper.Map<LicenseServer.Core.Model.Organization, Organization>(_team);
            return team;
        }

        public Organization UpdateTeam(Organization team)
        {
            var _team = AutoMapper.Mapper.Map<Organization, LicenseServer.Core.Model.Organization>(team);
            _team = Work.OrganizationRepository.Update(_team);
            Work.OrganizationRepository.Save();
            team = AutoMapper.Mapper.Map<LicenseServer.Core.Model.Organization, Organization>(_team);
            return team;
        }

        public bool DeleteTeam(object id)
        {
            var status = Work.OrganizationRepository.Delete(id);
            Work.OrganizationRepository.Save();
            return status;
        }
    }
}
