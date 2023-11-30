using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplaySelect : NetworkBehaviour
{
    [SerializeField] CharacterDatabase characterDatabase;
    [SerializeField] Transform charactersHolder;
    [SerializeField] CharacterSelectButton selectButtonPrefab;
    [SerializeField] PlayerCard[] playerCards;
    [SerializeField] GameObject characterInfoPanel;
    [SerializeField] TMP_Text characterNameText;
    [SerializeField] Transform introSpwanPoint;
    [SerializeField] Button lockInButton;
    
    GameObject introInstace;
    List<CharacterSelectButton> characterButtons = new List<CharacterSelectButton>();
    NetworkList<CharacterSelectState> players;

    void Awake()
    {
        players = new NetworkList<CharacterSelectState>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            Character[] allCharacter = characterDatabase.GetAllCharacters();
            foreach (var character in allCharacter)
            {
                var selectButtonInstance = Instantiate(selectButtonPrefab, charactersHolder);
                selectButtonInstance.SetCharacter(this, character);
                characterButtons.Add(selectButtonInstance);
            }

            players.OnListChanged += HandlePlayerStateChanged;
        }
        
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
            
            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
        }
    }

   

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            players.OnListChanged -= HandlePlayerStateChanged;
        }
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;

          
        }
    }

    void HandleClientConnected(ulong clientId)
    {
        players.Add(new CharacterSelectState(clientId));
    }

    void HandleClientDisconnected(ulong clientId)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].clienteId == clientId)
            {
                players.RemoveAt(i);
                break;    
            }
        }

        // players.Remove(players.Find(x => x.id == clientId));
    }

    public void Select(Character character)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].clienteId != NetworkManager.Singleton.LocalClientId) { continue; }

            if (players[i].IsLockedIn) { return; }

            if (players[i].characterId == character.Id) { return; }

            if (IsCharacterTake(character.Id, false)) { return; }
        }
        
        characterNameText.text = character.DisplayName;
        characterInfoPanel.SetActive(true);

        if (introInstace != null)
        {
            Destroy(introInstace);
        }

        introInstace = Instantiate(character.IntroPrefab, introSpwanPoint);
        SelectServerRpc(character.Id);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SelectServerRpc(int characterId, ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].clienteId != serverRpcParams.Receive.SenderClientId) { continue; }

            if (!characterDatabase.IsValidCharacterId(characterId)) { return; }

            if (IsCharacterTake(characterId, true)) { return; }

            players[i] = new CharacterSelectState(
                players[i].clienteId,
                characterId,
                players[i].IsLockedIn
            );
        }

        foreach (var player in players)
        {
            if (!player.IsLockedIn) return;
        }
        
        foreach (var player in players)
        {
            ServerManager.instance.SetCharacter(player.clienteId, player.characterId);
        }
        
        ServerManager.instance.StartGame();
    }
    
    
    [ServerRpc(RequireOwnership =  false)]
    private void LockInServerRPC(ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].clienteId == serverRpcParams.Receive.SenderClientId)
            {
                if (!characterDatabase.IsValidCharacterId(players[i].characterId)) return;
                
                if(IsCharacterTake(players[i].characterId, true)) return;
                
                players[i] = new CharacterSelectState(
                    players[i].clienteId,
                    players[i].characterId,
                    true
                );
            }
            
        }
    }

    public void lockIn()
    {
        LockInServerRPC();
    }
    
    void HandlePlayerStateChanged(NetworkListEvent<CharacterSelectState> changeEvent)
    {
        for (int i = 0; i < playerCards.Length; i++)
        {
            if (players.Count > i)
            {
                Debug.Log("players " + players.Count);
                playerCards[i].UpdateDisplay(players[i]);
            }
            else
            {
                playerCards[i].DisableDisplay();
            }
        }

        foreach (var button in characterButtons)
        {
            if (button.IsDisable) continue;

            if (IsCharacterTake(button.Character.Id, false))
            {
                button.SetDisable();
            }
        }

        foreach (var player in players)
        {
            if (player.clienteId != NetworkManager.Singleton.LocalClientId) continue;

            if (player.IsLockedIn)
            {
                lockInButton.interactable = false;
                break;
            }

            if (IsCharacterTake(player.characterId, false))
            {
                lockInButton.interactable = false;
                break;
            }

            lockInButton.interactable = true;
            break;

        }
    }


    bool IsCharacterTake(int characterId, bool checkAll)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (!checkAll)
            {
                if (players[i].clienteId == NetworkManager.Singleton.LocalClientId) { continue; }
            }

            if (players[i].IsLockedIn && players[i].characterId == characterId)
            {
                return true;
            }
        }

        return false;
    }
}
