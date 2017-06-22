using License.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.Logic.DataLogic
{
  public class NotificationLogic:BaseLogic
    {
        public List<Notification> GetNotifications()
        {
            List<Notification> notificationsList = new List<Notification>();
            var notificationObject = Work.NotificationRepository.GetData();
            foreach (var item in notificationObject)
            {
                notificationsList.Add(AutoMapper.Mapper.Map<Core.Model.Notification,Notification>(item));
            }
            return notificationsList;
        }
        public Notification CreateNotificationItem(Notification item)
        {
            Core.Model.Notification notificationItem = AutoMapper.Mapper.Map<Notification, Core.Model.Notification>(item);
            notificationItem = Work.NotificationRepository.Create(notificationItem);

            Work.NotificationRepository.Save();
            return AutoMapper.Mapper.Map<Notification>(notificationItem);
        }
        public Notification UpdateNotification(Notification item)
        {
            Core.Model.Notification notificationItem = Work.NotificationRepository.GetById(item.Id);
            notificationItem.NotificationData = item.NotificationData;
            notificationItem = Work.NotificationRepository.Update(notificationItem);

            Work.NotificationRepository.Save();
            return AutoMapper.Mapper.Map<Notification>(notificationItem);

        }
        public bool DeleteNotificationItem(int id)
        {
            bool status= Work.NotificationRepository.Delete(id);

            Work.NotificationRepository.Save();
            return status;
        }

    }
}
