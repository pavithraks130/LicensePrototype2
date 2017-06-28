using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.Core.Model;
using FeatureDataModel = License.Models.Feature;

namespace LicenseServer.Logic
{
    public class FeaturesLogic : BaseLogic
    {
        public List<FeatureDataModel> GetFeatureList()
        {
            var features = Work.FeaturesRepository.GetData().ToList();
            List<FeatureDataModel> featureList = new List<FeatureDataModel>();
            foreach (Feature f in features)
            {
                FeatureDataModel f1 = AutoMapper.Mapper.Map<FeatureDataModel>(f);
                featureList.Add(f1);
            }
            return featureList;
        }

        public FeatureDataModel CreateFeature(FeatureDataModel f)
        {
            Feature fet = AutoMapper.Mapper.Map<Feature>(f);
            fet = Work.FeaturesRepository.Create(fet);
            Work.FeaturesRepository.Save();
            return AutoMapper.Mapper.Map<FeatureDataModel>(fet);
        }

        public bool DeleteFeature(int id)
        {
            var status = Work.FeaturesRepository.Delete(id);
            Work.FeaturesRepository.Save();
            return status;
        }

        public FeatureDataModel Update(int id, FeatureDataModel f)
        {
            Feature fet = Work.FeaturesRepository.GetById(id);
            fet.Name = f.Name;
            fet.Description = f.Description;
            fet.Version = f.Version;
            Work.FeaturesRepository.Save();
            return AutoMapper.Mapper.Map<FeatureDataModel>(fet);
        }

        public FeatureDataModel GetFeatureById(int id)
        {
            var f = Work.FeaturesRepository.GetById(id);
            return AutoMapper.Mapper.Map<FeatureDataModel>(f);
        }

        public List<FeatureDataModel> GetFeatureByCategoryId(int categoryId)
        {
            List<FeatureDataModel> featureList = new List<FeatureDataModel>();
            var category = Work.SubscriptionCategoryRepo.GetById(categoryId);
            if (category != null)
            {
                if (category.Features != null && category.Features.Count > 0)
                {
                    foreach (var fet in category.Features)
                    {
                        var objFeture = AutoMapper.Mapper.Map<FeatureDataModel>(fet);
                        featureList.Add(objFeture);
                    }
                }
            }
            return featureList;
        }
    }
}
