using BRM.InteractionRecorder.TmpUi.Subscribers;
using BRM.InteractionRecorder.UnityUi;

namespace BRM.InteractionRecorder.TmpUi
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