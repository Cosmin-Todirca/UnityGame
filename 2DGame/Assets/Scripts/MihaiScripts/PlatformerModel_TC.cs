using UnityEngine;

[System.Serializable]
public class PlatformerModel_TC
{
    /// <summary>
    /// The main component which controls the player sprite, controlled 
    /// by the user.
    /// </summary>
    public PlayerController_TC player;

    /// <summary>
    /// The spawn point in the scene.
    /// </summary>
    public Transform spawnPoint;

    /// <summary>
    /// A global jump modifier applied to all initial jump velocities.
    /// </summary>
    public float jumpModifier = 1.5f;

    /// <summary>
    /// A global jump modifier applied to slow down an active jump when 
    /// the user releases the jump input.
    /// </summary>
    public float jumpDeceleration = 0.5f;

}