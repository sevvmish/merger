using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float Speed = 10;
    public bool IsX;
    public bool IsY;
    public bool IsZ;

    public bool IsGlobal;
    private float val;


  
    private void Update()
    {
        if (IsGlobal)
        {
            if (IsX || IsY || IsZ)
            {
                val += Speed * Time.deltaTime;

                if (IsX)
                {
                    transform.eulerAngles = new Vector3(val, transform.localEulerAngles.y, transform.eulerAngles.z);
                }
                else if (IsY)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, val, transform.eulerAngles.z);
                }
                else if (IsZ)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, val);
                }
            }
        }
        else
        {
            if (IsX || IsY || IsZ)
            {
                if (IsX)
                {
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + Speed * Time.deltaTime, transform.localEulerAngles.y, transform.localEulerAngles.z);
                }
                else if (IsY)
                {
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + Speed * Time.deltaTime, transform.localEulerAngles.z);
                }
                else if (IsZ)
                {
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + Speed * Time.deltaTime);
                }
            }
        }

        


    }


}
