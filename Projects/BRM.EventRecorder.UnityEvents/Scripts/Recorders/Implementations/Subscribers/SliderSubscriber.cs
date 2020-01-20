using System.Collections.Generic;
using BRM.EventRecorder.UnityEvents.Models;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BRM.EventRecorder.UnityEvents.Recorders
{
    public class SliderSubscriber : TouchSubscriber<Slider, SliderEvent>
    {
        private List<SliderEvent> _events = new List<SliderEvent>();
        private readonly List<UnityAction<float>> _onSlides = new List<UnityAction<float>>();
        public override string Name => nameof(SliderSubscriber);
        public override EventModelCollection ExtractNewEvents()
        {
            var collection = new EventModelCollection();
            collection.SliderEvents.AddRange(_events);
            _events.Clear();
            return collection;
        }

        protected override void OnSubscribe(Slider slider)
        {
            UnityAction<float> onSlide = newVal =>
            {
                var newEvent = new SliderEvent
                {
                    NewValue = newVal,
                };
                PopulateCommonEventData(ref newEvent, slider.transform);
                _events.Add(newEvent);
            };
            _onSlides.Add(onSlide);
            slider.onValueChanged.AddListener(onSlide);
        }

        protected override void OnUnsubscribe(Slider slider)
        {
            _onSlides.ForEach(onSlide => slider.onValueChanged.RemoveListener(onSlide));
        }
        
        public override void UnsubscribeAll()
        {
            base.UnsubscribeAll();
            _onSlides.Clear();
        }
    }
}