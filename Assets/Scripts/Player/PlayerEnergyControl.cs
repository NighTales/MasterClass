using System.Collections;
using UnityEngine;

/// <summary>
/// Скрипт, управляющий зарядом скафандра
/// </summary>
[HelpURL("https://docs.google.com/document/d/1KPj6PQdem4UZ1I2N8boi_pYtEZRDkm6yu8ZQ62RRO8E/edit?usp=sharing")]
public class PlayerEnergyControl : MonoBehaviour
{
    [SerializeField, Min(1), Tooltip("Максимальный уровень заряда")] private float maxEnergyValue;
    [SerializeField, Range(0, 1), Tooltip("Уровень расхода энергии при взаимодействиях")]
    private float interationDischarge;
    [SerializeField, Range(0, 1), Tooltip("Уровень расхода энергии при уроне")] private float dangersDischarge;
    [SerializeField, Tooltip("Точка воскрешения")] private Transform spawnPoint;
    [SerializeField, Range(1,100)]
    private float healForOneHealItem = 20;

    private PlayerUI playerUI;
    private PlayerLocomotion playerLocomotion;
    private PlayerInteraction playerInteraction;

    private Collider colliderBufer;
    private DangerPoint buferDangerPoint;
    private EnergyPoint buferEnergyPoint;

    private int healItemsCount = 0;


    private void Start()
    {
        playerUI = FindObjectOfType<PlayerUI>();
        playerLocomotion = FindObjectOfType<PlayerLocomotion>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();

        playerUI.energySlider.minValue = 0;
        playerUI.energySlider.maxValue = maxEnergyValue;
        playerUI.energySlider.value = maxEnergyValue;
        playerUI.healthSlider.minValue = 0;
        playerUI.healthSlider.maxValue = 100;
        playerUI.healthSlider.value = 100;

        Subscribe();
    }
    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && playerUI.healthSlider.value != playerUI.healthSlider.maxValue && healItemsCount > 0)
        {
            UseHealItem();
        }
    }

    private void Subscribe()
    {
        playerInteraction.spendEnergyToInteractEvent += SpendEnergyToInteraction;
        playerUI.foolAlphaDeathPanelEvent += PreparePlayer;
        playerUI.noAlphaDeathPanelEvent += UnblockPlayer;

    }
    private void Unsubscribe()
    {
        if(playerInteraction != null)
        {
            playerInteraction.spendEnergyToInteractEvent -= SpendEnergyToInteraction;
        }
        if(playerUI != null)
        {
            playerUI.foolAlphaDeathPanelEvent -= PreparePlayer;
            playerUI.noAlphaDeathPanelEvent -= UnblockPlayer;
        }
    }

    public void AddHealItem()
    {
        healItemsCount++;
        playerUI.healItemsCount.text = "x" + healItemsCount;
    }
    private void UseHealItem()
    {
        healItemsCount--;
        playerUI.healthSlider.value += healForOneHealItem;
        playerUI.healItemsCount.text = "X" + healItemsCount;
    }

    /// <summary>
    /// Разрядить скафандр на указанное количество единиц
    /// </summary>
    /// <param name="value">уровень разрдки</param>
    public void SpendEnergy(float value)
    {
        playerUI.energySlider.value -= value;
    }

    public void GetDamageFromFall(float value)
    {
        playerUI.healthSlider.value-= value;
        if(playerUI.healthSlider.value <= 0)
        {
            Death();
        }
        playerUI.damagePanel.SetTrigger("Hit");
    }

    private void GetDamage(float value)
    {
        SpendEnergy(value);
        if (playerUI.energySlider.value <= 0)
        {
            playerUI.shieldPanel.SetBool("IsActive", false);
            playerUI.damagePanel.SetBool("IsActive", true);
            playerUI.healthSlider.value -= value * 1.5f;
            if (playerUI.healthSlider.value <= 0)
            {
                Death();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(dangersDischarge > 0 && other.CompareTag("Danger"))
        {
            colliderBufer = other;
            if (playerUI.energySlider.value > 0)
            {
                playerUI.shieldPanel.SetBool("IsActive", true);
            }
            else
            {
                playerUI.damagePanel.SetBool("IsActive", true);
            }
            if(other.TryGetComponent<DangerPoint>(out buferDangerPoint))
            {
                playerUI.SetEffect(buferDangerPoint.effectSprite);
            }
        }
        else if(other.CompareTag("EnergyPoint") && playerUI.energySlider.value < 100)
        {
            if(other.TryGetComponent<EnergyPoint>(out buferEnergyPoint))
            {
                buferEnergyPoint.LaunchParticles(transform, false);
                spawnPoint = buferEnergyPoint.spawnPoint;
                StartCoroutine(EnergyToFoolCoroutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == colliderBufer)
        {
            playerUI.shieldPanel.SetBool("IsActive", false);
            playerUI.damagePanel.SetBool("IsActive", false);
            playerUI.ReturnEffectToDefault();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(dangersDischarge > 0 && other == colliderBufer && other.CompareTag("Danger"))
        {
            GetDamage(dangersDischarge * buferDangerPoint.damage * Time.deltaTime);
        }
    }

    private void Death()
    {
        playerLocomotion.SetBlockValueToPlayer(true);
        playerUI.DeathPanelToFoolAlpha();
        playerUI.ReturnEffectToDefault();
    }

    private void PreparePlayer()
    {
        playerLocomotion.TeleportToPoint(spawnPoint);
        playerUI.energySlider.value = maxEnergyValue;
        playerUI.healthSlider.value = maxEnergyValue;
    }
    private void UnblockPlayer()
    {
        playerLocomotion.SetBlockValueToPlayer(false);
        buferEnergyPoint?.LaunchParticles(transform, true);
    }

    private void SpendEnergyToInteraction()
    {
        SpendEnergy(interationDischarge);
    }

    private IEnumerator EnergyToFoolCoroutine()
    {
        while(playerUI.energySlider.value < maxEnergyValue)
        {
            playerUI.energySlider.value += Time.deltaTime * 15;
            yield return null;
        }
    }
}
