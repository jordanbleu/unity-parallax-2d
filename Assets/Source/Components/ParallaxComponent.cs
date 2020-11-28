using System;
using UnityEngine;

namespace Assets.Source.Components
{
    /// <summary>
    /// Follows the camera, but lags a bit behind, creating an illusion of depth.
    /// Layer multiple parallax backgrounds on top of each other for added effect.
    /// This Component should be added as a child of the camera object
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class ParallaxComponent : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Drag your main camera here.  If using Cinemachine, drag your virtual camera instead.")]
        private Transform followCamera;

        [SerializeField]
        [Tooltip("Enables horizontal scrolling")]
        private bool scrollHorizontal = true;

        [SerializeField]
        [Tooltip("Enables vertical scrolling")]
        private bool scrollVertical = true;

        [SerializeField]
        [Tooltip("The offset of the movement based on the camera position.  Numbers closer to 1 will move the same speed as the camera." +
            "Numbers closer to zero will lag behind more.")]
        [Range(0,1)]
        private float movementOffset = 0.3f;        

        private SpriteRenderer spriteRenderer;

        private Vector2 startPosition;

        private float width;
        private float height;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>()
                ?? throw new MissingComponentException($"Missing required component: {nameof(SpriteRenderer)}");

            width = CalculateWidthInUnits();
            height = CalculateHeightInUnits();

            CreateBuffers();
        }

        private void Start()
        {
            startPosition = followCamera.position;
            
        }


        private void CreateBuffers()
        {
            // Create a template object to clone 
            var template = new GameObject("template");

            // Create a shallow copy of our sprite renderer
            var spriteRendererTemplate = template.AddComponent<SpriteRenderer>();
            spriteRendererTemplate.sprite = spriteRenderer.sprite;
            spriteRendererTemplate.color = spriteRenderer.color;
            spriteRendererTemplate.flipX = spriteRenderer.flipX;
            spriteRendererTemplate.flipY = spriteRenderer.flipY;
            spriteRendererTemplate.size = spriteRenderer.size;
            spriteRendererTemplate.drawMode = spriteRenderer.drawMode;
            spriteRendererTemplate.tileMode = spriteRenderer.tileMode;
            spriteRendererTemplate.spriteSortPoint = spriteRenderer.spriteSortPoint;
            spriteRendererTemplate.shadowCastingMode = spriteRendererTemplate.shadowCastingMode;
            
            spriteRendererTemplate.maskInteraction = spriteRenderer.maskInteraction;
            spriteRendererTemplate.spriteSortPoint = spriteRenderer.spriteSortPoint;
            spriteRendererTemplate.material = spriteRenderer.material;
            spriteRendererTemplate.sortingLayerID = spriteRenderer.sortingLayerID;
            spriteRendererTemplate.sortingOrder = spriteRenderer.sortingOrder;

            // Create buffers to the left and right
            if (scrollHorizontal)
            {
                AddBufferObject(template, "LeftBuffer", new Vector3(transform.position.x - width, transform.position.y, transform.position.z));
                AddBufferObject(template, "RightBuffer", new Vector3(transform.position.x + width, transform.position.y, transform.position.z));
            }

            // Create buffers on top and bottom
            if (scrollVertical)
            {
                AddBufferObject(template, "TopBuffer", new Vector3(transform.position.x, transform.position.y + height, transform.position.z));
                AddBufferObject(template, "BottomBuffer", new Vector3(transform.position.x, transform.position.y - height, transform.position.z));

                // if also scrolling horizontal we should add diagonal buffers too
                if (scrollHorizontal) {
                    AddBufferObject(template, "TopLeftBuffer", new Vector3(transform.position.x - width, transform.position.y + height, transform.position.z));
                    AddBufferObject(template, "TopRightBuffer", new Vector3(transform.position.x + width, transform.position.y + height, transform.position.z));
                    AddBufferObject(template, "BottomLeftBuffer", new Vector3(transform.position.x - width, transform.position.y - height, transform.position.z));
                    AddBufferObject(template, "BottomRightBuffer", new Vector3(transform.position.x + width, transform.position.y - height, transform.position.z));
                }
            }

            // No need for the template to exist in the hierarchy
            Destroy(template);
        }

        // Duplicate an object and set some values
        private void AddBufferObject(GameObject template, string name, Vector3 position) {
            var dummyBottom = Instantiate(template, gameObject.transform);
            dummyBottom.name = name;
            dummyBottom.transform.position = position;
        }

        // Sprite's width in pixels / the pixels per unit
        private float CalculateWidthInUnits() => spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit;

        // Sprites height in pixels / the pixels per unit
        private float CalculateHeightInUnits() => spriteRenderer.sprite.rect.height / spriteRenderer.sprite.pixelsPerUnit;

        private void Update()
        {
            // This is hard to explain, but this is the camera's position offset by our movement offset.
            // This gives us our cameras position as if it were affected by the movement offset.
            float adjustedCameraPositionX = (followCamera.transform.position.x * (1 - movementOffset));
            float adjustedCameraPositionY = (followCamera.transform.position.y * (1 - movementOffset));

            var distanceFromCameraX = (followCamera.transform.position.x * movementOffset);
            var distanceFromCameraY = (followCamera.transform.position.y * movementOffset);

            var posX = transform.position.x;
            var posY = transform.position.y;

            var startPositionX = startPosition.x;
            var startPositionY = startPosition.y;

            // if scroll vertical / horizontal is checked, keep position locked in place.  Otherwise move it 
            // in the camera's direction by our offset amount 
            posX = (scrollHorizontal) ? startPosition.x + distanceFromCameraX : transform.position.x;
            posY = (scrollVertical) ? startPosition.y + distanceFromCameraY : transform.position.y;

            // If the camera is outside the area of the component's sprite, snap the whole component to the 
            // camera's position.  This creates a seamless blend for infinite scrolling
            if (scrollHorizontal) {
                if (adjustedCameraPositionX > (startPosition.x + width))
                {
                    startPositionX += width;
                }
                else if (adjustedCameraPositionX < startPosition.x - width)
                {
                    startPositionX -= width;
                }
            }


            // ...same thing for height
            if (scrollVertical) {
                if (adjustedCameraPositionY > (startPosition.y + height))
                {
                    startPositionY += height;
                }
                else if (adjustedCameraPositionY < (startPosition.y - height))
                {
                    startPositionY -= height;
                }
            }

            transform.position = new Vector2(posX, posY);
            startPosition = new Vector2(startPositionX, startPositionY);
        }

    

    }
}
