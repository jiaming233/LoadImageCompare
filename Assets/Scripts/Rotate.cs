//using DG.Tweening;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Rotate : MonoBehaviour
//{
//    public Transform Action;
//    private Sequence s;
//    private float playtime = 0.65f;

//    // Start is called before the first frame update
//    void Start()
//    {
//        Tweener t1 = Action.DOLocalRotate(new Vector3(0, 0, 105), playtime);
//        Tweener t2 = Action.DOLocalRotate(new Vector3(0, 0, 255f), playtime);

//        s = DOTween.Sequence();
//        s.Append(t1);
//        s.Insert(0.15f + t1.Duration(), t2);

//        s.SetAutoKill(false).SetLoops(-1);
//    }
//}
