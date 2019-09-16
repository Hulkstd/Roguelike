using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GCManager;

public class MoveBackGroundImage : MonoBehaviour
{
    public float MovementSpeed = 0.03f;
    public List<UnityEngine.UI.Image> Queue;
    private Queue<UnityEngine.UI.Image> Image;

    private void Start()
    {
        Image = new Queue<UnityEngine.UI.Image>();
        Queue.ForEach((item) => Image.Enqueue(item));
        StartCoroutine(ChangeMovementSpeed());        
    }

    IEnumerator ChangeMovementSpeed()
    {
        while (true)
        {
            yield return CoroDict.ContainsKey(4.5f) ? CoroDict[4.5f] : PushData(4.5f, new WaitForSeconds(4.5f));

            float toValue = Random.Range(-0.05f, 0.05f);
            Debug.Log(toValue);
            //MovementSpeed = toValue;

            for (int i = 0; i < 30; i++)
            {
                MovementSpeed = Mathf.Lerp(MovementSpeed, toValue, i / 30.0f);

                yield return CoroDict.ContainsKey(0.05f) ? CoroDict[0.05f] : PushData(0.05f, new WaitForSeconds(0.05f));
            }
        }
    }

    void FixedUpdate()
    {
        foreach(UnityEngine.UI.Image obj in Image.ToArray())
        {
            obj.transform.Translate(Vector3.left * MovementSpeed);
        }

        if (Image.ToArray()[2].rectTransform.localPosition.x >= -10 && Image.ToArray()[2].rectTransform.localPosition.x <= 10)
        {
            Image.ToArray()[0].rectTransform.localPosition = Image.ToArray()[2].rectTransform.localPosition + Vector3.right * 2680;

            Image.Enqueue(Image.Peek());
            Image.Dequeue();
        }

        if (Image.ToArray()[0].rectTransform.localPosition.x >= -10 && Image.ToArray()[0].rectTransform.localPosition.x <= 10)
        {
            Image.ToArray()[2].rectTransform.localPosition = Image.ToArray()[0].rectTransform.localPosition + Vector3.left * 2680;

            Image.Enqueue(Image.Peek());
            Image.Dequeue();
            Image.Enqueue(Image.Peek());
            Image.Dequeue();

            Image.ToArray()[0].rectTransform.localScale = Image.ToArray()[1].rectTransform.localScale.x < 0 ? Vector3.one : Vector3.one - Vector3.right * 2;
            Image.ToArray()[2].rectTransform.localScale = Image.ToArray()[1].rectTransform.localScale.x < 0 ? Vector3.one : Vector3.one - Vector3.right * 2;
        }
    }
}
