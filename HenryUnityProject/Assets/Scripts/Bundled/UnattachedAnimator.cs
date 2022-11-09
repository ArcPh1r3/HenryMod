using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animators : List<Animator> {


    public void AddAnimators(IEnumerable<Animator> animators) {
        AddRange(animators);
    }

    public void SetBool(string parameter, bool set) {
        for (int i = 0; i < Count; i++) {
            this[i]?.SetBool(parameter, set);
        }
    }

    public void SetFloat(string parameter, float amount) {
        for (int i = 0; i < Count; i++) {
            this[i]?.SetFloat(parameter, amount);
        }
    }

    public void Play(string anim, int layer) {
        for (int i = 0; i < Count; i++) {
            this[i]?.Play(anim, layer);
        }
    }

    public void Play(string anim) {
        for (int i = 0; i < Count; i++) {
            this[i]?.Play(anim);
        }
    }
}

public class UnattachedAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator[] animators;

    private Animators animatorList = new Animators();
    public Animators AnimatorList { get => animatorList; }



    [Header("whyt he fuck aren't these in the animator")]
    [SerializeField, Range(0, 0.999f)]
    private float aimPitch = 0.5f;
    [SerializeField, Range(0, 0.999f)]
    private float aimYaw = 0.5f;

    private float _jumpTim;

    private void Start() {

        animatorList.AddAnimators(animators);
    }

    void Update()
    {
        if (animatorList.Count == 0)
            return;

        Moob();
        Jumb();
        Aiming();
    }

    private void Moob() {
        //man it's been so long since I've written a moob function

        float hori = Input.GetAxis("Horizontal");
        float veri = Input.GetAxis("Vertical");

        animatorList.SetBool("isMoving", Mathf.Abs(hori) + Mathf.Abs(veri) > 0.01f);
        animatorList.SetFloat("forwardSpeed", veri);
        animatorList.SetFloat("rightSpeed", hori);
        
        animatorList.SetBool("isSprinting", Input.GetKey(KeyCode.LeftShift));
    }

    private void Jumb() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            animatorList.Play("Jump");
            animatorList.SetBool("isGrounded", false);
            _jumpTim = 1.5f;
        }

        _jumpTim -= Time.deltaTime;

        animatorList.SetFloat("upSpeed", Mathf.Lerp(-48, 16, _jumpTim / 2f));

        if(_jumpTim <= 0) {
            animatorList.SetBool("isGrounded", true);
        }
    }

    private void Aiming() {

        if (Input.GetKeyDown(KeyCode.Q))
            aimYaw += 0.2f;

        if (Input.GetKeyDown(KeyCode.E))
            aimYaw -= 0.2f;

        aimYaw = Mathf.Clamp(aimYaw, 0, 0.999f);

        animatorList.SetFloat("aimYawCycle", aimYaw);
        animatorList.SetFloat("aimPitchCycle", aimPitch);
    }
}
