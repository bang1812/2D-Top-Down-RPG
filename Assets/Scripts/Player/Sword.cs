using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    
    private Animator myAnimator;

    private PlayerController PlayerController;

    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private WeaponInfo weaponInfo;
    //[SerializeField] private float swordAttackCD = .5f;

    private Transform weaponCollider;
    private GameObject slashanim;
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        
    }
    private void Start()
    {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("SlashSpawnPoint").transform;
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
    public void Attack()
    {        
  
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);
        slashanim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashanim.transform.parent = this.transform.parent;
        
    }
    
    public void DoneAttackingAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }
    public void SwingUpFlipAnimEvent()
    {
        slashanim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
        if (PlayerController.Instance.FacingLeft)
        {
            slashanim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    public void SwingDownFlipAnimEvent()
    {
        slashanim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (PlayerController.Instance.FacingLeft)
        {
            slashanim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
