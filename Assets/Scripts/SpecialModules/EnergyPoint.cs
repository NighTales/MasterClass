using System.Collections;
using UnityEngine;

/// <summary>
/// Точка, пополняющая энергию игрока и меняющая его точку спавна
/// </summary>
[HelpURL("https://docs.google.com/document/d/1IP9ALvFZk9O6SwYpS9I7H1-WQci2OEaknke7YdTzZrc/edit?usp=sharing")]
public class EnergyPoint : MonoBehaviour
{
    [Tooltip("Точка возрождения")] public Transform spawnPoint;
    [SerializeField] [Tooltip("Частицы, которые будут летать")] private Transform particles;

    private void Start()
    {
        particles.gameObject.SetActive(false);
    }

    /// <summary>
    /// Запустить движение частиц между персонажем и точкой спавна
    /// </summary>
    /// <param name="player">Transform персонажа в глобальных координатах</param>
    /// <param name="toPlayer">Двигать частицы от точки спавна к персонажу или наоборот</param>
    public void LaunchParticles(Transform player, bool toPlayer)
    {
        StartCoroutine(LaunchParticlesCoroutine(player, toPlayer));
    }

    private IEnumerator LaunchParticlesCoroutine(Transform player, bool toPlayer)
    {
        particles.gameObject.SetActive(true);
        particles.position = toPlayer? transform.position : player.position;
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime;
            particles.position = Vector3.Lerp(
                toPlayer ? transform.position : player.position,
                toPlayer ? player.position : transform.position,
                t);
            yield return null;
        }

        yield return new WaitForSeconds(5);

        particles.gameObject.SetActive(false);
    }
}
