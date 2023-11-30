using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    public Image iconImage;
    [SerializeField] private GameObject disableOverlay;
    [SerializeField] Button button;

    CharacterDisplaySelect characterSelect;
    public Character Character { get; private set; }
    public bool IsDisable { get; private set; }
    
    public void SetCharacter(CharacterDisplaySelect characterSelect, Character character)
    {
        iconImage.sprite = character.Icon;
        this.characterSelect = characterSelect;
        Character = character;
    }

    public void SelectCharacter()
    {
        characterSelect.Select(Character);
    }

    public void SetDisable()
    {
        IsDisable = true; 
        disableOverlay.SetActive(true);
        button.interactable = false;
    }
}
