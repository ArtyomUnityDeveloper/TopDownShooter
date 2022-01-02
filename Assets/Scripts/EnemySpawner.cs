using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    // configuration parameters
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
    }


    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));


        }
    }


    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate( //����������� ������ ����� ����� ��� get-������� �� waveConfig � 0� ����� ���� �� waypoints ������
                     waveConfig.GetEnemyPrefab(),
                     waveConfig.GetWaypoints()[0].transform.position,
                     Quaternion.AngleAxis(180, new Vector3(0, 0, 1))); // Quaternion.AngleAxis(180,new Vector3(0, 0, 1))
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig); // ������� � ������ EnemyPathing (���. ��������
                                                               //� ����. �����) ���������� �� ������� �����, � ������ ������� wayConfig ������. ������
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }
}
