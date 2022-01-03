using UnityEngine;
using System;

// 건전한 커플링 해소
public class Player : MonoBehaviour
{
    public Action onDeath;
    public void Die()
    {
        // 사망 처리
        onDeath();
    }
    public class GameData : MonoBehaviour
    {
        private void Start()
        {
            Player player = FindObjectOfType<Player>();
            player.onDeath += Save;
        }
        public void Save()
        {
            Debug.Log("게임 저장");
        }
    }
}

// 변경된 코드에서 player 클래스는 gamedata 타입의 오브젝트를 비롯해 자신의 사망 사건에 관심 있는 상대방 오브젝트를 파악할 필요가 없다
// player의 사망에 관심이 있는 오브젝트가 player의 onDeath 이벤트를 구독하면 된다 

