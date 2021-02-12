using UnityEngine;
using System.Collections;

public abstract class Character : ScriptableObject
{
    public string characterName = "Default";
    public int startingHp = 100;
    public Sprite sprite;
    public Animator animator;

    public bool isJump = true;
    public bool isFly = false;
    public bool isSwim = false;

    public bool isActive = false;

    public float characterPickCooldown = 1f;
    public Ability[] characterAbilities;
}