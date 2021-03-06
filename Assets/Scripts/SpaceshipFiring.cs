﻿using UnityEngine;

public class SpaceshipFiring : MonoBehaviour
{
    [SerializeField] private KeyCode fire;
    [SerializeField] private GameObject projectile;
    [SerializeField] private AudioClip shotSound;

    private Sprite projectileSprite;
    private SpaceshipAttribute attribute;

    private float nextShootTime;

    public KeyCode Fire { get => fire; set => fire = value; }

    private void Start()
    {
        attribute = GetComponent<SpaceshipAttribute>();
        shotSound = Resources.Load<AudioClip>($"shotPlayer{MultiplayerScenePlayers.GetRandomNonRepeatingNumberBasedOnPlayerCount() + 1}");
        switch (attribute.Color.ToString())
        {
            case "RGBA(0.000, 0.000, 1.000, 1.000)":
                projectileSprite = Resources.Load<Sprite>("Laser Blue");
                break;
            case "RGBA(0.000, 1.000, 0.000, 1.000)":
                projectileSprite = Resources.Load<Sprite>("Laser Green");
                break;
            case "RGBA(1.000, 0.000, 0.000, 1.000)":
                projectileSprite = Resources.Load<Sprite>("Laser Red");
                break;
            case "RGBA(1.000, 0.000, 1.000, 1.000)":
                projectileSprite = Resources.Load<Sprite>("Laser Purple");
                break;
            default:
                break;
        }
        nextShootTime = Time.time + attribute.FireRate;
    }

    private void Update()
    {
        if (Input.GetKey(Fire) && Time.time > nextShootTime)
        {
            CreateProjectile();
            AudioSource.PlayClipAtPoint(shotSound, Vector3.zero);
            nextShootTime = attribute.FireRate + Time.time;
        }
    }

    private void CreateProjectile()
    {
        var project = Instantiate(projectile, transform.position, Quaternion.identity);
        project.GetComponent<StraightLineMovement>().Angle = transform.rotation.eulerAngles.z;
        project.GetComponent<SpriteRenderer>().sprite = projectileSprite;
        project.GetComponent<ProjectileAttributes>().Spaceship = GetComponent<SpaceshipAttribute>();
    }
}
