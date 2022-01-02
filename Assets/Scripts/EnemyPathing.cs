using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPathing : MonoBehaviour
{

    WaveConfig waveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;
    Vector3 nextWaypoint;
    [SerializeField] float rotSpeed = 360f;
    [SerializeField] bool rotateTowardsPath = true;
    // Vector3 rotateWaypoint;




    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        Move(); // ������� ������ ���������� ������ ����
    }


    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
        // ���������� waveConfig ������ EnemyPathing �������� ��-� ���������� waveConfig ��������������� ������� SetWaveConfig
    }
    // SetWaveConfig - ��� "set-�����" - ��������������� �������� �����. � ����������������� "get-������", "set-�����" ��
    // ����� ���������� �� ������� � ������� �� �������� � ������ � ������� ������!!!
    // ��������!!! ������ ����� ����� ����, � EnemyPathing ������, �������� ���� WaveConfig ���������� waveConfig
    // ����� �������, EnemyPathing ������ ����� � ����� WaveConfig ��� �������� � ������ Start() - ��� �� ����������
    // � ���������� waveConfig � ����� GetWaypoints() - ����� get-������� ������� �����, � ������ ���� �� ������.






    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            nextWaypoint = waypoints[waypointIndex].transform.position;
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards
                                (transform.position, nextWaypoint, movementThisFrame);


            if (rotateTowardsPath)
            {
                RotationTowardsPathMethod();
            }


            /*   if (waypointIndex > 0)
               { 
                   rotateWaypoint = nextWaypoint; // �������� ����� ������ ��� ����� 1 - ����� ������� �������� �� ��
               } */




            if (transform.position == nextWaypoint)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void RotationTowardsPathMethod()
    {
        Vector3 dir = waypoints[waypointIndex].transform.position - transform.position;
        dir.Normalize();
        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
    }
    // ���� ������ ������������ ������ ��� ����� ���������� ���������-1 (���� 0� ��������, �� ��� ������ ������ 5�����-1=4, ����� ���.)
    // (������� ���� � ����, ������� ����� 1 - ��� ������ ��������� ����� ���. 4)., � �����!   :
    // ��� ����-�� ���-� �����: ��������� � ������� �����. targetPosition ������� ���-� �������� ���������(��), �.�.
    // ������� ����� ��� �� ������� ��. �����: ��������� ����� �����. movement...Frame ������� �������� �� waveConfig 
    // � ������� ������� ������ ������� ������-� �� ����.������� (����� �� ���� ����������� ���� ������ �������-�)
    // ���������.������ ������� ������� (���������) ���� ������ ��-� ������ MoveTowards (����� ������� ����� � ������� ����)
    // ���� ������� ������� ����� == ������� ����, �� ������ �� ������� ������ ���������(��)
    // ����� ���������� ������������� (������ ������� �����) � ����� � ����������� ��������� ������� �����!
    // �������, ����� � ���.


    /*public Vector3 GetNextWaypoint()
     {
         return rotateWaypoint;
     } */
}
