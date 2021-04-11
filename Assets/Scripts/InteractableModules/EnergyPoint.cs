using System.Collections;
using UnityEngine;

/// <summary>
/// �����, ����������� ������� ������ � �������� ��� ����� ������
/// </summary>
public class EnergyPoint : MonoBehaviour
{
    public Transform spawnPoint;
    [SerializeField] private Transform particles;

    private void Start()
    {
        particles.gameObject.SetActive(false);
    }

    /// <summary>
    /// ��������� �������� ������ ����� ���������� � ������ ������
    /// </summary>
    /// <param name="player">Transform ��������� � ���������� �����������</param>
    /// <param name="toPlayer">������� ������� �� ����� ������ � ��������� ��� ��������</param>
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

        yield return new WaitForSeconds(2);

        particles.gameObject.SetActive(false);
    }
}
