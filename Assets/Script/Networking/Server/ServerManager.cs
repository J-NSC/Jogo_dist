using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerManager : MonoBehaviour
{
   [Header("Setting")] 
   [SerializeField] string characterSelectSceneName = "CharacterSelect";
   [SerializeField] string gameplaySceneName = "Level1";
   
   public static ServerManager instance { get; private set; }
   bool gameHasStarted;
   public Dictionary<ulong, ClientData> ClientData { get; private set; }

   void Awake()
   {
      if (instance != null && instance != this)
      {
         Destroy(gameObject);
      }
      else
      {
         instance = this;
         DontDestroyOnLoad(gameObject);
         
      }
   }

   public void StartServer()
   {
      NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
      NetworkManager.Singleton.OnServerStarted += OnNetworkReady;
      
      ClientData = new Dictionary<ulong, ClientData>();

      NetworkManager.Singleton.StartServer();
   }



   public void StartHost()
   {
      NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
      NetworkManager.Singleton.OnServerStarted += OnNetworkReady;

      ClientData = new Dictionary<ulong, ClientData>();
      
      NetworkManager.Singleton.StartHost();
   }



   void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
   {
      if (ClientData.Count >= 4  || gameHasStarted)
      {
         response.Approved = false;
         return;
      }

      response.Approved = true;
      response.CreatePlayerObject = false;
      response.Pending = false;

      ClientData[request.ClientNetworkId] = new ClientData(request.ClientNetworkId);
      Debug.Log($"Added client {request.ClientNetworkId}");

   }
   
   void OnNetworkReady()
   {
      NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDIsconnect;
      NetworkManager.Singleton.SceneManager.LoadScene(characterSelectSceneName, LoadSceneMode.Single);
   }

   void OnClientDIsconnect(ulong clientId)
   {
      if (ClientData.ContainsKey(clientId))
      {
         if (ClientData.Remove(clientId))
         {
            Debug.Log($"Removed client {clientId}");
         }
      }
   }

   public void SetCharacter(ulong clientId, int characterId)
   {
      if (ClientData.TryGetValue(clientId, out ClientData data))
      {
         data.characterId = characterId;
      }
   }

   public void StartGame()
   {
      gameHasStarted = true;

      NetworkManager.Singleton.SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
   }
}
