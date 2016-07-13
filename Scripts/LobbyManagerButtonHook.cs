using UnityEngine;
using UnityEngine.EventSystems;
using Prototype.NetworkLobby;

public class LobbyManagerButtonHook : MonoBehaviour {

    private EventTrigger eventTrigger;

    //This is a debug script that provides a button hook to the lobby manager to send players back to the lobby

    void Start()
    {
        eventTrigger = GetComponent<EventTrigger>();
        foreach (EventTrigger.Entry e in eventTrigger.triggers)
        {
            if (e.eventID == EventTriggerType.PointerUp)
            {
                e.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
            }
        }
    }

    public void OnPointerDownDelegate(PointerEventData data)
    {
        LobbyManager.s_Singleton.ServerReturnToLobby();
	}
}
