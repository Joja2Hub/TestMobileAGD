using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DropButtonController : MonoBehaviour
{
    [SerializeField] private Button dropButton;

    private ItemInteraction itemInteraction;

    private DiContainer _container; // Ссылка на контейнер Zenject

    private void Awake()
    {
        // Получаем ссылку на контейнер Zenject
        _container = FindAnyObjectByType<SceneContext>().Container;

        // Запрашиваем экземпляр ItemInteraction из контейнера
        if (_container != null)
        {
            itemInteraction = _container.Resolve<ItemInteraction>();
        }

        if (itemInteraction == null)
        {
            Debug.LogError("ItemInteraction is not resolved!");
        }
    }

    private void Start()
    {
        if (dropButton == null)
        {
            Debug.LogError("Drop button is not assigned!");
            return;
        }

        if (itemInteraction == null)
        {
            Debug.LogError("ItemInteraction is not injected!");
            return;
        }

        dropButton.onClick.AddListener(() => itemInteraction.DropItem());
        itemInteraction.OnItemPickedUp += ShowButton;
        itemInteraction.OnItemDropped += HideButton;

        HideButton();
    }

    private void ShowButton()
    {
        if (dropButton != null)
        {
            dropButton.gameObject.SetActive(true);
        }
    }

    private void HideButton()
    {
        if (dropButton != null)
        {
            dropButton.gameObject.SetActive(false);
        }
    }
}