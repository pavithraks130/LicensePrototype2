using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Models;
namespace LicenseServer.Logic
{
    public class OrganizationLogic : BaseLogic
    {
        public List<Organization> GetOrganizations()
        {
            List<Organization> teams = new List<Organization>();
            var teamlist = Work.OrganizationRepository.GetData();
            foreach (LicenseServer.Core.Model.Organization t in teamlist)
            {
                teams.Add(AutoMapper.Mapper.Map<LicenseServer.Core.Model.Organization, Organization>(t));
            }
            return teams;
        }

        public Organization GetOrganizationById(int id)
        {
            var obj = Work.OrganizationRepository.GetById(id);
            return AutoMapper.Mapper.Map<LicenseServer.Core.Model.Organization, Organization>(obj);
        }

        public Organization GetOrganizationByName(object name)
        {
            var obj = Work.OrganizationRepository.GetData().FirstOrDefault(t => t.Name.ToLower() == name.ToString().ToLower());
            return AutoMapper.Mapper.Map<Organization>(obj);
        }

        public Organization CreateOrganization(Organization team)
        {
            var _team = AutoMapper.Mapper.Map<Organization, LicenseServer.Core.Model.Organization>(team);
            _team = Work.OrganizationRepository.Create(_team);
            Work.OrganizationRepository.Save();
            team = AutoMapper.Mapper.Map<Organization>(_team);
            return team;
        }

        public Organization UpdateOrganization(Organization team)
        {
            var _team = AutoMapper.Mapper.Map<Organization, LicenseServer.Core.Model.Organization>(team);
            _team = Work.OrganizationRepository.Update(_team);
            Work.OrganizationRepository.Save();
            team = AutoMapper.Mapper.Map<Organization>(_team);
            return team;
        }

        public bool DeleteOrganization(object id)
        {
            var status = Work.OrganizationRepository.Delete(id);
            Work.OrganizationRepository.Save();
            return status;
        }
    }
}
