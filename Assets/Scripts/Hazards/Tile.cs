using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isTargetable { get; set; }
    [SerializeField]
    private Transform modelTransform;
    [SerializeField]
    private float crossFadeSpeed;
    [SerializeField]
    private float shakeAmount;
    [SerializeField]
    private float shakeDuration;
    private float currentShakeTime;

    private Animator anim;
    private Vector3 originPos;
    private Quaternion originRot;

    private void Start()
    {
        anim = GetComponent<Animator>();
        originPos = modelTransform.localPosition;
        originRot = modelTransform.localRotation;
        isTargetable = true;
    }

    public void StartShaking()
    {
        StartCoroutine(ShakingEffect());
    }

    IEnumerator ShakingEffect()
    {
        while(currentShakeTime < shakeDuration)
        {
            modelTransform.localPosition = originPos + Random.insideUnitSphere * shakeAmount;
            modelTransform.localRotation = new Quaternion(originRot.x, originRot.y, Random.Range(0, shakeAmount), 0);
            currentShakeTime += Time.deltaTime;
            yield return null;
        }
        currentShakeTime = 0;
        modelTransform.localPosition = originPos;
        modelTransform.localRotation = originRot;
        yield return new WaitForSeconds(2);
        anim.Play("Collapse");
        yield return new WaitForSeconds(5);
        anim.CrossFade("Idle", crossFadeSpeed);
        yield return new WaitForSeconds(2);
        isTargetable = true;
    }
}
