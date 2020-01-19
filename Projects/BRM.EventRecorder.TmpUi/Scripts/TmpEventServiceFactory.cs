using BRM.EventRecorder.TmpUi.Subscribers;
using BRM.EventRecorder.UnityUi;

namespace BRM.EventRecorder.TmpUi
{
    public class TmpEventServiceFactory : UnityEventServiceFactory
    {
        public override EventService Create()
        {
            var service = base.Create();
            service.AddUniqueSubscriber(new TmpDropdownSubscriber());
            service.AddUniqueSubscriber(new TmpTextInputSubscriber());
            return service;
        }
    }
}