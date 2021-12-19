using UnityEngine;
using System;

public class AboutAction : MonoBehaviour
{
    Action onClean;

    private void Start()
    {
        onClean += CleaningRoomA; // onClean�� ���� û���ϴ� �޼��带 ���
        onClean += CleaningRoomB;
        // Action Ÿ�̺��� �������� +=�� ����Ͽ� �޼��带 ����� �� �ִ�,
        // ����� �޼��� ���� ��ȣ�� ������ �ʰ� �̸��� ����Ѵ�,
        // ��ȣ�� ���̸� '���'�ϴ°� �ƴ϶� '����'�� �ϰ� �� ��ȯ���� �Ҵ��ϴ� ���� �ȴ�

        // onClean += CleaningRoomA(); //�ϸ� ����, ���ʿ� void�� ����Ǿ� �ֱ⵵ �ϰ� // '���'�� �ƴ� '����� ����'�� �Ǿ������
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onClean(); // CleaningRoomA()�� CleaningRoomB() ���� // ��ϵ� �������
        }
    }

    void CleaningRoomA()
    {
        Debug.Log("A�� û��");
    }

    void CleaningRoomB()
    {
        Debug.Log("B�� û��");
    }
}
