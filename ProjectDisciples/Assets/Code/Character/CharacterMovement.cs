using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterMovement : MonoBehaviourPunCallbacks, ICharacterMovement
{
    [Header("Movement Attributes")]
    public float moveSpeed = 3f;
    public float jumpSpeed = 5f;

    // Max Slope Climb Angle
    float maxClimbAngle = 50;
    float slopeRayHeight;

    // Jump Merchanics Values
    float fGroundedRememberTime = .2f;
    float fGroundedRemember;
    float fCutJumpHeight = .5f;

    // Ground Collision Checkers
    bool[] groundCollision = new bool[3];
    public float groundCheckOffset;
    public bool isGrounded;
    public bool isJumping;

    [Header("Object Variables")]
    [SerializeField] SpriteRenderer playerSprite;
    public LayerMask groundCheckLayer;
    Rigidbody2D rigidBody;

    // Movement Input Value
    [HideInInspector]
    public Vector2 moveInputValue;
    Vector2 velocity;
    Vector2 oldVelocity;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        // Save player foot position
        if (playerSprite) slopeRayHeight = playerSprite.bounds.extents.y;
    }

    private void Update()
    {
        CheckForGroundCollision();

        velocity.x = moveInputValue.x * moveSpeed;

        if (Time.frameCount % 5 == 0)
        {
            oldVelocity = rigidBody.velocity;
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
            if (isGrounded)
            {
                var groundForce = moveSpeed * 2f;
                rigidBody.AddForce(new Vector2((moveInputValue.x * groundForce - rigidBody.velocity.x) * groundForce, 0));
                rigidBody.velocity = new Vector2(velocity.x, rigidBody.velocity.y);

                RaycastHit2D slopeHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - slopeRayHeight), Vector2.right * Unity.Mathematics.math.sign(velocity.x), 1.5f, groundCheckLayer);
                if (slopeHit)
                {
                    float slopeAngle = Vector2.Angle(slopeHit.normal, Vector2.up);

                    if (slopeAngle <= maxClimbAngle)
                    {
                        //print("Slope: " + slopeHit.transform.name + " - " + slopeAngle);
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

        if (isGrounded)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
            isJumping = true;
        }
    }

    /// <summary>
    /// Cuts jumps in half if input is released
    /// </summary>
    public void CutJump()
    {
        if (!PhotonNetwork.InRoom && !photonView.IsMine) return;

        if (rigidBody.velocity.y > 0)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * fCutJumpHeight);
        }

        isJumping = false;
    }

    #endregion

    #region collisions

    /// <summary>
    /// Checks for ground colliders at the base of player
    /// </summary>
    void CheckForGroundCollision()
    {
        var halfHeight = playerSprite.bounds.extents.y;
        groundCollision[0] = Physics2D.OverlapCircle(new Vector2(transform.position.x + groundCheckOffset, transform.position.y - halfHeight), 0.1f, groundCheckLayer);
        groundCollision[1] = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - halfHeight), 0.1f, groundCheckLayer);
        groundCollision[2] = Physics2D.OverlapCircle(new Vector2(transform.position.x - groundCheckOffset, transform.position.y - halfHeight), 0.1f, groundCheckLayer);

        fGroundedRemember -= Time.deltaTime;

        for (int i = 0; i < groundCollision.Length; i++)
        {
            if (groundCollision[i])
            {
                fGroundedRemember = fGroundedRememberTime;
                isGrounded = true;

                return;
            }

            if (fGroundedRemember < 0 && !groundCollision[i])
            {
                isGrounded = false;
            }
        }

        if (isJumping)
        {
            fGroundedRemember = -1;
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
        //if (oldVelocity.y < -5)
        //{
        //    if (oldVelocity.y < -12)
        //    {
        //        CameraManager.Instance.ShakeCamera(2, 6, 0);
        //    }
        //    else
        //    {
        //        CameraManager.Instance.ShakeCamera(1, 0, 0);
        //    }
        //}
    }

    #endregion

    #region GetValues

    public void AimInputValue(Vector2 input)
    {

    }

    public void MovementInputValue(Vector2 moveInput)
    {
        moveInputValue = moveInput;
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        if (playerSprite)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(new Vector3(transform.position.x + groundCheckOffset, transform.position.y - slopeRayHeight, -2), 0.05f);
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - slopeRayHeight, -2), 0.05f);
            Gizmos.DrawWireSphere(new Vector3(transform.position.x - groundCheckOffset, transform.position.y - slopeRayHeight, -2), 0.05f);
        }
        Gizmos.DrawRay(new Vector2(transform.position.x, transform.position.y - slopeRayHeight), Vector2.right * Mathf.Sign(velocity.x));
    }

    #endregion
}
