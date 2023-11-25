using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    [SerializeField] Character[] characters = new Character[0];

    public Character[] GetAllCharacters() => characters;

    public Character getCharacterById(int id)
    {
        foreach (var character in characters)
        {
            if(character.Id == id)  
                return character;
        }

        return null;
    }
}
