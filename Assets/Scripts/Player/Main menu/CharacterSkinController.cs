using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSkinController : MonoBehaviour
{
    [SerializeField]
    private List<Material> skinColors;
    private enum EyePosition { normal, happy, angry, dead}
    private EyePosition eyeState;
    private Renderer[] characterMaterials;
    private int skinColorIndex;
    private PlayerMenuNavigator playerNav;

    private void Start()
    {
        playerNav = GetComponent<PlayerMenuNavigator>();
        characterMaterials = GetComponentsInChildren<Renderer>();
        ChangeEyeOffset(eyeState);
        ChangeSkinType(skinColorIndex);
    }

    private void ChangeEyeOffset(EyePosition pos)
    {
        Vector2 offset = Vector2.zero;

        switch (pos)
        {
            case EyePosition.normal:
                offset = new Vector2(0, 0);
                break;
            case EyePosition.happy:
                offset = new Vector2(.33f, 0);
                break;
            case EyePosition.angry:
                offset = new Vector2(.66f, 0);
                break;
            case EyePosition.dead:
                offset = new Vector2(.33f, .66f);
                break;
            default:
                break;
        }
        for (int i = 0; i < characterMaterials.Length; i++)
        {
            if (characterMaterials[i].transform.CompareTag("Eyes"))
                characterMaterials[i].material.SetTextureOffset("_MainTex", offset);
        }
    }
    private void ChangeSkinType(int index)
    {
        for (int i = 0; i < characterMaterials.Length; i++){
            if (!characterMaterials[i].transform.CompareTag("Eyes"))
            {
                characterMaterials[i].material = skinColors[index];
            }
        }
        PlayerJoinManager.changeColorDisplay.Invoke(playerNav.playerID, index, skinColors[skinColorIndex]);
    }

    public void NextSkinType(InputAction.CallbackContext context)
    {
        if (context.performed && !playerNav.isConfirmed && playerNav.canInteract)
        {
            skinColorIndex++;
            if (skinColorIndex > skinColors.Count - 1)
            {
                skinColorIndex = 0;
            }
            ChangeSkinType(skinColorIndex);
        }
    }

    public void PrevSkinType(InputAction.CallbackContext context)
    {
        if (context.performed && !playerNav.isConfirmed && playerNav.canInteract)
        {
            skinColorIndex--;
            if (skinColorIndex < 0)
            {
                skinColorIndex = skinColors.Count - 1;
            }
            ChangeSkinType(skinColorIndex);
        }
    }

    
}
