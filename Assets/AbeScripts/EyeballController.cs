using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballController : MonoBehaviour
{
    // This script creates a googly eye effect by using the character's Rigidbody2D (RB2D) physics to calculate the pupil's position.
    // The pupil is not a Rigidbody itself, so the effect is not fully physics-based but is more efficient than using live physics simulations.
    // Due to the Update method constantly checking the RB2D's velocity, avoid having too many googly eyes in one scene to maintain performance.
    // You can vary the effect by adjusting the exposed variables in the Inspector, allowing for subtler or more exaggerated "crazy eyes" effects.

    [Header("References")]
    public Transform eyeball; // Eyeball transform
    public Transform pupil;   // Pupil transform
    public Rigidbody2D characterRigidbody; // Reference to the character's Rigidbody2D

    [Header("Settings")]
    public float radius = 0.35f;          // Radius of the eyeball
    public float gravityStrength = 1.0f; // Strength of the downward pull
    public float movementSmoothing = 0.1f; // Smoothing factor for pupil movement

    private Vector2 velocity; // Used for smoothing movement

    private void Update()
    {
        UpdatePupilPosition();
    }

    private void UpdatePupilPosition()
    {

        // Get the center of the eyeball
        Vector2 eyeballCenter = eyeball.position;

        // Simulate gravity's pull
        Vector2 gravityOffset = new Vector2(0, -gravityStrength);

        // Add the character's velocity for an inertia effect
        Vector2 inertiaOffset = characterRigidbody.velocity * 0.1f;

        // Calculate the target position for the pupil
        Vector2 targetPosition = eyeballCenter + gravityOffset + inertiaOffset;

        // Clamp the target position within the eyeball's radius
        Vector2 offset = targetPosition - eyeballCenter;
        if (offset.magnitude > radius)
        {
            targetPosition = eyeballCenter + offset.normalized * radius;
        }

        // Smoothly move the pupil to the target position
        Vector2 smoothedPosition = Vector2.SmoothDamp(pupil.position, targetPosition, ref velocity, movementSmoothing);
        pupil.position = smoothedPosition;
    }
}