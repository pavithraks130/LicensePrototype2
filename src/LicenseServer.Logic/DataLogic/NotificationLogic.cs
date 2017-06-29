using License.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.Logic.DataLogic
{
    public class NotificationLogic : BaseLogic
    {
        /// <summary>
        /// Get notification item from DB
        /// </summary>
        /// <returns></returns>
        public List<Notification> GetNotifications()
        {
            List<Notification> notificationsList = new List<Notification>();
            var notificationObject = Work.NotificationRepository.GetData();
            notificationsList = notificationObject.Select(n => AutoMapper.Mapper.Map<Notification>(n)).ToList();
            return notificationsList;
        }
        /// <summary>
        /// Create Notification item 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Notification CreateNotificationItem(Notification item)
        {
            Core.Model.Notification notificationItem = AutoMapper.Mapper.Map<Notification, Core.Model.Notification>(item);
            notificationItem = Work.NotificationRepository.Create(notificationItem);

            Work.NotificationRepository.Save();
            return AutoMapper.Mapper.Map<Notification>(notificationItem);
        }
        /// <summary>
        /// Update Notification data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Notification UpdateNotification(Notification item)
        {
            Core.Model.Notification notificationItem = Work.NotificationRepository.GetById(item.Id);
            notificationItem.NotificationData = item.NotificationData;
            notificationItem = Work.NotificationRepository.Update(notificationItem);

            Work.NotificationRepository.Save();
            return AutoMapper.Mapper.Map<Notification>(notificationItem);

        }

        /// <summary>
        /// Delete Notification data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Notification DeleteNotificationItem(int id)
        {
            var notificationObj = Work.NotificationRepository.GetById(id);
            notificationObj = Work.NotificationRepository.Delete(notificationObj);
            Work.NotificationRepository.Save();
            return AutoMapper.Mapper.Map<Notification>(notificationObj);
        }

    }
}
