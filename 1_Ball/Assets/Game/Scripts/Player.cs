using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public float speed;
	public Text countText;
	public Text winText;

	private Rigidbody rigid;
	private int count;

	void Start ()
	{
		rigid = GetComponent<Rigidbody>();

		count = 0;
		SetCountText ();
		winText.text = "";
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rigid.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			other.gameObject.SetActive (false);

			count = count + 1;

			SetCountText ();
		}
	}

	void SetCountText()
	{
		countText.text = "Count: " + count.ToString ();
        
		if (count >= 12) 
		{
			winText.text = "You Win!";
		}
	}
}