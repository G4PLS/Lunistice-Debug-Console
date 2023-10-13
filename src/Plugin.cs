using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using Lunistice_DebugConsole.UI;
using Luna;
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
    public static ConfigWrapper<bool> ShowOnStart { get; private set; }
    
    public static ConfigWrapper<float> TimeScale { get; private set; }
    public static ConfigWrapper<float> GravityX { get; private set; }
    public static ConfigWrapper<float> GravityY { get; private set; }
    public static ConfigWrapper<float> GravityZ { get; private set; }
    
    public static ConfigWrapper<int>   HanaMaxLife { get; private set; }
    public static ConfigWrapper<float> HanaSprintSpeed { get; private set; }
    public static ConfigWrapper<float> HanaRunSpeed { get; private set; }
    public static ConfigWrapper<float> HanaTurboSpeed { get; private set; }
    public static ConfigWrapper<float> HanaJumpHeight { get; private set; }
    public static ConfigWrapper<float> HanaAttackJump { get; private set; }
    public static ConfigWrapper<int>   HanaMaxDoubleJumps { get; private set; }
    public static ConfigWrapper<float> HanaCoyoteTime { get; private set; }
    public static ConfigWrapper<float> HanaFriction { get; private set; }
    public static ConfigWrapper<float> HanaAirFriction { get; private set; }
    public static ConfigWrapper<float> HanaAcceleration { get; private set; }
    
    public static ConfigWrapper<int> ToreeMaxLife { get; private set; }
    public static ConfigWrapper<float> ToreeSprintSpeed { get; private set; }
    public static ConfigWrapper<float> ToreeRunSpeed { get; private set; }
    public static ConfigWrapper<float> ToreeTurboSpeed { get; private set; }
    public static ConfigWrapper<float> ToreeJumpHeight { get; private set; }
    public static ConfigWrapper<float> ToreeAttackJump { get; private set; }
    public static ConfigWrapper<int> ToreeMaxDoubleJumps { get; private set; }
    public static ConfigWrapper<float> ToreeCoyoteTime { get; private set; }
    public static ConfigWrapper<float> ToreeFriction { get; private set; }
    public static ConfigWrapper<float> ToreeAirFriction { get; private set; }
    public static ConfigWrapper<float> ToreeAcceleration { get; private set; }
    
    public static ConfigWrapper<int> ToukieMaxLife { get; private set; }
    public static ConfigWrapper<float> ToukieSprintSpeed { get; private set; }
    public static ConfigWrapper<float> ToukieRunSpeed { get; private set; }
    public static ConfigWrapper<float> ToukieTurboSpeed { get; private set; }
    public static ConfigWrapper<float> ToukieJumpHeight { get; private set; }
    public static ConfigWrapper<float> ToukieAttackJump { get; private set; }
    public static ConfigWrapper<int> ToukieMaxDoubleJumps { get; private set; }
    public static ConfigWrapper<float> ToukieCoyoteTime { get; private set; }
    public static ConfigWrapper<float> ToukieFriction { get; private set; }
    public static ConfigWrapper<float> ToukieAirFriction { get; private set; }
    public static ConfigWrapper<float> ToukieAcceleration { get; private set; }

    #endregion
    
    public override void Load()
    {
        // Plugin startup logic
        Log.LogInfo($"Plugin {GUID} is loaded!");

        #region Config Declaration

        ShowOnStart = new ConfigWrapper<bool>(PluginSettings, "Plugin", "Show On Start", true, "", null);
        
        #region Game

        TimeScale = new ConfigWrapper<float>(GameSettings, "Game", "Time Scale", 1f, "The Time Scale of the Game",
            val => Time.timeScale = val);
        GravityX = new ConfigWrapper<float>(GameSettings, "Game", "Gravity X", 0f, "Gravity on the X Axis",
            val =>
            {
                Player.Gravity =
                    new Vector3(val, Player.Gravity.y, Player.Gravity.z);
                Player.GravityReset = Player.Gravity;
            });
        GravityY = new ConfigWrapper<float>(GameSettings, "Game", "Gravity Y", 14f, "Gravity on the Y Axis",
            val =>
            {
                Player.Gravity =
                    new Vector3(Player.Gravity.x, val, Player.Gravity.z);
                Player.GravityReset = Player.Gravity;
            });
        GravityZ = new ConfigWrapper<float>(GameSettings, "Game", "Gravity Z", 0f, "Gravity on the Z Axis",
            val =>
            {
                Player.Gravity =
                    new Vector3(Player.Gravity.x, Player.Gravity.y, val);
                Player.GravityReset = Player.Gravity;
            });

        #endregion

        #region Hana

        HanaMaxLife = new ConfigWrapper<int>(HanaSettings, "Player", "Max Life", 3, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.MaxLife = val;
        });
        HanaSprintSpeed = new ConfigWrapper<float>(HanaSettings, "Player", "Sprint Speed", 5f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.SprintSpeed = val;
        });
        HanaRunSpeed = new ConfigWrapper<float>(HanaSettings, "Player", "Run Speed", 3f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.RunSpeed = val;
        });
        HanaTurboSpeed = new ConfigWrapper<float>(HanaSettings, "Player", "Turbo Speed", 9f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.TurboSpeed = val;
        });
        HanaJumpHeight = new ConfigWrapper<float>(HanaSettings, "Hana", "Jump Height", 5f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.JumpHeight = val;
        });
        HanaAttackJump = new ConfigWrapper<float>(HanaSettings, "Player", "Attack Jump", 3f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.AttackJumpHeight = val;
        });
        HanaMaxDoubleJumps = new ConfigWrapper<int>(HanaSettings, "Player", "Max Double Jumps", 1, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.MaxDoubleJumps = val;
        });
        HanaCoyoteTime = new ConfigWrapper<float>(HanaSettings, "Player", "Coyote Time", 0.2f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.CoyoteTime = val;
        });
        HanaFriction = new ConfigWrapper<float>(HanaSettings, "Player", "Friction", 20f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.Friction = val;
        });
        HanaAirFriction = new ConfigWrapper<float>(HanaSettings, "Player", "Air Friction", 8f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.AirFriction = val;
        });
        HanaAcceleration = new ConfigWrapper<float>(HanaSettings, "Player", "Acceleration", 14f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Hana)
                Player.Acceleration = val;
        });

        #endregion

        #region Toree

        ToreeMaxLife = new ConfigWrapper<int>(ToreeSettings, "Player", "Max Life", 3, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.MaxLife = val;
        });
        ToreeSprintSpeed = new ConfigWrapper<float>(ToreeSettings, "Player", "Sprint Speed", 5f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.SprintSpeed = val;
        });
        ToreeRunSpeed = new ConfigWrapper<float>(ToreeSettings, "Player", "Run Speed", 3f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.RunSpeed = val;
        });
        ToreeTurboSpeed = new ConfigWrapper<float>(ToreeSettings, "Player", "Turbo Speed", 9f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.TurboSpeed = val;
        });
        ToreeJumpHeight = new ConfigWrapper<float>(ToreeSettings, "Player", "Jump Height", 5f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.JumpHeight = val;
        });
        ToreeAttackJump = new ConfigWrapper<float>(ToreeSettings, "Player", "Attack Jump", 3f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.AttackJumpHeight = val;
        });
        ToreeMaxDoubleJumps = new ConfigWrapper<int>(ToreeSettings, "Player", "Max Double Jumps", 1, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.MaxDoubleJumps = val;
        });
        ToreeCoyoteTime = new ConfigWrapper<float>(ToreeSettings, "Player", "Coyote Time", 0.2f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.CoyoteTime = val;
        });
        ToreeFriction = new ConfigWrapper<float>(ToreeSettings, "Player", "Friction", 20f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.Friction = val;
        });
        ToreeAirFriction = new ConfigWrapper<float>(ToreeSettings, "Player", "Air Friction", 8f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.AirFriction = val;
        });
        ToreeAcceleration = new ConfigWrapper<float>(ToreeSettings, "Player", "Acceleration", 14f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toree)
                Player.Acceleration = val;
        });

        #endregion

        #region Toukie

        ToukieMaxLife = new ConfigWrapper<int>(ToukieSettings, "Player", "Max Life", 3, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.MaxLife = val;
        });
        ToukieSprintSpeed = new ConfigWrapper<float>(ToukieSettings, "Player", "Sprint Speed", 5f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.SprintSpeed = val;
        });
        ToukieRunSpeed = new ConfigWrapper<float>(ToukieSettings, "Player", "Run Speed", 3f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.RunSpeed = val;
        });
        ToukieTurboSpeed = new ConfigWrapper<float>(ToukieSettings, "Player", "Turbo Speed", 9f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.TurboSpeed = val;
        });
        ToukieJumpHeight = new ConfigWrapper<float>(ToukieSettings, "Player", "Jump Height", 5f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.JumpHeight = val;
        });
        ToukieAttackJump = new ConfigWrapper<float>(ToukieSettings, "Player", "Attack Jump", 3f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.AttackJumpHeight = val;
        });
        ToukieMaxDoubleJumps = new ConfigWrapper<int>(ToukieSettings, "Player", "Max Double Jumps", 1, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.MaxDoubleJumps = val;
        });
        ToukieCoyoteTime = new ConfigWrapper<float>(ToukieSettings, "Player", "Coyote Time", 0.2f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.CoyoteTime = val;
        });
        ToukieFriction = new ConfigWrapper<float>(ToukieSettings, "Player", "Friction", 20f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.Friction = val;
        });
        ToukieAirFriction = new ConfigWrapper<float>(ToukieSettings, "Player", "Air Friction", 8f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.AirFriction = val;
        });
        ToukieAcceleration = new ConfigWrapper<float>(ToukieSettings, "Player", "Acceleration", 14f, "", val =>
        {
            if(LevelTimer.Character == Timer.Character.Toukie)
                Player.Acceleration = val;
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
            Time.timeScale = 0.01f * TimeScale.GetValue();
        else
            Time.timeScale = TimeScale.GetValue();
    }

    private void LogHandler(string message, LogType type)
    {
        
    }
}