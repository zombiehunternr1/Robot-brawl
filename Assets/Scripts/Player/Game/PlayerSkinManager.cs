using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    private enum EyePosition { normal, happy, angry, dead }
    private EyePosition eyeState;
    private Renderer[] characterMaterials;

    private void OnEnable()
    {
        characterMaterials = GetComponentsInChildren<Renderer>();
        ChangeEyeOffset(eyeState);
    }

    public void ChangeSkinType(Material skinColor)
    {
        for (int i = 0; i < characterMaterials.Length; i++)
        {
            if (!characterMaterials[i].transform.CompareTag("Eyes"))
            {
                characterMaterials[i].material = skinColor;
            }
        }
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
}
