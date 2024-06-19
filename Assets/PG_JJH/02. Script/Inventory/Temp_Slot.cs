using UnityEngine;
using UnityEngine.UI;

public class Temp_Slot : MonoBehaviour
{
    public GameObject ItemInSlot;
    public Image slotImage;
    Color originalColor;

    private void Start()
    {
        slotImage = GetComponentInChildren<Image>();
        originalColor = slotImage.color;
    }

    private void OnTriggerStay(Collider other)
    {
        if (ItemInSlot != null) return;
        GameObject obj = other.gameObject;
        if (!IsItem(obj)) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            InsertItem(obj);
        }
    }

    bool IsItem(GameObject obj)
    {
        return obj.GetComponent<TempItem>();
    }

    void InsertItem(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.SetParent(gameObject.transform, true);
        obj.transform.localEulerAngles = obj.GetComponent<TempItem>().slotRotation;
        obj.GetComponent<TempItem>().inSlot = true;
        obj.GetComponent<TempItem>().currentSlot = this;
        ItemInSlot = obj;
        slotImage.color = Color.gray;

    }

    public void ResetColor()
    {
        slotImage.color=originalColor;
    }









    private void Update()
    {

    }



}
