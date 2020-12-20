using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterMovement : MonoBehaviourPunCallbacks, ICharacterMovement
{
    [Header("Values")]
    public float MoveSpeed = 5f;
    public float JumpForce = 8f;

    [Header("Conditions")]
    public bool IsGrounded;
    public bool IsJumping;

    [Header("References")]
    [SerializeField] SpriteRenderer _spriteRenderer;
    public LayerMask surfaceLayer;
    Collider2D _collider;
    Rigidbody2D _rigidBody;

    // Movement Adjusters
    float fCutJumpHeight = .5f;
    float maxClimbAngle = 50;
    float slopeRayHeight;

    // Ground Collision
    bool[] groundCollision = new bool[3];
    private float groundColliderSize = .05f;
    private float groundCollidersOffset;
    private float groundHeightOffset;
    float _surfaceCheckDelayValue = .2f;
    float _surfaceCheckDelay;

    // Movement Input Value
    [HideInInspector] public Vector2 moveInputValue;

    Vector2 velocity;
    Vector2 oldVelocity;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        CalculateColliderValues();
    }

    private void Update()
    {
        CheckForGroundCollision();

        velocity.x = moveInputValue.x * MoveSpeed;

        if (Time.frameCount % 5 == 0)
        {
            oldVelocity = _rigidBody.velocity;
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    #region Movement

    /// <summary>
    /// Handles player movement and jumping using input interfaces
    /// </summary>
    public void HandleMovement()
    {
        if (moveInputValue.x * System.Math.Sign(moveInputValue.x) > 0.01f)
        {
            if (IsGrounded)
            {
                var groundForce = MoveSpeed * 2f;
                _rigidBody.AddForce(new Vector2((moveInputValue.x * groundForce - _rigidBody.velocity.x) * groundForce, 0));
                _rigidBody.velocity = new Vector2(velocity.x, _rigidBody.velocity.y);

                RaycastHit2D slopeHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - slopeRayHeight), Vector2.right * Unity.Mathematics.math.sign(velocity.x), 1.5f, surfaceLayer);
                if (slopeHit)
                {
                    float slopeAngle = Vector2.Angle(slopeHit.normal, Vector2.up);

                    if (slopeAngle <= maxClimbAngle)
                    {
                        ClimbSlope(ref velocity, slopeAngle);
                    }
                }


                // Step sound playback

                //RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundCheckLayer);
                //if (groundHit.collider != null)
                //{
                //    if (groundHit.transform.gameObject.layer == 12)
                //    {
                //        AudioManager.PlaySound(AudioManager.Sound.PlayerWalkWood, transform.position);
                //    }
                //    else if (groundHit.transform.gameObject.layer == 13)
                //    {
                //        AudioManager.PlaySound(AudioManager.Sound.PlayerWalkGrass, transform.position);
                //    }
                //}
            }
        }
    }

    /// <summary>
    /// Compensates for the velocity at angles
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="slopeAngle"></param>
    void ClimbSlope(ref Vector2 velocity, float slopeAngle) // Made with the amazing tutorial of Sebastian Lague on youtube!
    {
        float moveDistance = Mathf.Abs(velocity.x);
        velocity.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
    }

    #endregion

    #region Jump

    public void Jump()
    {
        if (!PhotonNetwork.InRoom && !photonView.IsMine) return;

        if (IsGrounded)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, JumpForce);
            IsJumping = true;
        }
    }

    /// <summary>
    /// Cuts jumps in half if input is released
    /// </summary>
    public void CutJump()
    {
        if (!PhotonNetwork.InRoom && !photonView.IsMine) return;

        if (_rigidBody.velocity.y > 0)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _rigidBody.velocity.y * fCutJumpHeight);
        }

        IsJumping = false;
    }

    #endregion

    #region collisions

    /// <summary>
    /// Checks for ground colliders at the base of player
    /// </summary>
    void CheckForGroundCollision()
    {
        groundCollision[0] = Physics2D.OverlapCircle(new Vector2(transform.position.x + groundCollidersOffset, transform.position.y - groundHeightOffset), groundColliderSize, surfaceLayer);
        groundCollision[1] = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - groundHeightOffset), groundColliderSize, surfaceLayer);
        groundCollision[2] = Physics2D.OverlapCircle(new Vector2(transform.position.x - groundCollidersOffset, transform.position.y - groundHeightOffset), groundColliderSize, surfaceLayer);

        _surfaceCheckDelay -= Time.deltaTime;

        for (int i = 0; i < groundCollision.Length; i++)
        {
            if (groundCollision[i])
            {
                _surfaceCheckDelay = _surfaceCheckDelayValue;
                IsGrounded = true;

                return;
            }

            if (_surfaceCheckDelay < 0 && !groundCollision[i])
            {
                IsGrounded = false;
            }
        }

        if (IsJumping)
        {
            _surfaceCheckDelay = -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ground Impact Particle
        if (oldVelocity.y < -5)
        {
            //if (collision.transform.gameObject.layer == 12)
            //{
            //    AudioManager.PlaySound(AudioManager.Sound.PlayerLandWood, transform.position);
            //}
            //else if (collision.transform.gameObject.layer == 13)
            //{
            //    AudioManager.PlaySound(AudioManager.Sound.PlayerLandGrass, transform.position);
            //}
        }

        //Ground Impact Camera Shake
        if (oldVelocity.y < -7)
        {
            if (oldVelocity.y < -12)
            {
                CameraManager.Instance.ShakeCamera(2, 6, 0);
            }
            else
            {
                CameraManager.Instance.ShakeCamera(1, 0, 0);
            }
        }
    }

    #endregion

    #region GetValues

    /// <summary>
    /// Calculates the essential values for the ground check colliders
    /// </summary>
    private void CalculateColliderValues()
    {
        if (_collider)
        {
            slopeRayHeight = _collider.bounds.extents.y;
            groundHeightOffset = _collider.bounds.extents.y;
            groundCollidersOffset = _collider.bounds.size.x / 2;
        }
    }

    public void MovementInputValue(Vector2 moveInput)
    {
        moveInputValue = moveInput;
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        if (_spriteRenderer)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(new Vector3(transform.position.x + groundCollidersOffset, transform.position.y - groundHeightOffset, -2), groundColliderSize);
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - groundHeightOffset, -2), groundColliderSize);
            Gizmos.DrawWireSphere(new Vector3(transform.position.x - groundCollidersOffset, transform.position.y - groundHeightOffset, -2), groundColliderSize);
        }

        if (moveInputValue.x != 0)
        {
            Gizmos.DrawRay(new Vector2(transform.position.x, transform.position.y - slopeRayHeight), Vector2.right * Mathf.Sign(velocity.x));
        }
    }

    #endregion
}
