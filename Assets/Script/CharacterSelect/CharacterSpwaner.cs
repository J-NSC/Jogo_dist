using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSpwaner : NetworkBehaviour
{
    [SerializeField] CharacterDatabase characterDatabase;
    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;

        foreach (var client in ServerManager.instance.ClientData)
        {
            var character = characterDatabase.GetCharacterById(client.Value.characterId);
            if (character != null)
            {
                var spwanPos = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
                var characterInstance = Instantiate(character.GameplayerPrefab, spwanPos, Quaternion.identity);
                characterInstance.SpawnAsPlayerObject(client.Value.clientId);
            }
        }
    }
}
