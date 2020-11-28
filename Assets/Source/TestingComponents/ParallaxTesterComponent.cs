using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.TestingComponents
{
    /// <summary>
    /// This is a dumb component that is used for testing the parallax stuff.
    /// Add to your camera.
    /// </summary>
    public class ParallaxTesterComponent : MonoBehaviour
    {

        [SerializeField]
        [Tooltip("If true, camera moves on its own.  If false, move camera with WASD keys.")]
        private bool autoScroll = true;

        [SerializeField]
        private float speedX = 0.1f;
        
        [SerializeField]
        private float speedY = 0f;

        private void Update()
        {
            if (autoScroll)
            {
                transform.position = new Vector3(transform.position.x + speedX, transform.position.y + speedY, transform.position.z);
            }
            else {

                var posX = transform.position.x;
                var posY = transform.position.y;

                if (Input.GetKey(KeyCode.W))
                {
                    posY += speedY;
                }
                else if (Input.GetKey(KeyCode.S)) {
                    posY -= speedY;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    posX -= speedX;
                }
                else if (Input.GetKey(KeyCode.D)) {
                    posX += speedX;
                }

                transform.position = new Vector3(posX, posY, transform.position.z);
            }            
        }



    }
}
