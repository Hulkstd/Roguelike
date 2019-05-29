using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackGroundImage : MonoBehaviour
{
    public float MovementSpeed = 5.0f;
    public List<UnityEngine.UI.Image> Queue;
    public Queue<UnityEngine.UI.Image> Image;

    private void Start()
    {
        Image = new Queue<UnityEngine.UI.Image>();
        Queue.ForEach((item) => Image.Enqueue(item));
    }

    void FixedUpdate()
    {
        foreach(UnityEngine.UI.Image obj in Image.ToArray())
        {
            obj.transform.Translate(Vector3.left * MovementSpeed);
        }

        if(Image.ToArray()[1].rectTransform.localPosition.x >= -10 && Image.ToArray()[1].rectTransform.localPosition.x <= 10)
        {
            UnityEngine.UI.Image obj = Image.Peek();

            Image.Enqueue(obj);
            Image.Dequeue();

            obj.rectTransform.localPosition = Image.Peek().rectTransform.localPosition + new Vector3(2680, 0, 0);
        }
    }
}
