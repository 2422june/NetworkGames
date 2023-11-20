using Photon.Pun;
using Photon.Pun.Demo.SlotRacer.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AnimationController), typeof(InputModule), typeof(CoolTimeBarModule))]
public class PlayerController : MonoBehaviour
{
    private InputModule _input;
    private CoolTimeBarModule _coolTimeBar;
    private ArrowUIModule _arrowUI;
    private AnimationController _ani;

    private SpriteRenderer _renderer;
    private Rigidbody2D _rigid;

    private CameraController cam;
    public FXController _fx;

    private int _hp;
    private bool _isAttacked;

    private bool _isOnModuleCycle;
    private float _h, _jumpForce = 35f, _dashForce = 45f;
    private bool _isJump, _isDash, _isAttack;
    private bool _isAcioned;
    private Vector3 _dir;
    private string[] _actions = { "IsAttack", "IsDash", "IsJump" };

    private float _groundRadius = 0.2f;
    private bool _isOnGround, _isOnedGround;
    private Transform _groundCheck, _attackCheck;
    private LayerMask _groundLayer;

    private float _gravity;
    private PhotonView _pv;

    private void Awake()
    {
        _pv = GetComponent<PhotonView>();
        _rigid = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();

        if (!_pv.IsMine)
        {
            _rigid.isKinematic = true;
            return;
        }

        _groundLayer = LayerMask.GetMask("Ground");
        _dir = Vector3.zero;
        _hp = 5;
        _isAttacked = true;

        _groundCheck = transform.Find("GroundCheck");
        _attackCheck = transform.Find("AttackCheck");


        _ani = GetComponent<AnimationController>();
        _ani.Init();
        cam = GameObject.Find("MainCamera").GetComponent<CameraController>();
        cam.Init(transform);

        GameObject source = Resources.Load<GameObject>("FX/Smoke");
        _fx = Instantiate(source, transform.position, Quaternion.identity).GetComponent<FXController>();
        _fx.Init(transform);

        SetModule();
        Flip();
        OnModuleCycle();
    }

    private void SetModule()
    {
        _input = GetComponent<InputModule>();
        _coolTimeBar = GetComponent<CoolTimeBarModule>();
        _arrowUI = GetComponent<ArrowUIModule>();
        InitModule();
    }

    private void InitModule()
    {
        _arrowUI.Init();
        _input.Init();
        _coolTimeBar.Init();
    }

    private void Flip()
    {
        _h = _dir.x;

        if (_h == 0)
        {
            if (_dir.x == 0)
                return;

            _h = 0 - transform.position.x;
        }

        _renderer.flipX = (_h < 0);
        _pv.RPC("FlipRPC", RpcTarget.Others, _renderer.flipX);
    }

    [PunRPC]
    private void FlipRPC(bool flip)
    {
        if (_pv.IsMine)
            return;

        _renderer.flipX = flip;
    }

    public void OnModuleCycle()
    {
        _arrowUI.OnCycle();
        _coolTimeBar.OnCycle();
        _input.Init();

        _isOnModuleCycle = true;
    }

    public void OffModuleCycle()
    {
        _isOnModuleCycle = false;
        _arrowUI.OffCycle();
        _isAcioned = false;
    }

    private void Update()
    {
        if (!_pv.IsMine)
            return;

        ModuleCycle();
        ActionCycle();
    }

    private void ModuleCycle()
    {
        if (!_isOnModuleCycle)
            return;

        _coolTimeBar.Cycle();
        _input.Cycle();
        _arrowUI.Cycle();
    }

    private void ActionCycle()
    {
        if (_isOnModuleCycle)
            return;

        SetState();
        Action();
    }

    public void OnReady()
    {
        OffModuleCycle();
    }

    private void SetState()
    {
        CheckOnGround();

        _dir = _input.GetDir();

        _isDash = (_dir.x != 0);
        _isJump = (_dir.y == 1) && _isOnGround;
        _isAttack = _input.IsAttack();

        if(_isDash)
            _dir.x *= _dashForce;

        if (_isJump)
            _dir.y *= _jumpForce;
    }

    private void CheckOnGround()
    {
        _isOnGround = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundRadius, _groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _isOnGround = true;
                if(!_isOnedGround)
                {
                    _gravity = 0;
                    Managers.Audio.PlaySFX(Define.SFX.JumpLand, transform);
                }
                _isOnedGround = true;
                break;
            }
        }

        if(!_isOnGround)
            _isOnedGround = false;
    }

    private void Action()
    {
        if (_isAcioned)
            return;

        _rigid.gravityScale = 5;
        _rigid.velocity = Vector2.up * _gravity;
        _isAttacked = false;

        Flip();

        if (_isDash)
        {
            Dash();
        }
        else if (_isJump)
        {
            Jump();
        }

        _rigid.AddForce(_dir, ForceMode2D.Impulse);

        _isAcioned = true;
        if (_isAttack && !_isAttacked)
        {
            Attack();
        }
        Invoke("EndAction", 0.2f);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Attack();
            other.GetComponent<PlayerController>().GetDamage();
        }
    }*/

    private void PlayAni(int index)
    {
        for(int i = 0; i < _actions.Length; i++)
        {
            if (i == index)
                continue;

            _ani.PlayFalse(_actions[i]);
        }
        _ani.PlayTrue(_actions[index]);
        _fx.Show();
    }

    private void Dash() { PlayAni(1); }

    private void Jump() { PlayAni(2); }

    private void Attack() {
        _isAttacked = true;
        PlayAni(0);
        ShakeCamera();
        DoDashDamage();
    }

    private void EndAction()
    {
        ZeroVelocity();
        Invoke("OnModuleCycle", 0.4f);
    }

    private void ZeroVelocity()
    {
        if(_rigid.velocity.y < 0)
        {
            _gravity = _rigid.velocity.y;
        }
        _rigid.velocity = Vector2.zero;
        _rigid.gravityScale = 0;
    }

    [PunRPC]
    private void GetDamage(float dir)
    {
        _hp--;
        ShakeCamera();
        _ani.PlayTrue("Hit");
        Vector2 damageDir = Vector3.right * dir * 40f;
        _rigid.velocity = Vector2.zero;
        if (_hp <= 0)
        {
            _hp = 0;
            StartCoroutine(WaitToDead());
        }
    }

    private void GetDamage()
    {
        _pv.RPC("GetDamage", RpcTarget.Others, _dir.x);
    }

    [PunRPC]
    private void ShakeCamera()
    {
        cam.ShakeCamera();
    }

    public void DoDashDamage()
    {
        Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(_attackCheck.position, 0.9f);
        for (int i = 0; i < collidersEnemies.Length; i++)
        {
            if (collidersEnemies[i].gameObject.tag == "Player" && collidersEnemies[i].transform != transform)
            {
                collidersEnemies[i].GetComponent<PlayerController>().GetDamage();
            }
        }
    }

    private IEnumerator WaitToDead()
    {
        _ani.PlayTrue("IsDead");
        yield return new WaitForSeconds(0.4f);
        _rigid.velocity = new Vector2(0, _rigid.velocity.y);
        yield return new WaitForSeconds(1.1f);
    }
}
