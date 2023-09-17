using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDoll : MonoBehaviour
{
    [SerializeField] Rigidbody[] ragDollLimbs;
    private Animator anim;
    private Rigidbody rb;

    bool isRagDoll = false;

    private void Start()
    {
        anim = GetComponent <Animator>();
        rb = GetComponent<Rigidbody>();
        SetRagDoll(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isRagDoll = !isRagDoll;
            SetRagDoll(isRagDoll);
        }
    }

    void SetRagDoll(bool value)
    {
        anim.enabled = !value;
        foreach (Rigidbody limb in ragDollLimbs)
        {
            limb.isKinematic = !value;
        }
    }
}
