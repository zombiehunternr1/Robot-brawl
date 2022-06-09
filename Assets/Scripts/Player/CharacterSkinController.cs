using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSkinController : MonoBehaviour
{
    [SerializeField]
    private List<Material> skinColors;
    private Renderer[] characterMaterials;
    private int skinColorIndex;
    private PlayerMenuNavigator playerNav;
    private Vector2 offset;

    private void OnEnable()
    {
        characterMaterials = GetComponentsInChildren<Renderer>();
        ChangeSkinType(skinColorIndex);
    }

    private void Start()
    {
        playerNav = GetComponent<PlayerMenuNavigator>();
    }

    public void NormalExpression()
    {
        offset = new Vector2(0, 0);
        SetEyesExpression(offset);
    }

    public void StunnedExpression()
    {
        offset = new Vector2(0, .66f);
        SetEyesExpression(offset);
    }

    private void SetEyesExpression(Vector2 offset)
    {
        for (int i = 0; i < characterMaterials.Length; i++)
        {
            if (characterMaterials[i].transform.CompareTag("Eyes"))
                characterMaterials[i].material.mainTextureOffset = offset;
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
        MainMenuUIManager.changeColorDisplay.Invoke(playerNav.playerID, index, skinColors[skinColorIndex]);
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
