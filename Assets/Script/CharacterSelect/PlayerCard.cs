using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
   [SerializeField] CharacterDatabase characterDatabase;
   [SerializeField] GameObject visuals;
   [SerializeField] Image characterIconImage;
   [SerializeField] TMP_Text playerNameText;
   [SerializeField] TMP_Text characterNameText;

   public void UpdateDisplay(CharacterSelectState state)
   {
      if (state.characterId != -1)
      {
         var character = characterDatabase.GetCharacterById(state.characterId);

         characterIconImage.sprite = character.Icon;
         characterIconImage.enabled = true;
         characterNameText.text = character.DisplayName;
      }
      else
      {
         characterIconImage.enabled = false;
      }

      playerNameText.text =  state.IsLockedIn ? $"player {state.characterId}" : $"Player {state.clienteId} (Picking ...)";
      
      visuals.SetActive(true);
   }

   public void DisableDisplay()
   {
      visuals.SetActive(false);
   }
   
}
