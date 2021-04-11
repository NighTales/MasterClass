using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://docs.google.com/document/d/1RHcBnAu17RNpBXFCBjciXxfc9zIFLied7kyB2Yie18o/edit?usp=sharing")]
public class PlayerEnegyControl : MonoBehaviour
{
    [SerializeField, Min(1)] private float maxEnergyValue;
    [SerializeField, Range(0,1)] private float moveDischarge;
    [SerializeField, Range(0, 1)] private float jumpDischarge;
    [SerializeField, Range(0, 1)] private float interationDischarge;
    [SerializeField, Range(0, 1)] private float dangersDischarge;
    [SerializeField, Range(0, 1)] private float constantDischarge;
    [SerializeField] private Transform spawnPoint;


    private PlayerUI playerUI;
    private PlayerLocomotion playerLocomotion;
    private PlayerInteraction playerInteraction;

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
        playerLocomotion.spendEnergyToMoveEvent += () => SpendEnergy(Time.deltaTime * moveDischarge);
        playerLocomotion.spendEnergyToJumpEvent += () => SpendEnergy(jumpDischarge);
        playerInteraction.spendEnergyToInteractEvent += () => SpendEnergy(interationDischarge);
        playerUI.foolAlphaDeathPanelEvent += PreparePlayer;
        playerUI.noAlphaDeathPanelEvent += UnblockPlayer;

    }
    private void Unsubscribe()
    {
        playerLocomotion.spendEnergyToMoveEvent -= () => SpendEnergy(Time.deltaTime * moveDischarge);
        playerLocomotion.spendEnergyToJumpEvent -= () => SpendEnergy(jumpDischarge);
        playerInteraction.spendEnergyToInteractEvent -= () => SpendEnergy(interationDischarge);
        playerUI.foolAlphaDeathPanelEvent -= PreparePlayer;
        playerUI.noAlphaDeathPanelEvent -= UnblockPlayer;
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
        if (other.CompareTag("Danger"))
        {
            playerUI.ReturnEffectToDefault();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(dangersDischarge > 0 && other.CompareTag("Danger"))
        {
            SpendEnergy(dangersDischarge * buferDangerPoint.damage * Time.deltaTime);
        }
    }

    private void Death()
    {
        playerLocomotion.SetBlockValueToPlayer(true);
        playerUI.DeathPanelToFoolAlpha();
    }

    private void PreparePlayer()
    {
        playerLocomotion.FastTeleportToPoint(spawnPoint);
        playerUI.energySlider.value = maxEnergyValue;
    }
    private void UnblockPlayer()
    {
        playerLocomotion.SetBlockValueToPlayer(false);
        buferEnergyPoint?.LaunchParticles(transform, true);
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
