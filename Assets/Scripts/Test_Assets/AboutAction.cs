using UnityEngine;
using System;

public class AboutAction : MonoBehaviour
{
    Action onClean;

    private void Start()
    {
        onClean += CleaningRoomA; // onClean에 방을 청소하는 메서드를 등록
        onClean += CleaningRoomB;
        // Action 타이브이 변수에는 +=만 사용하여 메서드를 등록할 수 있다,
        // 등록할 메서드 끝에 괄호는 붙이지 않고 이름만 명시한다,
        // 괄호를 붙이면 '등록'하는게 아니라 '실행'을 하고 그 반환값을 할당하는 것이 된다

        // onClean += CleaningRoomA(); //하면 오류, 애초에 void로 선언되어 있기도 하고 // '등록'이 아닌 '결과값 대입'이 되어버린다
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onClean(); // CleaningRoomA()와 CleaningRoomB() 실행 // 등록된 순서대로
        }
    }

    void CleaningRoomA()
    {
        Debug.Log("A방 청소");
    }

    void CleaningRoomB()
    {
        Debug.Log("B방 청소");
    }
}
