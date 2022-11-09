using UnityEngine;

[RequireComponent(typeof(UnattachedAnimator))]
public class UnattachedAnimatorHenry : MonoBehaviour {

    [SerializeField]
    private UnattachedAnimator unattachedAnimator;

    [SerializeField]
    private string PrimaryShootAnim = "ShootGun";

    [SerializeField]
    private int PrimaryShootLayer = 6;

    [SerializeField]
    private string SecondaryShootAnim = "ShootAltGun";

    [SerializeField]
    private int SecondaryShootLayer = 5;

    void Reset() {
        unattachedAnimator = GetComponent<UnattachedAnimator>();
    }

    void Update() {

        Shoot();
    }

    private void Shoot() {
        if (Input.GetMouseButtonDown(0)) {
            unattachedAnimator.AnimatorList.Play(PrimaryShootAnim, PrimaryShootLayer);
        }

        if (Input.GetMouseButtonDown(1)) {
            unattachedAnimator.AnimatorList.Play(SecondaryShootAnim, SecondaryShootLayer);
        }
    }
}
