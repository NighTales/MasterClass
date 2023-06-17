using System.Collections;
using UnityEngine;

/// <summary>
/// �����, ����������� ������� ������ � �������� ��� ����� ������
/// </summary>
[HelpURL("https://docs.google.com/document/d/1IP9ALvFZk9O6SwYpS9I7H1-WQci2OEaknke7YdTzZrc/edit?usp=sharing")]
public class EnergyPoint : MonoBehaviour
{
    [Tooltip("����� �����������")] public Transform spawnPoint;
    [SerializeField] [Tooltip("�������, ������� ����� ������")] private Transform particles;

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

        yield return new WaitForSeconds(5);

        particles.gameObject.SetActive(false);
    }
}
