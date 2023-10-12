

/*
namespace Lunistice_DebugConsole
{
    public class DebugConsole : MonoBehaviour
    {
        public enum Tab { Player, Level, Game, Teleport }
        public enum PlayerCharacter { Hana, Toree, Toukie }
        public Rect WindowRect { get; private set; } = new Rect(20, 20, 500, 500);
        public Tab CurrentTab { get; private set; } = Tab.Game;
        public PlayerCharacter Character { get; private set; }
        public static bool ShowDebugGUI { get; private set; } = false;
        public GUIStyle NormalLabel { get; private set; }
        public GUIStyle ChangedLabel { get; private set; }
        public GUIStyle WindowStyle { get; private set; }
        public static Timer GameTimer { get; private set; }
        public static LevelSelectMenu GameLevelSelectMenu { get; private set; }
        public static BeatManager GameBeatManager { get; private set; }
        public List<(Vector3, int)> TeleportPoints { get; private set; } = new List<(Vector3, int)>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                ShowDebugGUI = !ShowDebugGUI;

                GameManager.Instance.Pause(ShowDebugGUI);
                UIController.Instance.onLockCursor();
            }
            else if (Input.GetKeyDown(KeyCode.R))
                GameLevelSelectMenu.StartLevel(GameManager.Instance.currentLevel);
            else if (Input.GetKeyDown(KeyCode.T))
                AddTeleportPoint(GameManager.Instance?.playerRef);
            else if (Input.GetKeyDown(KeyCode.G) && TeleportPoints.Count > 0)
                TeleportTo(GameManager.Instance.playerRef, TeleportPoints[^1].Item1);

            if (!GameManager.Instance.IsPaused() && !ShowDebugGUI)
            {
                if (GameManager.Instance.playerRef != null && GameManager.Instance.playerRef._currentFreezeFrameTime != 0)
                    Time.timeScale = 0.01f * Plugin.TimeScale.Value;
                else
                    Time.timeScale = Plugin.TimeScale.Value;
            }
            else if (ShowDebugGUI)
                GameManager.Instance.Pause(ShowDebugGUI);
        }

        private void OnGUI()
        {
            if (!ShowDebugGUI) return;

            WindowStyle = new GUIStyle(GUI.skin.window);
            WindowStyle.normal.textColor = Color.black;

            NormalLabel = new GUIStyle(GUI.skin.label);
            NormalLabel.normal.textColor = Color.green;
            NormalLabel.fontSize = 15;
            NormalLabel.fontStyle = FontStyle.Bold;

            ChangedLabel = new GUIStyle(GUI.skin.label);
            ChangedLabel.normal.textColor = Color.red;
            ChangedLabel.fontSize = 15;
            ChangedLabel.fontStyle = FontStyle.Bold;

            WindowRect = GUI.Window(0, WindowRect, (GUI.WindowFunction)CreateWindow, $"Debug Console by {Plugin.Author}", WindowStyle);
        }

        private void CreateWindow(int windowID)
        {
            GUI.DragWindow(new Rect(0, 0, Screen.width, 20));

            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(CurrentTab == Tab.Game, "Game", "Button"))
                CurrentTab = Tab.Game;
            if (GUILayout.Toggle(CurrentTab == Tab.Level, "Level", "Button"))
                CurrentTab = Tab.Level;
            if (GUILayout.Toggle(CurrentTab == Tab.Player, "Player", "Button"))
                CurrentTab = Tab.Player;
            if (GUILayout.Toggle(CurrentTab == Tab.Teleport, "Teleport", "Button"))
                CurrentTab = Tab.Teleport;
            GUILayout.EndHorizontal();

            switch (CurrentTab)
            {
                case Tab.Game:
                    GameTab();
                    break;
                case Tab.Level:
                    LevelTab();
                    break;
                case Tab.Player:
                    PlayerTab();
                    break;
                case Tab.Teleport:
                    TeleportTab();
                    break;
            }
        }

        private void GameTab()
        {
            GUILayout.BeginVertical();

            Plugin.TimeScale.Value = BuildSlider((float)Plugin.TimeScale.Value, 0f, 10f, (float)Plugin.TimeScale.DefaultValue, "Time Scale: ", "00.00");

            // TODO: MAYBE USE HARMONY PATCH TO SET GRAVITY ON LEVEL LOAD
            Plugin.GravityX.Value = BuildSlider((float)Plugin.GravityX.Value, -50f, 50f, (float)Plugin.GravityX.DefaultValue, "Gravity X: ", "00.00");
            Plugin.GravityY.Value = BuildSlider((float)Plugin.GravityY.Value, -50f, 50f, (float)Plugin.GravityY.DefaultValue, "Gravity Y: ", "00.00");
            Plugin.GravityZ.Value = BuildSlider((float)Plugin.GravityZ.Value, -50f, 50f, (float)Plugin.GravityZ.DefaultValue, "Gravity Z: ", "00.00");

            GUILayout.EndVertical();

            if (GameManager.Instance?.playerRef != null)
            {
                GameManager.Instance.playerRef.gravity = new Vector3(Plugin.GravityX.Value, Plugin.GravityY.Value, Plugin.GravityZ.Value);
                GameManager.Instance.playerRef._gravityReset = new Vector3(Plugin.GravityX.Value, Plugin.GravityY.Value, Plugin.GravityZ.Value);
            }
        }

        private void LevelTab()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("Level: " + CurrentLevelIDToString(true), NormalLabel);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Main Menu"))
                GameManager.Instance?.CloseResults(false);
            if (GUILayout.Button("Reset Current Level") && VerifyLevelID(GameManager.Instance?.currentLevel))
                GameLevelSelectMenu.StartLevel(GameManager.Instance.currentLevel);
            if (GUILayout.Button("Next Level") && VerifyLevelID(GameManager.Instance?.currentLevel + 1))
                GameLevelSelectMenu.StartLevel(GameManager.Instance.currentLevel + 1);
            if (GUILayout.Button("Previous Level") && VerifyLevelID(GameManager.Instance?.currentLevel - 1))
                GameLevelSelectMenu.StartLevel(GameManager.Instance.currentLevel - 1);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Start Timer"))
                GameTimer?.StartTimer();
            if (GUILayout.Button("Stop Timer"))
                GameTimer?.StopTimer();
            if (GUILayout.Button("Pause Timer"))
                GameTimer?.PauseTimer(!GameTimer.timerPaused);
            GUILayout.EndHorizontal();

            var levels = (Levels[])Enum.GetValues(typeof(Levels));

            int levelsPerRow = 3;
            int numRows = Mathf.CeilToInt((float)levels.Length / levelsPerRow);
            int index = 0;

            GUILayout.Label("LEVELS:", NormalLabel);

            for (int row = 0; row < numRows; row++)
            {
                GUILayout.BeginHorizontal();
                for (int col = 0; col < levelsPerRow; col++)
                {
                    index = row * levelsPerRow + col;
                    if (index < levels.Length)
                    {
                        if (GUILayout.Button(levels[index].ToString()))
                            GameLevelSelectMenu.StartLevel((int)levels[index]);
                        //GameManager.Instance?.SwitchLevel(false, (int)levels[index]);
                    }
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        private void PlayerTab()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Toggle(Character == PlayerCharacter.Hana, "Hana", "Button"))
                Character = PlayerCharacter.Hana;
            if (GUILayout.Toggle(Character == PlayerCharacter.Toree, "Toree", "Button"))
                Character = PlayerCharacter.Toree;
            if (GUILayout.Toggle(Character == PlayerCharacter.Toukie, "Toukie", "Button"))
                Character = PlayerCharacter.Toukie;
            GUILayout.EndHorizontal();

            // MAKE PLUGIN STATS VISIBLE PER CHARACTER
            switch (Character)
            {
                case PlayerCharacter.Hana:
                    CreateHanaConfigSliders();
                    break;
                case PlayerCharacter.Toree:
                    CreateToreeConfigSliders();
                    break;
                case PlayerCharacter.Toukie:
                    CreateToukieConfigSliders();
                    break;
            }

            GUILayout.EndVertical();

            var player = GameManager.Instance?.playerRef;
            if (player == null) return;
            GUILayout.Label(player.acceleration.ToString());

            UpdatePlayerValues(player);
        }

        private void TeleportTab()
        {
            var player = GameManager.Instance?.playerRef;

            GUILayout.BeginVertical();

            if (GUILayout.Button("Add Teleport Point"))
                AddTeleportPoint(player);

            for (int i = 0; i < TeleportPoints.Count; i++)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(LevelIDToString(TeleportPoints[i].Item2) + TeleportPoints[i].Item1.ToString(), NormalLabel);
                if (GUILayout.Button("Teleport"))
                    TeleportTo(player, TeleportPoints[i].Item1);
                if (GUILayout.Button("Delete"))
                    TeleportPoints.RemoveAt(i);

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        private void AddTeleportPoint(Bun.PlayerController player)
        {
            if (player == null) return;
            TeleportPoints.Add((player.transform.position, GameManager.Instance.currentLevel));
        }

        private void TeleportTo(Bun.PlayerController player, Vector3 point)
        {
            if (player == null) return;
            player.Teleport(point);
            player.ResetVelocityState();
        }

        private void CreateHanaConfigSliders()
        {
            Plugin.HanaMaxLife.Value = BuildSlider(Plugin.HanaMaxLife.Value, 0, 50, (int)Plugin.HanaMaxLife.DefaultValue, "Max Life: ", "00");
            Plugin.HanaSprintSpeed.Value = BuildSlider(Plugin.HanaSprintSpeed.Value, -50f, 50f, (float)Plugin.HanaSprintSpeed.DefaultValue, "Sprint Speed: ", " 00.00");
            Plugin.HanaRunSpeed.Value = BuildSlider(Plugin.HanaRunSpeed.Value, -50f, 50f, (float)Plugin.HanaRunSpeed.DefaultValue, "Run Speed: ", " 00.00");
            Plugin.HanaTurboSpeed.Value = BuildSlider(Plugin.HanaTurboSpeed.Value, -50f, 50f, (float)Plugin.HanaTurboSpeed.DefaultValue, "Turbo Speed: ", " 00.00");
            Plugin.HanaAcceleration.Value = BuildSlider(Plugin.HanaAcceleration.Value, -50f, 50f, (float)Plugin.HanaAcceleration.DefaultValue, "Acceleration: ", " 00.00");
            Plugin.HanaJumpHeight.Value = BuildSlider(Plugin.HanaJumpHeight.Value, -50f, 50f, (float)Plugin.HanaJumpHeight.DefaultValue, "Jump Height: ", " 00.00");
            Plugin.HanaAttackJump.Value = BuildSlider(Plugin.HanaAttackJump.Value, -50f, 50f, (float)Plugin.HanaAttackJump.DefaultValue, "Attack Jump Height: ", " 00.00");
            Plugin.HanaMaxDoubleJumps.Value = BuildSlider(Plugin.HanaMaxDoubleJumps.Value, 0, 50, (int)Plugin.HanaMaxDoubleJumps.DefaultValue, "Max Double Jumps: ", "00");
            Plugin.HanaCoyoteTime.Value = BuildSlider(Plugin.HanaCoyoteTime.Value, 0f, 25f, (float)Plugin.HanaCoyoteTime.DefaultValue, "Coyote Time: ", "00.00");
            Plugin.HanaFriction.Value = BuildSlider(Plugin.HanaFriction.Value, -50f, 50f, (float)Plugin.HanaFriction.DefaultValue, "Ground Friction: ", " 00.00");
            Plugin.HanaAirFriction.Value = BuildSlider(Plugin.HanaAirFriction.Value, -50f, 50f, (float)Plugin.HanaAirFriction.DefaultValue, "Air Friction: ", " 00.00");
        }

        private void CreateToreeConfigSliders()
        {
            Plugin.ToreeMaxLife.Value = BuildSlider(Plugin.ToreeMaxLife.Value, 0, 50, (int)Plugin.ToreeMaxLife.DefaultValue, "Max Life: ", "00");
            Plugin.ToreeSprintSpeed.Value = BuildSlider(Plugin.ToreeSprintSpeed.Value, -50f, 50f, (float)Plugin.ToreeSprintSpeed.DefaultValue, "Sprint Speed: ", " 00.00");
            Plugin.ToreeRunSpeed.Value = BuildSlider(Plugin.ToreeRunSpeed.Value, -50f, 50f, (float)Plugin.ToreeRunSpeed.DefaultValue, "Run Speed: ", " 00.00");
            Plugin.ToreeTurboSpeed.Value = BuildSlider(Plugin.ToreeTurboSpeed.Value, -50f, 50f, (float)Plugin.ToreeTurboSpeed.DefaultValue, "Turbo Speed: ", " 00.00");
            Plugin.ToreeAcceleration.Value = BuildSlider(Plugin.ToreeAcceleration.Value, -50f, 50f, (float)Plugin.ToreeAcceleration.DefaultValue, "Acceleration: ", " 00.00");
            Plugin.ToreeJumpHeight.Value = BuildSlider(Plugin.ToreeJumpHeight.Value, -50f, 50f, (float)Plugin.ToreeJumpHeight.DefaultValue, "Jump Height: ", " 00.00");
            Plugin.ToreeAttackJump.Value = BuildSlider(Plugin.ToreeAttackJump.Value, -50f, 50f, (float)Plugin.ToreeAttackJump.DefaultValue, "Attack Jump Height: ", " 00.00");
            Plugin.ToreeMaxDoubleJumps.Value = BuildSlider(Plugin.ToreeMaxDoubleJumps.Value, 0, 50, (int)Plugin.ToreeMaxDoubleJumps.DefaultValue, "Max Double Jumps: ", "00");
            Plugin.ToreeCoyoteTime.Value = BuildSlider(Plugin.ToreeCoyoteTime.Value, 0f, 25f, (float)Plugin.ToreeCoyoteTime.DefaultValue, "Coyote Time: ", "00.00");
            Plugin.ToreeFriction.Value = BuildSlider(Plugin.ToreeFriction.Value, -50f, 50f, (float)Plugin.ToreeFriction.DefaultValue, "Ground Friction: ", " 00.00");
            Plugin.ToreeAirFriction.Value = BuildSlider(Plugin.ToreeAirFriction.Value, -50f, 50f, (float)Plugin.ToreeAirFriction.DefaultValue, "Air Friction: ", " 00.00");
        }

        private void CreateToukieConfigSliders()
        {
            Plugin.ToukieMaxLife.Value = BuildSlider(Plugin.ToukieMaxLife.Value, 0, 50, (int)Plugin.ToukieMaxLife.DefaultValue, "Max Life: ", "00");
            Plugin.ToukieSprintSpeed.Value = BuildSlider(Plugin.ToukieSprintSpeed.Value, -50f, 50f, (float)Plugin.ToukieSprintSpeed.DefaultValue, "Sprint Speed: ", " 00.00");
            Plugin.ToukieRunSpeed.Value = BuildSlider(Plugin.ToukieRunSpeed.Value, -50f, 50f, (float)Plugin.ToukieRunSpeed.DefaultValue, "Run Speed: ", " 00.00");
            Plugin.ToukieTurboSpeed.Value = BuildSlider(Plugin.ToukieTurboSpeed.Value, -50f, 50f, (float)Plugin.ToukieTurboSpeed.DefaultValue, "Turbo Speed: ", " 00.00");
            Plugin.ToukieAcceleration.Value = BuildSlider(Plugin.ToukieAcceleration.Value, -50f, 50f, (float)Plugin.ToukieAcceleration.DefaultValue, "Acceleration: ", " 00.00");
            Plugin.ToukieJumpHeight.Value = BuildSlider(Plugin.ToukieJumpHeight.Value, -50f, 50f, (float)Plugin.ToukieJumpHeight.DefaultValue, "Jump Height: ", " 00.00");
            Plugin.ToukieAttackJump.Value = BuildSlider(Plugin.ToukieAttackJump.Value, -50f, 50f, (float)Plugin.ToukieAttackJump.DefaultValue, "Attack Jump Height: ", " 00.00");
            Plugin.ToukieMaxDoubleJumps.Value = BuildSlider(Plugin.ToukieMaxDoubleJumps.Value, 0, 50, (int)Plugin.ToukieMaxDoubleJumps.DefaultValue, "Max Double Jumps: ", "00");
            Plugin.ToukieCoyoteTime.Value = BuildSlider(Plugin.ToukieCoyoteTime.Value, 0f, 25f, (float)Plugin.ToukieCoyoteTime.DefaultValue, "Coyote Time: ", "00.00");
            Plugin.ToukieFriction.Value = BuildSlider(Plugin.ToukieFriction.Value, -50f, 50f, (float)Plugin.ToukieFriction.DefaultValue, "Ground Friction: ", " 00.00");
            Plugin.ToukieAirFriction.Value = BuildSlider(Plugin.ToukieAirFriction.Value, -50f, 50f, (float)Plugin.ToukieAirFriction.DefaultValue, "Air Friction: ", " 00.00");
        }

        private static void UpdatePlayerValues(Bun.PlayerController player)
        {
            switch (GameTimer.character)
            {
                case Timer.Character.Hana:
                    player.maxLife = Plugin.HanaMaxLife.Value;
                    player.sprintSpeed = Plugin.HanaSprintSpeed.Value;
                    player.runSpeed = Plugin.HanaRunSpeed.Value;
                    player.turboSpeed = Plugin.HanaTurboSpeed.Value;
                    player.jumpHeight = Plugin.HanaJumpHeight.Value;
                    player.attackJump = Plugin.HanaAttackJump.Value;
                    player.maxDoubleJumps = Plugin.HanaMaxDoubleJumps.Value;
                    player.coyoteTime = Plugin.HanaCoyoteTime.Value;
                    player.friction = Plugin.HanaFriction.Value;
                    player.airFriction = Plugin.HanaAirFriction.Value;
                    player.acceleration = Plugin.HanaAcceleration.Value;
                    break;
                case Timer.Character.Toree:
                    player.maxLife = Plugin.ToreeMaxLife.Value;
                    player.sprintSpeed = Plugin.ToreeSprintSpeed.Value;
                    player.runSpeed = Plugin.ToreeRunSpeed.Value;
                    player.turboSpeed = Plugin.ToreeTurboSpeed.Value;
                    player.jumpHeight = Plugin.ToreeJumpHeight.Value;
                    player.attackJump = Plugin.ToreeAttackJump.Value;
                    player.maxDoubleJumps = Plugin.ToreeMaxDoubleJumps.Value;
                    player.coyoteTime = Plugin.ToreeCoyoteTime.Value;
                    player.friction = Plugin.ToreeFriction.Value;
                    player.airFriction = Plugin.ToreeAirFriction.Value;
                    player.acceleration = Plugin.ToreeAcceleration.Value;
                    break;
                case Timer.Character.Toukie:
                    player.maxLife = Plugin.ToukieMaxLife.Value;
                    player.sprintSpeed = Plugin.ToukieSprintSpeed.Value;
                    player.runSpeed = Plugin.ToukieRunSpeed.Value;
                    player.turboSpeed = Plugin.ToukieTurboSpeed.Value;
                    player.jumpHeight = Plugin.ToukieJumpHeight.Value;
                    player.attackJump = Plugin.ToukieAttackJump.Value;
                    player.maxDoubleJumps = Plugin.ToukieMaxDoubleJumps.Value;
                    player.coyoteTime = Plugin.ToukieCoyoteTime.Value;
                    player.friction = Plugin.ToukieFriction.Value;
                    player.airFriction = Plugin.ToukieAirFriction.Value;
                    player.acceleration = Plugin.ToukieAcceleration.Value;
                    break;
            }
            player._currentSpeed = 0;
        }

        private float BuildSlider(float value, float min, float max, float @default, string text, string format = "")
        {
            GUILayout.BeginHorizontal();

            if (value != @default)
                GUILayout.Label(text + value.ToString(format), ChangedLabel);
            else
                GUILayout.Label(text + value.ToString(format), NormalLabel);

            value = GUILayout.DoHorizontalSlider(value, min, max, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, null);
            if (GUILayout.Button("Default"))
                value = @default;

            GUILayout.EndHorizontal();

            return value;
        }
        private int BuildSlider(int value, int min, int max, int @default, string text, string format = "")
        {
            GUILayout.BeginHorizontal();

            if (value != @default)
                GUILayout.Label(text + value.ToString(format), ChangedLabel);
            else
                GUILayout.Label(text + value.ToString(format), NormalLabel);

            value = (int)GUILayout.DoHorizontalSlider((float)value, (float)min, (float)max, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb, null);
            if (GUILayout.Button("Default"))
                value = @default;

            GUILayout.EndHorizontal();

            return value;
        }

        private string CurrentLevelIDToString(bool includeID = false)
        {
            string @out = "";
            int? id = GameManager.Instance?.currentLevel;

            if (id == null || !VerifyLevelID(id))
                return "";

            if (includeID)
                @out += $"(ID:{id})";
            @out += ((Levels)id).ToString();
            return @out;
        }
        private string LevelIDToString(int id)
        {
            if (VerifyLevelID(id))
                return ((Levels)id).ToString();
            return "";
        }
        private bool VerifyLevelID(int? id)
        {
            if (id == null) return false;
            if (System.Enum.IsDefined(typeof(Levels), id))
                return true;
            return false;
        }

        [HarmonyPatch(typeof(Timer), "Awake")]
        [HarmonyPostfix]
        private static void GetTimer(Timer instance) => GameTimer = instance;

        [HarmonyPatch(typeof(UIController), "Start")]
        [HarmonyPostfix]
        private static void GetLevelSelectMenu(UIController instance) => GameLevelSelectMenu = instance.levelSelectMenu;

        [HarmonyPatch(typeof(Bun.PlayerController), "Start")]
        [HarmonyPostfix]
        private static void LoadPlayerData()
        {
            if (GameManager.Instance?.playerRef == null) return;
            UpdatePlayerValues(GameManager.Instance.playerRef);
        }

        [HarmonyPatch(typeof(BeatManager), "Start")]
        [HarmonyPostfix]
        private static void ApplyTimeScale(BeatManager instance)
        {
            instance.beatLength = (int)(instance.beatLength / Time.timeScale);
        }
    }
}
*/