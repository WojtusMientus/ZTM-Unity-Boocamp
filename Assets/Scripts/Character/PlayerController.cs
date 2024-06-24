using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Quest;
using RPG.Utility;
using System;


namespace RPG.Character
{
    public class PlayerController : MonoBehaviour
    {
        public CharacterStatusSO stats;
        [NonSerialized] public Health healthCmp;
        [NonSerialized] public Combat combatCmp;

        private GameObject axeWeapon;
        private GameObject swordWeapon;

        public Weapons weapon = Weapons.AXE;

        public void Awake()
        {
            healthCmp = GetComponent<Health>();
            combatCmp = GetComponent<Combat>();
            axeWeapon = GameObject.FindGameObjectWithTag(Constants.AXE_TAG);
            swordWeapon = GameObject.FindGameObjectWithTag(Constants.SWORD_TAG);
        }

        private void OnEnable()
        {
            EventManager.OnReward += HandleReward;
        }

        private void OnDisable()
        {
            EventManager.OnReward -= HandleReward;
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey("Health"))
            {
                healthCmp.healthPoints = PlayerPrefs.GetFloat("Health");
                healthCmp.potionCount = PlayerPrefs.GetInt("Potions");
                combatCmp.damage = PlayerPrefs.GetFloat("Damage");
                weapon = (Weapons)PlayerPrefs.GetInt("Weapon");

                NavMeshAgent agentComponent = GetComponent<NavMeshAgent>();
                Portal portalComponent = FindObjectOfType<Portal>();

                agentComponent.Warp(portalComponent.spawnPoint.position);
                transform.rotation = portalComponent.spawnPoint.rotation;
            }

            else
            {
                healthCmp.healthPoints = stats.health;
                combatCmp.damage = stats.damage;
            }

            EventManager.RaiseChangePlayerHealth(healthCmp.healthPoints);
            EventManager.ChangePotionCount(healthCmp.potionCount);
            SetWeapon();
        }

        private void HandleReward(RewardSO reward)
        {
            healthCmp.healthPoints += reward.bonusHealth;
            healthCmp.potionCount += reward.bonusPotions;
            combatCmp.damage += reward.bonusDamage;

            EventManager.RaiseChangePlayerHealth(healthCmp.healthPoints);
            EventManager.ChangePotionCount(healthCmp.potionCount);

            if (reward.forceWeaponSwap)
            {
                weapon = reward.weapon;
                SetWeapon();
            }
        }

        private void SetWeapon()
        {
            if (weapon == Weapons.AXE)
            {
                axeWeapon.SetActive(true);
                swordWeapon.SetActive(false);
            }
            else
            {
                axeWeapon.SetActive(false);
                swordWeapon.SetActive(true);
            }

        }

    }
}

