using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon {  get; private set; }
    private PlayerControl playerControl;
    private float timeBetweenAttack;
    private bool attackButtonDown, isAttacking = false;
    protected override void Awake()
    {
        base.Awake();
        playerControl = new PlayerControl();
    }
    private void OnEnable()
    {
        playerControl.Enable();
    }
    private void Start()
    {
        playerControl.Combat.Attack.started += _ => StartAttacking();
        playerControl.Combat.Attack.canceled += _ => StopAttacking();

        AttackCoolDown();
    }
    private void Update()
    {
        Attack();
    }
    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;

        AttackCoolDown();
        timeBetweenAttack = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }
    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
    }
    private void AttackCoolDown()
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }
    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttack);
        isAttacking = false;
    }
    private void StartAttacking()
    {
        attackButtonDown = true;
    }
    private void StopAttacking()
    {
        attackButtonDown = false;
    }
    private void Attack()
    {
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon)
        {
            AttackCoolDown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }
}
