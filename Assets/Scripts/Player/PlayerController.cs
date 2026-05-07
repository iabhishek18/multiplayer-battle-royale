using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float sprintMultiplier = 1.6f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -20f;

    [Header("Combat")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int shield = 0;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isSprinting;
    private Camera playerCamera;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        currentHealth = maxHealth;

        if (!photonView.IsMine)
        {
            playerCamera.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        float speed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
        controller.Move(move.normalized * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * 2f;
        float mouseY = Input.GetAxis("Mouse Y") * 2f;
        transform.Rotate(Vector3.up * mouseX);
        playerCamera.transform.localRotation *= Quaternion.Euler(-mouseY, 0, 0);
    }

    [PunRPC]
    public void TakeDamage(int damage, string attackerName)
    {
        if (shield > 0)
        {
            int shieldDamage = Mathf.Min(shield, damage);
            shield -= shieldDamage;
            damage -= shieldDamage;
        }

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die(attackerName);
        }
    }

    private void Die(string killerName)
    {
        GameManager.Instance.PlayerEliminated(photonView.Owner.NickName, killerName);
        PhotonNetwork.Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
            stream.SendNext(shield);
        }
        else
        {
            currentHealth = (int)stream.ReceiveNext();
            shield = (int)stream.ReceiveNext();
        }
    }
}
