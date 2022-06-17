using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSkinController : MonoBehaviour
{
    [SerializeField]
    private GameEventChangeSkinColor changeSkinColor;
    [SerializeField]
    private List<Material> skinColors;
    private Renderer[] characterMaterials;
    [SerializeField]
    private PlayerMenuNavigator playerNav;
    private Vector2 offset;
    private int skinColorIndex;

    private void OnEnable()
    {
        characterMaterials = GetComponentsInChildren<Renderer>();
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
            AudioManager.instance.PlaySwitchColorEvent();
            changeSkinColor.RaiseChangeSkinColor(playerNav.playerID, index, skinColors[index]);
        }
    }

    public void NextSkinType()
    {
        skinColorIndex++;
        if (skinColorIndex > skinColors.Count - 1)
        {
            skinColorIndex = 0;
        }
        ChangeSkinType(skinColorIndex);
    }

    public void PrevSkinType()
    {
        skinColorIndex--;
        if (skinColorIndex < 0)
        {
            skinColorIndex = skinColors.Count - 1;
        }
        ChangeSkinType(skinColorIndex);
    }

    
}
