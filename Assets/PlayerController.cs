using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool TERRAIN_DOWN = false;
    bool TERRAIN_LEFT = false;
    bool TERRAIN_RIGHT = false;
    bool TERRAIN_UP = false;
    bool TERRAIN_NONE {get{return TERRAIN_DOWN || TERRAIN_LEFT || TERRAIN_RIGHT || TERRAIN_UP;}}


    Rigidbody2D _rigidbody;
    Collider2D _collider;
    PlayerInfo _playerInfo;

    public float moveGroundAccel = 5;
    public float moveGroundMaxSpeed = 20;
    public float moveGroundDecel = 5;
    public float moveAirAccel = 1;
    public float moveAirMaxSpeed = 20;
    public float moveAirDecel = 0;
    public float jumpImpulse = 10;
    public float jumpAntigravMax = 0.5F;
    public float gravity = 10;
    public float jumpForce = 10;
    public bool isOnGround = false;
    public bool jumpPressed = false;
    public float jumpAntigravTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _playerInfo = GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTerrainContacts();
        var moveInput = Input.GetAxisRaw(_playerInfo.HorizontalAxisName);

        var jumpPressedThisFrame = false;
        var jumpInput = Input.GetButton(_playerInfo.JumpButtonName);
        if (jumpInput && !jumpPressed) {jumpPressedThisFrame = true;}
        jumpPressed = jumpInput;
        isOnGround = CheckIsOnGround();
        
        UpdateJumpMovement(jumpPressed, jumpPressedThisFrame, Time.deltaTime);

        UpdateHorizontalMovement(moveInput, Time.deltaTime);
        
        TeleportVertical();
    }

    void UpdateJumpMovement(bool jumpPressed, bool jumpPressedThisFrame, float dT)
    {
        // Update antigrav timer
        if (!jumpPressed || TERRAIN_UP) {jumpAntigravTimer = 0;}
        if (jumpAntigravTimer > 0) {jumpAntigravTimer -= dT;}

        // Cut vspeed if on the ground
        if (isOnGround)
        {
            //_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        }

        // Check for a jump this frame
        if (isOnGround && jumpPressedThisFrame)
        {
            jumpAntigravTimer = jumpAntigravMax;
        }

        // Maintain vert speed if we're in antigrav time
        if (jumpAntigravTimer > 0)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpImpulse);
        }

        // Apply gravity if we're out of antigrav time and not on the ground
        if (!isOnGround && jumpAntigravTimer <= 0)
        {
            _rigidbody.gravityScale = gravity;
            //_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y + (gravity*dT*-1));
        }
        else
        {
            _rigidbody.gravityScale = 0;
        }
    }

    void UpdateHorizontalMovement(float moveInput, float dT)
    {
        float horizontalSpeed = _rigidbody.velocity.x;
        float moveDir = Mathf.Sign(horizontalSpeed);
        float speedMagnitude = Mathf.Abs(horizontalSpeed);

        // If we have move input, apply it
        var moveInputSign = Mathf.Sign(moveInput);
        var moveInputMagnitude = Mathf.Abs(moveInput);
        if (moveInputMagnitude > 0.1F)
        {
            if (moveDir != 0 && moveDir != moveInputSign)
            {
                var decel = isOnGround ? moveGroundDecel : moveAirDecel;
                horizontalSpeed += decel*moveInputSign*dT;
            }
            var accel = isOnGround ? moveGroundAccel : moveAirAccel;
            horizontalSpeed += accel*moveInputSign*dT;
        }

        // If we have no move input, decelerate
        else
        {
            var decel = isOnGround ? moveGroundDecel : moveAirDecel;
            speedMagnitude -= decel*dT;
            if (speedMagnitude < 0) {speedMagnitude = 0;}
            horizontalSpeed = speedMagnitude * moveDir;
        }

        // Apply max speed
        moveDir = Mathf.Sign(horizontalSpeed);
        speedMagnitude = Mathf.Abs(horizontalSpeed);
        var maxSpeed = isOnGround ? moveGroundMaxSpeed : moveAirMaxSpeed;
        if (speedMagnitude > maxSpeed)
        {
            speedMagnitude = maxSpeed;
            horizontalSpeed = speedMagnitude * moveDir;
        }

        gameObject.GetComponent<Animator>()?.SetFloat("HorizontalSpeed", Mathf.Abs(horizontalSpeed));
        gameObject.GetComponent<Animator>()?.SetBool("GoingRight", horizontalSpeed > 0.5f);
        gameObject.GetComponent<Animator>()?.SetBool("GoingLeft", horizontalSpeed < -0.5f);

        //_rigidbody.AddForce(new Vector2((horizontalSpeed - _rigidbody.velocity.x)*10, 0));
        _rigidbody.velocity = new Vector2(horizontalSpeed, _rigidbody.velocity.y);
    }

    void UpdateTerrainContacts()
    {
        TERRAIN_DOWN = false; TERRAIN_LEFT = false; TERRAIN_RIGHT = false; TERRAIN_UP = false;

        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Terrain"));
        _collider.GetContacts(filter, contacts);

        foreach (var contact in contacts)
        {
            if (contact.normal.x < 0) {TERRAIN_RIGHT = true;}
            if (contact.normal.x > 0) {TERRAIN_LEFT = true;}
            if (contact.normal.y > 0) {TERRAIN_DOWN = true;}
            if (contact.normal.y < 0) {TERRAIN_UP = true;}
            if (contact.separation > 0)
            {
                //transform.Translate(contact.normal * contact.separation);
            }

            // Bounce over slightly misaligned tiles
            if (contact.normal.y == 0 && contact.normalImpulse > 0)
            {
                transform.Translate(0,0.01F,0);
                //_rigidbody.AddForce(new Vector2(0, 5));
            }
        }
        

    }

    bool CheckIsOnGround()
    {
        return TERRAIN_DOWN;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1F, LayerMask.GetMask("Terrain"));
        
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, Vector2.down * 1.1F, Color.red);
            return true;
        }
        Debug.DrawRay(transform.position, Vector2.down * 1.1F, Color.green);
        return false;
    }

    void TeleportVertical()
    {
        if (transform.position.y < -31)
        {
            transform.position = new Vector2(transform.position.x, 1.5F);
        }
    }
}
