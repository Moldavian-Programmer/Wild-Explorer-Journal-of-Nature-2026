using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRDoubleBarrelShotgun : MonoBehaviour
{
    [Header("XR")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    [Header("Input")]
    public InputActionReference fireInput;
    public InputActionReference reloadInput;

    [Header("Fire Points")]
    public Transform leftBarrelFirePoint;
    public Transform rightBarrelFirePoint;

    [Header("Shotgun Settings")]
    public int pelletsPerShot = 8;
    public float damagePerPellet = 15f;
    public float range = 35f;
    public float spreadAngle = 6f;
    public float fireCooldown = 0.35f;
    public float reloadTime = 1.5f;

    [Header("Ammo")]
    public int maxShells = 2;
    public int currentShells = 2;

    [Header("Effects")]
    public ParticleSystem leftMuzzleFlash;
    public ParticleSystem rightMuzzleFlash;
    public AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip emptySound;
    public AudioClip reloadSound;

    [Header("Debug")]
    public bool drawDebugRays = true;

    private bool isHeld;
    private bool isReloading;
    private bool canFire = true;
    private int nextBarrelIndex;

    private void Reset()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (grabInteractable == null)
            grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }

        if (fireInput != null)
        {
            fireInput.action.performed += OnFireInput;
            fireInput.action.Enable();
        }

        if (reloadInput != null)
        {
            reloadInput.action.performed += OnReloadInput;
            reloadInput.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }

        if (fireInput != null)
            fireInput.action.performed -= OnFireInput;

        if (reloadInput != null)
            reloadInput.action.performed -= OnReloadInput;
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
    }

    private void OnFireInput(InputAction.CallbackContext context)
    {
        if (!isHeld)
            return;

        TryFire();
    }

    private void OnReloadInput(InputAction.CallbackContext context)
    {
        if (!isHeld)
            return;

        TryReload();
    }

    public void TryFire()
    {
        if (!canFire || isReloading)
            return;

        if (currentShells <= 0)
        {
            PlaySound(emptySound);
            return;
        }

        Transform firePoint = GetNextFirePoint();

        if (firePoint == null)
            return;

        currentShells--;

        FireShotgunBlast(firePoint);
        PlayMuzzleFlash(firePoint);
        PlaySound(fireSound);

        StartCoroutine(FireCooldownRoutine());
    }

    private Transform GetNextFirePoint()
    {
        Transform selected;

        if (nextBarrelIndex == 0)
        {
            selected = leftBarrelFirePoint;
            nextBarrelIndex = 1;
        }
        else
        {
            selected = rightBarrelFirePoint;
            nextBarrelIndex = 0;
        }

        if (selected == null)
            selected = leftBarrelFirePoint != null ? leftBarrelFirePoint : rightBarrelFirePoint;

        return selected;
    }

    private void FireShotgunBlast(Transform firePoint)
    {
        for (int i = 0; i < pelletsPerShot; i++)
        {
            Vector3 direction = GetSpreadDirection(firePoint.forward);

            if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, range))
            {
                DamageTarget target = hit.collider.GetComponentInParent<DamageTarget>();

                if (target != null)
                    target.TakeDamage(damagePerPellet);

                if (drawDebugRays)
                    Debug.DrawLine(firePoint.position, hit.point, Color.red, 1f);
            }
            else
            {
                if (drawDebugRays)
                    Debug.DrawRay(firePoint.position, direction * range, Color.yellow, 1f);
            }
        }
    }

    private Vector3 GetSpreadDirection(Vector3 forward)
    {
        float spreadX = Random.Range(-spreadAngle, spreadAngle);
        float spreadY = Random.Range(-spreadAngle, spreadAngle);

        Quaternion spreadRotation = Quaternion.Euler(spreadY, spreadX, 0f);
        return spreadRotation * forward;
    }

    public void TryReload()
    {
        if (isReloading)
            return;

        if (currentShells >= maxShells)
            return;

        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;

        PlaySound(reloadSound);

        yield return new WaitForSeconds(reloadTime);

        currentShells = maxShells;
        isReloading = false;
    }

    private IEnumerator FireCooldownRoutine()
    {
        canFire = false;
        yield return new WaitForSeconds(fireCooldown);
        canFire = true;
    }

    private void PlayMuzzleFlash(Transform firePoint)
    {
        if (firePoint == leftBarrelFirePoint && leftMuzzleFlash != null)
            leftMuzzleFlash.Play();

        if (firePoint == rightBarrelFirePoint && rightMuzzleFlash != null)
            rightMuzzleFlash.Play();
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}