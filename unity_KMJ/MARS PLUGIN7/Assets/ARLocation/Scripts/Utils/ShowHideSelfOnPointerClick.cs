using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARLocation.Utils
{

    public class ShowHideSelfOnPointerClick : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<Canvas>().enabled = !GetComponent<Canvas>().enabled;
            }
        }
    }
}
