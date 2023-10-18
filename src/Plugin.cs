using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using Lunistice_DebugConsole.UI;
using Luna;
using Luna.Config;
using UnityEngine;
using UniverseLib;
using UniverseLib.Config;
using UniverseLib.UI;
using Path = Il2CppSystem.IO.Path;
using Player = Luna.Player;

namespace Lunistice_DebugConsole;

[BepInPlugin(GUID, Name, Version)]
[BepInDependency("com.GAPLS.Luna")]
public class Plugin : BasePlugin
{
    public const string GUID = "com.GAPLS.Lunistice.DebugConsole";
    public const string Name = "Debug Console";
    public const string Version = "1.5";
    public const string Author = "GAPLS";

    public static readonly ConfigFile PluginSettings = new(Path.Combine(Paths.ConfigPath, Name, "Plugin.cfg"), true);
    public static readonly ConfigFile GameSettings = new(Path.Combine(Paths.ConfigPath, Name, "Game.cfg"), true);
    public static readonly ConfigFile HanaSettings = new(Path.Combine(Paths.ConfigPath, Name, "Hana.cfg"), true);
    public static readonly ConfigFile ToreeSettings = new(Path.Combine(Paths.ConfigPath, Name, "Toree.cfg"), true);
    public static readonly ConfigFile ToukieSettings = new(Path.Combine(Paths.ConfigPath, Name, "Toukie.cfg"), true);
    
    #region Config Definition

    public static Wrapper<bool> ShowOnStart { get; private set; }
    public static Wrapper<float> TimeScale { get; private set; }
    public static Wrapper<float> GravityX { get; private set; }
    public static Wrapper<float> GravityY { get; private set; }
    public static Wrapper<float> GravityZ { get; private set; }
    
    public static Wrapper<int>   HanaMaxLife { get; private set; }
    public static Wrapper<float> HanaSprintSpeed { get; private set; }
    public static Wrapper<float> HanaRunSpeed { get; private set; }
    public static Wrapper<float> HanaTurboSpeed { get; private set; }
    public static Wrapper<float> HanaJumpHeight { get; private set; }
    public static Wrapper<float> HanaAttackJump { get; private set; }
    public static Wrapper<int>   HanaMaxDoubleJumps { get; private set; }
    public static Wrapper<float> HanaCoyoteTime { get; private set; }
    public static Wrapper<float> HanaFriction { get; private set; }
    public static Wrapper<float> HanaAirFriction { get; private set; }
    public static Wrapper<float> HanaAcceleration { get; private set; }
    
    public static Wrapper<int> ToreeMaxLife { get; private set; }
    public static Wrapper<float> ToreeSprintSpeed { get; private set; }
    public static Wrapper<float> ToreeRunSpeed { get; private set; }
    public static Wrapper<float> ToreeTurboSpeed { get; private set; }
    public static Wrapper<float> ToreeJumpHeight { get; private set; }
    public static Wrapper<float> ToreeAttackJump { get; private set; }
    public static Wrapper<int> ToreeMaxDoubleJumps { get; private set; }
    public static Wrapper<float> ToreeCoyoteTime { get; private set; }
    public static Wrapper<float> ToreeFriction { get; private set; }
    public static Wrapper<float> ToreeAirFriction { get; private set; }
    public static Wrapper<float> ToreeAcceleration { get; private set; }
    
    public static Wrapper<int> ToukieMaxLife { get; private set; }
    public static Wrapper<float> ToukieSprintSpeed { get; private set; }
    public static Wrapper<float> ToukieRunSpeed { get; private set; }
    public static Wrapper<float> ToukieTurboSpeed { get; private set; }
    public static Wrapper<float> ToukieJumpHeight { get; private set; }
    public static Wrapper<float> ToukieAttackJump { get; private set; }
    public static Wrapper<int> ToukieMaxDoubleJumps { get; private set; }
    public static Wrapper<float> ToukieCoyoteTime { get; private set; }
    public static Wrapper<float> ToukieFriction { get; private set; }
    public static Wrapper<float> ToukieAirFriction { get; private set; }
    public static Wrapper<float> ToukieAcceleration { get; private set; }

    #endregion
    
    public override void Load()
    {
        // Plugin startup logic
        Log.LogInfo($"Plugin {GUID} is loaded!");

        #region Config Declaration

        ShowOnStart = new Wrapper<bool>(PluginSettings, "Plugin", "Show On Start", true, "", null);
        
        #region Game

        TimeScale = new Wrapper<float>(GameSettings, "Game", "Time Scale", 1f, "The Time Scale of the Game",
            (_, val) => Time.timeScale = val.BoxedValue.TryCast<float>());
        GravityX = new Wrapper<float>(GameSettings, "Game", "Gravity X", 0f, "Gravity on the X Axis",
            (_, val) =>
            {
                Player.Gravity =
                    new Vector3(val.BoxedValue.TryCast<float>(), Player.Gravity.y, Player.Gravity.z);
                Player.GravityReset = Player.Gravity;
            });
        GravityY = new Wrapper<float>(GameSettings, "Game", "Gravity Y", 14f, "Gravity on the Y Axis",
            (_, val) =>
            {
                Player.Gravity =
                    new Vector3(Player.Gravity.x, val.BoxedValue.TryCast<float>(), Player.Gravity.z);
                Player.GravityReset = Player.Gravity;
            });
        GravityZ = new Wrapper<float>(GameSettings, "Game", "Gravity Z", 0f, "Gravity on the Z Axis",
            (_, val) =>
            {
                Player.Gravity =
                    new Vector3(Player.Gravity.x, Player.Gravity.y, val.BoxedValue.TryCast<float>());
                Player.GravityReset = Player.Gravity;
            });

        #endregion

        #region Hana

        HanaMaxLife = new Wrapper<int>(HanaSettings, "Player", "Max Life", 3, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.MaxLife = val.BoxedValue.TryCast<int>();
        });
        HanaSprintSpeed = new Wrapper<float>(HanaSettings, "Player", "Sprint Speed", 5f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.SprintSpeed = val.BoxedValue.TryCast<float>();
        });
        HanaRunSpeed = new Wrapper<float>(HanaSettings, "Player", "Run Speed", 3f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.RunSpeed = val.BoxedValue.TryCast<float>();
        });
        HanaTurboSpeed = new Wrapper<float>(HanaSettings, "Player", "Turbo Speed", 9f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.TurboSpeed = val.BoxedValue.TryCast<float>();
        });
        HanaJumpHeight = new Wrapper<float>(HanaSettings, "Hana", "Jump Height", 5f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.JumpHeight = val.BoxedValue.TryCast<float>();
        });
        HanaAttackJump = new Wrapper<float>(HanaSettings, "Player", "Attack Jump", 3f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.AttackJumpHeight = val.BoxedValue.TryCast<float>();
        });
        HanaMaxDoubleJumps = new Wrapper<int>(HanaSettings, "Player", "Max Double Jumps", 1, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.MaxDoubleJumps = val.BoxedValue.TryCast<int>();
        });
        HanaCoyoteTime = new Wrapper<float>(HanaSettings, "Player", "Coyote Time", 0.2f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.CoyoteTime = val.BoxedValue.TryCast<float>();
        });
        HanaFriction = new Wrapper<float>(HanaSettings, "Player", "Friction", 20f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.Friction = val.BoxedValue.TryCast<float>();
        });
        HanaAirFriction = new Wrapper<float>(HanaSettings, "Player", "Air Friction", 8f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.AirFriction = val.BoxedValue.TryCast<float>();
        });
        HanaAcceleration = new Wrapper<float>(HanaSettings, "Player", "Acceleration", 14f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.Acceleration = val.BoxedValue.TryCast<float>();
        });

        #endregion

        #region Toree

        ToreeMaxLife = new Wrapper<int>(ToreeSettings, "Player", "Max Life", 1, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.MaxLife = val.BoxedValue.TryCast<int>();
        });
        ToreeSprintSpeed = new Wrapper<float>(ToreeSettings, "Player", "Sprint Speed", 6f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.SprintSpeed = val.BoxedValue.TryCast<float>();
        });
        ToreeRunSpeed = new Wrapper<float>(ToreeSettings, "Player", "Run Speed", 4f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.RunSpeed = val.BoxedValue.TryCast<float>();
        });
        ToreeTurboSpeed = new Wrapper<float>(ToreeSettings, "Player", "Turbo Speed", 10f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.TurboSpeed = val.BoxedValue.TryCast<float>();
        });
        ToreeJumpHeight = new Wrapper<float>(ToreeSettings, "Player", "Jump Height", 5f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.JumpHeight = val.BoxedValue.TryCast<float>();
        });
        ToreeAttackJump = new Wrapper<float>(ToreeSettings, "Player", "Attack Jump", 3f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.AttackJumpHeight = val.BoxedValue.TryCast<float>();
        });
        ToreeMaxDoubleJumps = new Wrapper<int>(ToreeSettings, "Player", "Max Double Jumps", 1, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.MaxDoubleJumps = val.BoxedValue.TryCast<int>();
        });
        ToreeCoyoteTime = new Wrapper<float>(ToreeSettings, "Player", "Coyote Time", 0.2f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.CoyoteTime = val.BoxedValue.TryCast<float>();
        });
        ToreeFriction = new Wrapper<float>(ToreeSettings, "Player", "Friction", 20f, "",(_, val)  =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.Friction = val.BoxedValue.TryCast<float>();
        });
        ToreeAirFriction = new Wrapper<float>(ToreeSettings, "Player", "Air Friction", 8f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.AirFriction = val.BoxedValue.TryCast<float>();
        });
        ToreeAcceleration = new Wrapper<float>(ToreeSettings, "Player", "Acceleration", 18f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.Acceleration = val.BoxedValue.TryCast<float>();
        });

        #endregion

        #region Toukie

        ToukieMaxLife = new Wrapper<int>(ToukieSettings, "Player", "Max Life", 2, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.MaxLife = val.BoxedValue.TryCast<int>();
        });
        ToukieSprintSpeed = new Wrapper<float>(ToukieSettings, "Player", "Sprint Speed", 5f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.SprintSpeed = val.BoxedValue.TryCast<float>();
        });
        ToukieRunSpeed = new Wrapper<float>(ToukieSettings, "Player", "Run Speed", 3f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.RunSpeed = val.BoxedValue.TryCast<float>();
        });
        ToukieTurboSpeed = new Wrapper<float>(ToukieSettings, "Player", "Turbo Speed", 9f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.TurboSpeed = val.BoxedValue.TryCast<float>();
        });
        ToukieJumpHeight = new Wrapper<float>(ToukieSettings, "Player", "Jump Height", 4f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.JumpHeight = val.BoxedValue.TryCast<float>();
        });
        ToukieAttackJump = new Wrapper<float>(ToukieSettings, "Player", "Attack Jump", 2f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.AttackJumpHeight = val.BoxedValue.TryCast<float>();
        });
        ToukieMaxDoubleJumps = new Wrapper<int>(ToukieSettings, "Player", "Max Double Jumps", 3, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.MaxDoubleJumps = val.BoxedValue.TryCast<int>();
        });
        ToukieCoyoteTime = new Wrapper<float>(ToukieSettings, "Player", "Coyote Time", 0.2f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.CoyoteTime = val.BoxedValue.TryCast<float>();
        });
        ToukieFriction = new Wrapper<float>(ToukieSettings, "Player", "Friction", 20f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.Friction = val.BoxedValue.TryCast<float>();
        });
        ToukieAirFriction = new Wrapper<float>(ToukieSettings, "Player", "Air Friction", 6f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.AirFriction = val.BoxedValue.TryCast<float>();
        });
        ToukieAcceleration = new Wrapper<float>(ToukieSettings, "Player", "Acceleration", 14f, "", (_, val) =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.Acceleration = val.BoxedValue.TryCast<float>();
        });

#endregion

        #endregion

        const float startupDelay = 1f;
        UniverseLibConfig config = new()
        {
            Disable_EventSystem_Override = false,
            Force_Unlock_Mouse = true,
            Unhollowed_Modules_Folder = Path.Combine(Paths.BepInExRootPath, "interop"),
        };

        Universe.Init(startupDelay, OnInitialised, LogHandler, config);
    }

    private UIBase _uiBase;
    private DebugMenuUI _debugMenu;
    private void OnInitialised()
    {
        _uiBase = UniversalUI.RegisterUI("DebugPanel", UiUpdate);
        _debugMenu = new(_uiBase);
    }

    private void UiUpdate()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(_debugMenu.Enabled)
                _debugMenu.Hide();
            else
                _debugMenu.Show();
        }
        else if(_debugMenu.Enabled)
            Game.Pause(_debugMenu.Enabled);

        if(Game.IsPaused()){}
        else if (Player.CurrentFreezeFrameTime != 0 && !_debugMenu.Enabled)
            Time.timeScale = 0.01f * TimeScale.Value;
        else
            Time.timeScale = TimeScale.Value;
    }

    private void LogHandler(string message, LogType type)
    {
        
    }
}