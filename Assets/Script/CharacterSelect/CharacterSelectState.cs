using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct CharacterSelectState : INetworkSerializable, IEquatable<CharacterSelectState>
{
    public ulong clinetId;
    public int characterId;

    public CharacterSelectState(ulong clinetId, int characterId = -1)
    {
        this.clinetId = clinetId;
        this.characterId = characterId;
    }
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clinetId);
        serializer.SerializeValue(ref characterId);
        
    }

    public bool Equals(CharacterSelectState other)
    {
        return clinetId == other.clinetId &&
               characterId == other.characterId;
    }
}
