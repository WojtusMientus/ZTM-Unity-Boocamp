using UnityEngine;
using RPG.Character;
using RPG.Utility;
using Ink.Parsed;
using System.Collections.Generic;
using System.Linq;
using RPG.Quest;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System;

namespace RPG.Core
{
    public class GameManager : MonoBehaviour
    {

        private List<string> sceneEnemyIDs = new List<string>();
        private List<GameObject> enemiesAlive = new List<GameObject>();
        private PlayerInput playerInput;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }


        private void Start()
        {
            List<GameObject> sceneEnemies = GameObject.FindGameObjectsWithTag(Constants.ENEMY_TAG).ToList<GameObject>();

            foreach (GameObject gameObject in sceneEnemies)
            {
                EnemyController enemyController = gameObject.GetComponent<EnemyController>();
                sceneEnemyIDs.Add(enemyController.enemyID);
            }
        }
        private void OnEnable()
        {
            EventManager.OnPortalEnter += HandlePortalEvent;
            EventManager.OnCutsceneUpdated += HandleCutsceneUpdate;
        }

        private void OnDisable()
        {
            EventManager.OnPortalEnter -= HandlePortalEvent;
            EventManager.OnCutsceneUpdated -= HandleCutsceneUpdate;
        }
        private void HandlePortalEvent(Collider player, int nextSceneIndex)
        {
            PlayerController playerComponent = player.GetComponent<PlayerController>();

            PlayerPrefs.SetFloat("Health", playerComponent.healthCmp.healthPoints);
            PlayerPrefs.SetInt("Potions", playerComponent.healthCmp.potionCount);
            PlayerPrefs.SetFloat("Damage", playerComponent.combatCmp.damage);
            PlayerPrefs.SetInt("Weapon", (int)playerComponent.weapon);
            PlayerPrefs.SetInt("SceneIndex", nextSceneIndex);

            if (PlayerPrefs.HasKey("WasInScene"))
                PlayerPrefs.SetInt("WasInScene", PlayerPrefs.GetInt("WasInScene") + 1);
            else
                PlayerPrefs.SetInt("WasInScene", 0);

            enemiesAlive.AddRange(GameObject.FindGameObjectsWithTag(Constants.ENEMY_TAG).ToList<GameObject>());

            sceneEnemyIDs.ForEach(SaveDefeatedEnemies);

            Inventory inventoryComponent = player.GetComponent<Inventory>();

            inventoryComponent.items.ForEach(SaveQuestItem);

            List<GameObject> NPCs = new List<GameObject>(GameObject.FindGameObjectsWithTag(Constants.NPC_QUEST_TAG));

            NPCs.ForEach(SaveNPCItem);
        }

        private void SaveDefeatedEnemies(string ID)
        {
            bool isAlive = false;

            foreach (GameObject enemy in enemiesAlive)
            {
                string enemyID = enemy.GetComponent<EnemyController>().enemyID;

                if (enemyID == ID)
                    isAlive = true;

            }

            if (isAlive) return;

            List<string> enemiesDefeated = PlayerPrefsUtility.GetString("EnemiesDefeated");

            if (!enemiesDefeated.Contains(ID))
                enemiesDefeated.Add(ID);

            PlayerPrefsUtility.SetString("EnemiesDefeated", enemiesDefeated);
        }

        private void SaveQuestItem(QuestItemSO item)
        {
            List<string> playerItems = PlayerPrefsUtility.GetString("PlayerItems");

            playerItems.Add(item.name);

            PlayerPrefsUtility.SetString("PlayerItems", playerItems);
        }

        private void SaveNPCItem(GameObject npc)
        {
            NPCController npcController = npc.GetComponent<NPCController>();

            if (!npcController.hasQuestItem)
                return;

            List<string> npcItems = PlayerPrefsUtility.GetString("NPCItems");

            npcItems.Add(npcController.desiredQuestItems.name);

            PlayerPrefsUtility.SetString("NPCItems", npcItems);
        }

        private void HandleCutsceneUpdate(bool isEnabled)
        {
            playerInput.enabled = isEnabled;
        }


    }
}