using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    public static Character Instance { get; protected set; }

    public enum AnimationState { Work = 0, Run = 1, Attacking = 2, Attacked = 3, Roll = 4, Skill = 5, Dead = 6, Stand = 7 }

    public int OriginalHP;
    public int HP;
    public int Damage;
    public int RunSpeed;
    public int RollSpeed;
    public int AttackSpeed; // 프레임 소모시간 

    [SerializeField]
    protected Rigidbody2D PlayerRigidbody;
    [SerializeField]
    protected Animation Attacking;
    [SerializeField]
    protected Animation Work;
    [SerializeField]
    protected Animation Run;
    [SerializeField]
    protected Animation Roll; // 구르기
    [SerializeField]
    protected Animation Attacked;
    [SerializeField]
    protected Animation Dead;
    [SerializeField]
    protected Animation Stand;
    [SerializeField]
    protected List<Animation> Skill;
    protected StatManager StatManagerInstance
    {
        get
        {
            return StatManager.Instance;
        }
    }

    public static AnimationState State;
    public static bool IsChangeState;

    protected void DoAnimation()
    {
        switch (State)
        {
            case AnimationState.Work:
                Work.Play();
                break;
            case AnimationState.Run:
                Run.Play();
                break;
            case AnimationState.Attacking:
                Attacking.Play();
                break;
            case AnimationState.Attacked:
                Attacked.Play();
                break;
            case AnimationState.Roll:
                Roll.Play();
                break;
            case AnimationState.Skill:
                Skill[SkillManager.UseSkillNumber].Play();
                AttackSpeed = 0;
                break;
            case AnimationState.Dead:
                Dead.Play();
                break;
            case AnimationState.Stand:
                Stand.Play();
                break;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO ...
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO ...
    }

    protected virtual void Move(Vector3 Force)
    {
        Force.Normalize();

        Force *= 100;

        PlayerRigidbody.AddForce(Force, ForceMode2D.Force);

        PlayerRigidbody.velocity = Vector2.ClampMagnitude(PlayerRigidbody.velocity, RunSpeed);
    }
}
