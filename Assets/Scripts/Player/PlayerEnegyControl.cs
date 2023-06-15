using System.Collections;
using UnityEngine;

/// <summary>
/// Скрипт, управляющий зарядом скафандра
/// </summary>
[HelpURL("https://docs.google.com/document/d/1OZ45iQgWRDoWCmRe4UW9zX_etUkL64Vo_nURmUOBerc/edit?usp=sharing")]
public class PlayerEnegyControl : MonoBehaviour
{
    [SerializeField, Min(1), Tooltip("Максимальный уровень заряда")] private float maxEnergyValue;
    [SerializeField, Range(0, 1), Tooltip("Уровень расхода энергии при взаимодействиях")]
    private float interationDischarge;
    [SerializeField, Range(0, 1), Tooltip("Уровень расхода энергии при уроне")] private float dangersDischarge;
    [SerializeField, Range(0, 1), Tooltip("Уровень постоянного расхода энергии")] private float constantDischarge;
    [SerializeField, Tooltip("Точка воскрешения")] private Transform spawnPoint;


    private PlayerUI playerUI;
    private PlayerLocomotion playerLocomotion;
    private PlayerInteraction playerInteraction;

    private Collider colliderBufer;
    private DangerPoint buferDangerPoint;
    private EnergyPoint buferEnergyPoint;

    private void Start()
    {
        playerUI = FindObjectOfType<PlayerUI>();
        playerLocomotion = FindObjectOfType<PlayerLocomotion>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();

        playerUI.energySlider.minValue = 0;
        playerUI.energySlider.maxValue = maxEnergyValue;
        playerUI.energySlider.value = maxEnergyValue;

        Subscribe();
    }
    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Update()
    {
        if(constantDischarge > 0)
        {
            SpendEnergy(Time.deltaTime * constantDischarge);
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

    /// <summary>
    /// Разрядить скафандр на указанное количество единиц
    /// </summary>
    /// <param name="value">уровень разрдки</param>
    public void SpendEnergy(float value)
    {
        playerUI.energySlider.value -= value;
        if(playerUI.energySlider.value <= 0)
        {
            Death();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(dangersDischarge > 0 && other.CompareTag("Danger"))
        {
            colliderBufer = other;
            if(other.TryGetComponent<DangerPoint>(out buferDangerPoint))
            {
                playerUI.SetEffect(buferDangerPoint.effectSprite);
            }
        }
        else if(other.CompareTag("EnergyPoint") && playerUI.energySlider.value > 0)
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
            playerUI.ReturnEffectToDefault();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(dangersDischarge > 0 && other == colliderBufer && other.CompareTag("Danger"))
        {
            SpendEnergy(dangersDischarge * buferDangerPoint.damage * Time.deltaTime);
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
            playerUI.energySlider.value += Time.deltaTime * 5;
            yield return null;
        }
    }
}
