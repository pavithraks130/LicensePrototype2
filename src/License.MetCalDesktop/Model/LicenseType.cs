using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace License.MetCalDesktop.Model
{
    /// <summary>
    /// License Type record details
    /// </summary>
    public class LicenseType
    {
        /// <summary>
        /// Constructor details
        /// </summary>
        public LicenseType()
        {
            TypeId = 0;
            TypeName = string.Empty;
            Description = string.Empty;
            ActiveDuration = 0;
        }

        /// <summary>
        /// License type id
        /// </summary>
        [Key]
        public int TypeId { get; set; }
        /// <summary>
        /// License tyoe name
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// License details 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// License activation duration time in days
        /// </summary>
        public int ActiveDuration { get; set; }
        /// <summary>
        /// License image url 
        /// </summary>
        public  string ImageUrl { get; set; }
        /// <summary>
        /// License price details
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// License feature list collection
        /// </summary>
        public virtual  ICollection<Feature> FeatureList { get; set; }
    }
}