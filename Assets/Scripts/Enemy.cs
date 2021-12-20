﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드를 가져오기

// 적 AI를 구현한다
public class Enemy : LivingEntity {
    public LayerMask whatIsTarget; // 추적 대상 레이어

    private LivingEntity targetEntity; // 추적할 대상
    private NavMeshAgent pathFinder; // 경로계산 AI 에이전트

    public ParticleSystem hitEffect; // 피격시 재생할 파티클 효과
    public AudioClip deathSound; // 사망시 재생할 소리
    public AudioClip hitSound; // 피격시 재생할 소리

    private Animator enemyAnimator; // 애니메이터 컴포넌트
    private AudioSource enemyAudioPlayer; // 오디오 소스 컴포넌트
    private Renderer enemyRenderer; // 렌더러 컴포넌트

    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점

    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    private bool hasTarget
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (targetEntity != null && !targetEntity.dead) // 추적할 대상은 어디에 넣어져 있나
            {
                return true;
            }

            // 그렇지 않다면 false
            return false;
        }
    }

    private void Awake() {
        // 초기화
        // 게임 오브젝트로부터 사용할 컴포넌트 가져오기
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudioPlayer = GetComponent<AudioSource>();

        // 렌더러 컴포넌트는 자식 게임 오브젝트에 있으므로
        // GetComponentInChildren() 사용
        enemyRenderer = GetComponentInChildren<Renderer>(); // ?
    }

    // 적 AI의 초기 스펙을 결정하는 셋업 메서드
    public void Setup(float newHealth, float newDamage, float newSpeed, Color skinColor) {
        // 체력설정
        startingHealth = newHealth;
        // 공격력설정
        damage = newDamage;
        // 내비메시 에이전트의 이동속도 설정
        pathFinder.speed = newSpeed;
        // 렌더러가 사용중인 머터리얼의 컬러를 변경, 외형 색이 변함
        enemyRenderer.material.color = skinColor;
    }

    private void Start() {
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath());
    }

    private void Update() {
        // 추적 대상의 존재 여부에 따라 다른 애니메이션을 재생
        enemyAnimator.SetBool("HasTarget", hasTarget);
    }

    // 주기적으로 추적할 대상의 위치를 찾아 경로를 갱신
    private IEnumerator UpdatePath() {
        // 살아있는 동안 무한 루프
        while (!dead) // 이해가 안 되는건 함수 부분 뿐
        {
            if (hasTarget)
            {
                // 추적 대상이 존재 : 경로를 갱신, AI 이동을 계속 진행
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position); // 목표위치를 입력받아 이동경로를 갱신하는 메서드
            }
            else
            {
                // 추적 대상이 없음. AI 이동을 중지
               pathFinder.isStopped = true;
                // 20유닛의 반지름을 가진 가상의 구를 그렸을 떄 구와 겹치는 모든 콜라이더를 가져옴
                // 단, whatIsTarger  레이어를 가진 콜라이더만 가져오도록 필터링
                Collider[] coliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

                // 모든 콜라이더를 순환하면서 살아 있는 LivingEntity 찾기
                for ( int i = 0 ; i < coliders.Length ; i++ )
                {
                    // 콜라이더로부터 LivingEntity 가져오기
                    LivingEntity livingEntity = coliders[i].GetComponent<LivingEntity>();
                    // ? 좀비도 따라가는거 아닌지

                    // LivingEntity 컴포넌트가 존재하며 , 해당 LivingEntity가 살아 있다면
                    if (livingEntity != null && livingEntity.dead)
                    {
                        // 추적 대상을 해당 LivingEntity로 설정
                        targetEntity = livingEntity;

                        // for문 루프 즉시 정지
                        break;
                    }
                }
            }
            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    // 데미지를 입었을때 실행할 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) {
        // 아직 사망하지 않은 경우에만 피격 효과 재생
        if (dead)
        {
            // 공격받은 방향과 지점으로 파티클 효과 재생
            hitEffect.transform.position = hitPoint; // ? hitpoint의 위치는?
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            // 피격 효과음 재생
            enemyAudioPlayer.PlayOneShot(hitSound);
        }
        // LivingEntity의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 사망 처리
    public override void Die() {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        // 다른 AI를 방해하지 않도록 자신의 모든 콜라이더를 비활성화
        Collider[] enemyColliers = GetComponents<Collider>();
        for (int i = 0; i < enemyColliers.Length; i++)
        {
            enemyColliers[i].enabled = false;
        }

        // AI가 추적을 중지하고 내비메시 컴포넌트 비활성화
        pathFinder.isStopped = true;
        pathFinder.enabled = false; // 게임 오브젝트 (setactive = false; 를 하는건? => 사망 애니메이션 없이 바로 삭제가 되어버림)

        // 사망 애니메이션 재생
        enemyAnimator.SetTrigger("Die");
        // 사망 효과음 재생
        enemyAudioPlayer.PlayOneShot(deathSound);
    }

    private void OnTriggerStay(Collider other) {
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행   
        // 자신이 사망하지 않았으며
        // 최근 공격 시점에서 timeBetAttack 이상의 시간이 지났다면 공격 가능
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            // 상대방의 LivingEntity 타입을 가져오기를 시도
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            // 상대방의 LivingENtity가 자신의 추적 대상이라면 공격을 실행
            if (attackTarget != null && attackTarget == targetEntity)
            {
                // 최근 공격 시간을 갱신
                lastAttackTime = Time.time;

                // 상대방의 피격 위치와 피격 방향을 근삿값으로 계산
                Vector3 hitPoint = other.ClosestPoint(transform.position); // ? // 피격 위치(근삿값)
                Vector3 hitNormal = transform.position - other.transform.position; // 방향

                // 공격 실행
                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }
    }
}
