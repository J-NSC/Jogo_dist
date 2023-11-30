using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct CharacterSelectState : INetworkSerializable, IEquatable<CharacterSelectState>
{
    public ulong clienteId;
    public int characterId;
    public bool IsLockedIn; 

    public CharacterSelectState(ulong clienteId, int characterId = -1, bool isLockedIn = false)
    {
        this.clienteId = clienteId;
        this.characterId = characterId;
        IsLockedIn = isLockedIn;
    }
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clienteId);
        serializer.SerializeValue(ref characterId);
        serializer.SerializeValue(ref IsLockedIn);
        
    }

    public bool Equals(CharacterSelectState other)
    {
        return clienteId == other.clienteId &&
               characterId == other.characterId &&
               IsLockedIn == other.IsLockedIn;
    }
}
