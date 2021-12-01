using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class Overseer : MonoBehaviour
{
    #region Instance
    public static Overseer Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Init();
    }
    #endregion

    public InputManager inputManager;
    public ProfileManager profileManager;
    public Player player;
    public ProceduralTerrain proceduralTerrain;
    public ProceduralFoliage proceduralFoliage;
    public DeadlyFogGen deadlyFogGen;
    public LevelGenerator levelGenerator;
    public CurrentSectionAssigner currentSectionAssigner;
    public SectionActivation sectionActivation;
    public CheckPoints checkPoints;
    public EnemyManager enemyManager;
    public AudioManager audioManager;
    public DialogueManager dialogueManager;
    public CamManager camManager;
    public CamShake camShake;
    public CamOffsetter camOffsetter;
    public Hud hud;
    public Prompt prompt;

    public Fails fails;

    public PauseMenu pauseMenu;

    List<Action> toInit;

    private void Init() // called from Awake() in instance code
    {
        toInit = new List<Action>();

        SetupPrompt();
        prompt.SetPrompt("Finding components");
        InterfaceUtility.Init();
        AssignComponents();

        PopulateToInitList();

        StartCoroutine(InitCoroutine());

        
    }

    IEnumerator InitCoroutine()
    {
        for (int i = 0; i < toInit.Count; i++)
        {
            toInit[i]();
            yield return null;
        }
    }

    void PopulateToInitList()
    {
        

        toInit.Add(() => prompt.SetPrompt("Init camera"));
        toInit.Add(camManager.Init);
        toInit.Add(camShake.Init);
        toInit.Add(camOffsetter.Init);

        toInit.Add(() => prompt.SetPrompt("Init profile"));
        toInit.Add(profileManager.Init);

        toInit.Add(() => prompt.SetPrompt("Generate terrain"));
        toInit.Add(proceduralTerrain.Init);

        toInit.Add(() => prompt.SetPrompt("Grow trees"));
        toInit.Add(proceduralFoliage.Init);

        toInit.Add(() => prompt.SetPrompt("Spray deadly fog"));
        toInit.Add(deadlyFogGen.Init);

        toInit.Add(() => prompt.SetPrompt("Make level"));
        toInit.Add(levelGenerator.Init);

        toInit.Add(() => prompt.SetPrompt("GC"));
        toInit.Add(GC.Collect);

        toInit.Add(prompt.ClearPrompt);
        toInit.Add(hud.HideLoadingSplash);
    }

    void SetupPrompt()
    {
        prompt = this.FindObjectOfTypeOrComplain<Prompt>();
    }

    void AssignComponents()
    {
        inputManager = this.FindObjectOfTypeOrComplain<InputManager>();
        profileManager = this.FindObjectOfTypeOrComplain<ProfileManager>();
        player = this.FindObjectOfTypeOrComplain<Player>();
        proceduralTerrain = this.FindObjectOfTypeOrComplain<ProceduralTerrain>();
        proceduralFoliage = this.FindObjectOfTypeOrComplain<ProceduralFoliage>();
        deadlyFogGen = this.FindObjectOfTypeOrComplain<DeadlyFogGen>();
        levelGenerator = this.FindObjectOfTypeOrComplain<LevelGenerator>();
        currentSectionAssigner = this.FindObjectOfTypeOrComplain<CurrentSectionAssigner>();
        sectionActivation = this.FindObjectOfTypeOrComplain<SectionActivation>();
        checkPoints = this.FindObjectOfTypeOrComplain<CheckPoints>();
        enemyManager = this.FindObjectOfTypeOrComplain<EnemyManager>();
        audioManager = this.FindObjectOfTypeOrComplain<AudioManager>();
        dialogueManager = this.FindObjectOfTypeOrComplain<DialogueManager>();
        camManager = this.FindObjectOfTypeOrComplain<CamManager>();
        camShake = this.FindObjectOfTypeOrComplain<CamShake>();
        camOffsetter = this.FindObjectOfTypeOrComplain<CamOffsetter>();
        hud = this.FindObjectOfTypeOrComplain<Hud>();
        fails = this.GetComponentOrComplain<Fails>();
        pauseMenu = this.FindObjectOfTypeOrComplain<PauseMenu>();
    }

    public void PlayerRespawn()
    {
        enemyManager.ResetEnemies();
        int curretnSectionIndex = currentSectionAssigner.AssignCurrentSection();
        sectionActivation.ActivateSection(curretnSectionIndex);
    }
}
