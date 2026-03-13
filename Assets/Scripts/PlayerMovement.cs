using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour {
    [Header("Movement Settings")]
    //[SerializeField] private Animator m_Animator;
    private Vector3 m_Movement;
    [SerializeField] private Animator m_Animator;

    public Rigidbody m_Rigidbody;
    public float m_Speed = 10f;
    private bool canMove = true; // change to false after implementing menus
    
    private float movementX;
    private float movementY;
    
    [Header("Dash Settings")]
    public float dash_speed = 30f;
    public float dash_cooldown = 2f;
    public float dash_duration = 1f;
    private float last_dash_time;
    private bool isDashing = false;
    private bool canDash = true;
    private bool dash_key;
    public Slider dash_slider;
    
    [Header("Raycast")]
    [SerializeField] private Camera camera;
    

    void Start() {
        m_Animator = GetComponentInChildren<Animator>();
        m_Rigidbody = GetComponent<Rigidbody> ();
    }


    void Update() {
        if (canMove) {
            if (isDashing) return; // if it is dashing, prevent any new inputs
            
            // receive inputs for movement and rotation (mouse)
            movementX = Input.GetAxisRaw("Horizontal");
            movementY = Input.GetAxisRaw("Vertical");
            dash_key = Input.GetKeyDown(KeyCode.LeftShift);
            
            // if cooldown has been completed, allow dash
            if (!canDash && Time.time > last_dash_time + dash_cooldown) {
                canDash = true;
            }
            
            // if dash key is down and can dash, DASH
            if (dash_key && canDash) {
                last_dash_time = Time.time;
                canDash = false;
                StartCoroutine(Dash());
                dash_slider.gameObject.SetActive(true);
                dash_slider.value = 0;
                dash_slider.DOValue (1, dash_cooldown).SetEase (Ease.Linear).OnComplete(() => dash_slider.gameObject.SetActive(false));
            }

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                Vector3 lookDir = hit.point - transform.position;
                lookDir.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookDir);
                transform.rotation = rotation;
            }
        }
    }
    
    void FixedUpdate() {
        Vector3 moveDir = new Vector3(movementX, 0, movementY).normalized;
        
        if (!isDashing) {
            Vector3 newVelocity = moveDir * m_Speed;
            newVelocity.y = m_Rigidbody.velocity.y; 
            m_Rigidbody.velocity = newVelocity;
        }
        
        // checks if player is moving, if it is, animate running
        bool hasHorizontalInput = !Mathf.Approximately(movementX, 0f);
        bool hasVerticalInput = !Mathf.Approximately(movementY, 0f);
        bool isRunning = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("isRunning", isRunning);
    }
    
    //private void OnAnimatorMove() {
    //    m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
    //}
    
    public void setCanMove (bool canMove) {
        this.canMove = canMove;    
    }

    public IEnumerator Dash() {
        // dashes for a short duration
        isDashing = true;
        m_Animator.SetBool("isDashing", true);
        Vector3 moveDir = new Vector3(movementX, 0, movementY).normalized;
        m_Rigidbody.velocity = new Vector3 (moveDir.x * dash_speed, 0, moveDir.z * dash_speed);
        yield return new WaitForSeconds(dash_duration);
        isDashing = false;
        m_Animator.SetBool("isDashing", false);
    }
}
