using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetCodeManagerUI : MonoBehaviour
{
    // [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;

    void Awake()
    {
        // serverBtn.onClick.AddListener(() =>
        // {
        //     NetworkManager.Singleton.StartServer();
        // });
        
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            Hider();
        });
        
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            Hider();
        });
    }

    public void Hider()
    {
        gameObject.SetActive(false);
    }
}
