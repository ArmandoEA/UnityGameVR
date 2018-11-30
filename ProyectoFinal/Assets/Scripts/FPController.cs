using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class FPController : MonoBehaviour {

    public float speed = 5f;
    public int totalTrash;
    public int currentHealth = 1;       //The box's current health point total
    public Text countText;
    public Text winText;

    private Transform cam;
    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
    private float mouseSensitivity = 250f;
    private float verticalLookRotation;
    private int count;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        count = totalTrash;
        winText.text = "";
        SetCountText();
    }
	
	// Update is called once per frame
	void Update () {
        float xMov = Input.GetAxisRaw("Horizontal");
        float yMov = Input.GetAxisRaw("Vertical");
        float zMov = Input.GetAxisRaw("Jump"); 

        Vector3 movHor = transform.right * xMov;
        Vector3 movVer = transform.forward * yMov;
        Vector3 movUp = transform.up * zMov;
        velocity = (movHor + movVer + movUp).normalized * speed;

        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity);
        verticalLookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        cam.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    private void FixedUpdate()
    {
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trash"))
        {
            other.gameObject.SetActive(false);
            count = count - 1;
            SetCountText();
        }
    }
    
    public void ModCount()
    {
        count = count - 1;
        SetCountText();
    }

    void SetCountText()
    {
        countText.text = "Basura: " + count.ToString();
        if(count == 0)
        {
            winText.text = "Has limpiado la ciudad";
            SceneManager.LoadScene("PantallaFinal");
        }
    }
}
