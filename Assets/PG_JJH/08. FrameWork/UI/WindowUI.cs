using UnityEngine.EventSystems;
using JJH;

namespace JJH
{
    public class WindowUI : BaseUI, IDragHandler, IPointerDownHandler
    {
        public void OnDrag(PointerEventData eventData)
        {
            transform.Translate(eventData.delta);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Manager.UI.SelectWindowUI(this);
        }

        public void Close()
        {
            Manager.UI.CloseWindowUI(this);
        }
    }

}

