using UnityEngine;
using Zenject;

public class ItemInteraction : IInitializable
{
    private Transform handPosition;
    private GameObject heldItem;
    private Camera mainCamera;

    public event System.Action OnItemPickedUp;
    public event System.Action OnItemDropped;

    [Inject]
    public ItemInteraction(Transform _handPosition)
    {
        handPosition = _handPosition;
    }

    public void Initialize()
    {
        mainCamera = Camera.main;
    }

    public void PickUpItem()
    {
        if (heldItem != null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f, LayerMask.GetMask("Interactable")))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                heldItem = hit.collider.gameObject;

                // Перемещаем предмет в руки
                heldItem.transform.SetParent(handPosition);
                heldItem.transform.localPosition = Vector3.zero;

                // Сбрасываем вращение предмета
                heldItem.transform.localRotation = Quaternion.identity;

                // Отключаем физическое взаимодействие
                Rigidbody rb = heldItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = false;
                    rb.isKinematic = true;
                }

                // Отключаем коллайдер
                Collider collider = heldItem.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = false;
                }

                OnItemPickedUp?.Invoke();
            }
        }
    }

    public void DropItem()
    {
        if (heldItem == null) return;

        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = mainCamera.transform.forward * 10f;
        }

        heldItem.transform.SetParent(null);

        Collider collider = heldItem.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;
        }

        heldItem = null;

        OnItemDropped?.Invoke();
    }
}