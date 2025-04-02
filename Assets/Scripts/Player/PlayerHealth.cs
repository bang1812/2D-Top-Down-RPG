using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead {  get; private set; }
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10;
    [SerializeField] private float damageRecoveryTime = 1;

    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private KnockBack knockback;
    private Flash flash;
    const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string TOWN_TEXT = "Scene1";
    readonly int DEATH_HASH = Animator.StringToHash("Death");
    protected override void Awake()
    {
        base.Awake();
        flash = GetComponent<Flash>();
        knockback = GetComponent<KnockBack>();
    }
    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        EnemyAI enemy = col.gameObject.GetComponent<EnemyAI>();
        if (enemy)
        {
            TakeDamage(1, col.transform);
        }
    }
    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }
    public void TakeDamage(int dameageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }

        ScreenShakeManager.Instance.ShakeScreen();
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());

        canTakeDamage = false;
        currentHealth -= dameageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }
    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }
    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        SceneManager.LoadScene(TOWN_TEXT);
    }
    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
}
